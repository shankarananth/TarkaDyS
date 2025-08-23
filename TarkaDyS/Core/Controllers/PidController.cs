using TarkaDyS.Core.Interfaces;
using System;
using System.ComponentModel;

namespace TarkaDyS.Core.Controllers
{
    /// <summary>
    /// Implementation of a Proportional-Integral-Derivative (PID) controller.
    /// This controller provides automated process control using feedback control loop mechanism.
    /// It continuously calculates an error value as the difference between a desired setpoint 
    /// and a measured process variable and applies a correction based on proportional, integral, 
    /// and derivative terms.
    /// </summary>
    public class PidController : IController, INotifyPropertyChanged
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

        // Internal calculation variables
        private double _previousError = 0.0;
        private double _integralSum = 0.0;
        private double _previousProcessVariable = 0.0;
        private DateTime _lastUpdateTime = DateTime.Now;

        // Output limits
        private double _outputMin = double.MinValue;
        private double _outputMax = double.MaxValue;

        // Integral windup protection
        private double _integralMin = double.MinValue;
        private double _integralMax = double.MaxValue;
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
                        
                        // When switching to manual mode, set manual output to current output
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
        /// Gets or sets the proportional gain (Kp) parameter.
        /// Higher values result in larger changes in output for a given change in error.
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
        /// Gets or sets the integral gain (Ki) parameter.
        /// Higher values result in faster elimination of steady-state error.
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
        /// Gets or sets the derivative gain (Kd) parameter.
        /// Higher values result in stronger reaction to rate of change of error.
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
                        
                        // Update integral limits if windup protection is enabled
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
                        
                        // Update integral limits if windup protection is enabled
                        if (_integralWindupProtection)
                        {
                            _integralMax = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether integral windup protection is enabled
        /// </summary>
        public bool IntegralWindupProtection
        {
            get
            {
                lock (_lockObject)
                {
                    return _integralWindupProtection;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (_integralWindupProtection != value)
                    {
                        _integralWindupProtection = value;
                        OnPropertyChanged(nameof(IntegralWindupProtection));
                        
                        if (value)
                        {
                            _integralMin = _outputMin;
                            _integralMax = _outputMax;
                        }
                        else
                        {
                            _integralMin = double.MinValue;
                            _integralMax = double.MaxValue;
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
        /// Initializes a new instance of the PidController class
        /// </summary>
        /// <param name="controllerId">Unique identifier for the controller</param>
        /// <param name="controllerName">Display name for the controller</param>
        public PidController(string controllerId, string controllerName)
        {
            ControllerId = controllerId ?? throw new ArgumentNullException(nameof(controllerId));
            ControllerName = controllerName ?? throw new ArgumentNullException(nameof(controllerName));
        }

        /// <summary>
        /// Initializes a new instance of the PidController class with PID parameters
        /// </summary>
        /// <param name="controllerId">Unique identifier for the controller</param>
        /// <param name="controllerName">Display name for the controller</param>
        /// <param name="kp">Proportional gain</param>
        /// <param name="ki">Integral gain</param>
        /// <param name="kd">Derivative gain</param>
        public PidController(string controllerId, string controllerName, double kp, double ki, double kd)
            : this(controllerId, controllerName)
        {
            Kp = kp;
            Ki = ki;
            Kd = kd;
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
                    // DON'T reset output to 0 - preserve existing value for steady state
                    // _output = 0.0;  // ? This was the problem!
                    _processVariable = 0.0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize PID controller", ex);
            }
        }

        /// <inheritdoc/>
        public double Update(double processVariable, double deltaTime)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(PidController));

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
                        Output = _manualOutput;
                        return _output;
                    }

                    // Calculate error
                    double error = _setpoint - processVariable;

                    // Proportional term
                    double proportional = _kp * error;

                    // Integral term
                    _integralSum += error * deltaTime;
                    
                    // Apply integral windup protection
                    if (_integralWindupProtection)
                    {
                        _integralSum = Math.Max(_integralMin / Math.Max(_ki, double.Epsilon), 
                                              Math.Min(_integralMax / Math.Max(_ki, double.Epsilon), _integralSum));
                    }
                    
                    double integral = _ki * _integralSum;

                    // Derivative term (derivative on measurement to avoid derivative kick)
                    double derivative = 0.0;
                    if (deltaTime > 0)
                    {
                        double processVariableDerivative = (processVariable - _previousProcessVariable) / deltaTime;
                        derivative = -_kd * processVariableDerivative;
                    }

                    // Calculate total output
                    double calculatedOutput = proportional + integral + derivative;

                    // Apply output limits
                    calculatedOutput = Math.Max(_outputMin, Math.Min(_outputMax, calculatedOutput));

                    // Store values for next iteration
                    _previousError = error;
                    _previousProcessVariable = processVariable;
                    
                    Output = calculatedOutput;
                    return _output;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during PID controller update", ex);
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
                throw new InvalidOperationException("Failed to reset PID controller", ex);
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