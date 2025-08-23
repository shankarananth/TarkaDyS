/*
 * File: TankLevelWithPidModel.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Tank level control system with PID control simulation model
 * Date Created: 2024-01-XX
 * Date Modified: 2024-01-XX
 * 
 * Description: Represents a tank level control system with PID control.
 * This model simulates a tank level process where the PID controller automatically
 * adjusts the inflow rate to maintain the tank level at the desired setpoint.
 * 
 * Modifications:
 * - 2024-01-XX: Changed units from meters to percentage for standardization
 * - 2024-01-XX: Fixed steady state initialization - all values start at proper steady state
 * - 2024-01-XX: Added output limits with anti-reset windup
 * - 2024-01-XX: Added multiple PID controller types support
 */

using TarkaDyS.Core.Controllers;
using TarkaDyS.Core.Models;
using TarkaDyS.Core.Interfaces;
using System;

namespace TarkaDyS.ProcessModels
{
    /// <summary>
    /// Represents a tank level control system with PID control.
    /// This model simulates a tank level process where the PID controller automatically
    /// adjusts the inflow rate to maintain the tank level at the desired setpoint.
    /// Common in water treatment, chemical processing, and storage applications.
    /// Uses single-timer architecture where the process model is called as a function.
    /// Units changed to percentage for standardization.
    /// </summary>
    public class TankLevelWithPidModel : ProcessModelBase
    {
        #region Private Fields
        private readonly object _lockObject = new object();
        private readonly PidController _tankLevelPidController;
        private bool _disposed = false;
        
        // Tank level control variables - FIXED: Changed to percentage units and proper steady state
        private double _tankLevelSetpoint = 50.0;            // Desired tank level (%)
        private double _tankLevelProcessVariable = 50.0;     // Current tank level (%) - start at setpoint
        private double _tankLevelControllerOutput = 50.0;    // Inflow rate (%) - start at steady state
        private double _tankLevelDisturbance = 0.0;          // External flow disturbance (%)
        
        // Tank parameters - converted to work with percentage units
        private double _tankLevelCrossSectionArea = 10.0;    // Tank cross-sectional area (m²) - kept for physics
        private double _tankLevelOutflowRate = 50.0;         // Fixed outflow rate (%) - matches steady state
        private double _tankLevelMaxLevel = 100.0;           // Maximum tank level (%) - full scale
        
        // Loop configuration
        private bool _tankLevelClosedLoop = true;
        private double _tankLevelManualOutput = 50.0;        // Start at steady state value
        private bool _tankLevelSetpointTracking = false;
        
        // Simulation speed control (multiplier for real-time)
        private double _simulationSpeedMultiplier = 1.0;     // 1.0 = real-time, 2.0 = 2x speed, 0.5 = half speed
        
        // Output limits - Added back per requirements
        private double _tankLevelOutputMin = 0.0;            // Minimum output (%)
        private double _tankLevelOutputMax = 100.0;          // Maximum output (%)
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tank level setpoint (%)
        /// </summary>
        public double TankLevelSetpoint
        {
            get
            {
                lock (_lockObject)
                {
                    return _tankLevelSetpoint;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    // Apply setpoint limits (0-100%)
                    double clampedValue = Math.Max(0.0, Math.Min(100.0, value));
                    if (Math.Abs(_tankLevelSetpoint - clampedValue) > double.Epsilon)
                    {
                        double oldValue = _tankLevelSetpoint;
                        _tankLevelSetpoint = clampedValue;
                        _tankLevelPidController.Setpoint = clampedValue;
                        OnPropertyChanged(nameof(TankLevelSetpoint));
                        OnParameterChanged(nameof(TankLevelSetpoint), oldValue, clampedValue);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current tank level (%)
        /// </summary>
        public double TankLevelProcessVariable
        {
            get
            {
                lock (_lockObject)
                {
                    return _tankLevelProcessVariable;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_tankLevelProcessVariable - value) > double.Epsilon)
                    {
                        // Clamp to 0-100% range
                        _tankLevelProcessVariable = Math.Max(0.0, Math.Min(100.0, value));
                        OnPropertyChanged(nameof(TankLevelProcessVariable));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current controller output (inflow rate in %)
        /// </summary>
        public double TankLevelControllerOutput
        {
            get
            {
                lock (_lockObject)
                {
                    return _tankLevelControllerOutput;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_tankLevelControllerOutput - value) > double.Epsilon)
                    {
                        // Apply output limits
                        _tankLevelControllerOutput = Math.Max(_tankLevelOutputMin, Math.Min(_tankLevelOutputMax, value));
                        OnPropertyChanged(nameof(TankLevelControllerOutput));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the control loop is closed (automatic) or open (manual)
        /// </summary>
        public bool TankLevelClosedLoop
        {
            get
            {
                lock (_lockObject)
                {
                    return _tankLevelClosedLoop;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_tankLevelClosedLoop != value)
                    {
                        _tankLevelClosedLoop = value;
                        _tankLevelPidController.AutoMode = value;
                        
                        // When switching to manual mode, sync manual output with current controller output
                        if (!value)
                        {
                            _tankLevelManualOutput = _tankLevelControllerOutput;
                            _tankLevelPidController.ManualOutput = _tankLevelManualOutput;
                        }
                        
                        OnPropertyChanged(nameof(TankLevelClosedLoop));
                        OnParameterChanged(nameof(TankLevelClosedLoop), _tankLevelClosedLoop ? 0 : 1, value ? 1 : 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the manual output value (used when TankLevelClosedLoop is false)
        /// </summary>
        public double TankLevelManualOutput
        {
            get
            {
                lock (_lockObject)
                {
                    return _tankLevelManualOutput;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_tankLevelManualOutput - value) > double.Epsilon)
                    {
                        double oldValue = _tankLevelManualOutput;
                        _tankLevelManualOutput = value;
                        _tankLevelPidController.ManualOutput = value;
                        
                        // If in manual mode, update the controller output immediately
                        if (!_tankLevelClosedLoop)
                        {
                            TankLevelControllerOutput = value;
                        }
                        
                        OnPropertyChanged(nameof(TankLevelManualOutput));
                        OnParameterChanged(nameof(TankLevelManualOutput), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether setpoint should track the process variable when in manual mode
        /// </summary>
        public bool TankLevelSetpointTracking
        {
            get
            {
                lock (_lockObject)
                {
                    return _tankLevelSetpointTracking;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_tankLevelSetpointTracking != value)
                    {
                        _tankLevelSetpointTracking = value;
                        OnPropertyChanged(nameof(TankLevelSetpointTracking));
                        OnParameterChanged(nameof(TankLevelSetpointTracking), _tankLevelSetpointTracking ? 0 : 1, value ? 1 : 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the external disturbance applied to the tank level process (%)
        /// </summary>
        public double TankLevelDisturbance
        {
            get
            {
                lock (_lockObject)
                {
                    return _tankLevelDisturbance;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_tankLevelDisturbance - value) > double.Epsilon)
                    {
                        double oldValue = _tankLevelDisturbance;
                        _tankLevelDisturbance = value;
                        OnPropertyChanged(nameof(TankLevelDisturbance));
                        OnParameterChanged(nameof(TankLevelDisturbance), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the tank cross-sectional area (m²) - kept for physics calculation
        /// </summary>
        public double TankLevelCrossSectionArea
        {
            get => _tankLevelCrossSectionArea;
            set => _tankLevelCrossSectionArea = Math.Max(0.1, value);
        }

        /// <summary>
        /// Gets or sets the fixed outflow rate (%) - normalized
        /// </summary>
        public double TankLevelOutflowRate
        {
            get => _tankLevelOutflowRate;
            set => _tankLevelOutflowRate = Math.Max(0, value);
        }

        /// <summary>
        /// Gets or sets the maximum tank level (%) - full scale
        /// </summary>
        public double TankLevelMaxLevel
        {
            get => _tankLevelMaxLevel;
            set => _tankLevelMaxLevel = Math.Max(1, value);
        }

        /// <summary>
        /// Gets the current control error (TankLevelSetpoint - TankLevelProcessVariable)
        /// </summary>
        public double TankLevelError
        {
            get
            {
                lock (_lockObject)
                {
                    return _tankLevelSetpoint - _tankLevelProcessVariable;
                }
            }
        }
        
        // PID parameters
        public double TankLevelKp { get => _tankLevelPidController.Kp; set => _tankLevelPidController.Kp = value; }
        public double TankLevelKi { get => _tankLevelPidController.Ki; set => _tankLevelPidController.Ki = value; }
        public double TankLevelKd { get => _tankLevelPidController.Kd; set => _tankLevelPidController.Kd = value; }

        /// <summary>
        /// Gets or sets the simulation speed multiplier (1.0 = real-time, 2.0 = 2x speed, etc.)
        /// Real-time (1x) = 1 second timestep, 10x = 0.1 second timestep
        /// </summary>
        public double SimulationSpeedMultiplier
        {
            get
            {
                lock (_lockObject)
                {
                    return _simulationSpeedMultiplier;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    // Clamp between 0.1x and 10x speed
                    double clampedValue = Math.Max(0.1, Math.Min(10.0, value));
                    if (Math.Abs(_simulationSpeedMultiplier - clampedValue) > double.Epsilon)
                    {
                        double oldValue = _simulationSpeedMultiplier;
                        _simulationSpeedMultiplier = clampedValue;
                        
                        // Update the TimeStep to reflect speed change
                        // Base TimeStep is 1.0 seconds for 1x speed (real-time)
                        // Higher speeds = smaller timesteps for more frequent updates
                        double newTimeStep = 1.0 / clampedValue;
                        TimeStep = newTimeStep;
                        
                        OnPropertyChanged(nameof(SimulationSpeedMultiplier));
                        OnParameterChanged(nameof(SimulationSpeedMultiplier), oldValue, clampedValue);
                        
                        System.Diagnostics.Debug.WriteLine($"Tank simulation speed changed to {clampedValue:F1}x, TimeStep = {newTimeStep:F3}s");
                    }
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TankLevelWithPidModel class
        /// </summary>
        public TankLevelWithPidModel(string modelId, string modelName) 
            : base(modelId, modelName)
        {
            // Create PID controller instance
            _tankLevelPidController = new PidController($"{modelId}_TankLevelController", $"{modelName} Tank Level Controller");
            
            // Set default PID parameters for tank level control
            _tankLevelPidController.SetTuning(2.0, 0.5, 0.1);
            _tankLevelPidController.SetOutputLimits(_tankLevelOutputMin, _tankLevelOutputMax); // Set proper output limits
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Initializes the tank level model with proper steady state
        /// FIXED: Changed to percentage units and proper steady state initialization
        /// </summary>
        protected override void InitializeModel()
        {
            lock (_lockObject)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"=== TankLevelWithPidModel.InitializeModel START ===");
                    
                    // FIXED: Start in proper steady state - inflow = outflow at 50% level
                    _tankLevelSetpoint = 50.0;           // 50% target level
                    _tankLevelProcessVariable = 50.0;    // Start at setpoint (steady state)
                    _tankLevelControllerOutput = 50.0;   // Inflow rate matches outflow for steady state
                    _tankLevelDisturbance = 0.0;         // No initial disturbance
                    _tankLevelClosedLoop = true;         // Start in automatic mode
                    _tankLevelManualOutput = 50.0;       // Manual output at steady state value
                    _tankLevelSetpointTracking = false;
                    _simulationSpeedMultiplier = 1.0;    // Initialize to real-time
                    
                    System.Diagnostics.Debug.WriteLine($"Tank initial steady state values: SP={_tankLevelSetpoint}%, PV={_tankLevelProcessVariable}%, MV={_tankLevelControllerOutput}%");
                    
                    // Set initial controller parameters for steady state
                    _tankLevelPidController.Setpoint = _tankLevelSetpoint;
                    _tankLevelPidController.AutoMode = _tankLevelClosedLoop;
                    _tankLevelPidController.ManualOutput = _tankLevelManualOutput;
                    
                    // FIXED: Set output limits with anti-reset windup
                    _tankLevelPidController.SetOutputLimits(_tankLevelOutputMin, _tankLevelOutputMax);
                    
                    System.Diagnostics.Debug.WriteLine($"Tank controller configured: SP={_tankLevelPidController.Setpoint}%, AutoMode={_tankLevelPidController.AutoMode}, Limits=[{_tankLevelOutputMin}%-{_tankLevelOutputMax}%]");
                    
                    // Initialize controller
                    _tankLevelPidController.Initialize();
                    
                    // Force controller to steady state by doing an update with zero error
                    _tankLevelPidController.Update(_tankLevelProcessVariable, 1.0);
                    
                    // FIXED: Force controller output to steady state value
                    _tankLevelControllerOutput = 50.0;
                    
                    System.Diagnostics.Debug.WriteLine($"Tank FORCED steady state: SP={_tankLevelSetpoint}%, PV={_tankLevelProcessVariable}%, MV={_tankLevelControllerOutput}%");
                    
                    System.Diagnostics.Debug.WriteLine($"TankLevelWithPid Initialized: SP={_tankLevelSetpoint}%, PV={_tankLevelProcessVariable:F2}%, MV={_tankLevelControllerOutput}%, Error={_tankLevelSetpoint - _tankLevelProcessVariable:F2}%");
                    
                    // Initialize model parameters
                    SetParameter("TankLevelSetpoint", _tankLevelSetpoint);
                    SetParameter("TankLevelProcessVariable", _tankLevelProcessVariable);
                    SetParameter("TankLevelControllerOutput", _tankLevelControllerOutput);
                    SetParameter("TankLevelDisturbance", _tankLevelDisturbance);
                    SetParameter("TankLevelClosedLoop", _tankLevelClosedLoop ? 1.0 : 0.0);
                    SetParameter("TankLevelManualOutput", _tankLevelManualOutput);
                    SetParameter("TankLevelSetpointTracking", _tankLevelSetpointTracking ? 1.0 : 0.0);
                    SetParameter("TankLevelKp", _tankLevelPidController.Kp);
                    SetParameter("TankLevelKi", _tankLevelPidController.Ki);
                    SetParameter("TankLevelKd", _tankLevelPidController.Kd);
                    SetParameter("TankLevelCrossSectionArea", _tankLevelCrossSectionArea);
                    SetParameter("TankLevelOutflowRate", _tankLevelOutflowRate);
                    SetParameter("TankLevelMaxLevel", _tankLevelMaxLevel);
                    SetParameter("TankLevelOutputMin", _tankLevelOutputMin);
                    SetParameter("TankLevelOutputMax", _tankLevelOutputMax);
                    SetParameter("SimulationSpeedMultiplier", _simulationSpeedMultiplier);
                    
                    System.Diagnostics.Debug.WriteLine($"=== TankLevelWithPidModel.InitializeModel COMPLETE ===");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"CRITICAL: Failed to initialize TankLevelWithPidModel: {ex.Message}");
                    OnModelError("Failed to initialize TankLevelWithPid model", ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the tank level with PID model for one time step
        /// SINGLE TIMER ARCHITECTURE: Tank model runs directly, no separate timer
        /// </summary>
        protected override void UpdateModel()
        {
            lock (_lockObject)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== SINGLE TIMER: TankLevelWithPidModel.UpdateModel CALL at Time={CurrentTime:F2}s ===");
                    System.Diagnostics.Debug.WriteLine($"BEFORE: SP={_tankLevelSetpoint:F2}%, PV={_tankLevelProcessVariable:F2}%, MV={_tankLevelControllerOutput:F2}%");
                    
                    // Step 1: Tank level dynamics: dH/dt = (Inflow - Outflow + Disturbance) / Area
                    // Convert percentages to flow rates for physics calculation
                    double inflowPercent = _tankLevelControllerOutput;
                    double outflowPercent = _tankLevelOutflowRate;
                    double netFlowPercent = inflowPercent - outflowPercent + _tankLevelDisturbance;
                    
                    // Step 2: Update tank level using simplified percentage-based dynamics
                    // For percentage units, we use a simplified model where 1% inflow change = some level change rate
                    double levelChangeRate = (netFlowPercent / 100.0) * 10.0; // 10 %/min max rate for 100% flow difference
                    double levelChange = levelChangeRate * (TimeStep / 60.0); // Convert to per-second rate
                    double newLevel = _tankLevelProcessVariable + levelChange;
                    
                    System.Diagnostics.Debug.WriteLine($"Tank dynamics: inflow={inflowPercent:F2}%, outflow={outflowPercent:F2}%, netFlow={netFlowPercent:F2}%, levelChange={levelChange:F4}%");
                    
                    TankLevelProcessVariable = newLevel;
                    System.Diagnostics.Debug.WriteLine($"Tank PV updated to: {_tankLevelProcessVariable:F2}%");
                    
                    // Step 3: Handle setpoint tracking in manual mode
                    if (!_tankLevelClosedLoop && _tankLevelSetpointTracking)
                    {
                        // In manual mode with tracking enabled, setpoint follows PV
                        double oldSetpoint = _tankLevelSetpoint;
                        _tankLevelSetpoint = _tankLevelProcessVariable;
                        _tankLevelPidController.Setpoint = _tankLevelSetpoint;
                        
                        // Only fire events if setpoint actually changed
                        if (Math.Abs(oldSetpoint - _tankLevelSetpoint) > double.Epsilon)
                        {
                            OnPropertyChanged(nameof(TankLevelSetpoint));
                            OnParameterChanged(nameof(TankLevelSetpoint), oldSetpoint, _tankLevelSetpoint);
                        }
                    }
                    
                    // Step 4: Update controller with current process variable to get new output
                    System.Diagnostics.Debug.WriteLine($"Calling Tank PID Update with PV={_tankLevelProcessVariable:F2}%, SP={_tankLevelPidController.Setpoint:F2}%");
                    
                    double newControllerOutput = _tankLevelPidController.Update(_tankLevelProcessVariable, TimeStep);
                    
                    System.Diagnostics.Debug.WriteLine($"Tank PID returned newMV = {newControllerOutput:F2}%");
                    
                    TankLevelControllerOutput = newControllerOutput;
                    System.Diagnostics.Debug.WriteLine($"Tank Controller Output now = {_tankLevelControllerOutput:F2}%");
                    
                    // CRITICAL FIX: In Auto mode, continuously sync manual output for bumpless transfer
                    if (_tankLevelClosedLoop)
                    {
                        _tankLevelManualOutput = _tankLevelControllerOutput;
                    }
                    
                    // Update parameter storage
                    SetParameter("TankLevelProcessVariable", _tankLevelProcessVariable);
                    SetParameter("TankLevelControllerOutput", _tankLevelControllerOutput);
                    SetParameter("TankLevelError", TankLevelError);
                    SetParameter("TankLevelSetpoint", _tankLevelSetpoint);
                    SetParameter("TankLevelManualOutput", _tankLevelManualOutput);
                    SetParameter("SimulationSpeedMultiplier", _simulationSpeedMultiplier);
                    
                    System.Diagnostics.Debug.WriteLine($"AFTER: SP={_tankLevelSetpoint:F2}%, PV={_tankLevelProcessVariable:F2}%, MV={_tankLevelControllerOutput:F2}%, Error={TankLevelError:F2}%");
                    System.Diagnostics.Debug.WriteLine($"=== SINGLE TIMER: TankLevelWithPidModel.UpdateModel END ===\n");
                }
                catch (Exception ex)
                {
                    OnModelError("Error updating TankLevelWithPid model", ex);
                    System.Diagnostics.Debug.WriteLine($"ERROR in Tank UpdateModel: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Gets output value for parameter name
        /// </summary>
        protected override double GetOutputValue(string parameterName)
        {
            return parameterName.ToUpperInvariant() switch
            {
                "TANKLEVELSETPOINT" or "TLSETPOINT" or "SETPOINT" => TankLevelSetpoint,
                "TANKLEVELPROCESSVARIABLE" or "TLPROCESSVARIABLE" or "PV" => TankLevelProcessVariable,
                "TANKLEVELCONTROLLEROUTPUT" or "TLCONTROLLEROUTPUT" or "MV" => TankLevelControllerOutput,
                "TANKLEVELERROR" or "TLERROR" or "ERROR" => TankLevelError,
                "TANKLEVELDISTURBANCE" or "TLDISTURBANCE" or "DISTURBANCE" => TankLevelDisturbance,
                "TANKLEVELCLOSEDLOOP" or "TLCLOSEDLOOP" or "CLOSEDLOOP" => TankLevelClosedLoop ? 1.0 : 0.0,
                "TANKLEVELMANUAKOUTPUT" or "TLMANUALOUTPUT" or "MANUALOUTPUT" => TankLevelManualOutput,
                "TANKLEVELSETPOINTTRACKING" or "TLSETPOINTTRACKING" or "SETPOINTTRACKING" => TankLevelSetpointTracking ? 1.0 : 0.0,
                "TANKLELKP" or "TLKP" or "KP" => TankLevelKp,
                "TANKLELKI" or "TLKI" or "KI" => TankLevelKi,
                "TANKLELKD" or "TLKD" or "KD" => TankLevelKd,
                "TANKLEVELCROSSSECTIONAREA" or "TLAREA" or "AREA" => TankLevelCrossSectionArea,
                "TANKLEVELOUTFLOWRATE" or "TLOUTFLOW" or "OUTFLOW" => TankLevelOutflowRate,
                "TANKLEVELMAXLEVEL" or "TLMAXLEVEL" or "MAXLEVEL" => TankLevelMaxLevel,
                "SIMULATIONSPD" or "SIMULATIONMULTIPLIER" or "SPEEDMULTIPLIER" => SimulationSpeedMultiplier,
                _ => GetParameter(parameterName)
            };
        }

        /// <summary>
        /// Handles input parameter changes
        /// </summary>
        protected override void OnInputChanged(string parameterName, double newValue)
        {
            switch (parameterName.ToUpperInvariant())
            {
                case "TANKLEVELSETPOINT":
                case "TLSETPOINT":
                case "SP":
                    TankLevelSetpoint = newValue;
                    break;
                
                case "TANKLEVELDISTURBANCE":
                case "TLDISTURBANCE":
                case "DISTURBANCE":
                    TankLevelDisturbance = newValue;
                    break;
                
                case "TANKLEVELCLOSEDLOOP":
                case "TLCLOSEDLOOP":
                case "CLOSEDLOOP":
                    TankLevelClosedLoop = newValue > 0.5;
                    break;
                
                case "TANKLEVELMANUAKOUTPUT":
                case "TLMANUALOUTPUT":
                case "MANUALOUTPUT":
                    TankLevelManualOutput = newValue;
                    break;
                
                case "TANKLEVELSETPOINTTRACKING":
                case "TLSETPOINTTRACKING":
                case "SETPOINTTRACKING":
                    TankLevelSetpointTracking = newValue > 0.5;
                    break;
                
                case "TANKLELKP":
                case "TLKP":
                case "KP":
                    TankLevelKp = newValue;
                    break;
                
                case "TANKLELKI":
                case "TLKI":
                case "KI":
                    TankLevelKi = newValue;
                    break;
                
                case "TANKLELKD":
                case "TLKD":
                case "KD":
                    TankLevelKd = newValue;
                    break;
                
                case "TANKLEVELCROSSSECTIONAREA":
                case "TLAREA":
                case "AREA":
                    TankLevelCrossSectionArea = newValue;
                    break;
                
                case "TANKLEVELOUTFLOWRATE":
                case "TLOUTFLOW":
                case "OUTFLOW":
                    TankLevelOutflowRate = newValue;
                    break;
                
                case "TANKLEVELMAXLEVEL":
                case "TLMAXLEVEL":
                case "MAXLEVEL":
                    TankLevelMaxLevel = newValue;
                    break;
                
                case "SIMULATIONSPD":
                case "SIMULATIONMULTIPLIER":
                case "SPEEDMULTIPLIER":
                    SimulationSpeedMultiplier = newValue;
                    break;
            }
        }

        #endregion

        #region Dispose Override

        /// <summary>
        /// Releases resources
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _tankLevelPidController?.Dispose();
                _disposed = true;
            }
            
            base.Dispose(disposing);
        }

        #endregion

        #region Model Control Overrides

        /// <summary>
        /// Called when the model is started
        /// </summary>
        protected override void OnModelStarted()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"TankLevelWithPidModel started with single timer architecture, speed={_simulationSpeedMultiplier:F1}x");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Tank OnModelStarted: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when the model is stopped
        /// </summary>
        protected override void OnModelStopped()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"TankLevelWithPidModel stopped");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Tank OnModelStopped: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when the model is reset
        /// </summary>
        protected override void OnModelReset()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Resetting TankLevelWithPidModel");
                
                // Reset simulation speed to default
                _simulationSpeedMultiplier = 1.0;
                TimeStep = 1.0; // 1x speed = 1 second timestep
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error resetting TankLevel: {ex.Message}");
            }
        }

        #endregion
    }
}