/*
 * File: Program.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Application entry point - simplified
 * Date Created: 2025-08-23
 * Date Modified: 2025-08-23
 */

using System;
using System.Windows.Forms;

namespace TarkaDyS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the simplified application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}