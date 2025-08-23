using System;

namespace TarkaDyS.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for all process models in the simulation system.
    /// This interface ensures consistent behavior across different process types.
    /// </summary>
    public interface IProcessModel : IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for this process model instance
        /// </summary>
        string ModelId { get; }

        /// <summary>
        /// Gets the display name of the process model
        /// </summary>
        string ModelName { get; }

        /// <summary>
        /// Gets the current simulation time in seconds
        /// </summary>
        double CurrentTime { get; }

        /// <summary>
        /// Gets or sets the simulation time step in seconds
        /// </summary>
        double TimeStep { get; set; }

        /// <summary>
        /// Gets a value indicating whether the model is currently running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Initializes the process model with default parameters
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts the simulation process
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the simulation process
        /// </summary>
        void Stop();

        /// <summary>
        /// Resets the model to its initial state
        /// </summary>
        void Reset();

        /// <summary>
        /// Updates the model by one time step
        /// </summary>
        void Update();

        /// <summary>
        /// Sets an input value for the specified parameter
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="value">Value to set</param>
        void SetInput(string parameterName, double value);

        /// <summary>
        /// Gets an output value for the specified parameter
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <returns>Current value of the parameter</returns>
        double GetOutput(string parameterName);

        /// <summary>
        /// Event raised when model parameters change
        /// </summary>
        event EventHandler<ModelParameterChangedEventArgs>? ParameterChanged;

        /// <summary>
        /// Event raised when an error occurs in the model
        /// </summary>
        event EventHandler<ModelErrorEventArgs>? ModelError;
    }

    /// <summary>
    /// Event arguments for parameter change notifications
    /// </summary>
    public class ModelParameterChangedEventArgs : EventArgs
    {
        public string ParameterName { get; set; } = string.Empty;
        public double OldValue { get; set; }
        public double NewValue { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    /// <summary>
    /// Event arguments for model error notifications
    /// </summary>
    public class ModelErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}