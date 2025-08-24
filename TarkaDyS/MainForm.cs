/*
 * File: MainForm.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Main application form with model management and single instance control
 * Date Created: 24-Aug-2025
 * Date Modified: 24-Aug-2025
 * 
 * Description: Main form for TarkaDyS PID simulation application.
 * Provides menu access to various process control models and manages
 * single instances of model forms to prevent duplicates.
 */

using TarkaDyS.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TarkaDyS.Utilities;

namespace TarkaDyS
{
    /// <summary>
    /// Main application form with model management capabilities
    /// Serves as the central hub for accessing different simulation models
    /// </summary>
    public partial class MainForm : Form
    {
        #region Private Fields
        /// <summary>Dictionary to track open model forms and prevent duplicates</summary>
        private readonly Dictionary<Type, Form> _openModelForms = new();
        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
            this.Text = "TarkaDyS - PID Control System Models";
            
            // Apply application icon from Resources folder if available
            ResourceHelper.ApplyAppIcon(this);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Opens a model form as a single instance (prevents duplicates)
        /// If form is already open, brings it to front instead of creating new instance
        /// </summary>
        /// <typeparam name="T">Type of form to open</typeparam>
        /// <param name="formName">Display name for logging and error messages</param>
        private void OpenSingleInstanceForm<T>(string formName) where T : Form, new()
        {
            try
            {
                var formType = typeof(T);
                
                // Check if this type of form is already open
                if (_openModelForms.ContainsKey(formType) && 
                    _openModelForms[formType] != null && 
                    !_openModelForms[formType].IsDisposed)
                {
                    // Bring existing form to front instead of creating new instance
                    _openModelForms[formType].BringToFront();
                    _openModelForms[formType].WindowState = FormWindowState.Normal;
                    System.Diagnostics.Debug.WriteLine($"{formName} already open - brought to front");
                    return;
                }

                // Create new instance of the requested form
                var newForm = new T();
                
                // Store reference for single instance management
                _openModelForms[formType] = newForm;
                
                // Setup cleanup when form is closed
                newForm.FormClosed += (sender, e) => {
                    if (_openModelForms.ContainsKey(formType))
                    {
                        _openModelForms.Remove(formType);
                        System.Diagnostics.Debug.WriteLine($"{formName} closed - reference removed");
                    }
                };
                
                // Show the new form
                newForm.Show();
                System.Diagnostics.Debug.WriteLine($"{formName} opened successfully");
            }
            catch (Exception ex)
            {
                // Show error message to user and log details
                MessageBox.Show($"Error opening {formName}: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error opening {formName}: {ex}");
            }
        }
        #endregion

        #region Model Menu Event Handlers
        /// <summary>
        /// Handle menu click for First Order Process with PID model
        /// </summary>
        private void MenuFirstOrderPid_Click(object sender, EventArgs e)
        {
            OpenSingleInstanceForm<FirstOrderProcessWithPidForm>("First Order Process with PID");
        }

        // Placeholder for future model implementations
        // Uncomment and implement as new models are added to the application
        
        /*
        /// <summary>Handle menu click for Second Order Process with PID model</summary>
        private void MenuSecondOrderPid_Click(object sender, EventArgs e)
        {
            OpenSingleInstanceForm<SecondOrderProcessWithPidForm>("Second Order Process with PID");
        }
        
        /// <summary>Handle menu click for Tank Level with PID model</summary>
        private void MenuTankLevelPid_Click(object sender, EventArgs e)
        {
            OpenSingleInstanceForm<TankLevelWithPidForm>("Tank Level with PID");
        }
        
        /// <summary>Handle menu click for Temperature Process with PID model</summary>
        private void MenuTemperaturePid_Click(object sender, EventArgs e)
        {
            OpenSingleInstanceForm<TemperatureWithPidForm>("Temperature Process with PID");
        }
        */
        #endregion

        #region Cleanup and Disposal
        /// <summary>
        /// Clean up resources when main form is disposed
        /// Ensures all open model forms are properly closed
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Close all open model forms when main form is disposed
                foreach (var form in _openModelForms.Values)
                {
                    if (form != null && !form.IsDisposed)
                    {
                        form.Close();
                    }
                }
                _openModelForms.Clear();
                
                // Dispose standard form components
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}