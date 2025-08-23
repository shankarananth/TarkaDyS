namespace TarkaDyS
{
    /// <summary>
    /// Main entry point for the TarkaDyS Process Dynamic Simulation Software application.
    /// This application provides a comprehensive platform for simulating various process models
    /// including PID controllers, first-order processes, and other control systems.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Initializes the application configuration and starts the main MDI parent form.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Enable visual styles and high DPI support for better UI appearance
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                
                // Configure application-wide error handling
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                
                // Start the main application form (MDI Parent)
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fatal application error: {ex.Message}", 
                              "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles unhandled exceptions on the UI thread
        /// </summary>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);
            MessageBox.Show($"An unexpected error occurred:\n{e.Exception.Message}\n\nThe application will continue running.", 
                          "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Handles unhandled exceptions on non-UI threads
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                LogException(exception);
                
                if (e.IsTerminating)
                {
                    MessageBox.Show($"A fatal error occurred and the application must close:\n{exception.Message}", 
                                  "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Logs exceptions for debugging purposes
        /// </summary>
        /// <param name="exception">The exception to log</param>
        private static void LogException(Exception exception)
        {
            try
            {
                // For now, just write to debug output
                // In a production application, you might want to log to a file or event log
                System.Diagnostics.Debug.WriteLine($"Exception: {DateTime.Now}: {exception}");
                
                // Could also write to console in debug mode
                #if DEBUG
                Console.WriteLine($"Exception: {DateTime.Now}: {exception}");
                #endif
            }
            catch
            {
                // Ignore logging errors to prevent recursive exceptions
            }
        }
    }
}