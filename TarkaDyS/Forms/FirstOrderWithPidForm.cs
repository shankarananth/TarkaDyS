using TarkaDyS.Core.Factories;
using TarkaDyS.Core.Interfaces;
using TarkaDyS.Core.Logging;
using TarkaDyS.ProcessModels;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TarkaDyS.Forms
{
    /// <summary>
    /// Form for first-order process with PID control simulation.
    /// This form provides a complete interface for configuring and running first-order process
    /// control simulations with real-time plotting and parameter adjustment capabilities.
    /// </summary>
    public partial class FirstOrderWithPidForm : Form
    {
        #region Private Fields
        private FirstOrderWithPidModel? _firstOrderPidModel;
        private readonly System.Windows.Forms.Timer _firstOrderPlotUpdateTimer;
        private bool _disposed = false;
        
        // First-order trend settings
        private double _firstOrderTrendDurationSeconds = 300.0; // Default 5 minutes

        // First-order plot data storage
        private readonly List<DataPoint> _firstOrderSetpointData = new();
        private readonly List<DataPoint> _firstOrderProcessVariableData = new();
        private readonly List<DataPoint> _firstOrderControllerOutputData = new();
        private readonly List<DataPoint> _firstOrderErrorData = new();
        
        // First-order plot series
        private LineSeries? _firstOrderSetpointSeries;
        private LineSeries? _firstOrderProcessVariableSeries;
        private LineSeries? _firstOrderControllerOutputSeries;
        private LineSeries? _firstOrderErrorSeries;
        
        // First-order plot model and axes
        private PlotModel? _firstOrderPlotModel;
        private LinearAxis? _firstOrderTimeAxis;
        private LinearAxis? _firstOrderValueAxis;
        
        // FIXED: Add performance monitoring
        private DateTime _lastUpdateTime = DateTime.Now;
        private int _performanceWarningCount = 0;
        private int _plotUpdateCounter = 0;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FirstOrderWithPidForm class
        /// FIXED: Optimized timer intervals to prevent freezing
        /// </summary>
        public FirstOrderWithPidForm()
        {
            InitializeComponent();
            
            this.Text = "First-Order Process with PID Control";
            
            // FIXED: Increased timer interval to prevent excessive UI updates
            // 200ms instead of 100ms reduces CPU load and prevents freezing
            _firstOrderPlotUpdateTimer = new System.Windows.Forms.Timer
            {
                Interval = 200, // FIXED: Update every 200ms instead of 100ms
                Enabled = false
            };
            _firstOrderPlotUpdateTimer.Tick += FirstOrderPlotUpdateTimer_Tick;
            
            // Add diagnostic shortcut (F12 key) and emergency recovery (F11 key)
            this.KeyPreview = true;
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.F12)
                {
                    ShowDiagnosticInfo();
                }
                else if (e.KeyCode == Keys.F11)
                {
                    EmergencyRecovery();
                }
            };
            
            InitializeFirstOrderSimulation();
            InitializeFirstOrderPlot();
            UpdateFirstOrderUI();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the first-order simulation model
        /// </summary>
        private void InitializeFirstOrderSimulation()
        {
            try
            {
                // Create the first-order with PID model
                _firstOrderPidModel = new FirstOrderWithPidModel(
                    Guid.NewGuid().ToString(), 
                    "First-Order Process with PID"
                );
                
                // Set default parameters
                _firstOrderPidModel.SetFirstOrderPidParameters(1.0, 0.1, 0.05);
                _firstOrderPidModel.SetFirstOrderProcessParameters(1.0, 10.0, 1.0);
                _firstOrderPidModel.SetFirstOrderOutputLimits(0.0, 100.0);
                _firstOrderPidModel.FirstOrderSetpoint = 50.0;

                // Subscribe to model events
                _firstOrderPidModel.ParameterChanged += FirstOrderProcessModel_ParameterChanged;
                _firstOrderPidModel.ModelError += FirstOrderProcessModel_ModelError;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing first-order simulation: {ex.Message}", 
                              "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initializes the first-order plot for data visualization
        /// </summary>
        private void InitializeFirstOrderPlot()
        {
            try
            {
                // Create plot model
                _firstOrderPlotModel = new PlotModel
                {
                    Title = "First-Order Process with PID Control Response",
                    Background = OxyColors.White
                };

                // Add axes
                _firstOrderTimeAxis = new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Time (seconds)",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                    MinorGridlineColor = OxyColor.FromRgb(240, 240, 240),
                    Minimum = 0,
                    Maximum = _firstOrderTrendDurationSeconds
                };
                _firstOrderPlotModel.Axes.Add(_firstOrderTimeAxis);

                _firstOrderValueAxis = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Value",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                    MinorGridlineColor = OxyColor.FromRgb(240, 240, 240)
                };
                _firstOrderPlotModel.Axes.Add(_firstOrderValueAxis);

                // Create series
                _firstOrderSetpointSeries = new LineSeries
                {
                    Title = "Setpoint",
                    Color = OxyColors.Blue,
                    StrokeThickness = 2,
                    LineStyle = LineStyle.Dash
                };
                _firstOrderPlotModel.Series.Add(_firstOrderSetpointSeries);

                _firstOrderProcessVariableSeries = new LineSeries
                {
                    Title = "Process Variable",
                    Color = OxyColors.Green,
                    StrokeThickness = 2
                };
                _firstOrderPlotModel.Series.Add(_firstOrderProcessVariableSeries);

                _firstOrderControllerOutputSeries = new LineSeries
                {
                    Title = "Controller Output",
                    Color = OxyColors.Red,
                    StrokeThickness = 2
                };
                _firstOrderPlotModel.Series.Add(_firstOrderControllerOutputSeries);

                _firstOrderErrorSeries = new LineSeries
                {
                    Title = "Error",
                    Color = OxyColors.Orange,
                    StrokeThickness = 1,
                    LineStyle = LineStyle.Dot
                };
                _firstOrderPlotModel.Series.Add(_firstOrderErrorSeries);

                // Set plot model to plot view
                if (plotFirstOrderView != null)
                {
                    plotFirstOrderView.Model = _firstOrderPlotModel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing first-order plot: {ex.Message}", 
                              "Plot Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates the first-order user interface with current model values
        /// EMERGENCY FIX: No logging to prevent mouse-movement freezing
        /// </summary>
        private void UpdateFirstOrderUI()
        {
            if (_firstOrderPidModel == null) return;

            try
            {
                // EMERGENCY FIX: Completely disable UI update logging
                // Mouse movement triggers UI events that compete with logging I/O

                // Update first-order specific numeric controls (only if they don't have focus to prevent interference)
                if (numFirstOrderSetpoint != null && !numFirstOrderSetpoint.Focused) 
                {
                    if (Math.Abs((double)numFirstOrderSetpoint.Value - _firstOrderPidModel.FirstOrderSetpoint) > 0.01)
                    {
                        numFirstOrderSetpoint.Value = (decimal)_firstOrderPidModel.FirstOrderSetpoint;
                    }
                }
                
                // Only update parameter controls if they don't have focus (allows user to edit during simulation)
                if (numFirstOrderKp != null && !numFirstOrderKp.Focused) 
                    numFirstOrderKp.Value = (decimal)_firstOrderPidModel.FirstOrderKp;
                if (numFirstOrderKi != null && !numFirstOrderKi.Focused) 
                    numFirstOrderKi.Value = (decimal)_firstOrderPidModel.FirstOrderKi;
                if (numFirstOrderKd != null && !numFirstOrderKd.Focused) 
                    numFirstOrderKd.Value = (decimal)_firstOrderPidModel.FirstOrderKd;
                if (numFirstOrderProcessGain != null && !numFirstOrderProcessGain.Focused) 
                    numFirstOrderProcessGain.Value = (decimal)_firstOrderPidModel.FirstOrderProcessGain;
                if (numFirstOrderTimeConstant != null && !numFirstOrderTimeConstant.Focused) 
                    numFirstOrderTimeConstant.Value = (decimal)_firstOrderPidModel.FirstOrderTimeConstant;
                if (numFirstOrderDeadTime != null && !numFirstOrderDeadTime.Focused) 
                    numFirstOrderDeadTime.Value = (decimal)_firstOrderPidModel.FirstOrderDeadTime;
                if (numFirstOrderDisturbance != null && !numFirstOrderDisturbance.Focused) 
                    numFirstOrderDisturbance.Value = (decimal)_firstOrderPidModel.FirstOrderDisturbance;
                
                // Update simulation speed trackbar and label
                if (trkSimulationSpeed != null && lblSimulationSpeedValue != null)
                {
                    int expectedTrackbarValue = (int)Math.Round(_firstOrderPidModel.SimulationSpeedMultiplier * 10);
                    if (trkSimulationSpeed.Value != expectedTrackbarValue && !trkSimulationSpeed.Focused)
                    {
                        trkSimulationSpeed.Value = Math.Max(1, Math.Min(100, expectedTrackbarValue));
                    }
                    
                    lblSimulationSpeedValue.Text = $"{_firstOrderPidModel.SimulationSpeedMultiplier:F1}x";
                    
                    // Color coding for speed
                    if (_firstOrderPidModel.SimulationSpeedMultiplier < 1.0)
                        lblSimulationSpeedValue.ForeColor = Color.Orange;
                    else if (_firstOrderPidModel.SimulationSpeedMultiplier == 1.0)
                        lblSimulationSpeedValue.ForeColor = Color.Green;
                    else
                        lblSimulationSpeedValue.ForeColor = Color.Red;
                }
                
                // Update first-order Auto/Manual button states
                if (btnFirstOrderAuto != null && btnFirstOrderManual != null)
                {
                    if (_firstOrderPidModel.FirstOrderClosedLoop)
                    {
                        btnFirstOrderAuto.BackColor = Color.LightGreen;
                        btnFirstOrderManual.BackColor = Color.LightGray;
                        btnFirstOrderAuto.Text = "AUTOMATIC";
                        btnFirstOrderManual.Text = "MANUAL";
                    }
                    else
                    {
                        btnFirstOrderAuto.BackColor = Color.LightGray;
                        btnFirstOrderManual.BackColor = Color.LightCoral;
                        btnFirstOrderAuto.Text = "AUTOMATIC";
                        btnFirstOrderManual.Text = "MANUAL";
                    }
                }
                
                // Update first-order manual output control - KEY FIX HERE!
                if (numFirstOrderManualOutput != null)
                {
                    // CRITICAL FIX: In Auto mode, continuously sync manual output with current MV
                    // This ensures bumpless transfer when switching to Manual mode
                    if (_firstOrderPidModel.FirstOrderClosedLoop)
                    {
                        // In Auto mode: Manual output tracks current controller output (bumpless transfer preparation)
                        if (!numFirstOrderManualOutput.Focused)
                        {
                            numFirstOrderManualOutput.Value = (decimal)_firstOrderPidModel.FirstOrderControllerOutput;
                            // Update the model's internal manual output value to stay in sync
                            _firstOrderPidModel.FirstOrderManualOutput = _firstOrderPidModel.FirstOrderControllerOutput;
                        }
                        numFirstOrderManualOutput.Enabled = false; // Disabled in Auto mode
                        numFirstOrderManualOutput.BackColor = Color.LightGray;
                    }
                    else
                    {
                        // In Manual mode: User can edit, and it controls the actual output
                        if (!numFirstOrderManualOutput.Focused && 
                            Math.Abs((double)numFirstOrderManualOutput.Value - _firstOrderPidModel.FirstOrderManualOutput) > 0.01)
                        {
                            numFirstOrderManualOutput.Value = (decimal)_firstOrderPidModel.FirstOrderManualOutput;
                        }
                        numFirstOrderManualOutput.Enabled = true; // Enabled in Manual mode
                        numFirstOrderManualOutput.BackColor = Color.White;
                    }
                }
                
                // Update first-order setpoint tracking checkbox
                if (chkFirstOrderSetpointTracking != null)
                {
                    chkFirstOrderSetpointTracking.Checked = _firstOrderPidModel.FirstOrderSetpointTracking;
                    chkFirstOrderSetpointTracking.Enabled = !_firstOrderPidModel.FirstOrderClosedLoop;
                }

                // Update first-order display labels
                if (lblFirstOrderProcessVariable != null) lblFirstOrderProcessVariable.Text = _firstOrderPidModel.FirstOrderProcessVariable.ToString("F2");
                if (lblFirstOrderControllerOutput != null) lblFirstOrderControllerOutput.Text = _firstOrderPidModel.FirstOrderControllerOutput.ToString("F2");
                if (lblFirstOrderError != null) lblFirstOrderError.Text = _firstOrderPidModel.FirstOrderError.ToString("F2");
                if (lblFirstOrderSimulationTime != null) lblFirstOrderSimulationTime.Text = _firstOrderPidModel.CurrentTime.ToString("F1");
                
                // Update first-order button states - Enable controls for user interaction
                if (btnFirstOrderStart != null) btnFirstOrderStart.Enabled = !_firstOrderPidModel.IsRunning;
                if (btnFirstOrderStop != null) btnFirstOrderStop.Enabled = _firstOrderPidModel.IsRunning;
                if (btnFirstOrderReset != null) btnFirstOrderReset.Enabled = !_firstOrderPidModel.IsRunning;
                
                // Keep parameter controls enabled during simulation so user can tune in real-time
                if (numFirstOrderKp != null) numFirstOrderKp.Enabled = true;
                if (numFirstOrderKi != null) numFirstOrderKi.Enabled = true;
                if (numFirstOrderKd != null) numFirstOrderKd.Enabled = true;
                if (numFirstOrderProcessGain != null) numFirstOrderProcessGain.Enabled = true;
                if (numFirstOrderTimeConstant != null) numFirstOrderTimeConstant.Enabled = true;
                if (numFirstOrderDeadTime != null) numFirstOrderDeadTime.Enabled = true;
                if (numFirstOrderDisturbance != null) numFirstOrderDisturbance.Enabled = true;
                if (numFirstOrderSetpoint != null) numFirstOrderSetpoint.Enabled = true;
                if (btnFirstOrderAuto != null) btnFirstOrderAuto.Enabled = true; // Always enable Auto/Manual buttons
                if (btnFirstOrderManual != null) btnFirstOrderManual.Enabled = true;
                if (trkSimulationSpeed != null) trkSimulationSpeed.Enabled = true;
            }
            catch (Exception ex)
            {
                // EMERGENCY: Only log to debug output, no file I/O
                System.Diagnostics.Debug.WriteLine($"Error updating first-order UI: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the first-order plot with current data
        /// FIXED: Optimized for performance to prevent freezing
        /// </summary>
        private void UpdateFirstOrderPlot()
        {
            if (_firstOrderPidModel == null || _firstOrderPlotModel == null) return;

            try
            {
                double currentTime = _firstOrderPidModel.CurrentTime;
                
                // FIXED: Only update plot every 2nd call to reduce CPU load
                // This cuts plot updates in half without significantly affecting visual quality
                if (_plotUpdateCounter % 2 != 0)
                {
                    _plotUpdateCounter++;
                    return;
                }
                _plotUpdateCounter++;
                
                // Add new data points
                _firstOrderSetpointData.Add(new DataPoint(currentTime, _firstOrderPidModel.FirstOrderSetpoint));
                _firstOrderProcessVariableData.Add(new DataPoint(currentTime, _firstOrderPidModel.FirstOrderProcessVariable));
                _firstOrderControllerOutputData.Add(new DataPoint(currentTime, _firstOrderPidModel.FirstOrderControllerOutput));
                _firstOrderErrorData.Add(new DataPoint(currentTime, _firstOrderPidModel.FirstOrderError));

                // FIXED: More aggressive data trimming to prevent memory buildup
                double cutoffTime = currentTime - _firstOrderTrendDurationSeconds;
                
                // Remove old data points more frequently to prevent list growth
                if (_firstOrderSetpointData.Count > 1500) // Limit to ~5 minutes of data at 200ms intervals
                {
                    int removeCount = _firstOrderSetpointData.Count - 1000; // Keep last 1000 points
                    _firstOrderSetpointData.RemoveRange(0, removeCount);
                    _firstOrderProcessVariableData.RemoveRange(0, removeCount);
                    _firstOrderControllerOutputData.RemoveRange(0, removeCount);
                    _firstOrderErrorData.RemoveRange(0, removeCount);
                    
                    System.Diagnostics.Debug.WriteLine($"PERFORMANCE: Trimmed {removeCount} old plot data points to prevent memory buildup");
                }
                else
                {
                    // Normal trimming based on time window
                    _firstOrderSetpointData.RemoveAll(dp => dp.X < cutoffTime);
                    _firstOrderProcessVariableData.RemoveAll(dp => dp.X < cutoffTime);
                    _firstOrderControllerOutputData.RemoveAll(dp => dp.X < cutoffTime);
                    _firstOrderErrorData.RemoveAll(dp => dp.X < cutoffTime);
                }

                // Update series (only every other call)
                _firstOrderSetpointSeries?.Points.Clear();
                _firstOrderSetpointSeries?.Points.AddRange(_firstOrderSetpointData);

                _firstOrderProcessVariableSeries?.Points.Clear();
                _firstOrderProcessVariableSeries?.Points.AddRange(_firstOrderProcessVariableData);

                _firstOrderControllerOutputSeries?.Points.Clear();
                _firstOrderControllerOutputSeries?.Points.AddRange(_firstOrderControllerOutputData);

                _firstOrderErrorSeries?.Points.Clear();
                _firstOrderErrorSeries?.Points.AddRange(_firstOrderErrorData);

                // Update time axis
                if (_firstOrderTimeAxis != null)
                {
                    if (currentTime > _firstOrderTrendDurationSeconds)
                    {
                        _firstOrderTimeAxis.Minimum = currentTime - _firstOrderTrendDurationSeconds;
                        _firstOrderTimeAxis.Maximum = currentTime;
                    }
                    else
                    {
                        _firstOrderTimeAxis.Minimum = 0;
                        _firstOrderTimeAxis.Maximum = _firstOrderTrendDurationSeconds;
                    }
                }

                // FIXED: Use lighter refresh method
                plotFirstOrderView?.InvalidatePlot(false); // false = don't update data, just redraw
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating first-order plot: {ex.Message}");
                // Don't log errors to prevent cascading issues during freezing
            }
        }

        /// <summary>
        /// Clears all first-order plot data
        /// </summary>
        private void ClearFirstOrderPlotData()
        {
            _firstOrderSetpointData.Clear();
            _firstOrderProcessVariableData.Clear();
            _firstOrderControllerOutputData.Clear();
            _firstOrderErrorData.Clear();

            _firstOrderSetpointSeries?.Points.Clear();
            _firstOrderProcessVariableSeries?.Points.Clear();
            _firstOrderControllerOutputSeries?.Points.Clear();
            _firstOrderErrorSeries?.Points.Clear();

            // Reset time axis
            if (_firstOrderTimeAxis != null)
            {
                _firstOrderTimeAxis.Minimum = 0;
                _firstOrderTimeAxis.Maximum = _firstOrderTrendDurationSeconds;
            }

            plotFirstOrderView?.InvalidatePlot(true);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles parameter changes in the first-order process model
        /// </summary>
        private void FirstOrderProcessModel_ParameterChanged(object? sender, ModelParameterChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => FirstOrderProcessModel_ParameterChanged(sender, e)));
                return;
            }

            UpdateFirstOrderUI();
        }

        /// <summary>
        /// Handles errors in the first-order process model
        /// </summary>
        private void FirstOrderProcessModel_ModelError(object? sender, ModelErrorEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => FirstOrderProcessModel_ModelError(sender, e)));
                return;
            }

            MessageBox.Show($"First-Order Model Error: {e.ErrorMessage}", "Simulation Error", 
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Handles the first-order plot update timer tick
        /// EMERGENCY FIX: Minimal logging to prevent freezing
        /// </summary>
        private void FirstOrderPlotUpdateTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                // EMERGENCY FIX: Remove performance monitoring that could cause I/O
                // Just do the essential updates
                
                UpdateFirstOrderPlot();
                UpdateFirstOrderUI();
                
                _lastUpdateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in plot timer tick: {ex.Message}");
                // EMERGENCY: No logging to prevent cascading freezing
            }
        }

        /// <summary>
        /// Handles the first-order Start button click
        /// EMERGENCY FIX: Completely disable logging during simulation
        /// </summary>
        private void BtnFirstOrderStart_Click(object? sender, EventArgs e)
        {
            try
            {
                // EMERGENCY FIX: Disable ALL logging for the entire simulation
                ControlLoopLogger.SetLoggingEnabled(false);
                
                // Reset performance counters
                _performanceWarningCount = 0;
                _lastUpdateTime = DateTime.Now;
                
                _firstOrderPidModel?.Start();
                _firstOrderPlotUpdateTimer.Enabled = true;
                UpdateFirstOrderUI();
                
                if (_firstOrderPidModel != null)
                {
                    // Only log to debug output - no file I/O
                    System.Diagnostics.Debug.WriteLine($"SIMULATION STARTED: SP={_firstOrderPidModel.FirstOrderSetpoint:F1}, Speed={_firstOrderPidModel.SimulationSpeedMultiplier:F1}x - LOGGING DISABLED");
                }
            }
            catch (Exception ex)
            {
                // EMERGENCY: Only critical error logging to debug
                System.Diagnostics.Debug.WriteLine($"Error starting simulation: {ex.Message}");
                MessageBox.Show($"Error starting first-order simulation: {ex.Message}", 
                              "Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Emergency method to recover from freezing conditions
        /// Called via F11 key or when performance issues are detected
        /// </summary>
        private void EmergencyRecovery()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("EMERGENCY RECOVERY: Attempting to recover from freezing...");
                
                // 1. Disable all logging
                ControlLoopLogger.SetLoggingEnabled(false);
                
                // 2. Temporarily stop the plot timer
                _firstOrderPlotUpdateTimer.Enabled = false;
                
                // 3. Clear plot data to free memory
                ClearFirstOrderPlotData();
                
                // 4. Reset performance counters
                _performanceWarningCount = 0;
                _lastUpdateTime = DateTime.Now;
                
                // 5. Force garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
                
                // 6. Restart plot timer with longer interval
                _firstOrderPlotUpdateTimer.Interval = Math.Max(500, _firstOrderPlotUpdateTimer.Interval);
                _firstOrderPlotUpdateTimer.Enabled = true;
                
                // 7. Re-enable logging after delay
                System.Windows.Forms.Timer recoveryTimer = new()
                {
                    Interval = 3000,
                    Enabled = true
                };
                recoveryTimer.Tick += (s, args) =>
                {
                    // EMERGENCY: Keep logging disabled permanently
                    System.Diagnostics.Debug.WriteLine("EMERGENCY RECOVERY: Logging will remain disabled to prevent freezing");
                    recoveryTimer.Dispose();
                };
                
                MessageBox.Show("Emergency recovery initiated.\nAll logging permanently disabled to prevent mouse-movement freezing.\nSimulation should now be responsive.", 
                               "Recovery", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                System.Diagnostics.Debug.WriteLine("EMERGENCY RECOVERY: Complete");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EMERGENCY RECOVERY FAILED: {ex.Message}");
                MessageBox.Show($"Emergency recovery failed: {ex.Message}", "Recovery Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the first-order Stop button click
        /// </summary>
        private void BtnFirstOrderStop_Click(object? sender, EventArgs e)
        {
            try
            {
                _firstOrderPidModel?.Stop();
                _firstOrderPlotUpdateTimer.Enabled = false;
                UpdateFirstOrderUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping first-order simulation: {ex.Message}", 
                              "Stop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the first-order Reset button click
        /// </summary>
        private void BtnFirstOrderReset_Click(object? sender, EventArgs e)
        {
            try
            {
                _firstOrderPidModel?.Reset();
                ClearFirstOrderPlotData();
                UpdateFirstOrderUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting first-order simulation: {ex.Message}", 
                              "Reset Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles first-order parameter value changes
        /// </summary>
        private void FirstOrderParameterValue_Changed(object? sender, EventArgs e)
        {
            if (_firstOrderPidModel == null) return;

            try
            {
                // Update first-order model parameters based on which control changed
                if (sender == numFirstOrderSetpoint) 
                {
                    _firstOrderPidModel.FirstOrderSetpoint = (double)numFirstOrderSetpoint.Value;
                }
                else if (sender == numFirstOrderKp) 
                {
                    _firstOrderPidModel.FirstOrderKp = (double)numFirstOrderKp.Value;
                }
                else if (sender == numFirstOrderKi) 
                {
                    _firstOrderPidModel.FirstOrderKi = (double)numFirstOrderKi.Value;
                }
                else if (sender == numFirstOrderKd) 
                {
                    _firstOrderPidModel.FirstOrderKd = (double)numFirstOrderKd.Value;
                }
                else if (sender == numFirstOrderProcessGain) 
                {
                    _firstOrderPidModel.FirstOrderProcessGain = (double)numFirstOrderProcessGain.Value;
                }
                else if (sender == numFirstOrderTimeConstant) 
                {
                    _firstOrderPidModel.FirstOrderTimeConstant = (double)numFirstOrderTimeConstant.Value;
                }
                else if (sender == numFirstOrderDeadTime) 
                {
                    _firstOrderPidModel.FirstOrderDeadTime = (double)numFirstOrderDeadTime.Value;
                }
                else if (sender == numFirstOrderDisturbance) 
                {
                    _firstOrderPidModel.FirstOrderDisturbance = (double)numFirstOrderDisturbance.Value;
                }
                else if (sender == numFirstOrderManualOutput) 
                {
                    _firstOrderPidModel.FirstOrderManualOutput = (double)numFirstOrderManualOutput.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating first-order parameter: {ex.Message}", 
                              "Parameter Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the first-order Auto button click
        /// EMERGENCY FIX: No logging to prevent freezing
        /// </summary>
        private void BtnFirstOrderAuto_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_firstOrderPidModel != null)
                {
                    System.Diagnostics.Debug.WriteLine("=== AUTO BUTTON CLICKED ===");
                    
                    _firstOrderPidModel.FirstOrderClosedLoop = true;
                    
                    System.Diagnostics.Debug.WriteLine($"Set to Auto mode: ClosedLoop = {_firstOrderPidModel.FirstOrderClosedLoop}");
                    
                    // Force immediate UI update
                    UpdateFirstOrderUI();
                    
                    // Show confirmation
                    if (btnFirstOrderAuto != null)
                        btnFirstOrderAuto.BackColor = Color.LightGreen;
                    if (btnFirstOrderManual != null)
                        btnFirstOrderManual.BackColor = Color.LightGray;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Auto button clicked but model is null!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Auto button click: {ex.Message}");
                MessageBox.Show($"Error switching to Auto mode: {ex.Message}", 
                              "Mode Switch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the first-order Manual button click
        /// EMERGENCY FIX: No logging to prevent freezing
        /// </summary>
        private void BtnFirstOrderManual_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_firstOrderPidModel != null)
                {
                    System.Diagnostics.Debug.WriteLine("=== MANUAL BUTTON CLICKED ===");
                    
                    _firstOrderPidModel.FirstOrderClosedLoop = false;
                    
                    System.Diagnostics.Debug.WriteLine($"Set to Manual mode: ClosedLoop = {_firstOrderPidModel.FirstOrderClosedLoop}");
                    System.Diagnostics.Debug.WriteLine($"Manual output value = {_firstOrderPidModel.FirstOrderManualOutput:F2}");
                    
                    // Force immediate UI update
                    UpdateFirstOrderUI();
                    
                    // Show confirmation
                    if (btnFirstOrderAuto != null)
                        btnFirstOrderAuto.BackColor = Color.LightGray;
                    if (btnFirstOrderManual != null)
                        btnFirstOrderManual.BackColor = Color.LightCoral;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Manual button clicked but model is null!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Manual button click: {ex.Message}");
                MessageBox.Show($"Error switching to Manual mode: {ex.Message}", 
                              "Mode Switch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the first-order setpoint tracking checkbox changed event
        /// </summary>
        private void ChkFirstOrderSetpointTracking_CheckedChanged(object? sender, EventArgs e)
        {
            if (_firstOrderPidModel != null && sender is CheckBox checkBox)
            {
                _firstOrderPidModel.FirstOrderSetpointTracking = checkBox.Checked;
                UpdateFirstOrderUI();
            }
        }

        /// <summary>
        /// Show diagnostic information for troubleshooting
        /// EMERGENCY FIX: No logging to prevent freezing
        /// </summary>
        private void ShowDiagnosticInfo()
        {
            try
            {
                if (_firstOrderPidModel != null)
                {
                    var info = $"=== DIAGNOSTIC INFO ===\n" +
                              $"Model Type: {_firstOrderPidModel.GetType().Name}\n" +
                              $"Is Running: {_firstOrderPidModel.IsRunning}\n" +
                              $"Current Time: {_firstOrderPidModel.CurrentTime:F2}s\n" +
                              $"Closed Loop: {_firstOrderPidModel.FirstOrderClosedLoop}\n" +
                              $"Setpoint: {_firstOrderPidModel.FirstOrderSetpoint:F2}%\n" +
                              $"Process Variable: {_firstOrderPidModel.FirstOrderProcessVariable:F2}%\n" +
                              $"Controller Output: {_firstOrderPidModel.FirstOrderControllerOutput:F2}%\n" +
                              $"Manual Output: {_firstOrderPidModel.FirstOrderManualOutput:F2}%\n" +
                              $"Error: {_firstOrderPidModel.FirstOrderError:F2}%\n" +
                              $"Speed Multiplier: {_firstOrderPidModel.SimulationSpeedMultiplier:F1}x\n" +
                              $"Time Step: {_firstOrderPidModel.TimeStep:F3}s\n\n" +
                              $"=== UI CONTROLS STATE ===\n" +
                              $"Auto Button Enabled: {btnFirstOrderAuto?.Enabled}\n" +
                              $"Manual Button Enabled: {btnFirstOrderManual?.Enabled}\n" +
                              $"Manual Output Enabled: {numFirstOrderManualOutput?.Enabled}\n" +
                              $"Stop Button Enabled: {btnFirstOrderStop?.Enabled}\n" +
                              $"Manual Output Value: {numFirstOrderManualOutput?.Value}\n" +
                              $"Setpoint Control Value: {numFirstOrderSetpoint?.Value}\n\n" +
                              $"=== PERFORMANCE INFO ===\n" +
                              $"LOGGING DISABLED FOR PERFORMANCE\n" +
                              $"Plot Update Counter: {_plotUpdateCounter}\n" +
                              $"Performance Warning Count: {_performanceWarningCount}";
                              
                    MessageBox.Show(info, "FirstOrder Diagnostic", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    System.Diagnostics.Debug.WriteLine(info);
                }
                else
                {
                    MessageBox.Show("Model is NULL!", "Diagnostic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting diagnostic info: {ex.Message}", "Diagnostic Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles simulation speed trackbar changes
        /// EMERGENCY FIX: No logging to prevent freezing
        /// </summary>
        private void TrkSimulationSpeed_ValueChanged(object? sender, EventArgs e)
        {
            if (_firstOrderPidModel == null || trkSimulationSpeed == null || lblSimulationSpeedValue == null)
                return;

            try
            {
                // Convert trackbar value (1-100) to speed multiplier (0.1x - 10.0x)
                // Trackbar: 1=0.1x, 10=1.0x, 50=5.0x, 100=10.0x
                double speedMultiplier = trkSimulationSpeed.Value / 10.0;
                
                // Update the model's simulation speed
                _firstOrderPidModel.SimulationSpeedMultiplier = speedMultiplier;
                
                // Update the label
                lblSimulationSpeedValue.Text = $"{speedMultiplier:F1}x";
                
                // Change label color based on speed
                if (speedMultiplier < 1.0)
                {
                    lblSimulationSpeedValue.ForeColor = Color.Orange; // Slow
                }
                else if (speedMultiplier == 1.0)
                {
                    lblSimulationSpeedValue.ForeColor = Color.Green;  // Real-time
                }
                else
                {
                    lblSimulationSpeedValue.ForeColor = Color.Red;    // Fast
                }
                
                System.Diagnostics.Debug.WriteLine($"Simulation speed changed to {speedMultiplier:F1}x via slider");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error changing simulation speed: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the trend duration update button click
        /// </summary>
        private void BtnFirstOrderUpdateTrend_Click(object? sender, EventArgs e)
        {
            try
            {
                if (txtFirstOrderTrendDuration != null && double.TryParse(txtFirstOrderTrendDuration.Text, out double newDuration))
                {
                    if (newDuration > 0 && newDuration <= 3600) // Limit to 1 hour max
                    {
                        _firstOrderTrendDurationSeconds = newDuration;
                        ClearFirstOrderPlotData();
                        MessageBox.Show($"Trend duration updated to {newDuration} seconds", "Trend Updated", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Please enter a duration between 1 and 3600 seconds", "Invalid Duration", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for trend duration", "Invalid Input", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating trend duration: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _firstOrderPlotUpdateTimer?.Stop();
                _firstOrderPlotUpdateTimer?.Dispose();
                _firstOrderPidModel?.Stop();
                _firstOrderPidModel?.Dispose();
                _disposed = true;
            }
            
            base.Dispose(disposing);
        }

        #endregion
    }
}