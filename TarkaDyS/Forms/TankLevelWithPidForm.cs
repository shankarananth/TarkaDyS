using TarkaDyS.ProcessModels;
using TarkaDyS.Core.Interfaces;
using TarkaDyS.Core.Logging;
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
    /// Form for tank level process with PID control simulation.
    /// This form provides a complete interface for configuring and running tank level
    /// control simulations with real-time plotting and parameter adjustment capabilities.
    /// </summary>
    public partial class TankLevelWithPidForm : Form
    {
        #region Private Fields
        private TankLevelWithPidModel? _tankLevelPidModel;
        private readonly System.Windows.Forms.Timer _tankLevelPlotUpdateTimer;
        private bool _disposed = false;
        
        // Tank level trend settings
        private double _tankLevelTrendDurationSeconds = 300.0; // Default 5 minutes

        // Tank level plot data storage
        private readonly List<DataPoint> _tankLevelSetpointData = new();
        private readonly List<DataPoint> _tankLevelProcessVariableData = new();
        private readonly List<DataPoint> _tankLevelControllerOutputData = new();
        private readonly List<DataPoint> _tankLevelErrorData = new();
        
        // Tank level plot series
        private LineSeries? _tankLevelSetpointSeries;
        private LineSeries? _tankLevelProcessVariableSeries;
        private LineSeries? _tankLevelControllerOutputSeries;
        private LineSeries? _tankLevelErrorSeries;
        
        // Tank level plot model and axes
        private PlotModel? _tankLevelPlotModel;
        private LinearAxis? _tankLevelTimeAxis;
        private LinearAxis? _tankLevelValueAxis;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TankLevelWithPidForm class
        /// </summary>
        public TankLevelWithPidForm()
        {
            InitializeComponent();
            
            this.Text = "Tank Level Process with PID Control";
            
            // Initialize timer for plot updates
            _tankLevelPlotUpdateTimer = new System.Windows.Forms.Timer
            {
                Interval = 100, // Update every 100ms
                Enabled = false
            };
            _tankLevelPlotUpdateTimer.Tick += TankLevelPlotUpdateTimer_Tick;
            
            // Add diagnostic shortcut (F12 key)
            this.KeyPreview = true;
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.F12)
                {
                    ShowDiagnosticInfo();
                }
            };
            
            InitializeTankLevelSimulation();
            InitializeTankLevelPlot();
            UpdateTankLevelUI();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the tank level simulation model
        /// </summary>
        private void InitializeTankLevelSimulation()
        {
            try
            {
                // Create the tank level with PID model
                _tankLevelPidModel = new TankLevelWithPidModel(
                    Guid.NewGuid().ToString(), 
                    "Tank Level Process with PID"
                );

                // Set default PID parameters suitable for tank level control
                _tankLevelPidModel.TankLevelKp = 2.0;
                _tankLevelPidModel.TankLevelKi = 0.5;
                _tankLevelPidModel.TankLevelKd = 0.1;
                _tankLevelPidModel.TankLevelSetpoint = 5.0; // 5 meters

                // Subscribe to model events
                _tankLevelPidModel.ParameterChanged += TankLevelProcessModel_ParameterChanged;
                _tankLevelPidModel.ModelError += TankLevelProcessModel_ModelError;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing tank level simulation: {ex.Message}", 
                              "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initializes the tank level plot for data visualization
        /// </summary>
        private void InitializeTankLevelPlot()
        {
            try
            {
                // Create plot model for tank level
                _tankLevelPlotModel = new PlotModel
                {
                    Title = "Tank Level Process with PID Control Response",
                    Background = OxyColors.White
                };

                // Add axes specific to tank level (meters for level, m³/min for flow)
                _tankLevelTimeAxis = new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Time (seconds)",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                    MinorGridlineColor = OxyColor.FromRgb(240, 240, 240),
                    Minimum = 0,
                    Maximum = _tankLevelTrendDurationSeconds
                };
                _tankLevelPlotModel.Axes.Add(_tankLevelTimeAxis);

                _tankLevelValueAxis = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Level (m) / Flow (m³/min)",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                    MinorGridlineColor = OxyColor.FromRgb(240, 240, 240)
                };
                _tankLevelPlotModel.Axes.Add(_tankLevelValueAxis);

                // Create series for tank level
                _tankLevelSetpointSeries = new LineSeries
                {
                    Title = "Tank Level Setpoint (m)",
                    Color = OxyColors.Blue,
                    StrokeThickness = 2,
                    LineStyle = LineStyle.Dash
                };
                _tankLevelPlotModel.Series.Add(_tankLevelSetpointSeries);

                _tankLevelProcessVariableSeries = new LineSeries
                {
                    Title = "Tank Level (PV) (m)",
                    Color = OxyColors.Green,
                    StrokeThickness = 2
                };
                _tankLevelPlotModel.Series.Add(_tankLevelProcessVariableSeries);

                _tankLevelControllerOutputSeries = new LineSeries
                {
                    Title = "Inflow Rate (MV) (m³/min)",
                    Color = OxyColors.Red,
                    StrokeThickness = 2
                };
                _tankLevelPlotModel.Series.Add(_tankLevelControllerOutputSeries);

                _tankLevelErrorSeries = new LineSeries
                {
                    Title = "Level Error (m)",
                    Color = OxyColors.Orange,
                    StrokeThickness = 1,
                    LineStyle = LineStyle.Dot
                };
                _tankLevelPlotModel.Series.Add(_tankLevelErrorSeries);

                // Set plot model to plot view
                if (plotTankLevelView != null)
                {
                    plotTankLevelView.Model = _tankLevelPlotModel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing tank level plot: {ex.Message}", 
                              "Plot Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates the tank level user interface with current model values
        /// EMERGENCY FIX: No logging to prevent mouse-movement freezing
        /// FIXED: Updated for percentage units and output limits
        /// </summary>
        private void UpdateTankLevelUI()
        {
            if (_tankLevelPidModel == null) return;

            try
            {
                // EMERGENCY FIX: Completely disable UI update logging
                // Mouse movement triggers UI events that compete with logging I/O

                // Update tank level specific numeric controls (only if they don't have focus to prevent interference)
                if (numTankLevelSetpoint != null && !numTankLevelSetpoint.Focused) 
                {
                    if (Math.Abs((double)numTankLevelSetpoint.Value - _tankLevelPidModel.TankLevelSetpoint) > 0.01)
                    {
                        numTankLevelSetpoint.Value = (decimal)_tankLevelPidModel.TankLevelSetpoint;
                    }
                }
                
                // Update PID parameters (only if they don't have focus)
                if (numTankLevelKp != null && !numTankLevelKp.Focused) 
                    numTankLevelKp.Value = (decimal)_tankLevelPidModel.TankLevelKp;
                if (numTankLevelKi != null && !numTankLevelKi.Focused) 
                    numTankLevelKi.Value = (decimal)_tankLevelPidModel.TankLevelKi;
                if (numTankLevelKd != null && !numTankLevelKd.Focused) 
                    numTankLevelKd.Value = (decimal)_tankLevelPidModel.TankLevelKd;
                
                // Update tank parameters
                if (numTankLevelCrossSectionArea != null && !numTankLevelCrossSectionArea.Focused) 
                    numTankLevelCrossSectionArea.Value = (decimal)_tankLevelPidModel.TankLevelCrossSectionArea;
                if (numTankLevelOutflowRate != null && !numTankLevelOutflowRate.Focused) 
                    numTankLevelOutflowRate.Value = (decimal)_tankLevelPidModel.TankLevelOutflowRate;
                if (numTankLevelMaxLevel != null && !numTankLevelMaxLevel.Focused) 
                    numTankLevelMaxLevel.Value = (decimal)_tankLevelPidModel.TankLevelMaxLevel;
                if (numTankLevelDisturbance != null && !numTankLevelDisturbance.Focused) 
                    numTankLevelDisturbance.Value = (decimal)_tankLevelPidModel.TankLevelDisturbance;
                
                // Update Auto/Manual button states
                if (btnTankLevelAuto != null && btnTankLevelManual != null)
                {
                    if (_tankLevelPidModel.TankLevelClosedLoop)
                    {
                        btnTankLevelAuto.BackColor = Color.LightGreen;
                        btnTankLevelManual.BackColor = Color.LightGray;
                        btnTankLevelAuto.Text = "AUTOMATIC";
                        btnTankLevelManual.Text = "MANUAL";
                    }
                    else
                    {
                        btnTankLevelAuto.BackColor = Color.LightGray;
                        btnTankLevelManual.BackColor = Color.LightCoral;
                        btnTankLevelAuto.Text = "AUTOMATIC";
                        btnTankLevelManual.Text = "MANUAL";
                    }
                }
                
                // Update manual output control with proper enabling logic
                if (numTankLevelManualOutput != null)
                {
                    if (_tankLevelPidModel.TankLevelClosedLoop)
                    {
                        // In Auto mode: Manual output tracks current controller output (bumpless transfer)
                        if (!numTankLevelManualOutput.Focused)
                        {
                            numTankLevelManualOutput.Value = (decimal)_tankLevelPidModel.TankLevelControllerOutput;
                            _tankLevelPidModel.TankLevelManualOutput = _tankLevelPidModel.TankLevelControllerOutput;
                        }
                        numTankLevelManualOutput.Enabled = false;
                        numTankLevelManualOutput.BackColor = Color.LightGray;
                    }
                    else
                    {
                        // In Manual mode: User can edit
                        if (!numTankLevelManualOutput.Focused && 
                            Math.Abs((double)numTankLevelManualOutput.Value - _tankLevelPidModel.TankLevelManualOutput) > 0.01)
                        {
                            numTankLevelManualOutput.Value = (decimal)_tankLevelPidModel.TankLevelManualOutput;
                        }
                        numTankLevelManualOutput.Enabled = true;
                        numTankLevelManualOutput.BackColor = Color.White;
                    }
                }
                
                // Update setpoint tracking checkbox
                if (chkTankLevelSetpointTracking != null)
                {
                    chkTankLevelSetpointTracking.Checked = _tankLevelPidModel.TankLevelSetpointTracking;
                    chkTankLevelSetpointTracking.Enabled = !_tankLevelPidModel.TankLevelClosedLoop;
                }

                // Update tank level display labels (FIXED: Changed to percentage units)
                if (lblTankLevelProcessVariable != null) 
                    lblTankLevelProcessVariable.Text = _tankLevelPidModel.TankLevelProcessVariable.ToString("F2") + "%";
                if (lblTankLevelControllerOutput != null) 
                    lblTankLevelControllerOutput.Text = _tankLevelPidModel.TankLevelControllerOutput.ToString("F2") + "%";
                if (lblTankLevelError != null) 
                    lblTankLevelError.Text = _tankLevelPidModel.TankLevelError.ToString("F2") + "%";
                if (lblTankLevelSimulationTime != null) 
                    lblTankLevelSimulationTime.Text = _tankLevelPidModel.CurrentTime.ToString("F1") + "s";
                
                // Update button states - Keep controls enabled during simulation
                if (btnTankLevelStart != null) btnTankLevelStart.Enabled = !_tankLevelPidModel.IsRunning;
                if (btnTankLevelStop != null) btnTankLevelStop.Enabled = _tankLevelPidModel.IsRunning;
                if (btnTankLevelReset != null) btnTankLevelReset.Enabled = !_tankLevelPidModel.IsRunning;
                
                // Keep parameter controls enabled during simulation for real-time tuning
                if (numTankLevelKp != null) numTankLevelKp.Enabled = true;
                if (numTankLevelKi != null) numTankLevelKi.Enabled = true;
                if (numTankLevelKd != null) numTankLevelKd.Enabled = true;
                if (numTankLevelCrossSectionArea != null) numTankLevelCrossSectionArea.Enabled = true;
                if (numTankLevelOutflowRate != null) numTankLevelOutflowRate.Enabled = true;
                if (numTankLevelMaxLevel != null) numTankLevelMaxLevel.Enabled = true;
                if (numTankLevelDisturbance != null) numTankLevelDisturbance.Enabled = true;
                if (numTankLevelSetpoint != null) numTankLevelSetpoint.Enabled = true;
                if (btnTankLevelAuto != null) btnTankLevelAuto.Enabled = true;
                if (btnTankLevelManual != null) btnTankLevelManual.Enabled = true;
            }
            catch (Exception ex)
            {
                // EMERGENCY: Only log to debug output, no file I/O
                System.Diagnostics.Debug.WriteLine($"Error updating tank level UI: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the tank level plot with current data
        /// </summary>
        private void UpdateTankLevelPlot()
        {
            if (_tankLevelPidModel == null || _tankLevelPlotModel == null) return;

            try
            {
                double currentTime = _tankLevelPidModel.CurrentTime;
                
                // Add new data points
                _tankLevelSetpointData.Add(new DataPoint(currentTime, _tankLevelPidModel.TankLevelSetpoint));
                _tankLevelProcessVariableData.Add(new DataPoint(currentTime, _tankLevelPidModel.TankLevelProcessVariable));
                _tankLevelControllerOutputData.Add(new DataPoint(currentTime, _tankLevelPidModel.TankLevelControllerOutput));
                _tankLevelErrorData.Add(new DataPoint(currentTime, _tankLevelPidModel.TankLevelError));

                // Remove data points outside the trend duration window
                double cutoffTime = currentTime - _tankLevelTrendDurationSeconds;
                _tankLevelSetpointData.RemoveAll(dp => dp.X < cutoffTime);
                _tankLevelProcessVariableData.RemoveAll(dp => dp.X < cutoffTime);
                _tankLevelControllerOutputData.RemoveAll(dp => dp.X < cutoffTime);
                _tankLevelErrorData.RemoveAll(dp => dp.X < cutoffTime);

                // Update series
                _tankLevelSetpointSeries?.Points.Clear();
                _tankLevelSetpointSeries?.Points.AddRange(_tankLevelSetpointData);

                _tankLevelProcessVariableSeries?.Points.Clear();
                _tankLevelProcessVariableSeries?.Points.AddRange(_tankLevelProcessVariableData);

                _tankLevelControllerOutputSeries?.Points.Clear();
                _tankLevelControllerOutputSeries?.Points.AddRange(_tankLevelControllerOutputData);

                _tankLevelErrorSeries?.Points.Clear();
                _tankLevelErrorSeries?.Points.AddRange(_tankLevelErrorData);

                // Update time axis
                if (_tankLevelTimeAxis != null)
                {
                    if (currentTime > _tankLevelTrendDurationSeconds)
                    {
                        _tankLevelTimeAxis.Minimum = currentTime - _tankLevelTrendDurationSeconds;
                        _tankLevelTimeAxis.Maximum = currentTime;
                    }
                    else
                    {
                        _tankLevelTimeAxis.Minimum = 0;
                        _tankLevelTimeAxis.Maximum = _tankLevelTrendDurationSeconds;
                    }
                }

                // Refresh plot
                plotTankLevelView?.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating tank level plot: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears all tank level plot data
        /// </summary>
        private void ClearTankLevelPlotData()
        {
            _tankLevelSetpointData.Clear();
            _tankLevelProcessVariableData.Clear();
            _tankLevelControllerOutputData.Clear();
            _tankLevelErrorData.Clear();

            _tankLevelSetpointSeries?.Points.Clear();
            _tankLevelProcessVariableSeries?.Points.Clear();
            _tankLevelControllerOutputSeries?.Points.Clear();
            _tankLevelErrorSeries?.Points.Clear();

            // Reset time axis
            if (_tankLevelTimeAxis != null)
            {
                _tankLevelTimeAxis.Minimum = 0;
                _tankLevelTimeAxis.Maximum = _tankLevelTrendDurationSeconds;
            }

            plotTankLevelView?.InvalidatePlot(true);
        }

        /// <summary>
        /// Show diagnostic information for troubleshooting
        /// FIXED: Updated for percentage units
        /// </summary>
        private void ShowDiagnosticInfo()
        {
            try
            {
                if (_tankLevelPidModel != null)
                {
                    var info = $"=== TANK LEVEL DIAGNOSTIC INFO ===\n" +
                              $"Model Type: {_tankLevelPidModel.GetType().Name}\n" +
                              $"Is Running: {_tankLevelPidModel.IsRunning}\n" +
                              $"Current Time: {_tankLevelPidModel.CurrentTime:F2}s\n" +
                              $"Closed Loop: {_tankLevelPidModel.TankLevelClosedLoop}\n" +
                              $"Setpoint: {_tankLevelPidModel.TankLevelSetpoint:F2}%\n" +
                              $"Process Variable: {_tankLevelPidModel.TankLevelProcessVariable:F2}%\n" +
                              $"Controller Output: {_tankLevelPidModel.TankLevelControllerOutput:F2}%\n" +
                              $"Manual Output: {_tankLevelPidModel.TankLevelManualOutput:F2}%\n" +
                              $"Error: {_tankLevelPidModel.TankLevelError:F2}%\n" +
                              $"Tank Area: {_tankLevelPidModel.TankLevelCrossSectionArea:F2}m²\n" +
                              $"Outflow Rate: {_tankLevelPidModel.TankLevelOutflowRate:F2}%\n" +
                              $"Max Level: {_tankLevelPidModel.TankLevelMaxLevel:F2}%\n" +
                              $"PID Gains: Kp={_tankLevelPidModel.TankLevelKp:F3}, Ki={_tankLevelPidModel.TankLevelKi:F3}, Kd={_tankLevelPidModel.TankLevelKd:F3}\n" +
                              $"Speed Multiplier: {_tankLevelPidModel.SimulationSpeedMultiplier:F1}x\n" +
                              $"Time Step: {_tankLevelPidModel.TimeStep:F3}s";
                              
                    MessageBox.Show(info, "Tank Level Diagnostic", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Tank Level Model is NULL!", "Diagnostic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting diagnostic info: {ex.Message}", "Diagnostic Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        private void TankLevelProcessModel_ParameterChanged(object? sender, ModelParameterChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => TankLevelProcessModel_ParameterChanged(sender, e)));
                return;
            }

            UpdateTankLevelUI();
        }

        private void TankLevelProcessModel_ModelError(object? sender, ModelErrorEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => TankLevelProcessModel_ModelError(sender, e)));
                return;
            }

            MessageBox.Show($"Tank Level Model Error: {e.ErrorMessage}", "Simulation Error", 
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void TankLevelPlotUpdateTimer_Tick(object? sender, EventArgs e)
        {
            UpdateTankLevelPlot();
            UpdateTankLevelUI();
        }

        private void BtnTankLevelStart_Click(object? sender, EventArgs e)
        {
            try
            {
                _tankLevelPidModel?.Start();
                _tankLevelPlotUpdateTimer.Enabled = true;
                UpdateTankLevelUI();
                
                System.Diagnostics.Debug.WriteLine("=== TANK LEVEL SIMULATION STARTED ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error starting tank level simulation: {ex.Message}");
                MessageBox.Show($"Error starting tank level simulation: {ex.Message}", 
                              "Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTankLevelStop_Click(object? sender, EventArgs e)
        {
            try
            {
                _tankLevelPidModel?.Stop();
                _tankLevelPlotUpdateTimer.Enabled = false;
                UpdateTankLevelUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping tank level simulation: {ex.Message}", 
                              "Stop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTankLevelReset_Click(object? sender, EventArgs e)
        {
            try
            {
                _tankLevelPidModel?.Reset();
                ClearTankLevelPlotData();
                UpdateTankLevelUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting tank level simulation: {ex.Message}", 
                              "Reset Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles tank level parameter value changes
        /// </summary>
        private void TankLevelParameterValue_Changed(object? sender, EventArgs e)
        {
            if (_tankLevelPidModel == null) return;

            try
            {
                // Update tank level model parameters based on which control changed
                if (sender == numTankLevelSetpoint) 
                {
                    _tankLevelPidModel.TankLevelSetpoint = (double)numTankLevelSetpoint.Value;
                }
                else if (sender == numTankLevelKp) 
                {
                    _tankLevelPidModel.TankLevelKp = (double)numTankLevelKp.Value;
                }
                else if (sender == numTankLevelKi) 
                {
                    _tankLevelPidModel.TankLevelKi = (double)numTankLevelKi.Value;
                }
                else if (sender == numTankLevelKd) 
                {
                    _tankLevelPidModel.TankLevelKd = (double)numTankLevelKd.Value;
                }
                else if (sender == numTankLevelCrossSectionArea) 
                {
                    _tankLevelPidModel.TankLevelCrossSectionArea = (double)numTankLevelCrossSectionArea.Value;
                }
                else if (sender == numTankLevelOutflowRate) 
                {
                    _tankLevelPidModel.TankLevelOutflowRate = (double)numTankLevelOutflowRate.Value;
                }
                else if (sender == numTankLevelMaxLevel) 
                {
                    _tankLevelPidModel.TankLevelMaxLevel = (double)numTankLevelMaxLevel.Value;
                }
                else if (sender == numTankLevelDisturbance) 
                {
                    _tankLevelPidModel.TankLevelDisturbance = (double)numTankLevelDisturbance.Value;
                }
                else if (sender == numTankLevelManualOutput) 
                {
                    _tankLevelPidModel.TankLevelManualOutput = (double)numTankLevelManualOutput.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating tank level parameter: {ex.Message}", 
                              "Parameter Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the tank level Auto button click
        /// </summary>
        private void BtnTankLevelAuto_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_tankLevelPidModel != null)
                {
                    _tankLevelPidModel.TankLevelClosedLoop = true;
                    UpdateTankLevelUI();
                    ControlLoopLogger.LogInfo("Tank level switched to Auto mode");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error switching to Auto mode: {ex.Message}", 
                              "Mode Switch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the tank level Manual button click
        /// </summary>
        private void BtnTankLevelManual_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_tankLevelPidModel != null)
                {
                    _tankLevelPidModel.TankLevelClosedLoop = false;
                    UpdateTankLevelUI();
                    ControlLoopLogger.LogInfo("Tank level switched to Manual mode");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error switching to Manual mode: {ex.Message}", 
                              "Mode Switch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the tank level setpoint tracking checkbox changed event
        /// </summary>
        private void ChkTankLevelSetpointTracking_CheckedChanged(object? sender, EventArgs e)
        {
            if (_tankLevelPidModel != null && sender is CheckBox checkBox)
            {
                _tankLevelPidModel.TankLevelSetpointTracking = checkBox.Checked;
                UpdateTankLevelUI();
            }
        }

        #endregion

        #region IDisposable Implementation

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _tankLevelPlotUpdateTimer?.Stop();
                _tankLevelPlotUpdateTimer?.Dispose();
                _tankLevelPidModel?.Stop();
                _tankLevelPidModel?.Dispose();
                _disposed = true;
            }
            
            base.Dispose(disposing);
        }

        #endregion
    }
}