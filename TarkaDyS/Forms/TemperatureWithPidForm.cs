/*
 * File: TemperatureWithPidForm.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Industrial heat exchanger temperature control system form with PID control
 * Date Created: 2024-01-XX
 * Date Modified: 2024-01-XX
 * 
 * Description: Form for industrial heat exchanger temperature process with PID control simulation.
 * This form provides a complete interface for configuring and running temperature control
 * simulations with real-time plotting, multiple PID controller types, and parameter adjustment capabilities.
 * 
 * Modifications:
 * - 2024-01-XX: Initial creation with industrial heat exchanger focus
 * - 2024-01-XX: Added enhanced PID controller with multiple types support
 * - 2024-01-XX: Added output limits and anti-reset windup configuration
 * - 2024-01-XX: Added proper steady state initialization
 */

using TarkaDyS.ProcessModels;
using TarkaDyS.Core.Controllers;
using TarkaDyS.Core.Interfaces;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TarkaDyS.Forms
{
    /// <summary>
    /// Form for industrial heat exchanger temperature process with PID control simulation.
    /// This form provides a complete interface for configuring and running temperature control
    /// simulations with real-time plotting and parameter adjustment capabilities.
    /// Industrial heat exchanger example with steam heating and cooling water systems.
    /// </summary>
    public partial class TemperatureWithPidForm : Form
    {
        #region Private Fields
        private TemperatureWithPidModel? _temperaturePidModel;
        private readonly System.Windows.Forms.Timer _temperaturePlotUpdateTimer;
        private bool _disposed = false;
        
        // Temperature trend settings
        private double _temperatureTrendDurationSeconds = 600.0; // Default 10 minutes for temperature (slower process)

        // Temperature plot data storage
        private readonly List<DataPoint> _temperatureSetpointData = new();
        private readonly List<DataPoint> _temperatureProcessVariableData = new();
        private readonly List<DataPoint> _temperatureControllerOutputData = new();
        private readonly List<DataPoint> _temperatureErrorData = new();
        
        // Temperature plot series
        private LineSeries? _temperatureSetpointSeries;
        private LineSeries? _temperatureProcessVariableSeries;
        private LineSeries? _temperatureControllerOutputSeries;
        private LineSeries? _temperatureErrorSeries;
        
        // Temperature plot model and axes
        private PlotModel? _temperaturePlotModel;
        private LinearAxis? _temperatureTimeAxis;
        private LinearAxis? _temperatureValueAxis;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TemperatureWithPidForm class
        /// </summary>
        public TemperatureWithPidForm()
        {
            InitializeComponent();
            
            this.Text = "Industrial Heat Exchanger Temperature Control with PID";
            
            // Initialize timer for plot updates
            _temperaturePlotUpdateTimer = new System.Windows.Forms.Timer
            {
                Interval = 500, // Update every 500ms (slower than tank level due to thermal inertia)
                Enabled = false
            };
            _temperaturePlotUpdateTimer.Tick += TemperaturePlotUpdateTimer_Tick;
            
            // Add diagnostic shortcut (F12 key)
            this.KeyPreview = true;
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.F12)
                {
                    ShowDiagnosticInfo();
                }
            };
            
            InitializeTemperatureSimulation();
            InitializeTemperaturePlot();
            UpdateTemperatureUI();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the temperature simulation model
        /// </summary>
        private void InitializeTemperatureSimulation()
        {
            try
            {
                // Create the temperature with PID model
                _temperaturePidModel = new TemperatureWithPidModel(
                    Guid.NewGuid().ToString(), 
                    "Industrial Heat Exchanger Temperature Process"
                );

                // Set default PID parameters suitable for temperature control (slower response)
                _temperaturePidModel.TemperatureKp = 0.5;
                _temperaturePidModel.TemperatureKi = 0.02;
                _temperaturePidModel.TemperatureKd = 0.1;
                _temperaturePidModel.TemperatureSetpoint = 50.0; // 50% (equivalent to 50°C in 0-100°C range)
                
                // Set controller type to PI-D (good for temperature control to avoid derivative kick)
                _temperaturePidModel.TemperatureControllerType = PidControllerType.PI_D;

                // Subscribe to model events
                _temperaturePidModel.ParameterChanged += TemperatureProcessModel_ParameterChanged;
                _temperaturePidModel.ModelError += TemperatureProcessModel_ModelError;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing temperature simulation: {ex.Message}", 
                              "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Show diagnostic information for troubleshooting
        /// </summary>
        private void ShowDiagnosticInfo()
        {
            try
            {
                if (_temperaturePidModel != null)
                {
                    var info = $"=== TEMPERATURE DIAGNOSTIC INFO ===\n" +
                              $"Model Type: {_temperaturePidModel.GetType().Name}\n" +
                              $"Is Running: {_temperaturePidModel.IsRunning}\n" +
                              $"Current Time: {_temperaturePidModel.CurrentTime:F2}s\n" +
                              $"Controller Type: {_temperaturePidModel.TemperatureControllerType}\n" +
                              $"Closed Loop: {_temperaturePidModel.TemperatureClosedLoop}\n" +
                              $"Setpoint: {_temperaturePidModel.TemperatureSetpoint:F2}% ({_temperaturePidModel.TemperatureSetpoint:F1}°C)\n" +
                              $"Process Variable: {_temperaturePidModel.TemperatureProcessVariable:F2}% ({_temperaturePidModel.TemperatureProcessVariable:F1}°C)\n" +
                              $"Controller Output: {_temperaturePidModel.TemperatureControllerOutput:F2}%\n" +
                              $"Manual Output: {_temperaturePidModel.TemperatureManualOutput:F2}%\n" +
                              $"Error: {_temperaturePidModel.TemperatureError:F2}%\n" +
                              $"Output Limits: [{_temperaturePidModel.TemperatureOutputMin:F1}% - {_temperaturePidModel.TemperatureOutputMax:F1}%]\n" +
                              $"PID Gains: Kp={_temperaturePidModel.TemperatureKp:F3}, Ki={_temperaturePidModel.TemperatureKi:F3}, Kd={_temperaturePidModel.TemperatureKd:F3}\n" +
                              $"Mass: {_temperaturePidModel.TemperatureMass:F0}kg\n" +
                              $"Heat Transfer Coeff: {_temperaturePidModel.TemperatureHeatTransferCoeff:F0}W/K\n" +
                              $"Ambient Temp: {_temperaturePidModel.TemperatureAmbientTemp:F1}°C\n" +
                              $"Max Power: {_temperaturePidModel.TemperatureMaxPower:F0}W\n" +
                              $"Speed Multiplier: {_temperaturePidModel.SimulationSpeedMultiplier:F1}x\n" +
                              $"Time Step: {_temperaturePidModel.TimeStep:F3}s";
                              
                    MessageBox.Show(info, "Temperature Diagnostic", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Temperature Model is NULL!", "Diagnostic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting diagnostic info: {ex.Message}", "Diagnostic Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers - Placeholder Methods

        private void TemperatureProcessModel_ParameterChanged(object? sender, ModelParameterChangedEventArgs e)
        {
            // Implementation will be added with designer file
        }

        private void TemperatureProcessModel_ModelError(object? sender, ModelErrorEventArgs e)
        {
            // Implementation will be added with designer file
        }

        private void TemperaturePlotUpdateTimer_Tick(object? sender, EventArgs e)
        {
            // Implementation will be added with designer file
        }

        #endregion

        #region IDisposable Implementation

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _temperaturePlotUpdateTimer?.Stop();
                _temperaturePlotUpdateTimer?.Dispose();
                _temperaturePidModel?.Stop();
                _temperaturePidModel?.Dispose();
                _disposed = true;
            }
            
            base.Dispose(disposing);
        }

        #endregion
    }
}