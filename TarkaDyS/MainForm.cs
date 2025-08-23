/*
 * File: MainForm.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Main application form with Models menu and single instance management
 * Date Created: 2025-08-23
 * Date Modified: 2025-08-23
 */

using TarkaDyS.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TarkaDyS
{
    /// <summary>
    /// Main form for the TarkaDyS PID simulation application with model management
    /// </summary>
    public partial class MainForm : Form
    {
        // Dictionary to track single instances of model forms
        private readonly Dictionary<Type, Form> _openModelForms = new();

        public MainForm()
        {
            InitializeComponent();
            this.Text = "TarkaDyS - PID Control System Models";
        }

        /// <summary>
        /// Opens a model form as a single instance
        /// </summary>
        /// <typeparam name="T">Type of the form to open</typeparam>
        /// <param name="formName">Display name for logging and messages</param>
        private void OpenSingleInstanceForm<T>(string formName) where T : Form, new()
        {
            try
            {
                var formType = typeof(T);
                
                // Check if form is already open
                if (_openModelForms.ContainsKey(formType) && 
                    _openModelForms[formType] != null && 
                    !_openModelForms[formType].IsDisposed)
                {
                    // Bring existing form to front
                    _openModelForms[formType].BringToFront();
                    _openModelForms[formType].WindowState = FormWindowState.Normal;
                    System.Diagnostics.Debug.WriteLine($"{formName} form already open - brought to front");
                    return;
                }

                // Create new instance
                var newForm = new T();
                
                // Store reference
                _openModelForms[formType] = newForm;
                
                // Handle form closing to clean up reference
                newForm.FormClosed += (sender, e) => {
                    if (_openModelForms.ContainsKey(formType))
                    {
                        _openModelForms.Remove(formType);
                        System.Diagnostics.Debug.WriteLine($"{formName} form closed - reference removed");
                    }
                };
                
                // Show the form
                newForm.Show();
                System.Diagnostics.Debug.WriteLine($"{formName} form opened successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening {formName}: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error opening {formName}: {ex}");
            }
        }

        #region Model Menu Event Handlers

        private void MenuFirstOrderPid_Click(object sender, EventArgs e)
        {
            OpenSingleInstanceForm<FirstOrderProcessWithPidForm>("First Order Process with PID");
        }

        // Future model handlers can be added here
        // private void MenuSecondOrderPid_Click(object sender, EventArgs e)
        // {
        //     OpenSingleInstanceForm<SecondOrderProcessWithPidForm>("Second Order Process with PID");
        // }
        
        // private void MenuTankLevelPid_Click(object sender, EventArgs e)
        // {
        //     OpenSingleInstanceForm<TankLevelWithPidForm>("Tank Level with PID");
        // }
        
        // private void MenuTemperaturePid_Click(object sender, EventArgs e)
        // {
        //     OpenSingleInstanceForm<TemperatureWithPidForm>("Temperature Process with PID");
        // }

        #endregion

        #region Cleanup

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
                
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}