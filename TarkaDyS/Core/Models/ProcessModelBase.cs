using TarkaDyS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TarkaDyS.Core.Models
{
    /// <summary>
    /// Abstract base class for all process models in the simulation system.
    /// Provides common functionality and enforces consistent implementation patterns.
    /// </summary>
    public abstract class ProcessModelBase : IProcessModel, INotifyPropertyChanged
    {
        #region Private Fields
        private readonly object _lockObject = new object();
        private readonly Dictionary<string, double> _parameters = new();
        private readonly System.Threading.Timer? _simulationTimer;
        private string _modelId = string.Empty;
        private string _modelName = string.Empty;
        private double _currentTime = 0.0;
        private double _timeStep = 0.1;
        private double _simulationSpeedMultiplier = 1.0; // Add this field
        private bool _isRunning = false;
        private bool _disposed = false;
        #endregion

        #region Properties

        /// <inheritdoc/>
        public virtual string ModelId
        {
            get => _modelId;
            protected set
            {
                if (_modelId != value)
                {
                    _modelId = value;
                    OnPropertyChanged(nameof(ModelId));
                }
            }
        }

        /// <inheritdoc/>
        public virtual string ModelName
        {
            get => _modelName;
            protected set
            {
                if (_modelName != value)
                {
                    _modelName = value;
                    OnPropertyChanged(nameof(ModelName));
                }
            }
        }

        /// <inheritdoc/>
        public virtual double CurrentTime
        {
            get
            {
                lock (_lockObject)
                {
                    return _currentTime;
                }
            }
            protected set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_currentTime - value) > double.Epsilon)
                    {
                        _currentTime = value;
                        OnPropertyChanged(nameof(CurrentTime));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public virtual double TimeStep
        {
            get
            {
                lock (_lockObject)
                {
                    return _timeStep;
                }
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Time step must be greater than zero.", nameof(value));

                lock (_lockObject)
                {
                    if (Math.Abs(_timeStep - value) > double.Epsilon)
                    {
                        _timeStep = value;
                        OnPropertyChanged(nameof(TimeStep));
                        
                        // Update timer interval if running
                        if (IsRunning)
                        {
                            UpdateTimerInterval();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the simulation speed multiplier (1.0 = real-time, 2.0 = 2x speed, etc.)
        /// Real-time (1x) = 1 second timestep, 10x = 0.1 second timestep
        /// </summary>
        public virtual double SimulationSpeedMultiplier
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
                if (value <= 0)
                    throw new ArgumentException("Simulation speed multiplier must be greater than zero.", nameof(value));

                lock (_lockObject)
                {
                    if (Math.Abs(_simulationSpeedMultiplier - value) > double.Epsilon)
                    {
                        _simulationSpeedMultiplier = value;
                        OnPropertyChanged(nameof(SimulationSpeedMultiplier));
                        
                        // Update timer interval if running
                        if (IsRunning)
                        {
                            UpdateTimerInterval();
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public virtual bool IsRunning
        {
            get
            {
                lock (_lockObject)
                {
                    return _isRunning;
                }
            }
            protected set
            {
                lock (_lockObject)
                {
                    if (_isRunning != value)
                    {
                        _isRunning = value;
                        OnPropertyChanged(nameof(IsRunning));
                    }
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ProcessModelBase class
        /// </summary>
        /// <param name="modelId">Unique identifier for the model</param>
        /// <param name="modelName">Display name for the model</param>
        protected ProcessModelBase(string modelId, string modelName)
        {
            ModelId = modelId ?? throw new ArgumentNullException(nameof(modelId));
            ModelName = modelName ?? throw new ArgumentNullException(nameof(modelName));

            // Initialize simulation timer
            _simulationTimer = new System.Threading.Timer(OnTimerTick, null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public virtual void Initialize()
        {
            try
            {
                lock (_lockObject)
                {
                    _currentTime = 0.0;
                    _parameters.Clear();
                    IsRunning = false;
                }
                
                InitializeModel();
            }
            catch (Exception ex)
            {
                OnModelError("Failed to initialize model", ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual void Start()
        {
            try
            {
                lock (_lockObject)
                {
                    if (_disposed)
                        throw new ObjectDisposedException(nameof(ProcessModelBase));

                    if (IsRunning)
                        return;

                    IsRunning = true;
                    
                    // Start the simulation timer with proper speed calculation
                    UpdateTimerInterval();
                }

                OnModelStarted();
                System.Diagnostics.Debug.WriteLine($"Model {ModelName} started successfully with speed {SimulationSpeedMultiplier:F1}x");
            }
            catch (Exception ex)
            {
                IsRunning = false;
                System.Diagnostics.Debug.WriteLine($"Error starting model {ModelName}: {ex.Message}");
                OnModelError("Failed to start model", ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual void Stop()
        {
            try
            {
                lock (_lockObject)
                {
                    if (!IsRunning)
                        return;

                    IsRunning = false;
                    
                    // Stop the simulation timer
                    _simulationTimer?.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                }

                OnModelStopped();
                System.Diagnostics.Debug.WriteLine($"Model {ModelName} stopped successfully");
            }
            catch (Exception ex)
            {
                OnModelError("Failed to stop model", ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual void Reset()
        {
            try
            {
                Stop();
                Initialize();
                OnModelReset();
            }
            catch (Exception ex)
            {
                OnModelError("Failed to reset model", ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual void Update()
        {
            try
            {
                if (!IsRunning)
                    return;

                lock (_lockObject)
                {
                    UpdateModel();
                    _currentTime += _timeStep;
                    OnPropertyChanged(nameof(CurrentTime));
                }
            }
            catch (Exception ex)
            {
                OnModelError("Error during model update", ex);
            }
        }

        /// <inheritdoc/>
        public virtual void SetInput(string parameterName, double value)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException("Parameter name cannot be null or empty.", nameof(parameterName));

            try
            {
                lock (_lockObject)
                {
                    double oldValue = _parameters.ContainsKey(parameterName) ? _parameters[parameterName] : 0.0;
                    _parameters[parameterName] = value;
                    
                    OnParameterChanged(parameterName, oldValue, value);
                    OnInputChanged(parameterName, value);
                }
            }
            catch (Exception ex)
            {
                OnModelError($"Failed to set input parameter '{parameterName}'", ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual double GetOutput(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException("Parameter name cannot be null or empty.", nameof(parameterName));

            try
            {
                lock (_lockObject)
                {
                    return GetOutputValue(parameterName);
                }
            }
            catch (Exception ex)
            {
                OnModelError($"Failed to get output parameter '{parameterName}'", ex);
                throw;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Updates the timer interval based on current time step and simulation speed
        /// FIXED: Prevent extremely fast timer intervals that can freeze the UI
        /// </summary>
        private void UpdateTimerInterval()
        {
            lock (_lockObject)
            {
                try
                {
                    // Calculate timer interval: base timestep divided by speed multiplier
                    double intervalSeconds = _timeStep / _simulationSpeedMultiplier;
                    
                    // FIXED: Enforce minimum interval of 50ms to prevent UI freezing
                    // Even at 10x speed, we won't go below 50ms timer intervals
                    int intervalMs = Math.Max(50, (int)(intervalSeconds * 1000));
                    
                    // FIXED: Cap maximum interval at 1000ms (1 second)
                    intervalMs = Math.Min(1000, intervalMs);
                    
                    // FIXED: Only log timer updates occasionally to reduce overhead
                    if (_simulationSpeedMultiplier == 1.0 || (int)(DateTime.Now.Millisecond / 200) == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"SAFE Timer: {ModelName} Speed={_simulationSpeedMultiplier:F1}x, Interval={intervalMs}ms (min 50ms enforced)");
                    }
                    
                    _simulationTimer?.Change(0, intervalMs);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error updating timer interval: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Gets a parameter value by name
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <returns>Parameter value, or 0.0 if not found</returns>
        protected double GetParameter(string parameterName)
        {
            return _parameters.ContainsKey(parameterName) ? _parameters[parameterName] : 0.0;
        }

        /// <summary>
        /// Sets a parameter value by name
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="value">Value to set</param>
        protected void SetParameter(string parameterName, double value)
        {
            if (!string.IsNullOrWhiteSpace(parameterName))
            {
                _parameters[parameterName] = value;
            }
        }

        /// <summary>
        /// Called when the model needs to be initialized with specific parameters
        /// </summary>
        protected abstract void InitializeModel();

        /// <summary>
        /// Called on each simulation time step to update the model
        /// </summary>
        protected abstract void UpdateModel();

        /// <summary>
        /// Called to get the output value for a specific parameter
        /// </summary>
        /// <param name="parameterName">Name of the output parameter</param>
        /// <returns>Current output value</returns>
        protected abstract double GetOutputValue(string parameterName);

        /// <summary>
        /// Called when an input parameter changes
        /// </summary>
        /// <param name="parameterName">Name of the changed parameter</param>
        /// <param name="newValue">New parameter value</param>
        protected virtual void OnInputChanged(string parameterName, double newValue)
        {
            // Override in derived classes if needed
        }

        /// <summary>
        /// Called when the model is started
        /// </summary>
        protected virtual void OnModelStarted()
        {
            // Override in derived classes if needed
        }

        /// <summary>
        /// Called when the model is stopped
        /// </summary>
        protected virtual void OnModelStopped()
        {
            // Override in derived classes if needed
        }

        /// <summary>
        /// Called when the model is reset
        /// </summary>
        protected virtual void OnModelReset()
        {
            // Override in derived classes if needed
        }

        #endregion

        #region Event Handlers

        private void OnTimerTick(object? state)
        {
            try
            {
                // FIXED: Only log occasionally to avoid spam and UI freezing
                if ((int)CurrentTime % 10 == 0 && CurrentTime > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"ProcessModelBase.OnTimerTick: {ModelName} at Time={CurrentTime:F1}s, Speed={_simulationSpeedMultiplier:F1}x");
                }
                Update();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnTimerTick for {ModelName}: {ex.Message}");
                OnModelError("Timer tick error", ex);
            }
        }

        #endregion

        #region Events

        /// <inheritdoc/>
        public event EventHandler<ModelParameterChangedEventArgs>? ParameterChanged;

        /// <inheritdoc/>
        public event EventHandler<ModelErrorEventArgs>? ModelError;

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
            ParameterChanged?.Invoke(this, new ModelParameterChangedEventArgs
            {
                ParameterName = parameterName,
                OldValue = oldValue,
                NewValue = newValue,
                TimeStamp = DateTime.Now
            });
        }

        /// <summary>
        /// Raises the ModelError event
        /// </summary>
        protected virtual void OnModelError(string errorMessage, Exception? exception = null)
        {
            ModelError?.Invoke(this, new ModelErrorEventArgs
            {
                ErrorMessage = errorMessage,
                Exception = exception,
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
                    Stop();
                    _simulationTimer?.Dispose();
                }

                _disposed = true;
            }
        }

        #endregion
    }
}