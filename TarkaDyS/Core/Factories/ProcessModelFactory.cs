using TarkaDyS.Core.Interfaces;
using TarkaDyS.ProcessModels;
using System;
using System.Collections.Generic;

namespace TarkaDyS.Core.Factories
{
    /// <summary>
    /// Enumeration of available process model types in the simulation system
    /// </summary>
    public enum ProcessModelType
    {
        /// <summary>
        /// First-order process with dead time
        /// </summary>
        FirstOrderProcess,
        
        /// <summary>
        /// PID controller with first-order process (closed-loop control system)
        /// </summary>
        PidProcess,
        
        /// <summary>
        /// Level control process (tank level simulation)
        /// </summary>
        LevelProcess,
        
        /// <summary>
        /// Temperature control process (heat exchanger simulation)
        /// </summary>
        TemperatureProcess,
        
        /// <summary>
        /// Flow control process
        /// </summary>
        FlowProcess,
        
        /// <summary>
        /// Distillation column process
        /// </summary>
        DistillationColumn
    }

    /// <summary>
    /// Factory class for creating process model instances.
    /// This factory implements the Factory pattern to provide a centralized way
    /// to create different types of process models with proper initialization.
    /// </summary>
    public static class ProcessModelFactory
    {
        #region Private Fields
        private static readonly Dictionary<ProcessModelType, ProcessModelInfo> _modelRegistry = new();
        
        static ProcessModelFactory()
        {
            RegisterDefaultModels();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new process model instance of the specified type
        /// </summary>
        /// <param name="modelType">Type of process model to create</param>
        /// <param name="modelId">Unique identifier for the model instance</param>
        /// <param name="modelName">Display name for the model instance</param>
        /// <returns>New process model instance</returns>
        /// <exception cref="ArgumentException">Thrown when modelType is not supported</exception>
        /// <exception cref="ArgumentNullException">Thrown when modelId or modelName is null or empty</exception>
        public static IProcessModel CreateModel(ProcessModelType modelType, string modelId, string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelId))
                throw new ArgumentNullException(nameof(modelId), "Model ID cannot be null or empty");

            if (string.IsNullOrWhiteSpace(modelName))
                throw new ArgumentNullException(nameof(modelName), "Model name cannot be null or empty");

            if (!_modelRegistry.ContainsKey(modelType))
                throw new ArgumentException($"Process model type '{modelType}' is not supported", nameof(modelType));

            try
            {
                var modelInfo = _modelRegistry[modelType];
                var model = modelInfo.CreateInstance(modelId, modelName);
                
                // Initialize the model with default parameters
                model.Initialize();
                
                return model;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create process model of type '{modelType}'", ex);
            }
        }

        /// <summary>
        /// Creates a new process model instance with default naming
        /// </summary>
        /// <param name="modelType">Type of process model to create</param>
        /// <returns>New process model instance with auto-generated ID and name</returns>
        public static IProcessModel CreateModel(ProcessModelType modelType)
        {
            string modelId = Guid.NewGuid().ToString();
            string modelName = GetDefaultModelName(modelType);
            return CreateModel(modelType, modelId, modelName);
        }

        /// <summary>
        /// Gets information about all available process model types
        /// </summary>
        /// <returns>Dictionary mapping model types to their information</returns>
        public static Dictionary<ProcessModelType, ProcessModelInfo> GetAvailableModels()
        {
            return new Dictionary<ProcessModelType, ProcessModelInfo>(_modelRegistry);
        }

        /// <summary>
        /// Gets information about a specific process model type
        /// </summary>
        /// <param name="modelType">Type of process model</param>
        /// <returns>Model information</returns>
        /// <exception cref="ArgumentException">Thrown when modelType is not supported</exception>
        public static ProcessModelInfo GetModelInfo(ProcessModelType modelType)
        {
            if (!_modelRegistry.ContainsKey(modelType))
                throw new ArgumentException($"Process model type '{modelType}' is not supported", nameof(modelType));

            return _modelRegistry[modelType];
        }

        /// <summary>
        /// Checks if a process model type is supported
        /// </summary>
        /// <param name="modelType">Type of process model to check</param>
        /// <returns>True if supported, false otherwise</returns>
        public static bool IsSupported(ProcessModelType modelType)
        {
            return _modelRegistry.ContainsKey(modelType);
        }

        /// <summary>
        /// Registers a custom process model type (for future extensibility)
        /// </summary>
        /// <param name="modelType">Type of process model</param>
        /// <param name="modelInfo">Model information and factory function</param>
        public static void RegisterModel(ProcessModelType modelType, ProcessModelInfo modelInfo)
        {
            if (modelInfo == null)
                throw new ArgumentNullException(nameof(modelInfo));

            _modelRegistry[modelType] = modelInfo;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers the default process models available in the system
        /// </summary>
        private static void RegisterDefaultModels()
        {
            // Register First Order Process
            _modelRegistry[ProcessModelType.FirstOrderProcess] = new ProcessModelInfo
            {
                Type = ProcessModelType.FirstOrderProcess,
                Name = "First Order Process",
                Description = "A first-order process with dead time. Fundamental building block for process control systems.",
                Category = "Basic Processes",
                CreateInstance = (id, name) => (IProcessModel)new FirstOrderProcessModel(id, name)
            };

            // Register PID Process
            _modelRegistry[ProcessModelType.PidProcess] = new ProcessModelInfo
            {
                Type = ProcessModelType.PidProcess,
                Name = "First Order Process with PID",
                Description = "Complete closed-loop PID control system with first-order process. Based on VB PID Simulator.",
                Category = "Control Systems",
                CreateInstance = (id, name) => (IProcessModel)new FirstOrderWithPidModel(id, name)
            };

            // Placeholder registrations for future models
            _modelRegistry[ProcessModelType.LevelProcess] = new ProcessModelInfo
            {
                Type = ProcessModelType.LevelProcess,
                Name = "Level Control Process",
                Description = "Tank level control system with inlet and outlet flows.",
                Category = "Level Control",
                CreateInstance = (id, name) => throw new NotImplementedException("Level process model not yet implemented")
            };

            _modelRegistry[ProcessModelType.TemperatureProcess] = new ProcessModelInfo
            {
                Type = ProcessModelType.TemperatureProcess,
                Name = "Temperature Control Process",
                Description = "Heat exchanger temperature control system.",
                Category = "Temperature Control",
                CreateInstance = (id, name) => throw new NotImplementedException("Temperature process model not yet implemented")
            };

            _modelRegistry[ProcessModelType.FlowProcess] = new ProcessModelInfo
            {
                Type = ProcessModelType.FlowProcess,
                Name = "Flow Control Process",
                Description = "Flow control system with control valve.",
                Category = "Flow Control",
                CreateInstance = (id, name) => throw new NotImplementedException("Flow process model not yet implemented")
            };

            _modelRegistry[ProcessModelType.DistillationColumn] = new ProcessModelInfo
            {
                Type = ProcessModelType.DistillationColumn,
                Name = "Distillation Column",
                Description = "Multi-stage distillation column process.",
                Category = "Separation Processes",
                CreateInstance = (id, name) => throw new NotImplementedException("Distillation column model not yet implemented")
            };
        }

        /// <summary>
        /// Gets the default model name for a given model type
        /// </summary>
        /// <param name="modelType">Type of process model</param>
        /// <returns>Default model name</returns>
        private static string GetDefaultModelName(ProcessModelType modelType)
        {
            if (_modelRegistry.ContainsKey(modelType))
            {
                return _modelRegistry[modelType].Name;
            }

            return modelType.ToString();
        }

        #endregion
    }

    /// <summary>
    /// Contains information about a process model type, including metadata and factory function
    /// </summary>
    public class ProcessModelInfo
    {
        /// <summary>
        /// Gets or sets the process model type
        /// </summary>
        public ProcessModelType Type { get; set; }

        /// <summary>
        /// Gets or sets the display name of the model
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the model
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category of the model (for grouping in UI)
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the factory function for creating model instances
        /// </summary>
        public Func<string, string, IProcessModel> CreateInstance { get; set; } = null!;

        /// <summary>
        /// Gets or sets the icon name or path for the model (for UI display)
        /// </summary>
        public string? IconName { get; set; }

        /// <summary>
        /// Gets or sets additional metadata for the model
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}