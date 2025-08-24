/*
 * File: Program.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Application entry point with splash screen initialization
 * Date Created: 24-Aug-2025
 * Date Modified: 24-Aug-2025
 * 
 * Description: Main entry point for TarkaDyS PID simulation application.
 * Displays splash screen for 3 seconds then opens the main application window.
 */

using System;
using System.Windows.Forms;
using TarkaDyS.Forms;

namespace TarkaDyS
{
    /// <summary>
    /// Application entry point and startup logic
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the TarkaDyS application
        /// Initializes application, shows splash screen, then starts main form
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize Windows Forms application
            ApplicationConfiguration.Initialize();

            // Show splash screen and wait for it to close (3 seconds)
            using (var splash = new SplashScreen())
            {
                splash.ShowDialog(); // Blocks until splash screen closes
            }

            // Start main application after splash screen closes
            Application.Run(new MainForm());
        }
    }
}