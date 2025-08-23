namespace TarkaDyS.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for controller components in the simulation system.
    /// Controllers can be PID controllers, fuzzy controllers, or other control algorithms.
    /// </summary>
    public interface IController : IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for this controller instance
        /// </summary>
        string ControllerId { get; }

        /// <summary>
        /// Gets the display name of the controller
        /// </summary>
        string ControllerName { get; }

        /// <summary>
        /// Gets or sets the setpoint value
        /// </summary>
        double Setpoint { get; set; }

        /// <summary>
        /// Gets the current process variable (feedback) value
        /// </summary>
        double ProcessVariable { get; }

        /// <summary>
        /// Gets the current output value from the controller
        /// </summary>
        double Output { get; }

        /// <summary>
        /// Gets or sets whether the controller is in automatic mode
        /// </summary>
        bool AutoMode { get; set; }

        /// <summary>
        /// Gets or sets the manual output value (used when not in auto mode)
        /// </summary>
        double ManualOutput { get; set; }

        /// <summary>
        /// Initializes the controller with default parameters
        /// </summary>
        void Initialize();

        /// <summary>
        /// Updates the controller with a new process variable value
        /// </summary>
        /// <param name="processVariable">Current process variable value</param>
        /// <param name="deltaTime">Time elapsed since last update (seconds)</param>
        /// <returns>Controller output value</returns>
        double Update(double processVariable, double deltaTime);

        /// <summary>
        /// Resets the controller to its initial state
        /// </summary>
        void Reset();

        /// <summary>
        /// Event raised when controller parameters change
        /// </summary>
        event EventHandler<ControllerParameterChangedEventArgs>? ParameterChanged;
    }

    /// <summary>
    /// Event arguments for controller parameter change notifications
    /// </summary>
    public class ControllerParameterChangedEventArgs : EventArgs
    {
        public string ParameterName { get; set; } = string.Empty;
        public double OldValue { get; set; }
        public double NewValue { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}