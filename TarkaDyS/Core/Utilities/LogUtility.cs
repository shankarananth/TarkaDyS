using TarkaDyS.Core.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TarkaDyS.Core.Utilities
{
    /// <summary>
    /// Utility class for managing debug logs and providing easy access to log files
    /// </summary>
    public static class LogUtility
    {
        /// <summary>
        /// Opens the current log file in the default text editor
        /// </summary>
        public static void OpenLogFile()
        {
            try
            {
                string logPath = ControlLoopLogger.GetLogFilePath();
                if (File.Exists(logPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = logPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Log file not found. Start a simulation to generate log data.", 
                        "Log File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening log file: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens the log directory in Windows Explorer
        /// </summary>
        public static void OpenLogDirectory()
        {
            try
            {
                string logPath = ControlLoopLogger.GetLogFilePath();
                string logDirectory = Path.GetDirectoryName(logPath) ?? "";
                
                if (Directory.Exists(logDirectory))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = logDirectory,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Log directory not found.", 
                        "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening log directory: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets the current log file path for display
        /// </summary>
        public static string GetLogFilePath()
        {
            return ControlLoopLogger.GetLogFilePath();
        }

        /// <summary>
        /// Shows information about the current log file
        /// </summary>
        public static void ShowLogInfo()
        {
            try
            {
                string logPath = ControlLoopLogger.GetLogFilePath();
                if (File.Exists(logPath))
                {
                    FileInfo fileInfo = new FileInfo(logPath);
                    string message = $"Log File Information:\n\n" +
                                   $"Path: {logPath}\n" +
                                   $"Size: {fileInfo.Length:N0} bytes\n" +
                                   $"Created: {fileInfo.CreationTime:yyyy-MM-dd HH:mm:ss}\n" +
                                   $"Modified: {fileInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}\n\n" +
                                   $"Click OK to open the log file.";

                    if (MessageBox.Show(message, "Log File Information", 
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        OpenLogFile();
                    }
                }
                else
                {
                    MessageBox.Show("No log file exists yet. Start a simulation to generate log data.", 
                        "No Log File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting log file information: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}