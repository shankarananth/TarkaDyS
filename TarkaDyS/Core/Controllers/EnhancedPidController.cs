/*
 * File: EnhancedPidController.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Enhanced PID controller with multiple algorithm types
 * Date Created: 2024-01-XX
 * Date Modified: 2024-01-XX
 * 
 * Description: Implementation of an enhanced PID controller supporting multiple algorithms:
 * 1) Basic PID (PID on Error) - Traditional P, I, D all on error
 * 2) I-PD Controller - Integral on error, P and D on measurement (derivative kick avoidance)
 * 3) PI-D Controller - P and I on error, D on measurement (partial derivative kick avoidance)
 * 
 * Modifications:
 * - 2024-01-XX: Initial creation with three PID types
 * - 2024-01-XX: Added anti-reset windup with output limits
 * - 2024-01-XX: Added proper bumpless transfer between modes
 */

using TarkaDyS.Core.Interfaces;
using System;
using System.ComponentModel;

namespace TarkaDyS.Core.Controllers
{
    /// <summary>
    /// Enumeration of PID controller algorithm types
    /// </summary>
    public enum PidControllerType
    {
        /// <summary>
        /// Basic PID - P, I, and D all calculated on error (traditional)
        /// </summary>
        BasicPID = 0,
        
        /// <summary>
        /// I-PD Controller - Integral on error, Proportional and Derivative on measurement
        /// Avoids derivative kick and reduces proportional kick
        /// </summary>
        I_PD = 1,
        
        /// <summary>
        /// PI-D Controller - Proportional and Integral on error, Derivative on measurement
        /// Avoids derivative kick while maintaining proportional response to setpoint changes
        /// </summary>
        PI_D = 2
    }

    /// <summary>
    /// Enhanced PID controller implementation supporting multiple algorithm types.
    /// This controller provides automated process control using feedback control loop mechanism
    /// with selectable algorithms to optimize performance for different applications.
    /// </summary>
    public class EnhancedPidController : IController, INotifyPropertyChanged
    {
        #region Private Fields
        private readonly object _lockObject = new object();
        private string _controllerId = string.Empty;
        private string _controllerName = string.Empty;
        private double _setpoint = 0.0;
        private double _processVariable = 0.0;
        private double _output = 0.0;
        private double _manualOutput = 0.0;
        private bool _autoMode = true;
        private bool _disposed = false;

        // PID Parameters
        private double _kp = 1.0;  // Proportional gain
        private double _ki = 0.0;  // Integral gain
        private double _kd = 0.0;  // Derivative gain

        // Controller algorithm type
        private PidControllerType _controllerType = PidControllerType.BasicPID;

        // Internal calculation variables
        private double _previousError = 0.0;
        private double _integralSum = 0.0;
        private double _previousProcessVariable = 0.0;
        private DateTime _lastUpdateTime = DateTime.Now;

        // Output limits with anti-reset windup
        private double _outputMin = 0.0;
        private double _outputMax = 100.0;

        // Integral windup protection
        private double _integralMin = 0.0;
        private double _integralMax = 100.0;
        private bool _integralWindupProtection = true;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public string ControllerId
        {
            get => _controllerId;
            set
            {
                if (_controllerId != value)
                {
                    _controllerId = value;
                    OnPropertyChanged(nameof(ControllerId));
                }
            }
        }

        /// <inheritdoc/>
        public string ControllerName
        {
            get => _controllerName;
            set
            {
                if (_controllerName != value)
                {
                    _controllerName = value;
                    OnPropertyChanged(nameof(ControllerName));
                }
            }
        }

        /// <summary>
        /// Gets or sets the PID controller algorithm type
        /// </summary>
        public PidControllerType ControllerType
        {
            get
            {
                lock (_lockObject)
                {
                    return _controllerType;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_controllerType != value)
                    {
                        _controllerType = value;
                        OnPropertyChanged(nameof(ControllerType));
                        System.Diagnostics.Debug.WriteLine($"PID Controller type changed to: {value}");
                    }
                }
            }
        }

        /// <inheritdoc/>
        public double Setpoint
        {
            get
            {
                lock (_lockObject)
                {
                    return _setpoint;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_setpoint - value) > double.Epsilon)
                    {
                        double oldValue = _setpoint;
                        _setpoint = value;
                        OnPropertyChanged(nameof(Setpoint));
                        OnParameterChanged(nameof(Setpoint), oldValue, value);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public double ProcessVariable
        {
            get
            {
                lock (_lockObject)
                {
                    return _processVariable;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_processVariable - value) > double.Epsilon)
                    {
                        _processVariable = value;
                        OnPropertyChanged(nameof(ProcessVariable));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public double Output
        {
            get
            {
                lock (_lockObject)
                {
                    return _output;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_output - value) > double.Epsilon)
                    {
                        _output = value;
                        OnPropertyChanged(nameof(Output));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public bool AutoMode
        {
            get
            {
                lock (_lockObject)
                {
                    return _autoMode;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_autoMode != value)
                    {
                        _autoMode = value;
                        OnPropertyChanged(nameof(AutoMode));
                        OnParameterChanged(nameof(AutoMode), _autoMode ? 0 : 1, value ? 1 : 0);
                        
                        // When switching to manual mode, set manual output to current output for bumpless transfer
                        if (!value)
                        {
                            ManualOutput = _output;
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public double ManualOutput
        {
            get
            {
                lock (_lockObject)
                {
                    return _manualOutput;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    // Apply output limits to manual output as well
                    double limitedValue = Math.Max(_outputMin, Math.Min(_outputMax, value));
                    
                    if (Math.Abs(_manualOutput - limitedValue) > double.Epsilon)
                    {
                        double oldValue = _manualOutput;
                        _manualOutput = limitedValue;
                        OnPropertyChanged(nameof(ManualOutput));
                        OnParameterChanged(nameof(ManualOutput), oldValue, limitedValue);
                        
                        // If in manual mode, update the output immediately
                        if (!_autoMode)
                        {
                            Output = _manualOutput;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the proportional gain (Kp) parameter
        /// </summary>
        public double Kp
        {
            get
            {
                lock (_lockObject)
                {
                    return _kp;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_kp - value) > double.Epsilon)
                    {
                        double oldValue = _kp;
                        _kp = value;
                        OnPropertyChanged(nameof(Kp));
                        OnParameterChanged(nameof(Kp), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the integral gain (Ki) parameter
        /// </summary>
        public double Ki
        {
            get
            {
                lock (_lockObject)
                {
                    return _ki;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_ki - value) > double.Epsilon)
                    {
                        double oldValue = _ki;
                        _ki = value;
                        OnPropertyChanged(nameof(Ki));
                        OnParameterChanged(nameof(Ki), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the derivative gain (Kd) parameter
        /// </summary>
        public double Kd
        {
            get
            {
                lock (_lockObject)
                {
                    return _kd;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_kd - value) > double.Epsilon)
                    {
                        double oldValue = _kd;
                        _kd = value;
                        OnPropertyChanged(nameof(Kd));
                        OnParameterChanged(nameof(Kd), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum output limit
        /// </summary>
        public double OutputMin
        {
            get
            {
                lock (_lockObject)
                {
                    return _outputMin;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_outputMin - value) > double.Epsilon)
                    {
                        _outputMin = value;
                        OnPropertyChanged(nameof(OutputMin));
                        
                        // Update integral limits for anti-reset windup
                        if (_integralWindupProtection)
                        {
                            _integralMin = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum output limit
        /// </summary>
        public double OutputMax
        {
            get
            {
                lock (_lockObject)
                {
                    return _outputMax;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_outputMax - value) > double.Epsilon)
                    {
                        _outputMax = value;
                        OnPropertyChanged(nameof(OutputMax));
                        
                        // Update integral limits for anti-reset windup
                        if (_integralWindupProtection)
                        {
                            _integralMax = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current error value (Setpoint - ProcessVariable)
        /// </summary>
        public double Error
        {
            get
            {
                lock (_lockObject)
                {
                    return _setpoint - _processVariable;
                }
            }
        }

        /// <summary>
        /// Gets the current integral sum
        /// </summary>
        public double IntegralSum
        {
            get
            {
                lock (_lockObject)
                {
                    return _integralSum;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the EnhancedPidController class
        /// </summary>
        /// <param name="controllerId">Unique identifier for the controller</param>
        /// <param name="controllerName">Display name for the controller</param>
        public EnhancedPidController(string controllerId, string controllerName)
        {
            ControllerId = controllerId ?? throw new ArgumentNullException(nameof(controllerId));
            ControllerName = controllerName ?? throw new ArgumentNullException(nameof(controllerName));
        }

        /// <summary>
        /// Initializes a new instance of the EnhancedPidController class with PID parameters
        /// </summary>
        /// <param name="controllerId">Unique identifier for the controller</param>
        /// <param name="controllerName">Display name for the controller</param>
        /// <param name="kp">Proportional gain</param>
        /// <param name="ki">Integral gain</param>
        /// <param name="kd">Derivative gain</param>
        /// <param name="controllerType">PID algorithm type</param>
        public EnhancedPidController(string controllerId, string controllerName, double kp, double ki, double kd, PidControllerType controllerType = PidControllerType.BasicPID)
            : this(controllerId, controllerName)
        {
            Kp = kp;
            Ki = ki;
            Kd = kd;
            ControllerType = controllerType;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Initialize()
        {
            try
            {
                lock (_lockObject)
                {
                    _previousError = 0.0;
                    _integralSum = 0.0;
                    _previousProcessVariable = 0.0;
                    _lastUpdateTime = DateTime.Now;
                    // Preserve existing output value for bumpless initialization
                    _processVariable = 0.0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize enhanced PID controller", ex);
            }
        }

        /// <inheritdoc/>
        public double Update(double processVariable, double deltaTime)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(EnhancedPidController));

            if (deltaTime <= 0)
                throw new ArgumentException("Delta time must be greater than zero", nameof(deltaTime));

            try
            {
                lock (_lockObject)
                {
                    ProcessVariable = processVariable;
                    _lastUpdateTime = DateTime.Now;

                    if (!_autoMode)
                    {
                        // In manual mode, output the manual value
                        Output = _manualOutput;
                        return _output;
                    }

                    double calculatedOutput;

                    // Calculate PID terms based on controller type
                    switch (_controllerType)
                    {
                        case PidControllerType.BasicPID:
                            calculatedOutput = CalculateBasicPID(processVariable, deltaTime);
                            break;
                            
                        case PidControllerType.I_PD:
                            calculatedOutput = CalculateI_PD(processVariable, deltaTime);
                            break;
                            
                        case PidControllerType.PI_D:
                            calculatedOutput = CalculatePI_D(processVariable, deltaTime);
                            break;
                            
                        default:
                            calculatedOutput = CalculateBasicPID(processVariable, deltaTime);
                            break;
                    }

                    // Apply output limits
                    calculatedOutput = Math.Max(_outputMin, Math.Min(_outputMax, calculatedOutput));

                    // Store values for next iteration
                    _previousProcessVariable = processVariable;
                    
                    Output = calculatedOutput;
                    return _output;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during enhanced PID controller update", ex);
            }
        }

        /// <inheritdoc/>
        public void Reset()
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to reset enhanced PID controller", ex);
            }
        }

        /// <summary>
        /// Sets the PID tuning parameters
        /// </summary>
        /// <param name="kp">Proportional gain</param>
        /// <param name="ki">Integral gain</param>
        /// <param name="kd">Derivative gain</param>
        public void SetTuning(double kp, double ki, double kd)
        {
            if (kp < 0 || ki < 0 || kd < 0)
                throw new ArgumentException("PID gains cannot be negative");

            Kp = kp;
            Ki = ki;
            Kd = kd;
        }

        /// <summary>
        /// Sets the output limits
        /// </summary>
        /// <param name="min">Minimum output value</param>
        /// <param name="max">Maximum output value</param>
        public void SetOutputLimits(double min, double max)
        {
            if (min >= max)
                throw new ArgumentException("Minimum limit must be less than maximum limit");

            OutputMin = min;
            OutputMax = max;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculate Basic PID output (traditional algorithm)
        /// P, I, and D all calculated on error
        /// </summary>
        private double CalculateBasicPID(double processVariable, double deltaTime)
        {
            double error = _setpoint - processVariable;

            // Proportional term (on error)
            double proportional = _kp * error;

            // Integral term (on error)
            _integralSum += error * deltaTime;
            
            // Apply integral windup protection
            if (_integralWindupProtection)
            {
                double integralOutput = _ki * _integralSum;
                if (integralOutput > _integralMax)
                    _integralSum = _integralMax / Math.Max(_ki, double.Epsilon);
                else if (integralOutput < _integralMin)
                    _integralSum = _integralMin / Math.Max(_ki, double.Epsilon);
            }
            
            double integral = _ki * _integralSum;

            // Derivative term (on error)
            double derivative = 0.0;
            if (deltaTime > 0)
            {
                double errorDerivative = (error - _previousError) / deltaTime;
                derivative = _kd * errorDerivative;
            }
            
            _previousError = error;

            return proportional + integral + derivative;
        }

        /// <summary>
        /// Calculate I-PD output
        /// Integral on error, Proportional and Derivative on measurement
        /// </summary>
        private double CalculateI_PD(double processVariable, double deltaTime)
        {
            double error = _setpoint - processVariable;

            // Proportional term (on measurement - negative for control action)
            double proportional = -_kp * processVariable;

            // Integral term (on error)
            _integralSum += error * deltaTime;
            
            // Apply integral windup protection
            if (_integralWindupProtection)
            {
                double integralOutput = _ki * _integralSum;
                if (integralOutput > _integralMax)
                    _integralSum = _integralMax / Math.Max(_ki, double.Epsilon);
                else if (integralOutput < _integralMin)
                    _integralSum = _integralMin / Math.Max(_ki, double.Epsilon);
            }
            
            double integral = _ki * _integralSum;

            // Derivative term (on measurement - negative for control action)
            double derivative = 0.0;
            if (deltaTime > 0)
            {
                double measurementDerivative = (processVariable - _previousProcessVariable) / deltaTime;
                derivative = -_kd * measurementDerivative;
            }

            // Add setpoint contribution to proportional term
            double setpointContribution = _kp * _setpoint;

            return setpointContribution + proportional + integral + derivative;
        }

        /// <summary>
        /// Calculate PI-D output
        /// Proportional and Integral on error, Derivative on measurement
        /// </summary>
        private double CalculatePI_D(double processVariable, double deltaTime)
        {
            double error = _setpoint - processVariable;

            // Proportional term (on error)
            double proportional = _kp * error;

            // Integral term (on error)
            _integralSum += error * deltaTime;
            
            // Apply integral windup protection
            if (_integralWindupProtection)
            {
                double integralOutput = _ki * _integralSum;
                if (integralOutput > _integralMax)
                    _integralSum = _integralMax / Math.Max(_ki, double.Epsilon);
                else if (integralOutput < _integralMin)
                    _integralSum = _integralMin / Math.Max(_ki, double.Epsilon);
            }
            
            double integral = _ki * _integralSum;

            // Derivative term (on measurement - negative for control action)
            double derivative = 0.0;
            if (deltaTime > 0)
            {
                double measurementDerivative = (processVariable - _previousProcessVariable) / deltaTime;
                derivative = -_kd * measurementDerivative;
            }

            return proportional + integral + derivative;
        }

        #endregion

        #region Events

        /// <inheritdoc/>
        public event EventHandler<ControllerParameterChangedEventArgs>? ParameterChanged;

        /// <summary>
        /// Occurs when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Event Invocation

        /// <summary>
        /// Raises the ParameterChanged event
        /// </summary>
        protected virtual void OnParameterChanged(string parameterName, double oldValue, double newValue)
        {
            ParameterChanged?.Invoke(this, new ControllerParameterChangedEventArgs
            {
                ParameterName = parameterName,
                OldValue = oldValue,
                NewValue = newValue,
                TimeStamp = DateTime.Now
            });
        }

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDisposable Implementation

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Clean up managed resources
                }

                _disposed = true;
            }
        }

        #endregion
    }
}