using TarkaDyS.Core.Factories;
using TarkaDyS.Core.Interfaces;
using TarkaDyS.Core.Utilities;
using TarkaDyS.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TarkaDyS
{
    /// <summary>
    /// Main MDI parent form for the TarkaDyS Process Dynamic Simulation Software.
    /// This form serves as the container for all child simulation forms and provides
    /// the main application interface including menus, toolbars, and status information.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Private Fields
        private int _childFormCounter = 0;
        private readonly Dictionary<string, Form> _openChildForms = new();
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm class
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeApplication();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the application settings and UI
        /// </summary>
        private void InitializeApplication()
        {
            // Set up MDI properties
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "TarkaDyS - Process Dynamic Simulation Software";

            // Set up form properties
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);

            // Initialize status strip if it exists
            UpdateStatusBar("Ready");
        }

        /// <summary>
        /// Updates the status bar with the specified message
        /// </summary>
        /// <param name="message">Message to display</param>
        private void UpdateStatusBar(string message)
        {
            if (statusLabel != null)
            {
                statusLabel.Text = message;
            }
            else
            {
                // Fallback: update form title if status label is not available
                this.Text = $"TarkaDyS - Process Dynamic Simulation Software - {message}";
            }
        }

        /// <summary>
        /// Creates a new child form for the specified process model type
        /// </summary>
        /// <param name="modelType">Type of process model to create</param>
        private void CreateChildForm(ProcessModelType modelType)
        {
            try
            {
                _childFormCounter++;
                string formName = $"{ProcessModelFactory.GetModelInfo(modelType).Name} {_childFormCounter}";
                
                Form? childForm = null;

                // Create appropriate child form based on model type
                switch (modelType)
                {
                    case ProcessModelType.PidProcess:
                        childForm = new FirstOrderWithPidForm();
                        break;
                    
                    case ProcessModelType.LevelProcess:
                        childForm = new TankLevelWithPidForm();
                        break;
                    
                    case ProcessModelType.TemperatureProcess:
                        // TODO: Implement TemperatureWithPidForm
                        MessageBox.Show("Temperature control process form is not yet implemented.", 
                                      "Coming Soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    
                    default:
                        MessageBox.Show($"Process model type '{modelType}' is not yet implemented.", 
                                      "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                }

                if (childForm != null)
                {
                    // Configure child form
                    childForm.MdiParent = this;
                    childForm.FormClosed += ChildForm_FormClosed;
                    
                    // Add to tracking dictionary
                    _openChildForms[childForm.Name] = childForm;
                    
                    // Show the form
                    childForm.Show();
                    
                    UpdateStatusBar($"Opened {formName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating child form: {ex.Message}", 
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatusBar("Error creating child form");
            }
        }

        /// <summary>
        /// Handles the FormClosed event for child forms
        /// </summary>
        private void ChildForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (sender is Form childForm)
            {
                _openChildForms.Remove(childForm.Name);
                childForm.FormClosed -= ChildForm_FormClosed;
                
                UpdateStatusBar($"Closed {childForm.Text}");
            }
        }

        /// <summary>
        /// Closes all open child forms
        /// </summary>
        private void CloseAllChildForms()
        {
            try
            {
                var formsToClose = _openChildForms.Values.ToList();
                foreach (var form in formsToClose)
                {
                    form.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing child forms: {ex.Message}", 
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event for the New First Order Process with PID menu item
        /// </summary>
        private void NewPidProcess_Click(object? sender, EventArgs e)
        {
            CreateChildForm(ProcessModelType.PidProcess);
        }

        /// <summary>
        /// Handles the Click event for the New Tank Level Process with PID menu item
        /// </summary>
        private void NewLevelProcess_Click(object? sender, EventArgs e)
        {
            CreateChildForm(ProcessModelType.LevelProcess);
        }

        /// <summary>
        /// Handles the Click event for the New Temperature Process with PID menu item
        /// </summary>
        private void NewTemperatureProcess_Click(object? sender, EventArgs e)
        {
            CreateChildForm(ProcessModelType.TemperatureProcess);
        }

        /// <summary>
        /// Handles the Click event for the Exit menu item
        /// </summary>
        private void Exit_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Click event for the About menu item
        /// </summary>
        private void About_Click(object? sender, EventArgs e)
        {
            string aboutMessage = "TarkaDyS - Process Dynamic Simulation Software\n\n" +
                                 "Version 1.0.0\n" +
                                 "Built with .NET 8.0\n\n" +
                                 "This software provides dynamic simulation capabilities for various process models\n" +
                                 "including PID controllers, first-order processes, and other control systems.\n\n" +
                                 "Features:\n" +
                                 "• First-Order Process with PID Controller Simulation (based on VB PID Simulator)\n" +
                                 "• Real-time Plotting and Data Visualization\n" +
                                 "• Parameter Tuning and Analysis\n" +
                                 "• Extensible Architecture for Additional Process Models";

            MessageBox.Show(aboutMessage, "About TarkaDyS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles the FormClosing event to ensure proper cleanup
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // Check if there are unsaved changes in any child forms
                bool hasUnsavedChanges = false;
                foreach (var childForm in _openChildForms.Values)
                {
                    // Note: Removed save functionality - forms start fresh each time
                    // if (childForm is FirstOrderWithPidForm pidForm && pidForm.HasUnsavedChanges)
                    // {
                    //     hasUnsavedChanges = true;
                    //     break;
                    // }
                }

                if (hasUnsavedChanges)
                {
                    var result = MessageBox.Show(
                        "There are unsaved changes in one or more simulation forms. Do you want to exit without saving?",
                        "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                // Close all child forms
                CloseAllChildForms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during application shutdown: {ex.Message}", 
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            base.OnFormClosing(e);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the number of currently open child forms
        /// </summary>
        /// <returns>Number of open child forms</returns>
        public int GetOpenChildFormCount()
        {
            return _openChildForms.Count;
        }

        /// <summary>
        /// Gets a list of currently open child form names
        /// </summary>
        /// <returns>List of child form names</returns>
        public List<string> GetOpenChildFormNames()
        {
            return new List<string>(_openChildForms.Keys);
        }

        #endregion
    }
}