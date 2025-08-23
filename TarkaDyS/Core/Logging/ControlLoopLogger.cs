using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace TarkaDyS.Core.Logging
{
    /// <summary>
    /// Detailed logging system for debugging PID control loops and process models.
    /// Creates comprehensive logs with timestamps, thread safety, and structured data.
    /// FIXED: Uses project directory and asynchronous logging to prevent UI freezing.
    /// </summary>
    public static class ControlLoopLogger
    {
        private static readonly object _lockObject = new object();
        
        // FIXED: Use project directory instead of Desktop
        private static readonly string _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string _logFileName = $"ControlLoop_Debug_{DateTime.Now:yyyyMMdd_HHmms}.txt";
        private static readonly string _fullLogPath = Path.Combine(_logDirectory, _logFileName);
        
        // FIXED: Add asynchronous logging queue to prevent UI blocking
        private static readonly ConcurrentQueue<string> _logQueue = new();
        private static readonly CancellationTokenSource _cancellationTokenSource = new();
        private static Task? _logWriterTask;
        
        private static bool _initialized = false;
        private static volatile bool _loggingEnabled = true; // Allow disabling during high-frequency operations

        /// <summary>
        /// Initializes the logging system
        /// </summary>
        public static void Initialize()
        {
            lock (_lockObject)
            {
                if (_initialized) return;

                try
                {
                    // Create log directory if it doesn't exist
                    if (!Directory.Exists(_logDirectory))
                    {
                        Directory.CreateDirectory(_logDirectory);
                    }

                    // Start the background log writer task
                    _logWriterTask = Task.Run(async () => await LogWriterLoop(), _cancellationTokenSource.Token);

                    // Create initial log entry
                    WriteLogEntry("SYSTEM", "=== TarkaDyS Control Loop Debug Log Initialized ===");
                    WriteLogEntry("SYSTEM", $"Log file: {_fullLogPath}");
                    WriteLogEntry("SYSTEM", $"Started at: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                    WriteLogEntry("SYSTEM", $"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                    WriteLogEntry("SYSTEM", $"Project Directory: {AppDomain.CurrentDomain.BaseDirectory}");
                    WriteLogEntry("SYSTEM", "");

                    _initialized = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to initialize ControlLoopLogger: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Background task that writes log entries asynchronously
        /// </summary>
        private static async Task LogWriterLoop()
        {
            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (_logQueue.TryDequeue(out string? logEntry))
                    {
                        try
                        {
                            await File.AppendAllTextAsync(_fullLogPath, logEntry + Environment.NewLine, _cancellationTokenSource.Token);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to write log entry: {ex.Message}");
                        }
                    }
                    else
                    {
                        // No logs to write, wait a bit
                        await Task.Delay(50, _cancellationTokenSource.Token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when shutting down
            }
        }

        /// <summary>
        /// Enables or disables logging (useful during high-frequency operations)
        /// </summary>
        public static void SetLoggingEnabled(bool enabled)
        {
            _loggingEnabled = enabled;
            if (enabled)
            {
                WriteLogEntry("SYSTEM", "Logging re-enabled");
            }
            else
            {
                WriteLogEntry("SYSTEM", "Logging temporarily disabled for performance");
            }
        }

        /// <summary>
        /// Logs initialization data for a model
        /// </summary>
        public static void LogInitialization(string modelName, double setpoint, double processVariable, double controllerOutput, 
            double processGain, double timeConstant, double deadTime, double kp, double ki, double kd)
        {
            if (!_loggingEnabled) return;
            
            WriteSection($"{modelName} INITIALIZATION");
            WriteLogEntry("INIT", $"Setpoint (SP): {setpoint:F2}");
            WriteLogEntry("INIT", $"Process Variable (PV): {processVariable:F2}");
            WriteLogEntry("INIT", $"Controller Output (MV): {controllerOutput:F2}");
            WriteLogEntry("INIT", $"Process Gain (K): {processGain:F2}");
            WriteLogEntry("INIT", $"Time Constant (Tau): {timeConstant:F2} s");
            WriteLogEntry("INIT", $"Dead Time (Td): {deadTime:F2} s");
            WriteLogEntry("INIT", $"PID Kp: {kp:F3}");
            WriteLogEntry("INIT", $"PID Ki: {ki:F3}");
            WriteLogEntry("INIT", $"PID Kd: {kd:F3}");
            WriteLogEntry("INIT", "");
        }

        /// <summary>
        /// Logs initialization data for tank level model (overload for different parameters)
        /// </summary>
        public static void LogTankInitialization(string modelName, double setpoint, double processVariable, double controllerOutput, 
            double tankArea, double outflowRate, double maxLevel, double kp, double ki, double kd)
        {
            if (!_loggingEnabled) return;
            
            WriteSection($"{modelName} INITIALIZATION");
            WriteLogEntry("INIT", $"Setpoint (SP): {setpoint:F2}");
            WriteLogEntry("INIT", $"Process Variable (PV): {processVariable:F2}");
            WriteLogEntry("INIT", $"Controller Output (MV): {controllerOutput:F2}");
            WriteLogEntry("INIT", $"Tank Area: {tankArea:F2} m²");
            WriteLogEntry("INIT", $"Outflow Rate: {outflowRate:F2} m³/min");
            WriteLogEntry("INIT", $"Max Level: {maxLevel:F2} m");
            WriteLogEntry("INIT", $"PID Kp: {kp:F3}");
            WriteLogEntry("INIT", $"PID Ki: {ki:F3}");
            WriteLogEntry("INIT", $"PID Kd: {kd:F3}");
            WriteLogEntry("INIT", "");
        }

        /// <summary>
        /// Logs a complete control loop update cycle (REDUCED FREQUENCY)
        /// </summary>
        public static void LogControlLoopUpdate(string modelName, double currentTime, 
            double setpoint, double processVariable, double controllerOutput, double error,
            string stepDescription = "")
        {
            // FIXED: Only log every 10th update to reduce frequency (reduce from every 0.1s to every 1s)
            if (!_loggingEnabled || ((int)(currentTime * 10) % 10 != 0)) return;
            
            if (!string.IsNullOrEmpty(stepDescription))
            {
                WriteLogEntry("LOOP", $"[{currentTime:F1}s] {stepDescription}");
            }
            
            WriteLogEntry("LOOP", $"[{currentTime:F1}s] SP={setpoint:F2}, PV={processVariable:F2}, MV={controllerOutput:F2}, Error={error:F2}");
        }

        /// <summary>
        /// Logs process model calculation details
        /// </summary>
        public static void LogProcessCalculation(string modelName, double currentTime, 
            double processInput, double processOutput, double effectiveInput, double steadyStateOutput,
            double derivative, double timeStep, double processGain, double timeConstant)
        {
            // FIXED: Reduce frequency of detailed process logs
            if (!_loggingEnabled || currentTime < 5.0 || ((int)currentTime % 5 != 0)) return;
            
            WriteSection($"{modelName} PROCESS CALCULATION [{currentTime:F2}s]");
            WriteLogEntry("PROC", $"Input: {processInput:F2} -> Effective Input: {effectiveInput:F2}");
            WriteLogEntry("PROC", $"Current Output: {processOutput:F2}");
            WriteLogEntry("PROC", $"Steady State Target: K * U = {processGain:F2} * {effectiveInput:F2} = {steadyStateOutput:F2}");
            WriteLogEntry("PROC", $"Derivative: (Yss - Y) / Tau = ({steadyStateOutput:F2} - {processOutput:F2}) / {timeConstant:F2} = {derivative:F6}");
            WriteLogEntry("PROC", $"Time Step: {timeStep:F3}");
            WriteLogEntry("PROC", $"Output Change: dt * derivative = {timeStep:F3} * {derivative:F6} = {timeStep * derivative:F6}");
            WriteLogEntry("PROC", $"New Output: {processOutput:F2} + {timeStep * derivative:F6} = {processOutput + (timeStep * derivative):F6}");
            WriteLogEntry("PROC", "");
        }

        /// <summary>
        /// Logs PID controller calculation details
        /// </summary>
        public static void LogPidCalculation(string modelName, double currentTime,
            double setpoint, double processVariable, double error, double controllerOutput,
            double proportional, double integral, double derivative, 
            double kp, double ki, double kd)
        {
            // FIXED: Only log PID details during first 10 seconds or every 10 seconds
            if (!_loggingEnabled || (currentTime > 10.0 && (int)currentTime % 10 != 0)) return;
            
            WriteSection($"{modelName} PID CALCULATION [{currentTime:F2}s]");
            WriteLogEntry("PID", $"Error: SP - PV = {setpoint:F2} - {processVariable:F2} = {error:F2}");
            WriteLogEntry("PID", $"Proportional: Kp * Error = {kp:F3} * {error:F2} = {proportional:F2}");
            WriteLogEntry("PID", $"Integral: Ki * ?Error = {ki:F3} * ? = {integral:F2}");
            WriteLogEntry("PID", $"Derivative: Kd * dPV/dt = {kd:F3} * d/dt = {derivative:F2}");
            WriteLogEntry("PID", $"Output: P + I + D = {proportional:F2} + {integral:F2} + {derivative:F2} = {controllerOutput:F2}");
            WriteLogEntry("PID", "");
        }

        /// <summary>
        /// Logs data flow between components (REDUCED FREQUENCY)
        /// </summary>
        public static void LogDataFlow(string modelName, double currentTime, string fromComponent, string toComponent, 
            string dataType, double value, string notes = "")
        {
            // FIXED: Only log data flow occasionally to reduce spam
            if (!_loggingEnabled || ((int)(currentTime * 2) % 5 != 0)) return;
            
            string notesPart = string.IsNullOrEmpty(notes) ? "" : $" ({notes})";
            WriteLogEntry("FLOW", $"[{currentTime:F1}s] {fromComponent} -> {toComponent}: {dataType} = {value:F2}{notesPart}");
        }

        /// <summary>
        /// Logs errors and exceptions
        /// </summary>
        public static void LogError(string modelName, string errorMessage, Exception? exception = null)
        {
            WriteSection($"ERROR in {modelName}");
            WriteLogEntry("ERROR", errorMessage);
            if (exception != null)
            {
                WriteLogEntry("ERROR", $"Exception: {exception.Message}");
                WriteLogEntry("ERROR", $"Stack Trace: {exception.StackTrace}");
            }
            WriteLogEntry("ERROR", "");
        }

        /// <summary>
        /// Logs UI updates (VERY REDUCED FREQUENCY)
        /// </summary>
        public static void LogUIUpdate(string formName, double setpoint, double processVariable, 
            double controllerOutput, double error, double simulationTime)
        {
            // FIXED: Only log UI updates every 5 seconds to prevent spam
            if (!_loggingEnabled || ((int)simulationTime % 5 != 0)) return;
            
            WriteLogEntry("UI", $"{formName} Update: SP={setpoint:F2}, PV={processVariable:F2}, MV={controllerOutput:F2}, Error={error:F2}, Time={simulationTime:F1}s");
        }

        /// <summary>
        /// Logs method entry and exit for tracing
        /// </summary>
        public static void LogMethodCall(string className, string methodName, string parameters = "")
        {
            if (!_loggingEnabled) return;
            
            string paramPart = string.IsNullOrEmpty(parameters) ? "" : $"({parameters})";
            WriteLogEntry("TRACE", $">>> {className}.{methodName}{paramPart}");
        }

        /// <summary>
        /// Logs method exit
        /// </summary>
        public static void LogMethodExit(string className, string methodName, string returnValue = "")
        {
            if (!_loggingEnabled) return;
            
            string returnPart = string.IsNullOrEmpty(returnValue) ? "" : $" -> {returnValue}";
            WriteLogEntry("TRACE", $"<<< {className}.{methodName}{returnPart}");
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        public static void LogWarning(string modelName, string message)
        {
            WriteLogEntry("WARN", $"{modelName}: {message}");
        }

        /// <summary>
        /// Logs an informational message
        /// </summary>
        public static void LogInfo(string message)
        {
            WriteLogEntry("INFO", message);
        }

        /// <summary>
        /// Logs the complete system state
        /// </summary>
        public static void LogSystemState(string modelName, double currentTime,
            double setpoint, double processVariable, double controllerOutput, double error,
            bool isRunning, bool closedLoop, string additionalInfo = "")
        {
            WriteSection($"{modelName} SYSTEM STATE [{currentTime:F2}s]");
            WriteLogEntry("STATE", $"Running: {isRunning}");
            WriteLogEntry("STATE", $"Mode: {(closedLoop ? "AUTOMATIC" : "MANUAL")}");
            WriteLogEntry("STATE", $"Setpoint: {setpoint:F2}");
            WriteLogEntry("STATE", $"Process Variable: {processVariable:F2}");
            WriteLogEntry("STATE", $"Controller Output: {controllerOutput:F2}");
            WriteLogEntry("STATE", $"Error: {error:F2}");
            if (!string.IsNullOrEmpty(additionalInfo))
            {
                WriteLogEntry("STATE", $"Additional: {additionalInfo}");
            }
            WriteLogEntry("STATE", "");
        }

        /// <summary>
        /// Closes the log file gracefully
        /// </summary>
        public static void Close()
        {
            lock (_lockObject)
            {
                _loggingEnabled = false;
                WriteLogEntry("SYSTEM", "");
                WriteLogEntry("SYSTEM", "=== TarkaDyS Control Loop Debug Log Closed ===");
                WriteLogEntry("SYSTEM", $"Closed at: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                
                // Cancel the background writer and wait for it to finish
                _cancellationTokenSource.Cancel();
                try
                {
                    _logWriterTask?.Wait(TimeSpan.FromSeconds(2));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error waiting for log writer to finish: {ex.Message}");
                }
                
                _initialized = false;
            }
        }

        /// <summary>
        /// Gets the full path to the current log file
        /// </summary>
        public static string GetLogFilePath() => _fullLogPath;

        #region Private Methods

        private static void WriteSection(string sectionName)
        {
            if (!_loggingEnabled) return;
            
            WriteLogEntry("", "");
            WriteLogEntry("", $"================== {sectionName} ==================");
        }

        private static void WriteLogEntry(string category, string message)
        {
            if (!_loggingEnabled && category != "SYSTEM" && category != "ERROR") return;
            
            if (!_initialized && category != "SYSTEM")
            {
                Initialize();
            }

            try
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                string threadId = Thread.CurrentThread.ManagedThreadId.ToString("D2");
                string categoryPart = string.IsNullOrEmpty(category) ? "" : $"[{category,-5}] ";
                string logLine = $"{timestamp} T{threadId} {categoryPart}{message}";

                // FIXED: Queue the log entry for asynchronous writing instead of blocking
                _logQueue.Enqueue(logLine);

                // Also write to debug output for immediate feedback
                System.Diagnostics.Debug.WriteLine($"LOG: {logLine}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to queue log entry: {ex.Message}");
            }
        }

        #endregion
    }
}