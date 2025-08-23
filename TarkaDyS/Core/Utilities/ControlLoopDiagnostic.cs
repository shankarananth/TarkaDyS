using System;
using System.IO;
using System.Windows.Forms;
using TarkaDyS.Core.Logging;

namespace TarkaDyS.Core.Utilities
{
    /// <summary>
    /// Simple diagnostic utility to help identify control loop issues
    /// </summary>
    public static class ControlLoopDiagnostic
    {
        /// <summary>
        /// Creates a simple diagnostic report and shows it in a message box
        /// </summary>
        public static void ShowDiagnosticReport(object model)
        {
            try
            {
                string report = GenerateDiagnosticReport(model);
                
                MessageBox.Show(report, "Control Loop Diagnostic Report", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating diagnostic report: {ex.Message}", 
                    "Diagnostic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves diagnostic report to a simple text file on Desktop
        /// </summary>
        public static void SaveDiagnosticReport(object model)
        {
            try
            {
                string report = GenerateDiagnosticReport(model);
                string fileName = $"TarkaDyS_Diagnostic_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                
                File.WriteAllText(filePath, report);
                
                MessageBox.Show($"Diagnostic report saved to:\n{filePath}", 
                    "Report Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving diagnostic report: {ex.Message}", 
                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GenerateDiagnosticReport(object model)
        {
            var report = new System.Text.StringBuilder();
            report.AppendLine("=== TarkaDyS Control Loop Diagnostic Report ===");
            report.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            if (model == null)
            {
                report.AppendLine("ERROR: Model is null!");
                return report.ToString();
            }

            // Use reflection to get model properties
            var modelType = model.GetType();
            report.AppendLine($"Model Type: {modelType.Name}");
            report.AppendLine();

            // Get key properties using reflection
            var properties = modelType.GetProperties();
            
            report.AppendLine("=== KEY VALUES ===");
            foreach (var prop in properties)
            {
                try
                {
                    if (prop.CanRead && (prop.PropertyType == typeof(double) || 
                                        prop.PropertyType == typeof(bool) ||
                                        prop.PropertyType == typeof(string)))
                    {
                        var value = prop.GetValue(model);
                        report.AppendLine($"{prop.Name}: {value}");
                    }
                }
                catch (Exception ex)
                {
                    report.AppendLine($"{prop.Name}: ERROR - {ex.Message}");
                }
            }

            report.AppendLine();
            report.AppendLine("=== SPECIFIC CHECKS ===");

            // Check specific properties we know should exist
            CheckProperty(report, model, "FirstOrderSetpoint", "Setpoint");
            CheckProperty(report, model, "FirstOrderProcessVariable", "Process Variable (PV)");
            CheckProperty(report, model, "FirstOrderControllerOutput", "Controller Output (MV)");
            CheckProperty(report, model, "FirstOrderError", "Error");
            CheckProperty(report, model, "IsRunning", "Is Running");
            CheckProperty(report, model, "CurrentTime", "Current Time");

            return report.ToString();
        }

        private static void CheckProperty(System.Text.StringBuilder report, object model, 
            string propertyName, string displayName)
        {
            try
            {
                var prop = model.GetType().GetProperty(propertyName);
                if (prop != null)
                {
                    var value = prop.GetValue(model);
                    report.AppendLine($"? {displayName}: {value}");
                }
                else
                {
                    report.AppendLine($"? {displayName}: Property '{propertyName}' not found");
                }
            }
            catch (Exception ex)
            {
                report.AppendLine($"? {displayName}: ERROR - {ex.Message}");
            }
        }
    }
}