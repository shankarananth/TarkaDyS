using TarkaDyS.Core.Models;
using TarkaDyS.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TarkaDyS.ProcessModels
{
    /// <summary>
    /// Represents a first-order process with dead time, commonly used in process control.
    /// The transfer function is: K * e^(-Td*s) / (Tau*s + 1)
    /// Where:
    /// - K is the process gain
    /// - Tau is the time constant  
    /// - Td is the dead time (transport delay)
    /// This model is fundamental in process control and represents many real-world processes
    /// such as temperature control, level control, and flow control systems.
    /// </summary>
    public class FirstOrderProcessModel : ProcessModelBase
    {
        #region Private Fields
        private readonly object _lockObject = new object();
        
        // Process parameters
        private double _processGain = 1.0;           // K - steady-state gain
        private double _timeConstant = 10.0;         // Tau - time constant (seconds)
        private double _deadTime = 0.0;              // Td - dead time (seconds)
        
        // Process variables - START AT STEADY STATE FOR PID SIMULATION
        private double _processInput = 50.0;         // Current input to the process (steady state)
        private double _processOutput = 50.0;        // Current output from the process (steady state)
        private double _steadyStateOutput = 50.0;    // Target steady-state output
        
        // Dead time implementation using circular buffer
        private readonly Queue<DeadTimeValue> _deadTimeBuffer = new();
        
        // Disturbance
        private double _disturbance = 0.0;           // External disturbance
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the process gain (K).
        /// This represents the steady-state change in output per unit change in input.
        /// </summary>
        public double ProcessGain
        {
            get
            {
                lock (_lockObject)
                {
                    return _processGain;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_processGain - value) > double.Epsilon)
                    {
                        double oldValue = _processGain;
                        _processGain = value;
                        OnPropertyChanged(nameof(ProcessGain));
                        OnParameterChanged(nameof(ProcessGain), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the time constant (Tau) in seconds.
        /// This represents how fast the process responds to input changes.
        /// Smaller values mean faster response.
        /// </summary>
        public double TimeConstant
        {
            get
            {
                lock (_lockObject)
                {
                    return _timeConstant;
                }
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Time constant must be greater than zero", nameof(value));

                lock (_lockObject)
                {
                    if (Math.Abs(_timeConstant - value) > double.Epsilon)
                    {
                        double oldValue = _timeConstant;
                        _timeConstant = value;
                        OnPropertyChanged(nameof(TimeConstant));
                        OnParameterChanged(nameof(TimeConstant), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the dead time (Td) in seconds.
        /// This represents the delay between input change and the start of output response.
        /// </summary>
        public double DeadTime
        {
            get
            {
                lock (_lockObject)
                {
                    return _deadTime;
                }
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Dead time cannot be negative", nameof(value));

                lock (_lockObject)
                {
                    if (Math.Abs(_deadTime - value) > double.Epsilon)
                    {
                        double oldValue = _deadTime;
                        _deadTime = value;
                        OnPropertyChanged(nameof(DeadTime));
                        OnParameterChanged(nameof(DeadTime), oldValue, value);
                        
                        // Clear and rebuild dead time buffer when dead time changes
                        _deadTimeBuffer.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current process input value
        /// </summary>
        public double ProcessInput
        {
            get
            {
                lock (_lockObject)
                {
                    return _processInput;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_processInput - value) > double.Epsilon)
                    {
                        _processInput = value;
                        OnPropertyChanged(nameof(ProcessInput));
                        SetParameter("Input", value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current process output value
        /// EMERGENCY FIX: Minimal debug output to prevent freezing
        /// </summary>
        public double ProcessOutput
        {
            get
            {
                lock (_lockObject)
                {
                    return _processOutput;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_processOutput - value) > double.Epsilon)
                    {
                        double oldValue = _processOutput;
                        _processOutput = value;
                        OnPropertyChanged(nameof(ProcessOutput));
                        OnParameterChanged(nameof(ProcessOutput), oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the external disturbance applied to the process
        /// </summary>
        public double Disturbance
        {
            get
            {
                lock (_lockObject)
                {
                    return _disturbance;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    if (Math.Abs(_disturbance - value) > double.Epsilon)
                    {
                        double oldValue = _disturbance;
                        _disturbance = value;
                        OnPropertyChanged(nameof(Disturbance));
                        OnParameterChanged(nameof(Disturbance), oldValue, value);
                    }
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FirstOrderProcessModel class
        /// </summary>
        /// <param name="modelId">Unique identifier for the model</param>
        /// <param name="modelName">Display name for the model</param>
        public FirstOrderProcessModel(string modelId, string modelName) 
            : base(modelId, modelName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FirstOrderProcessModel class with parameters
        /// </summary>
        /// <param name="modelId">Unique identifier for the model</param>
        /// <param name="modelName">Display name for the model</param>
        /// <param name="processGain">Process gain (K)</param>
        /// <param name="timeConstant">Time constant (Tau) in seconds</param>
        /// <param name="deadTime">Dead time (Td) in seconds</param>
        public FirstOrderProcessModel(string modelId, string modelName, 
            double processGain, double timeConstant, double deadTime) 
            : base(modelId, modelName)
        {
            ProcessGain = processGain;
            TimeConstant = timeConstant;
            DeadTime = deadTime;
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Initializes the first-order process model with default parameters
        /// EMERGENCY FIX: Minimal logging to prevent freezing
        /// </summary>
        protected override void InitializeModel()
        {
            lock (_lockObject)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"FirstOrderProcessModel InitializeModel START");
                    
                    // We start at steady state by default (50.0), but allow override to zero for other use cases
                    if (_processInput == 0.0 && _processOutput == 0.0)
                    {
                        // Only reset to zero if explicitly requested (by setting them to 0 before Initialize())
                        _steadyStateOutput = 0.0;
                        _disturbance = 0.0;
                        System.Diagnostics.Debug.WriteLine($"FirstOrderProcessModel Initialized to ZERO state");
                    }
                    else
                    {
                        // Normal case - preserve steady state values (50.0 by default)
                        _steadyStateOutput = _processGain * (_processInput + _disturbance);
                        System.Diagnostics.Debug.WriteLine($"FirstOrderProcessModel Initialized at STEADY STATE: Input={_processInput:F2}, Output={_processOutput:F2}");
                    }
                    
                    // Clear dead time buffer and initialize for steady state
                    InitializeDeadTimeBuffer();
                    
                    System.Diagnostics.Debug.WriteLine($"FirstOrderProcessModel Initialized: Input={_processInput:F2}, Output={_processOutput:F2}, Gain={_processGain:F2}, Tau={_timeConstant:F2}s, Td={_deadTime:F2}s");
                    
                    // Initialize model parameters for storage
                    SetParameter("ProcessInput", _processInput);
                    SetParameter("ProcessOutput", _processOutput);
                    SetParameter("ProcessGain", _processGain);
                    SetParameter("TimeConstant", _timeConstant);
                    SetParameter("DeadTime", _deadTime);
                    SetParameter("Disturbance", _disturbance);
                    
                    System.Diagnostics.Debug.WriteLine($"FirstOrderProcessModel InitializeModel COMPLETE");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"CRITICAL: FirstOrderProcessModel failed to initialize: {ex.Message}");
                    OnModelError("Failed to initialize first-order process model", ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Initializes the dead time buffer with current process input
        /// </summary>
        private void InitializeDeadTimeBuffer()
        {
            _deadTimeBuffer.Clear();
            
            if (_deadTime > 0)
            {
                int bufferSize = Math.Max(1, (int)Math.Ceiling(_deadTime / TimeStep));
                for (int i = 0; i < bufferSize; i++)
                {
                    // Initialize buffer with current input value (for steady state)
                    _deadTimeBuffer.Enqueue(new DeadTimeValue 
                    { 
                        Value = _processInput, 
                        Time = CurrentTime - (bufferSize - i) * TimeStep 
                    });
                }
            }
        }

        /// <summary>
        /// Updates the first-order process model for one time step
        /// Uses Euler integration to solve the differential equation:
        /// dY/dt = (K*U - Y) / Tau
        /// Where Y is output, U is (delayed) input, K is gain, Tau is time constant
        /// EMERGENCY FIX: No logging to prevent mouse-movement freezing
        /// </summary>
        public void UpdateProcessModel()
        {
            lock (_lockObject)
            {
                try
                {
                    // EMERGENCY: Completely disable all logging in this method
                    // This is called frequently and any I/O here will cause freezing
                    
                    // Get effective input (after dead time and with disturbance)
                    double effectiveInput = GetDelayedInput();
                    
                    // Calculate target steady-state output
                    _steadyStateOutput = _processGain * (effectiveInput + _disturbance);
                    
                    // Apply first-order dynamics using Euler integration
                    // dY/dt = (Yss - Y) / Tau
                    // Y(t+dt) = Y(t) + dt * (Yss - Y(t)) / Tau
                    double derivative = (_steadyStateOutput - _processOutput) / _timeConstant;
                    double newOutput = _processOutput + TimeStep * derivative;
                    
                    ProcessOutput = newOutput;
                    
                    // Update delayed input buffer for dead time
                    UpdateDeadTimeBuffer();
                }
                catch (Exception ex)
                {
                    // EMERGENCY: Only critical error to debug output, no file I/O
                    System.Diagnostics.Debug.WriteLine($"CRITICAL: FirstOrderProcessModel UpdateProcessModel ERROR: {ex.Message}");
                    OnModelError("Error updating first-order process model", ex);
                }
            }
        }

        /// <summary>
        /// Override the base UpdateModel to do nothing - we use UpdateProcessModel instead
        /// </summary>
        protected override void UpdateModel()
        {
            // This process model is now function-based, called by parent model
            // Do nothing here to avoid timer conflicts
        }

        /// <summary>
        /// Gets the output value for the specified parameter name
        /// </summary>
        /// <param name="parameterName">Name of the output parameter</param>
        /// <returns>Current output value</returns>
        protected override double GetOutputValue(string parameterName)
        {
            return parameterName.ToUpperInvariant() switch
            {
                "OUTPUT" or "PROCESSOUTPUT" or "PV" => ProcessOutput,
                "INPUT" or "PROCESSINPUT" or "MV" => ProcessInput,
                "PROCESSGAIN" or "GAIN" or "K" => ProcessGain,
                "TIMECONSTANT" or "TAU" => TimeConstant,
                "DEADTIME" or "DELAY" or "TD" => DeadTime,
                "DISTURBANCE" => Disturbance,
                _ => GetParameter(parameterName)
            };
        }

        /// <summary>
        /// Called when an input parameter changes
        /// </summary>
        /// <param name="parameterName">Name of the changed parameter</param>
        /// <param name="newValue">New parameter value</param>
        protected override void OnInputChanged(string parameterName, double newValue)
        {
            switch (parameterName.ToUpperInvariant())
            {
                case "INPUT":
                case "PROCESSINPUT":
                case "MV":
                    ProcessInput = newValue;
                    break;
                
                case "PROCESSGAIN":
                case "GAIN":
                case "K":
                    ProcessGain = newValue;
                    break;
                
                case "TIMECONSTANT":
                case "TAU":
                    TimeConstant = newValue;
                    break;
                
                case "DEADTIME":
                case "DELAY":
                case "TD":
                    DeadTime = newValue;
                    break;
                
                case "DISTURBANCE":
                    Disturbance = newValue;
                    break;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the input value after applying dead time delay
        /// EMERGENCY FIX: Minimal debug output to prevent freezing
        /// </summary>
        /// <returns>Delayed input value</returns>
        private double GetDelayedInput()
        {
            if (_deadTime <= 0 || _deadTimeBuffer.Count == 0)
            {
                return _processInput;
            }

            // Find the appropriate delayed value
            double targetTime = CurrentTime - _deadTime;
            
            var delayedValue = _deadTimeBuffer
                .Where(v => v.Time <= targetTime)
                .LastOrDefault();

            double result = delayedValue?.Value ?? _processInput; // Return current input if no delayed value found, not 0.0!
            
            return result;
        }

        /// <summary>
        /// Updates the dead time buffer with current input value
        /// </summary>
        private void UpdateDeadTimeBuffer()
        {
            if (_deadTime <= 0)
                return;

            // Add current input to buffer
            _deadTimeBuffer.Enqueue(new DeadTimeValue
            {
                Time = CurrentTime,
                Value = _processInput
            });

            // Remove old values that are no longer needed
            double oldestNeededTime = CurrentTime - _deadTime - (10 * TimeStep); // Keep extra buffer
            while (_deadTimeBuffer.Count > 0 && _deadTimeBuffer.Peek().Time < oldestNeededTime)
            {
                _deadTimeBuffer.Dequeue();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the process parameters
        /// </summary>
        /// <param name="processGain">Process gain</param>
        /// <param name="timeConstant">Time constant in seconds</param>
        /// <param name="deadTime">Dead time in seconds</param>
        public void SetProcessParameters(double processGain, double timeConstant, double deadTime)
        {
            ProcessGain = processGain;
            TimeConstant = timeConstant;
            DeadTime = deadTime;
        }

        /// <summary>
        /// Applies a step input to the process
        /// </summary>
        /// <param name="stepValue">Step input value</param>
        public void ApplyStepInput(double stepValue)
        {
            ProcessInput = stepValue;
        }

        /// <summary>
        /// Applies a disturbance to the process
        /// </summary>
        /// <param name="disturbanceValue">Disturbance value</param>
        public void ApplyDisturbance(double disturbanceValue)
        {
            Disturbance = disturbanceValue;
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// Represents a value with timestamp for dead time implementation
        /// </summary>
        private class DeadTimeValue
        {
            public double Time { get; set; }
            public double Value { get; set; }
        }

        #endregion
    }
}