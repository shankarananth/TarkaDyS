/*
 * File: TemperatureWithPidModel.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Industrial heat exchanger temperature control system with PID control
 * Date Created: 2024-01-XX
 * Date Modified: 2024-01-XX
 * 
 * Description: Represents an industrial heat exchanger temperature control system with PID control.
 * This model simulates a heat exchanger where the PID controller automatically adjusts the 
 * heating/cooling power to maintain the outlet temperature at the desired setpoint.
 * Common in chemical processing, HVAC systems, and industrial heating applications.
 * 
 * Modifications:
 * - 2024-01-XX: Initial creation based on industrial heat exchanger dynamics
 * - 2024-01-XX: Added proper steady state initialization
 * - 2024-01-XX: Added enhanced PID controller with multiple types
 * - 2024-01-XX: Added output limits with anti-reset windup
 */

using TarkaDyS.Core.Controllers;
using TarkaDyS.Core.Models;
using TarkaDyS.Core.Interfaces;
using System;

namespace TarkaDyS.ProcessModels
{
    /// <summary>
    /// Represents an industrial heat exchanger temperature control system with PID control.
    /// This model simulates a temperature control process where the PID controller automatically
    /// adjusts the heater/cooler power to maintain the temperature at the desired setpoint.
    /// Used in chemical processing, HVAC systems, and industrial heating applications.
    /// </summary>
    public class TemperatureWithPidModel : ProcessModelBase
    {
        #region Private Fields
        private readonly object _lockObject = new object();
        private readonly EnhancedPidController _temperaturePidController;
        private bool _disposed = false;
        
        // Temperature control variables - Proper steady state initialization
        private double _temperatureSetpoint = 50.0;            // Desired temperature (%)
        private double _temperatureProcessVariable = 50.0;     // Current temperature (%) - start at setpoint
        private double _temperatureControllerOutput = 50.0;    // Heater/cooler power (%) - start at steady state
        private double _temperatureDisturbance = 0.0;          // External temperature disturbance (%)
        
        // Heat exchanger parameters (industrial scale)
        private double _temperatureMass = 1000.0;              // Mass of fluid being heated/cooled (kg)
        private double _temperatureSpecificHeat = 4200.0;      // Specific heat capacity (J/kg°K) - water
        private double _temperatureHeatTransferCoeff = 500.0;  // Heat transfer coefficient (W/K) - typical for industrial HX
        private double _temperatureAmbientTemp = 25.0;         // Ambient temperature (°C)
        private double _temperatureMaxPower = 50000.0;         // Maximum heater/cooler power (W) - 50kW industrial
        
        // Temperature scale conversion (for percentage units)
        private double _temperatureMinScale = 0.0;             // Minimum temperature (°C)
        private double _temperatureMaxScale = 100.0;           // Maximum temperature (°C)
        
        // Loop configuration
        private bool _temperatureClosedLoop = true;
        private double _temperatureManualOutput = 50.0;        // Start at steady state
        private bool _temperatureSetpointTracking = false;
        
        // Simulation speed control
        private double _simulationSpeedMultiplier = 1.0;       // 1.0 = real-time
        
        // Output limits - Added per requirements
        private double _temperatureOutputMin = 0.0;            // Minimum output (%)
        private double _temperatureOutputMax = 100.0;          // Maximum output (%)
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the temperature setpoint (%)
        /// </summary>
        public double TemperatureSetpoint
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureSetpoint;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    // Apply setpoint limits (0-100%)
                    double clampedValue = Math.Max(0.0, Math.Min(100.0, value));
                    if (Math.Abs(_temperatureSetpoint - clampedValue) > double.Epsilon)
                    {
                        double oldValue = _temperatureSetpoint;
                        _temperatureSetpoint = clampedValue;
                        _temperaturePidController.Setpoint = clampedValue;
                        OnPropertyChanged(nameof(TemperatureSetpoint));
                        OnParameterChanged(nameof(TemperatureSetpoint), oldValue, clampedValue);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current temperature (%)
        /// </summary>
        public double TemperatureProcessVariable
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureProcessVariable;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_temperatureProcessVariable - value) > double.Epsilon)
                    {
                        // Clamp to 0-100% range
                        _temperatureProcessVariable = Math.Max(0.0, Math.Min(100.0, value));
                        OnPropertyChanged(nameof(TemperatureProcessVariable));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current controller output (heater/cooler power %)
        /// </summary>
        public double TemperatureControllerOutput
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureControllerOutput;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_temperatureControllerOutput - value) > double.Epsilon)
                    {
                        // Apply output limits
                        _temperatureControllerOutput = Math.Max(_temperatureOutputMin, Math.Min(_temperatureOutputMax, value));
                        OnPropertyChanged(nameof(TemperatureControllerOutput));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the control loop is closed (automatic) or open (manual)
        /// </summary>
        public bool TemperatureClosedLoop
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureClosedLoop;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_temperatureClosedLoop != value)
                    {
                        _temperatureClosedLoop = value;
                        _temperaturePidController.AutoMode = value;
                        
                        // When switching to manual mode, sync manual output with current controller output
                        if (!value)
                        {
                            _temperatureManualOutput = _temperatureControllerOutput;
                            _temperaturePidController.ManualOutput = _temperatureManualOutput;
                        }
                        
                        OnPropertyChanged(nameof(TemperatureClosedLoop));
                        OnParameterChanged(nameof(TemperatureClosedLoop), _temperatureClosedLoop ? 0 : 1, value ? 1 : 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the manual output value (used when TemperatureClosedLoop is false)
        /// </summary>
        public double TemperatureManualOutput
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureManualOutput;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_temperatureManualOutput - value) > double.Epsilon)
                    {
                        double oldValue = _temperatureManualOutput;
                        _temperatureManualOutput = value;
                        _temperaturePidController.ManualOutput = value;
                        
                        // If in manual mode, update the controller output immediately
                        if (!_temperatureClosedLoop)
                        {
                            TemperatureControllerOutput = value;
                        }
                        
                        OnPropertyChanged(nameof(TemperatureManualOutput));
                        OnParameterChanged(nameof(TemperatureManualOutput), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether setpoint should track the process variable when in manual mode
        /// </summary>
        public bool TemperatureSetpointTracking
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureSetpointTracking;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_temperatureSetpointTracking != value)
                    {
                        _temperatureSetpointTracking = value;
                        OnPropertyChanged(nameof(TemperatureSetpointTracking));
                        OnParameterChanged(nameof(TemperatureSetpointTracking), _temperatureSetpointTracking ? 0 : 1, value ? 1 : 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the external disturbance applied to the temperature process (%)
        /// </summary>
        public double TemperatureDisturbance
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureDisturbance;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_temperatureDisturbance - value) > double.Epsilon)
                    {
                        double oldValue = _temperatureDisturbance;
                        _temperatureDisturbance = value;
                        OnPropertyChanged(nameof(TemperatureDisturbance));
                        OnParameterChanged(nameof(TemperatureDisturbance), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current control error (TemperatureSetpoint - TemperatureProcessVariable)
        /// </summary>
        public double TemperatureError
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureSetpoint - _temperatureProcessVariable;
                }
            }
        }

        // Heat exchanger parameters
        /// <summary>
        /// Gets or sets the mass of fluid being heated/cooled (kg)
        /// </summary>
        public double TemperatureMass
        {
            get => _temperatureMass;
            set => _temperatureMass = Math.Max(1.0, value);
        }

        /// <summary>
        /// Gets or sets the specific heat capacity (J/kg°K)
        /// </summary>
        public double TemperatureSpecificHeat
        {
            get => _temperatureSpecificHeat;
            set => _temperatureSpecificHeat = Math.Max(100.0, value);
        }

        /// <summary>
        /// Gets or sets the heat transfer coefficient (W/K)
        /// </summary>
        public double TemperatureHeatTransferCoeff
        {
            get => _temperatureHeatTransferCoeff;
            set => _temperatureHeatTransferCoeff = Math.Max(1.0, value);
        }

        /// <summary>
        /// Gets or sets the ambient temperature (°C)
        /// </summary>
        public double TemperatureAmbientTemp
        {
            get => _temperatureAmbientTemp;
            set => _temperatureAmbientTemp = value;
        }

        /// <summary>
        /// Gets or sets the maximum heater/cooler power (W)
        /// </summary>
        public double TemperatureMaxPower
        {
            get => _temperatureMaxPower;
            set => _temperatureMaxPower = Math.Max(1000.0, value);
        }

        // PID parameters with enhanced controller support
        /// <summary>
        /// Gets or sets the proportional gain (Kp)
        /// </summary>
        public double TemperatureKp
        {
            get => _temperaturePidController.Kp;
            set => _temperaturePidController.Kp = value;
        }

        /// <summary>
        /// Gets or sets the integral gain (Ki)
        /// </summary>
        public double TemperatureKi
        {
            get => _temperaturePidController.Ki;
            set => _temperaturePidController.Ki = value;
        }

        /// <summary>
        /// Gets or sets the derivative gain (Kd)
        /// </summary>
        public double TemperatureKd
        {
            get => _temperaturePidController.Kd;
            set => _temperaturePidController.Kd = value;
        }

        /// <summary>
        /// Gets or sets the PID controller algorithm type
        /// </summary>
        public PidControllerType TemperatureControllerType
        {
            get => _temperaturePidController.ControllerType;
            set => _temperaturePidController.ControllerType = value;
        }

        /// <summary>
        /// Gets or sets the minimum output limit (%)
        /// </summary>
        public double TemperatureOutputMin
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureOutputMin;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_temperatureOutputMin - value) > double.Epsilon)
                    {
                        double oldValue = _temperatureOutputMin;
                        _temperatureOutputMin = value;
                        _temperaturePidController.SetOutputLimits(_temperatureOutputMin, _temperatureOutputMax);
                        OnPropertyChanged(nameof(TemperatureOutputMin));
                        OnParameterChanged(nameof(TemperatureOutputMin), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum output limit (%)
        /// </summary>
        public double TemperatureOutputMax
        {
            get
            {
                lock (_lockObject)
                {
                    return _temperatureOutputMax;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_temperatureOutputMax - value) > double.Epsilon)
                    {
                        double oldValue = _temperatureOutputMax;
                        _temperatureOutputMax = value;
                        _temperaturePidController.SetOutputLimits(_temperatureOutputMin, _temperatureOutputMax);
                        OnPropertyChanged(nameof(TemperatureOutputMax));
                        OnParameterChanged(nameof(TemperatureOutputMax), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the simulation speed multiplier
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
                    double clampedValue = Math.Max(0.1, Math.Min(10.0, value));
                    if (Math.Abs(_simulationSpeedMultiplier - clampedValue) > double.Epsilon)
                    {
                        double oldValue = _simulationSpeedMultiplier;
                        _simulationSpeedMultiplier = clampedValue;
                        
                        // Update TimeStep - for temperature systems, use longer base timestep
                        double newTimeStep = 1.0 / clampedValue; // Base 1 second timestep
                        TimeStep = newTimeStep;
                        
                        OnPropertyChanged(nameof(SimulationSpeedMultiplier));
                        OnParameterChanged(nameof(SimulationSpeedMultiplier), oldValue, clampedValue);
                        
                        System.Diagnostics.Debug.WriteLine($"Temperature simulation speed changed to {clampedValue:F1}x, TimeStep = {newTimeStep:F3}s");
                    }
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TemperatureWithPidModel class
        /// </summary>
        public TemperatureWithPidModel(string modelId, string modelName) 
            : base(modelId, modelName)
        {
            // Create enhanced PID controller instance
            _temperaturePidController = new EnhancedPidController($"{modelId}_TemperatureController", $"{modelName} Temperature Controller");
            
            // Set default PID parameters for temperature control
            _temperaturePidController.SetTuning(0.5, 0.02, 0.1);
            _temperaturePidController.SetOutputLimits(_temperatureOutputMin, _temperatureOutputMax);
            _temperaturePidController.ControllerType = PidControllerType.PI_D; // Good for temperature control
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Initializes the temperature model with proper steady state
        /// </summary>
        protected override void InitializeModel()
        {
            lock (_lockObject)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"=== TemperatureWithPidModel.InitializeModel START ===");
                    
                    // Start in proper steady state - all values at 50% for steady state
                    _temperatureSetpoint = 50.0;        // 50% target temperature
                    _temperatureProcessVariable = 50.0; // Start at setpoint (steady state)
                    _temperatureControllerOutput = 50.0; // Heater power at steady state
                    _temperatureDisturbance = 0.0;       // No initial disturbance
                    _temperatureClosedLoop = true;       // Start in automatic mode
                    _temperatureManualOutput = 50.0;     // Manual output at steady state
                    _temperatureSetpointTracking = false;
                    _simulationSpeedMultiplier = 1.0;    // Initialize to real-time
                    
                    System.Diagnostics.Debug.WriteLine($"Temperature initial steady state values: SP={_temperatureSetpoint}%, PV={_temperatureProcessVariable}%, MV={_temperatureControllerOutput}%");
                    
                    // Set initial controller parameters for steady state
                    _temperaturePidController.Setpoint = _temperatureSetpoint;
                    _temperaturePidController.AutoMode = _temperatureClosedLoop;
                    _temperaturePidController.ManualOutput = _temperatureManualOutput;
                    
                    // Set output limits with anti-reset windup
                    _temperaturePidController.SetOutputLimits(_temperatureOutputMin, _temperatureOutputMax);
                    
                    System.Diagnostics.Debug.WriteLine($"Temperature controller configured: SP={_temperaturePidController.Setpoint}%, AutoMode={_temperaturePidController.AutoMode}, Type={_temperaturePidController.ControllerType}");
                    
                    // Initialize controller
                    _temperaturePidController.Initialize();
                    
                    // Force controller to steady state
                    _temperaturePidController.Update(_temperatureProcessVariable, 1.0);
                    _temperatureControllerOutput = 50.0; // Force to steady state
                    
                    System.Diagnostics.Debug.WriteLine($"Temperature FORCED steady state: SP={_temperatureSetpoint}%, PV={_temperatureProcessVariable}%, MV={_temperatureControllerOutput}%");
                    
                    // Initialize model parameters
                    SetParameter("TemperatureSetpoint", _temperatureSetpoint);
                    SetParameter("TemperatureProcessVariable", _temperatureProcessVariable);
                    SetParameter("TemperatureControllerOutput", _temperatureControllerOutput);
                    SetParameter("TemperatureDisturbance", _temperatureDisturbance);
                    SetParameter("TemperatureClosedLoop", _temperatureClosedLoop ? 1.0 : 0.0);
                    SetParameter("TemperatureManualOutput", _temperatureManualOutput);
                    SetParameter("TemperatureSetpointTracking", _temperatureSetpointTracking ? 1.0 : 0.0);
                    SetParameter("TemperatureKp", _temperaturePidController.Kp);
                    SetParameter("TemperatureKi", _temperaturePidController.Ki);
                    SetParameter("TemperatureKd", _temperaturePidController.Kd);
                    SetParameter("TemperatureMass", _temperatureMass);
                    SetParameter("TemperatureSpecificHeat", _temperatureSpecificHeat);
                    SetParameter("TemperatureHeatTransferCoeff", _temperatureHeatTransferCoeff);
                    SetParameter("TemperatureAmbientTemp", _temperatureAmbientTemp);
                    SetParameter("TemperatureMaxPower", _temperatureMaxPower);
                    SetParameter("TemperatureOutputMin", _temperatureOutputMin);
                    SetParameter("TemperatureOutputMax", _temperatureOutputMax);
                    SetParameter("SimulationSpeedMultiplier", _simulationSpeedMultiplier);
                    
                    System.Diagnostics.Debug.WriteLine($"=== TemperatureWithPidModel.InitializeModel COMPLETE ===");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"CRITICAL: Failed to initialize TemperatureWithPidModel: {ex.Message}");
                    OnModelError("Failed to initialize TemperatureWithPid model", ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the temperature model for one time step
        /// Industrial heat exchanger dynamics
        /// </summary>
        protected override void UpdateModel()
        {
            lock (_lockObject)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== TemperatureWithPidModel.UpdateModel at Time={CurrentTime:F2}s ===");
                    System.Diagnostics.Debug.WriteLine($"BEFORE: SP={_temperatureSetpoint:F2}%, PV={_temperatureProcessVariable:F2}%, MV={_temperatureControllerOutput:F2}%");
                    
                    // Convert percentage to actual temperature (0-100% = 0-100°C for simplicity)
                    double actualTemperature = (_temperatureProcessVariable / 100.0) * (_temperatureMaxScale - _temperatureMinScale) + _temperatureMinScale;
                    
                    // Heat exchanger dynamics: dT/dt = (Q_input + Q_disturbance - Q_loss) / (m * Cp)
                    double powerInput = (_temperatureControllerOutput / 100.0) * _temperatureMaxPower; // Convert % to Watts
                    double powerLoss = _temperatureHeatTransferCoeff * (actualTemperature - _temperatureAmbientTemp);
                    double powerDisturbance = (_temperatureDisturbance / 100.0) * _temperatureMaxPower; // Disturbance as % of max power
                    
                    double netPower = powerInput + powerDisturbance - powerLoss;
                    double thermalCapacity = _temperatureMass * _temperatureSpecificHeat;
                    
                    // Update temperature using Euler integration
                    double temperatureChange = (netPower / thermalCapacity) * TimeStep;
                    double newActualTemperature = actualTemperature + temperatureChange;
                    
                    // Convert back to percentage
                    double newTemperaturePercent = ((newActualTemperature - _temperatureMinScale) / (_temperatureMaxScale - _temperatureMinScale)) * 100.0;
                    
                    System.Diagnostics.Debug.WriteLine($"Temperature dynamics: PowerIn={powerInput:F0}W, PowerLoss={powerLoss:F0}W, NetPower={netPower:F0}W, TempChange={temperatureChange:F3}°C");
                    
                    TemperatureProcessVariable = newTemperaturePercent;
                    System.Diagnostics.Debug.WriteLine($"Temperature PV updated to: {_temperatureProcessVariable:F2}%");
                    
                    // Handle setpoint tracking in manual mode
                    if (!_temperatureClosedLoop && _temperatureSetpointTracking)
                    {
                        double oldSetpoint = _temperatureSetpoint;
                        _temperatureSetpoint = _temperatureProcessVariable;
                        _temperaturePidController.Setpoint = _temperatureSetpoint;
                        
                        if (Math.Abs(oldSetpoint - _temperatureSetpoint) > double.Epsilon)
                        {
                            OnPropertyChanged(nameof(TemperatureSetpoint));
                            OnParameterChanged(nameof(TemperatureSetpoint), oldSetpoint, _temperatureSetpoint);
                        }
                    }
                    
                    // Update PID controller
                    System.Diagnostics.Debug.WriteLine($"Calling Temperature PID Update with PV={_temperatureProcessVariable:F2}%, SP={_temperaturePidController.Setpoint:F2}%");
                    
                    double newControllerOutput = _temperaturePidController.Update(_temperatureProcessVariable, TimeStep);
                    
                    System.Diagnostics.Debug.WriteLine($"Temperature PID ({_temperaturePidController.ControllerType}) returned newMV = {newControllerOutput:F2}%");
                    
                    TemperatureControllerOutput = newControllerOutput;
                    
                    // In Auto mode, sync manual output for bumpless transfer
                    if (_temperatureClosedLoop)
                    {
                        _temperatureManualOutput = _temperatureControllerOutput;
                    }
                    
                    // Update parameter storage
                    SetParameter("TemperatureProcessVariable", _temperatureProcessVariable);
                    SetParameter("TemperatureControllerOutput", _temperatureControllerOutput);
                    SetParameter("TemperatureError", TemperatureError);
                    SetParameter("TemperatureSetpoint", _temperatureSetpoint);
                    SetParameter("TemperatureManualOutput", _temperatureManualOutput);
                    SetParameter("SimulationSpeedMultiplier", _simulationSpeedMultiplier);
                    
                    System.Diagnostics.Debug.WriteLine($"AFTER: SP={_temperatureSetpoint:F2}%, PV={_temperatureProcessVariable:F2}%, MV={_temperatureControllerOutput:F2}%, Error={TemperatureError:F2}%");
                    System.Diagnostics.Debug.WriteLine($"=== TemperatureWithPidModel.UpdateModel END ===\n");
                }
                catch (Exception ex)
                {
                    OnModelError("Error updating TemperatureWithPid model", ex);
                    System.Diagnostics.Debug.WriteLine($"ERROR in Temperature UpdateModel: {ex.Message}");
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
                "TEMPERATURESETPOINT" or "TEMPSETPOINT" or "SP" => TemperatureSetpoint,
                "TEMPERATUREPROCESSVARIABLE" or "TEMPPROCESSVARIABLE" or "PV" => TemperatureProcessVariable,
                "TEMPERATURECONTROLLEROUTPUT" or "TEMPCONTROLLEROUTPUT" or "MV" => TemperatureControllerOutput,
                "TEMPERATUREERROR" or "TEMPERROR" or "ERROR" => TemperatureError,
                "TEMPERATUREDISTURBANCE" or "TEMPDISTURBANCE" or "DISTURBANCE" => TemperatureDisturbance,
                "TEMPERATURECLOSEDLOOP" or "TEMPCLOSEDLOOP" or "CLOSEDLOOP" => TemperatureClosedLoop ? 1.0 : 0.0,
                "TEMPERATUREMANUALOUTPUT" or "TEMPMANUALOUTPUT" or "MANUALOUTPUT" => TemperatureManualOutput,
                "TEMPERATURESETPOINTTRACKING" or "TEMPSETPOINTTRACKING" or "SETPOINTTRACKING" => TemperatureSetpointTracking ? 1.0 : 0.0,
                "TEMPERATUREKP" or "TEMPKP" or "KP" => TemperatureKp,
                "TEMPERATUREKI" or "TEMPKI" or "KI" => TemperatureKi,
                "TEMPERATUREKD" or "TEMPKD" or "KD" => TemperatureKd,
                "TEMPERATURECONTROLLERTYPE" or "TEMPCONTROLLERTYPE" or "CONTROLLERTYPE" => (double)TemperatureControllerType,
                "TEMPERATUREOUTPUTMIN" or "TEMPOUTPUTMIN" or "OUTPUTMIN" => TemperatureOutputMin,
                "TEMPERATUREOUTPUTMAX" or "TEMPOUTPUTMAX" or "OUTPUTMAX" => TemperatureOutputMax,
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
                case "TEMPERATURESETPOINT":
                case "TEMPSETPOINT":
                case "SP":
                    TemperatureSetpoint = newValue;
                    break;
                    
                case "TEMPERATUREDISTURBANCE":
                case "TEMPDISTURBANCE":
                case "DISTURBANCE":
                    TemperatureDisturbance = newValue;
                    break;
                    
                case "TEMPERATURECLOSEDLOOP":
                case "TEMPCLOSEDLOOP":
                case "CLOSEDLOOP":
                    TemperatureClosedLoop = newValue > 0.5;
                    break;
                    
                case "TEMPERATUREMANUALOUTPUT":
                case "TEMPMANUALOUTPUT":
                case "MANUALOUTPUT":
                    TemperatureManualOutput = newValue;
                    break;
                    
                case "TEMPERATURESETPOINTTRACKING":
                case "TEMPSETPOINTTRACKING":
                case "SETPOINTTRACKING":
                    TemperatureSetpointTracking = newValue > 0.5;
                    break;
                    
                case "TEMPERATUREKP":
                case "TEMPKP":
                case "KP":
                    TemperatureKp = newValue;
                    break;
                    
                case "TEMPERATUREKI":
                case "TEMPKI":
                case "KI":
                    TemperatureKi = newValue;
                    break;
                    
                case "TEMPERATUREKD":
                case "TEMPKD":
                case "KD":
                    TemperatureKd = newValue;
                    break;
                    
                case "TEMPERATURECONTROLLERTYPE":
                case "TEMPCONTROLLERTYPE":
                case "CONTROLLERTYPE":
                    TemperatureControllerType = (PidControllerType)Math.Max(0, Math.Min(2, (int)newValue));
                    break;
                    
                case "TEMPERATUREOUTPUTMIN":
                case "TEMPOUTPUTMIN":
                case "OUTPUTMIN":
                    TemperatureOutputMin = newValue;
                    break;
                    
                case "TEMPERATUREOUTPUTMAX":
                case "TEMPOUTPUTMAX":
                case "OUTPUTMAX":
                    TemperatureOutputMax = newValue;
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
                _temperaturePidController?.Dispose();
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
                System.Diagnostics.Debug.WriteLine($"TemperatureWithPidModel started with {_temperaturePidController.ControllerType} controller");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Temperature OnModelStarted: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when the model is stopped
        /// </summary>
        protected override void OnModelStopped()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"TemperatureWithPidModel stopped");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Temperature OnModelStopped: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when the model is reset
        /// </summary>
        protected override void OnModelReset()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Resetting TemperatureWithPidModel");
                
                // Reset simulation speed to default
                _simulationSpeedMultiplier = 1.0;
                TimeStep = 1.0; // 1x speed = 1 second timestep
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error resetting Temperature: {ex.Message}");
            }
        }

        #endregion
    }
}