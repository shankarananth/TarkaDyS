/*
 * File: FirstOrderWithPidModel.cs
 * Author: Shankar Ananth Asokan
 * Purpose: First-order process with PID control system simulation model
 * Date Created: 2024-01-XX
 * Date Modified: 2024-01-XX
 * 
 * Description: Represents a first-order process with PID control system.
 * This model simulates a closed-loop control system where the PID controller automatically
 * adjusts the first-order process input to maintain the process output at the desired setpoint.
 * 
 * Modifications:
 * - 2024-01-XX: Fixed steady state initialization - MV now starts at 50.0 for proper steady state
 * - 2024-01-XX: Added multiple PID controller types support
 * - 2024-01-XX: Added output limits with anti-reset windup
 */

using TarkaDyS.Core.Controllers;
using TarkaDyS.Core.Models;
using TarkaDyS.Core.Interfaces;
using TarkaDyS.Core.Logging;
using System;

namespace TarkaDyS.ProcessModels
{
    /// <summary>
    /// Represents a first-order process with PID control system.
    /// This model simulates a closed-loop control system where the PID controller automatically
    /// adjusts the first-order process input to maintain the process output at the desired setpoint.
    /// This is a specific implementation combining a first-order process model with PID control.
    /// </summary>
    public class FirstOrderWithPidModel : ProcessModelBase
    {
        #region Private Fields
        private readonly object _lockObject = new object();
        private readonly FirstOrderProcessModel _firstOrderProcess;
        private readonly PidController _pidController;
        private bool _disposed = false;
        
        // Control loop variables - FIXED: Initialize to steady state values
        private double _firstOrderSetpoint = 50.0;          // Target setpoint (%)
        private double _firstOrderProcessVariable = 50.0;    // Start PV at setpoint (%)
        private double _firstOrderControllerOutput = 50.0;   // Start MV at steady state (%)
        private double _firstOrderDisturbance = 0.0;         // No initial disturbance
        
        // Loop configuration
        private bool _firstOrderClosedLoop = true;
        private double _firstOrderManualOutput = 50.0;      // Initialize to steady state MV
        private bool _firstOrderSetpointTracking = false;
        
        // Simulation speed control (multiplier for real-time)
        private double _simulationSpeedMultiplier = 1.0; // 1.0 = real-time, 2.0 = 2x speed, 0.5 = half speed
        
        // Output limits - Added back per requirements
        private double _firstOrderOutputMin = 0.0;
        private double _firstOrderOutputMax = 100.0;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the setpoint for the first-order control loop (%)
        /// </summary>
        public double FirstOrderSetpoint
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderSetpoint;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    // Apply setpoint limits
                    double clampedValue = Math.Max(0.0, Math.Min(100.0, value));
                    if (Math.Abs(_firstOrderSetpoint - clampedValue) > double.Epsilon)
                    {
                        double oldValue = _firstOrderSetpoint;
                        _firstOrderSetpoint = clampedValue;
                        _pidController.Setpoint = clampedValue;
                        OnPropertyChanged(nameof(FirstOrderSetpoint));
                        OnParameterChanged(nameof(FirstOrderSetpoint), oldValue, clampedValue);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current process variable (first-order process output) (%)
        /// </summary>
        public double FirstOrderProcessVariable
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderProcessVariable;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_firstOrderProcessVariable - value) > double.Epsilon)
                    {
                        _firstOrderProcessVariable = value;
                        OnPropertyChanged(nameof(FirstOrderProcessVariable));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current controller output (manipulated variable) (%)
        /// </summary>
        public double FirstOrderControllerOutput
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderControllerOutput;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_firstOrderControllerOutput - value) > double.Epsilon)
                    {
                        _firstOrderControllerOutput = value;
                        OnPropertyChanged(nameof(FirstOrderControllerOutput));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum output limit (%)
        /// </summary>
        public double FirstOrderOutputMin
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderOutputMin;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_firstOrderOutputMin - value) > double.Epsilon)
                    {
                        double oldValue = _firstOrderOutputMin;
                        _firstOrderOutputMin = value;
                        _pidController.SetOutputLimits(_firstOrderOutputMin, _firstOrderOutputMax);
                        OnPropertyChanged(nameof(FirstOrderOutputMin));
                        OnParameterChanged(nameof(FirstOrderOutputMin), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum output limit (%)
        /// </summary>
        public double FirstOrderOutputMax
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderOutputMax;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_firstOrderOutputMax - value) > double.Epsilon)
                    {
                        double oldValue = _firstOrderOutputMax;
                        _firstOrderOutputMax = value;
                        _pidController.SetOutputLimits(_firstOrderOutputMin, _firstOrderOutputMax);
                        OnPropertyChanged(nameof(FirstOrderOutputMax));
                        OnParameterChanged(nameof(FirstOrderOutputMax), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the control loop is closed (automatic) or open (manual)
        /// </summary>
        public bool FirstOrderClosedLoop
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderClosedLoop;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_firstOrderClosedLoop != value)
                    {
                        _firstOrderClosedLoop = value;
                        _pidController.AutoMode = value;
                        
                        // When switching to manual mode, sync manual output with current controller output
                        if (!value)
                        {
                            _firstOrderManualOutput = _firstOrderControllerOutput;
                            _pidController.ManualOutput = _firstOrderManualOutput;
                        }
                        
                        OnPropertyChanged(nameof(FirstOrderClosedLoop));
                        OnParameterChanged(nameof(FirstOrderClosedLoop), _firstOrderClosedLoop ? 0 : 1, value ? 1 : 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the manual output value (used when FirstOrderClosedLoop is false)
        /// </summary>
        public double FirstOrderManualOutput
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderManualOutput;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_firstOrderManualOutput - value) > double.Epsilon)
                    {
                        double oldValue = _firstOrderManualOutput;
                        _firstOrderManualOutput = value;
                        _pidController.ManualOutput = value;
                        
                        // If in manual mode, update the controller output immediately
                        if (!_firstOrderClosedLoop)
                        {
                            FirstOrderControllerOutput = value;
                        }
                        
                        OnPropertyChanged(nameof(FirstOrderManualOutput));
                        OnParameterChanged(nameof(FirstOrderManualOutput), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether setpoint should track the process variable when in manual mode
        /// </summary>
        public bool FirstOrderSetpointTracking
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderSetpointTracking;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_firstOrderSetpointTracking != value)
                    {
                        _firstOrderSetpointTracking = value;
                        OnPropertyChanged(nameof(FirstOrderSetpointTracking));
                        OnParameterChanged(nameof(FirstOrderSetpointTracking), _firstOrderSetpointTracking ? 0 : 1, value ? 1 : 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the external disturbance applied to the first-order process
        /// </summary>
        public double FirstOrderDisturbance
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderDisturbance;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_firstOrderDisturbance - value) > double.Epsilon)
                    {
                        double oldValue = _firstOrderDisturbance;
                        _firstOrderDisturbance = value;
                        _firstOrderProcess.Disturbance = value;
                        OnPropertyChanged(nameof(FirstOrderDisturbance));
                        OnParameterChanged(nameof(FirstOrderDisturbance), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current control error (FirstOrderSetpoint - FirstOrderProcessVariable)
        /// </summary>
        public double FirstOrderError
        {
            get
            {
                lock (_lockObject)
                {
                    return _firstOrderSetpoint - _firstOrderProcessVariable;
                }
            }
        }

        /// <summary>
        /// Gets or sets the proportional gain of the PID controller
        /// </summary>
        public double FirstOrderKp
        {
            get => _pidController.Kp;
            set => _pidController.Kp = value;
        }

        /// <summary>
        /// Gets or sets the integral gain of the PID controller
        /// </summary>
        public double FirstOrderKi
        {
            get => _pidController.Ki;
            set => _pidController.Ki = value;
        }

        /// <summary>
        /// Gets or sets the derivative gain of the PID controller
        /// </summary>
        public double FirstOrderKd
        {
            get => _pidController.Kd;
            set => _pidController.Kd = value;
        }

        /// <summary>
        /// Gets or sets the process gain
        /// </summary>
        public double FirstOrderProcessGain
        {
            get => _firstOrderProcess.ProcessGain;
            set => _firstOrderProcess.ProcessGain = value;
        }

        /// <summary>
        /// Gets or sets the process time constant
        /// </summary>
        public double FirstOrderTimeConstant
        {
            get => _firstOrderProcess.TimeConstant;
            set => _firstOrderProcess.TimeConstant = value;
        }

        /// <summary>
        /// Gets or sets the process dead time
        /// </summary>
        public double FirstOrderDeadTime
        {
            get => _firstOrderProcess.DeadTime;
            set => _firstOrderProcess.DeadTime = value;
        }

        /// <summary>
        /// Gets the PID controller instance for advanced configuration
        /// </summary>
        public PidController FirstOrderController => _pidController;

        /// <summary>
        /// Gets the process model instance for advanced configuration
        /// </summary>
        public FirstOrderProcessModel FirstOrderProcess => _firstOrderProcess;

        /// <summary>
        /// Gets or sets the simulation speed multiplier (1.0 = real-time, 2.0 = 2x speed, etc.)
        /// Real-time (1x) = 0.1 second timestep, 10x = 0.01 second timestep
        /// FIXED: Better timestep calculation to prevent computational overload
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
                    // FIXED: Clamp between 0.1x and 5x speed (reduced max to prevent overload)
                    double clampedValue = Math.Max(0.1, Math.Min(5.0, value));
                    if (Math.Abs(_simulationSpeedMultiplier - clampedValue) > double.Epsilon)
                    {
                        double oldValue = _simulationSpeedMultiplier;
                        _simulationSpeedMultiplier = clampedValue;
                        
                        // FIXED: Better timestep calculation - base 0.1s instead of 1.0s
                        // This prevents extremely small timesteps that can cause numerical issues
                        double baseTimeStep = 0.1; // Base timestep of 100ms
                        double newTimeStep = baseTimeStep / clampedValue;
                        
                        // FIXED: Ensure minimum timestep of 20ms to prevent excessive CPU usage
                        newTimeStep = Math.Max(0.02, newTimeStep);
                        
                        TimeStep = newTimeStep;
                        _firstOrderProcess.TimeStep = newTimeStep;
                        
                        OnPropertyChanged(nameof(SimulationSpeedMultiplier));
                        OnParameterChanged(nameof(SimulationSpeedMultiplier), oldValue, clampedValue);
                        
                        System.Diagnostics.Debug.WriteLine($"FIXED Speed: {clampedValue:F1}x, TimeStep = {newTimeStep:F3}s (min 20ms enforced)");
                    }
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FirstOrderWithPidModel class
        /// </summary>
        /// <param name="modelId">Unique identifier for the model</param>
        /// <param name="modelName">Display name for the model</param>
        public FirstOrderWithPidModel(string modelId, string modelName) 
            : base(modelId, modelName)
        {
            // Create process and controller instances
            _firstOrderProcess = new FirstOrderProcessModel($"{modelId}_Process", $"{modelName} Process");
            _pidController = new PidController($"{modelId}_Controller", $"{modelName} Controller");
            
            // Set default PID parameters (typical starting values)
            _pidController.SetTuning(1.0, 0.1, 0.05);
            _pidController.SetOutputLimits(0.0, 100.0);
            
            // Set default process parameters
            _firstOrderProcess.SetProcessParameters(1.0, 10.0, 1.0);
            
            // Wire up events
            _pidController.ParameterChanged += (sender, e) => OnParameterChanged(e.ParameterName, e.OldValue, e.NewValue);
            _firstOrderProcess.ParameterChanged += (sender, e) => OnParameterChanged(e.ParameterName, e.OldValue, e.NewValue);
            _firstOrderProcess.ModelError += (sender, e) => OnModelError($"First-order process model error: {e.ErrorMessage}", e.Exception);
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Initializes the PID process model with default parameters
        /// EMERGENCY FIX: Minimal logging to prevent startup freezing
        /// FIXED: Proper steady state initialization with MV=50 for steady state
        /// </summary>
        protected override void InitializeModel()
        {
            lock (_lockObject)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"=== FirstOrderWithPidModel.InitializeModel START ===");
                    
                    // FIXED: Start in proper steady state - all values at 50% for unity gain process
                    _firstOrderSetpoint = 50.0;        // Target setpoint (%)
                    _firstOrderProcessVariable = 50.0;  // Start PV at setpoint (steady state)
                    _firstOrderControllerOutput = 50.0; // FIXED: MV starts at 50% for steady state with gain=1
                    _firstOrderDisturbance = 0.0;       // No initial disturbance
                    _firstOrderClosedLoop = true;       // Start in automatic mode
                    _firstOrderManualOutput = 50.0;     // FIXED: Manual output also at steady state value
                    _firstOrderSetpointTracking = false;
                    _simulationSpeedMultiplier = 1.0;   // Initialize to real-time
                    
                    System.Diagnostics.Debug.WriteLine($"Initial steady state values: SP={_firstOrderSetpoint}%, PV={_firstOrderProcessVariable}%, MV={_firstOrderControllerOutput}%");
                    
                    // Set initial controller parameters for steady state
                    _pidController.Setpoint = _firstOrderSetpoint;
                    _pidController.AutoMode = _firstOrderClosedLoop;
                    _pidController.ManualOutput = _firstOrderManualOutput;
                    
                    // FIXED: Set output limits to proper % range with anti-reset windup
                    _pidController.SetOutputLimits(_firstOrderOutputMin, _firstOrderOutputMax);
                    
                    System.Diagnostics.Debug.WriteLine($"Controller configured: SP={_pidController.Setpoint}%, AutoMode={_pidController.AutoMode}, Limits=[{_firstOrderOutputMin}%-{_firstOrderOutputMax}%]");
                    
                    // Set process to steady state BEFORE initializing it
                    // For unity gain process: if MV=50%, then PV should be 50% at steady state
                    _firstOrderProcess.ProcessInput = _firstOrderControllerOutput;
                    _firstOrderProcess.ProcessOutput = _firstOrderProcessVariable;
                    
                    System.Diagnostics.Debug.WriteLine($"Process initialized with steady state: Input={_firstOrderProcess.ProcessInput}%, Output={_firstOrderProcess.ProcessOutput}%");
                    
                    // Initialize process - NO TIMER START! Process is function-based now
                    _firstOrderProcess.Initialize();
                    
                    System.Diagnostics.Debug.WriteLine($"Process initialized as function-based model (no independent timer)");
                    
                    // Initialize controller 
                    _pidController.Initialize();
                    
                    System.Diagnostics.Debug.WriteLine($"After Initialize: Process Input={_firstOrderProcess.ProcessInput}%, Output={_firstOrderProcess.ProcessOutput}%");
                    System.Diagnostics.Debug.WriteLine($"After Initialize: Controller Output={_pidController.Output}%");
                    
                    // Force controller to steady state by doing an update with zero error
                    _pidController.Update(_firstOrderProcessVariable, 0.1);
                    
                    // FIXED: Force controller output to proper steady state value
                    _firstOrderControllerOutput = 50.0;
                    
                    // Ensure process gets the correct steady state input
                    _firstOrderProcess.ProcessInput = _firstOrderControllerOutput;
                    _firstOrderProcess.ProcessOutput = _firstOrderProcessVariable;
                    
                    System.Diagnostics.Debug.WriteLine($"FORCED steady state: SP={_firstOrderSetpoint}%, PV={_firstOrderProcessVariable}%, MV={_firstOrderControllerOutput}%");
                    
                    System.Diagnostics.Debug.WriteLine($"FirstOrderWithPid Initialized: SP={_firstOrderSetpoint}%, PV={_firstOrderProcessVariable:F2}%, MV={_firstOrderControllerOutput}%, Error={_firstOrderSetpoint - _firstOrderProcessVariable:F2}%");
                    
                    // Initialize model parameters
                    SetParameter("FirstOrderSetpoint", _firstOrderSetpoint);
                    SetParameter("FirstOrderProcessVariable", _firstOrderProcessVariable);
                    SetParameter("FirstOrderControllerOutput", _firstOrderControllerOutput);
                    SetParameter("FirstOrderDisturbance", _firstOrderDisturbance);
                    SetParameter("FirstOrderClosedLoop", _firstOrderClosedLoop ? 1.0 : 0.0);
                    SetParameter("FirstOrderManualOutput", _firstOrderManualOutput);
                    SetParameter("FirstOrderSetpointTracking", _firstOrderSetpointTracking ? 1.0 : 0.0);
                    SetParameter("FirstOrderKp", _pidController.Kp);
                    SetParameter("FirstOrderKi", _pidController.Ki);
                    SetParameter("FirstOrderKd", _pidController.Kd);
                    SetParameter("FirstOrderProcessGain", _firstOrderProcess.ProcessGain);
                    SetParameter("FirstOrderTimeConstant", _firstOrderProcess.TimeConstant);
                    SetParameter("FirstOrderDeadTime", _firstOrderProcess.DeadTime);
                    SetParameter("FirstOrderOutputMin", _firstOrderOutputMin);
                    SetParameter("FirstOrderOutputMax", _firstOrderOutputMax);
                    SetParameter("SimulationSpeedMultiplier", _simulationSpeedMultiplier);
                    
                    System.Diagnostics.Debug.WriteLine($"=== FirstOrderWithPidModel.InitializeModel COMPLETE ===");
                }
                catch (Exception ex)
                {
                    // EMERGENCY: Only critical error logging to debug output
                    System.Diagnostics.Debug.WriteLine($"CRITICAL: Failed to initialize FirstOrderWithPidModel: {ex.Message}");
                    OnModelError("Failed to initialize FirstOrderWithPid model", ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the first-order with PID model for one time step
        /// SINGLE TIMER ARCHITECTURE: Process model is called as function, not separate timer
        /// EMERGENCY FIX: Disable all logging to prevent mouse-movement freezing
        /// </summary>
        protected override void UpdateModel()
        {
            lock (_lockObject)
            {
                try
                {
                    // EMERGENCY FIX: Completely disable logging during UpdateModel
                    // Mouse movement triggers UI events that compete with logging I/O
                    

                    // Step 1: Apply current controller output to process input
                    _firstOrderProcess.ProcessInput = _firstOrderControllerOutput;
                    
                    // Step 2: CALL PROCESS FUNCTION (no separate timer!)
                    _firstOrderProcess.UpdateProcessModel(); // Function call, not timer-based!
                    double newPV = _firstOrderProcess.ProcessOutput;
                    
                    // CRITICAL: Update our local copy
                    FirstOrderProcessVariable = newPV;
                    
                    // Step 3: Handle setpoint tracking in manual mode
                    if (!_firstOrderClosedLoop && _firstOrderSetpointTracking)
                    {
                        // In manual mode with tracking enabled, setpoint follows PV
                        double oldSetpoint = _firstOrderSetpoint;
                        _firstOrderSetpoint = _firstOrderProcessVariable;
                        _pidController.Setpoint = _firstOrderSetpoint;
                        
                        // Only fire events if setpoint actually changed
                        if (Math.Abs(oldSetpoint - _firstOrderSetpoint) > double.Epsilon)
                        {
                            OnPropertyChanged(nameof(FirstOrderSetpoint));
                            OnParameterChanged(nameof(FirstOrderSetpoint), oldSetpoint, _firstOrderSetpoint);
                        }
                    }
                    
                    // Step 4: Update controller with current process variable to get new output
                    double newControllerOutput = _pidController.Update(_firstOrderProcessVariable, TimeStep);
                    FirstOrderControllerOutput = newControllerOutput;
                    
                    // CRITICAL FIX: In Auto mode, continuously sync manual output for bumpless transfer
                    if (_firstOrderClosedLoop)
                    {
                        _firstOrderManualOutput = _firstOrderControllerOutput;
                    }
                    
                    // Update parameter storage
                    SetParameter("FirstOrderProcessVariable", _firstOrderProcessVariable);
                    SetParameter("FirstOrderControllerOutput", _firstOrderControllerOutput);
                    SetParameter("FirstOrderError", FirstOrderError);
                    SetParameter("FirstOrderSetpoint", _firstOrderSetpoint);
                    SetParameter("FirstOrderManualOutput", _firstOrderManualOutput); // Keep this updated
                    SetParameter("SimulationSpeedMultiplier", _simulationSpeedMultiplier);
                }
                catch (Exception ex)
                {
                    // EMERGENCY: Only log critical errors to debug output, no file I/O
                    System.Diagnostics.Debug.WriteLine($"CRITICAL ERROR in UpdateModel: {ex.Message}");
                    OnModelError("Error updating FirstOrderWithPid model", ex);
                }
            }
        }

        /// <summary>
        /// Gets the output value for the specified parameter name
        /// </summary>
        protected override double GetOutputValue(string parameterName)
        {
            return parameterName.ToUpperInvariant() switch
            {
                "FIRSTORDERSETPOINT" or "FOSETPOINT" or "SP" => FirstOrderSetpoint,
                "FIRSTORDERPROCESSVARIABLE" or "FOPROCESSVARIABLE" or "PV" or "OUTPUT" => FirstOrderProcessVariable,
                "FIRSTORDERCONTROLLEROUTPUT" or "FOCONTROLLEROUTPUT" or "MV" => FirstOrderControllerOutput,
                "FIRSTORDERERROR" or "FOERROR" or "ERROR" => FirstOrderError,
                "FIRSTORDERDISTURBANCE" or "FODISTURBANCE" or "DISTURBANCE" => FirstOrderDisturbance,
                "FIRSTORDERCLOSEDLOOP" or "FOCLOSEDLOOP" or "CLOSEDLOOP" => FirstOrderClosedLoop ? 1.0 : 0.0,
                "FIRSTORDERMANUALOUTPUT" or "FOMANUALOUTPUT" or "MANUALOUTPUT" => FirstOrderManualOutput,
                "FIRSTORDERSETPOINTTRACKING" or "FOSETPOINTTRACKING" or "SETPOINTTRACKING" => FirstOrderSetpointTracking ? 1.0 : 0.0,
                "FIRSTORDERKP" or "FOKP" or "KP" => FirstOrderKp,
                "FIRSTORDERKI" or "FOKI" or "KI" => FirstOrderKi,
                "FIRSTORDERKD" or "FOKD" or "KD" => FirstOrderKd,
                "FIRSTORDERPROCESSGAIN" or "FOPROCESSGAIN" or "PROCESSGAIN" or "GAIN" => FirstOrderProcessGain,
                "FIRSTORDERTIMECONSTANT" or "FOTIMECONSTANT" or "TIMECONSTANT" or "TAU" => FirstOrderTimeConstant,
                "FIRSTORDERDEADTIME" or "FODEADTIME" or "DEADTIME" or "DELAY" => FirstOrderDeadTime,
                "SIMULATIONSPD" or "SIMULATIONMULTIPLIER" or "SPEEDMULTIPLIER" => SimulationSpeedMultiplier,
                _ => GetParameter(parameterName)
            };
        }

        /// <summary>
        /// Called when an input parameter changes
        /// </summary>
        protected override void OnInputChanged(string parameterName, double newValue)
        {
            switch (parameterName.ToUpperInvariant())
            {
                case "FIRSTORDERSETPOINT":
                case "FOSETPOINT":
                case "SP":
                    FirstOrderSetpoint = newValue;
                    break;
                
                case "FIRSTORDERDISTURBANCE":
                case "FODISTURBANCE":
                case "DISTURBANCE":
                    FirstOrderDisturbance = newValue;
                    break;
                
                case "FIRSTORDERCLOSEDLOOP":
                case "FOCLOSEDLOOP":
                case "CLOSEDLOOP":
                    FirstOrderClosedLoop = newValue > 0.5;
                    break;
                
                case "FIRSTORDERMANUALOUTPUT":
                case "FOMANUALOUTPUT":
                case "MANUALOUTPUT":
                    FirstOrderManualOutput = newValue;
                    break;
                
                case "FIRSTORDERSETPOINTTRACKING":
                case "FOSETPOINTTRACKING":
                case "SETPOINTTRACKING":
                    FirstOrderSetpointTracking = newValue > 0.5;
                    break;
                
                case "FIRSTORDERKP":
                case "FOKP":
                case "KP":
                    FirstOrderKp = newValue;
                    break;
                
                case "FIRSTORDERKI":
                case "FOKI":
                case "KI":
                    FirstOrderKi = newValue;
                    break;
                
                case "FIRSTORDERKD":
                case "FOKD":
                case "KD":
                    FirstOrderKd = newValue;
                    break;
                
                case "FIRSTORDERPROCESSGAIN":
                case "FOPROCESSGAIN":
                case "PROCESSGAIN":
                case "GAIN":
                    FirstOrderProcessGain = newValue;
                    break;
                
                case "FIRSTORDERTIMECONSTANT":
                case "FOTIMECONSTANT":
                case "TIMECONSTANT":
                case "TAU":
                    FirstOrderTimeConstant = newValue;
                    break;
                
                case "FIRSTORDERDEADTIME":
                case "FODEADTIME":
                case "DEADTIME":
                case "DELAY":
                    FirstOrderDeadTime = newValue;
                    break;
                
                case "SIMULATIONSPD":
                case "SIMULATIONMULTIPLIER":
                case "SPEEDMULTIPLIER":
                    SimulationSpeedMultiplier = newValue;
                    break;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the PID controller parameters for first-order process
        /// </summary>
        public void SetFirstOrderPidParameters(double kp, double ki, double kd)
        {
            _pidController.SetTuning(kp, ki, kd);
        }

        /// <summary>
        /// Sets the first-order process model parameters
        /// </summary>
        public void SetFirstOrderProcessParameters(double processGain, double timeConstant, double deadTime)
        {
            _firstOrderProcess.SetProcessParameters(processGain, timeConstant, deadTime);
        }

        /// <summary>
        /// Sets the controller output limits for first-order process
        /// </summary>
        public void SetFirstOrderOutputLimits(double min, double max)
        {
            _pidController.SetOutputLimits(min, max);
        }

        #endregion

        #region Dispose Override

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _pidController?.Dispose();
                _firstOrderProcess?.Dispose();
                _disposed = true;
            }
            
            base.Dispose(disposing);
        }

        #endregion

        #region Model Control Overrides

        /// <summary>
        /// Called when the model is started
        /// EMERGENCY FIX: No logging to prevent freezing
        /// </summary>
        protected override void OnModelStarted()
        {
            try
            {
                // No need to start process model - it's function-based now!
                System.Diagnostics.Debug.WriteLine($"FirstOrderWithPidModel started with single timer architecture");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnModelStarted: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when the model is stopped
        /// EMERGENCY FIX: No logging to prevent freezing
        /// </summary>
        protected override void OnModelStopped()
        {
            try
            {
                // No need to stop process model - it's function-based now!
                System.Diagnostics.Debug.WriteLine($"FirstOrderWithPidModel stopped");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnModelStopped: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when the model is reset
        /// EMERGENCY FIX: No logging to prevent freezing
        /// </summary>
        protected override void OnModelReset()
        {
            try
            {
                // Reset the process model (no timer involved)
                System.Diagnostics.Debug.WriteLine($"Resetting FirstOrderProcess from OnModelReset");
                _firstOrderProcess.Reset();
                
                // Reset simulation speed to default
                _simulationSpeedMultiplier = 1.0;
                TimeStep = 0.1; // FIXED: Use 0.1s default timestep instead of 1.0s
                _firstOrderProcess.TimeStep = 0.1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error resetting FirstOrderProcess: {ex.Message}");
            }
        }

        #endregion
    }
}