/*
 * File: FirstOrderProcessWithPidForm.cs
 * Author: Shankar Ananth Asokan
 * Purpose: First Order Process with PID simulation form with professional features
 * Date Created: 2025-08-23
 * Date Modified: 2025-08-23
 * 
 * Description: A comprehensive simulation form that directly manages
 * the PID controller and first-order process with professional control features.
 */

using TarkaDyS.Controllers;
using TarkaDyS.Models;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TarkaDyS.Forms
{
    /// <summary>
    /// First Order Process with PID simulation form with professional control management features
    /// </summary>
    public partial class FirstOrderProcessWithPidForm : Form
    {
        #region Private Fields
        // Core simulation components
        private readonly PidController _pidController;
        private readonly FirstOrderProcess _process;
        private readonly System.Windows.Forms.Timer _simulationTimer;
        
        // Simulation state
        private bool _isRunning = false;
        private double _currentTime = 0.0;
        private double _simulationSpeed = 1.0; // 1.0 = real-time
        private bool _disposed = false;
        
        // Plot data
        private readonly List<DataPoint> _setpointData = new();
        private readonly List<DataPoint> _processVariableData = new();
        private readonly List<DataPoint> _controllerOutputData = new();
        private readonly List<DataPoint> _errorData = new();
        
        // Plot components
        private PlotModel? _plotModel;
        private LineSeries? _setpointSeries;
        private LineSeries? _processVariableSeries;
        private LineSeries? _controllerOutputSeries;
        private LineSeries? _errorSeries;
        private LinearAxis? _timeAxis;
        private LinearAxis? _valueAxis;
        
        // Plot settings
        private double _trendDuration = 300.0; // 5 minutes
        private int _plotUpdateCounter = 0;
        
        // Enhanced control limits
        private double _setpointLow = 0.0;
        private double _setpointHigh = 100.0;
        private double _outputLow = 0.0;
        private double _outputHigh = 100.0;
        
        // Plot axis limits
        private double _xAxisMin = 0.0;
        private double _xAxisMax = 300.0;
        private double _yAxisMin = 0.0;
        private double _yAxisMax = 100.0;
        #endregion

        #region Constructor
        public FirstOrderProcessWithPidForm()
        {
            InitializeComponent();
            
            // Initialize core components
            _pidController = new PidController();
            _process = new FirstOrderProcess();
            
            // Initialize simulation timer
            _simulationTimer = new System.Windows.Forms.Timer
            {
                Interval = 100, // 100ms = 10Hz update rate
                Enabled = false
            };
            _simulationTimer.Tick += SimulationTimer_Tick;
            
            // Initialize everything to steady state
            InitializeToSteadyState();
            InitializePlot();
            UpdateUI();
            
            this.Text = "First Order Process with PID - Professional Edition";
        }
        #endregion

        #region Initialization Methods
        private void InitializeToSteadyState()
        {
            // Initialize both process and controller to 50% steady state
            _process.Initialize(50.0, 50.0);
            _pidController.Initialize(50.0, 50.0);
            _pidController.Setpoint = 50.0;
            
            // CONSERVATIVE PID tuning as requested: Kp=0.5, Ki=0.1, Kd=0.0
            _pidController.Kp = 0.5;        // Conservative proportional gain
            _pidController.Ki = 0.1;        // Conservative integral gain  
            _pidController.Kd = 0.0;        // No derivative action
            
            // Ensure default algorithm is set
            _pidController.Algorithm = PidAlgorithm.BasicPID;
            
            // Set default process parameters
            _process.ProcessGain = 1.0;
            _process.TimeConstant = 10.0;
            _process.DeadTime = 1.0;
            
            // Set initial output limits
            _pidController.SetOutputLimits(_outputLow, _outputHigh);
            
            _currentTime = 0.0;
            
            // Verify initialization
            System.Diagnostics.Debug.WriteLine($"=== VELOCITY FORM INITIALIZATION COMPLETE ===");
            System.Diagnostics.Debug.WriteLine($"PID Algorithm: {_pidController.Algorithm}");
            System.Diagnostics.Debug.WriteLine($"CONSERVATIVE PID Tuning - Kp: {_pidController.Kp:F1}, Ki: {_pidController.Ki:F1}, Kd: {_pidController.Kd:F1}");
            System.Diagnostics.Debug.WriteLine($"Process Model - K: {_process.ProcessGain:F1}, Tau: {_process.TimeConstant:F1}s, Td: {_process.DeadTime:F1}s");
            System.Diagnostics.Debug.WriteLine($"SP: {_pidController.Setpoint:F1}%, PV: {_process.Output:F1}%, MV: {_pidController.Output:F1}%");
        }

        private void InitializePlot()
        {
            try
            {
                // Create plot model
                _plotModel = new PlotModel
                {
                    Title = "First Order Process with PID Control Response",
                    Background = OxyColors.White,
                    TitleFontSize = 14,
                    DefaultFontSize = 12
                };

                // Create axes with configurable limits
                _timeAxis = new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Time (seconds)",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                    MinorGridlineColor = OxyColor.FromRgb(240, 240, 240),
                    Minimum = _xAxisMin,
                    Maximum = _xAxisMax
                };
                _plotModel.Axes.Add(_timeAxis);

                _valueAxis = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Value (%)",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                    MinorGridlineColor = OxyColor.FromRgb(240, 240, 240),
                    Minimum = _yAxisMin,
                    Maximum = _yAxisMax
                };
                _plotModel.Axes.Add(_valueAxis);

                // Create series
                _setpointSeries = new LineSeries
                {
                    Title = "Setpoint",
                    Color = OxyColors.Blue,
                    StrokeThickness = 2,
                    LineStyle = LineStyle.Dash
                };
                _plotModel.Series.Add(_setpointSeries);

                _processVariableSeries = new LineSeries
                {
                    Title = "Process Variable",
                    Color = OxyColors.Green,
                    StrokeThickness = 2
                };
                _plotModel.Series.Add(_processVariableSeries);

                _controllerOutputSeries = new LineSeries
                {
                    Title = "Controller Output",
                    Color = OxyColors.Red,
                    StrokeThickness = 2
                };
                _plotModel.Series.Add(_controllerOutputSeries);

                _errorSeries = new LineSeries
                {
                    Title = "Error",
                    Color = OxyColors.Orange,
                    StrokeThickness = 1,
                    LineStyle = LineStyle.Dot
                };
                _plotModel.Series.Add(_errorSeries);

                // Set plot model to plot view
                if (plotView != null)
                {
                    plotView.Model = _plotModel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing plot: {ex.Message}", "Plot Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Simulation Methods
        private void SimulationTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                // Calculate actual time step based on simulation speed
                double timeStep = (_simulationTimer.Interval / 1000.0) * _simulationSpeed;
                
                // Update simulation time
                _currentTime += timeStep;
                
                // Set process time step for accurate simulation
                _process.TimeStep = timeStep;
                
                // Handle SP tracking PV in Manual mode
                // FIXED: Setpoint tracking should make SP follow PV in Manual mode
                if (!_pidController.AutoMode && chkSetpointTracking?.Checked == true)
                {
                    _pidController.Setpoint = _process.Output;
                }
                
                // Update PID controller
                double controllerOutput = _pidController.Update(_process.Output, timeStep);
                
                // Apply controller output to process
                _process.Input = controllerOutput;
                
                // Update process
                _process.Update();
                
                // Update plot data (every 5th iteration to reduce CPU load)
                if (_plotUpdateCounter % 5 == 0)
                {
                    UpdatePlotData();
                }
                _plotUpdateCounter++;
                
                // Update UI
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Simulation error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopSimulation();
            }
        }

        private void UpdatePlotData()
        {
            if (_plotModel == null) return;

            try
            {
                // Add new data points
                _setpointData.Add(new DataPoint(_currentTime, _pidController.Setpoint));
                _processVariableData.Add(new DataPoint(_currentTime, _process.Output));
                _controllerOutputData.Add(new DataPoint(_currentTime, _pidController.Output));
                _errorData.Add(new DataPoint(_currentTime, _pidController.Error));

                // Trim old data based on current axis limits
                double cutoffTime = Math.Max(0, _currentTime - (_xAxisMax - _xAxisMin));
                _setpointData.RemoveAll(dp => dp.X < cutoffTime);
                _processVariableData.RemoveAll(dp => dp.X < cutoffTime);
                _controllerOutputData.RemoveAll(dp => dp.X < cutoffTime);
                _errorData.RemoveAll(dp => dp.X < cutoffTime);

                // Update series
                _setpointSeries?.Points.Clear();
                _setpointSeries?.Points.AddRange(_setpointData);
                
                _processVariableSeries?.Points.Clear();
                _processVariableSeries?.Points.AddRange(_processVariableData);
                
                _controllerOutputSeries?.Points.Clear();
                _controllerOutputSeries?.Points.AddRange(_controllerOutputData);
                
                _errorSeries?.Points.Clear();
                _errorSeries?.Points.AddRange(_errorData);

                // Update time axis dynamically if auto-scaling is enabled
                if (_timeAxis != null && chkAutoScale?.Checked == true)
                {
                    if (_currentTime > _xAxisMax)
                    {
                        _timeAxis.Minimum = _currentTime - (_xAxisMax - _xAxisMin);
                        _timeAxis.Maximum = _currentTime;
                    }
                }

                // Refresh plot
                plotView?.InvalidatePlot(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Plot update error: {ex.Message}");
            }
        }

        private void StartSimulation()
        {
            try
            {
                _isRunning = true;
                _simulationTimer.Enabled = true;
                UpdateUI();
                System.Diagnostics.Debug.WriteLine($"First Order Process simulation started - SP: {_pidController.Setpoint:F1}%, PV: {_process.Output:F1}%, MV: {_pidController.Output:F1}%");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting simulation: {ex.Message}", "Start Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopSimulation()
        {
            try
            {
                _isRunning = false;
                _simulationTimer.Enabled = false;
                UpdateUI();
                System.Diagnostics.Debug.WriteLine("First Order Process simulation stopped");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping simulation: {ex.Message}");
            }
        }

        private void ResetSimulation()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== COMPLETE RESET TO INITIAL STATE ===");
                
                // Stop simulation first
                if (_isRunning)
                {
                    StopSimulation();
                }
                
                // Reset time and counters
                _currentTime = 0.0;
                _plotUpdateCounter = 0;
                
                // FIXED: Reset ALL parameters to initial defaults (like starting fresh)
                
                // Reset PID Controller to initial defaults with CONSERVATIVE tuning
                _pidController.Reset();
                _pidController.Setpoint = 50.0;              // Default setpoint
                _pidController.Kp = 0.5;                     // CONSERVATIVE: Lower proportional gain
                _pidController.Ki = 0.1;                     // CONSERVATIVE: Lower integral gain  
                _pidController.Kd = 0.0;                     // CONSERVATIVE: No derivative action
                _pidController.AutoMode = true;              // Start in Auto mode
                _pidController.Algorithm = PidAlgorithm.BasicPID; // Default algorithm
                _pidController.Initialize(50.0, 50.0);      // Initialize to 50% steady state
                
                // Reset Process Model to initial defaults
                _process.Reset();
                _process.ProcessGain = 1.0;                  // Default process gain
                _process.TimeConstant = 10.0;               // Default time constant
                _process.DeadTime = 1.0;                    // Default dead time
                _process.Disturbance = 0.0;                 // No disturbance
                _process.Initialize(50.0, 50.0);           // Initialize to 50% steady state
                
                // Reset control limits to defaults
                _setpointLow = 0.0;
                _setpointHigh = 100.0;
                _outputLow = 0.0;
                _outputHigh = 100.0;
                _pidController.SetOutputLimits(_outputLow, _outputHigh);
                
                // Reset simulation parameters
                _simulationSpeed = 1.0;                     // Real-time speed
                
                // Reset plot axis limits to defaults
                _xAxisMin = 0.0;
                _xAxisMax = 300.0;
                _yAxisMin = 0.0;
                _yAxisMax = 100.0;
                
                // Update axes
                if (_timeAxis != null)
                {
                    _timeAxis.Minimum = _xAxisMin;
                    _timeAxis.Maximum = _xAxisMax;
                }
                if (_valueAxis != null)
                {
                    _valueAxis.Minimum = _yAxisMin;
                    _valueAxis.Maximum = _yAxisMax;
                }
                
                // Clear all plot data
                _setpointData.Clear();
                _processVariableData.Clear();
                _controllerOutputData.Clear();
                _errorData.Clear();
                
                _setpointSeries?.Points.Clear();
                _processVariableSeries?.Points.Clear();
                _controllerOutputSeries?.Points.Clear();
                _errorSeries?.Points.Clear();
                
                // Update all UI controls to show reset values
                UpdateUIControlsToDefaults();
                
                plotView?.InvalidatePlot(true);
                UpdateUI();
                
                System.Diagnostics.Debug.WriteLine($"COMPLETE RESET FINISHED - All parameters restored to defaults");
                System.Diagnostics.Debug.WriteLine($"Final State - SP: {_pidController.Setpoint:F1}%, PV: {_process.Output:F1}%, MV: {_pidController.Output:F1}%");
                System.Diagnostics.Debug.WriteLine($"PID Gains - Kp: {_pidController.Kp:F3}, Ki: {_pidController.Ki:F3}, Kd: {_pidController.Kd:F3}");
                System.Diagnostics.Debug.WriteLine($"Process - Gain: {_process.ProcessGain:F2}, Tau: {_process.TimeConstant:F1}, Td: {_process.DeadTime:F1}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting simulation: {ex.Message}", "Reset Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Reset error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Update UI controls to show default/reset values
        /// </summary>
        private void UpdateUIControlsToDefaults()
        {
            try
            {
                // Reset PID parameter controls with CONSERVATIVE tuning
                if (numSetpoint != null) numSetpoint.Value = 50.0M;
                if (numKp != null) numKp.Value = 0.5M;       // CONSERVATIVE: Lower Kp
                if (numKi != null) numKi.Value = 0.1M;       // CONSERVATIVE: Lower Ki
                if (numKd != null) numKd.Value = 0.0M;       // CONSERVATIVE: No derivative
                if (numManualOutput != null) numManualOutput.Value = 50.0M;
                if (cmbPidAlgorithm != null) cmbPidAlgorithm.SelectedIndex = 0; // BasicPID
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateUIControlsToDefaults: {ex.Message}");
            }
        }
        #endregion

        #region Parameter Tuning Methods
        private void numKp_ValueChanged(object sender, EventArgs e)
        {
            if (numKp != null)
                _pidController.Kp = (double)numKp.Value;
        }

        private void numKi_ValueChanged(object sender, EventArgs e)
        {
            if (numKi != null)
                _pidController.Ki = (double)numKi.Value;
        }

        private void numKd_ValueChanged(object sender, EventArgs e)
        {
            if (numKd != null)
                _pidController.Kd = (double)numKd.Value;
        }

        private void numSetpoint_ValueChanged(object sender, EventArgs e)
        {
            if (numSetpoint != null)
            {
                double newValue = (double)numSetpoint.Value;
                newValue = Math.Max(0.0, Math.Min(newValue, 100.0));
                
                if (Math.Abs(newValue - _pidController.Setpoint) > 1e-6)
                {
                    _pidController.Setpoint = newValue;
                    System.Diagnostics.Debug.WriteLine($"Setpoint set to {newValue:F2}");
                }
            }
        }

        private void numManualOutput_ValueChanged(object sender, EventArgs e)
        {
            if (numManualOutput != null)
            {
                double newValue = (double)numManualOutput.Value;
                newValue = Math.Max(0.0, Math.Min(newValue, 100.0));
                
                if (Math.Abs(newValue - _pidController.ManualOutput) > 1e-6)
                {
                    _pidController.ManualOutput = newValue;
                    System.Diagnostics.Debug.WriteLine($"Manual output set to {newValue:F2}");
                }
            }
        }
        #endregion

        #region UI Update Methods
        private void UpdateUI()
        {
            try
            {
                // Update parameter controls (only if not focused)
                if (numSetpoint != null && !numSetpoint.Focused)
                    numSetpoint.Value = (decimal)_pidController.Setpoint;
                if (numKp != null && !numKp.Focused)
                    numKp.Value = (decimal)_pidController.Kp;
                if (numKi != null && !numKi.Focused)
                    numKi.Value = (decimal)_pidController.Ki;
                if (numKd != null && !numKd.Focused)
                    numKd.Value = (decimal)_pidController.Kd;
                if (numProcessGain != null && !numProcessGain.Focused)
                    numProcessGain.Value = (decimal)_process.ProcessGain;
                if (numTimeConstant != null && !numTimeConstant.Focused)
                    numTimeConstant.Value = (decimal)_process.TimeConstant;
                if (numDeadTime != null && !numDeadTime.Focused)
                    numDeadTime.Value = (decimal)_process.DeadTime;
                if (numDisturbance != null && !numDisturbance.Focused)
                    numDisturbance.Value = (decimal)_process.Disturbance;

                // Update Auto/Manual controls
                if (btnAuto != null && btnManual != null)
                {
                    if (_pidController.AutoMode)
                    {
                        btnAuto.BackColor = Color.LightGreen;
                        btnManual.BackColor = Color.LightGray;
                    }
                    else
                    {
                        btnAuto.BackColor = Color.LightGray;
                        btnManual.BackColor = Color.LightCoral;
                    }
                }

                // Update manual output control
                if (numManualOutput != null)
                {
                    if (_pidController.AutoMode)
                    {
                        // In Auto mode: sync manual output for bumpless transfer
                        if (!numManualOutput.Focused)
                            numManualOutput.Value = (decimal)_pidController.Output;
                        _pidController.ManualOutput = _pidController.Output;
                        numManualOutput.Enabled = false;
                        numManualOutput.BackColor = Color.LightGray;
                    }
                    else
                    {
                        // In Manual mode: user can edit
                        if (!numManualOutput.Focused)
                            numManualOutput.Value = (decimal)_pidController.ManualOutput;
                        numManualOutput.Enabled = true;
                        numManualOutput.BackColor = Color.White;
                    }
                }

                // Update PID algorithm combobox
                if (cmbPidAlgorithm != null && !cmbPidAlgorithm.Focused)
                {
                    cmbPidAlgorithm.SelectedIndex = (int)_pidController.Algorithm;
                }

                // Update simulation speed
                if (trkSimulationSpeed != null && lblSimulationSpeed != null)
                {
                    int trackbarValue = (int)Math.Round(_simulationSpeed * 10);
                    if (trkSimulationSpeed.Value != trackbarValue && !trkSimulationSpeed.Focused)
                        trkSimulationSpeed.Value = Math.Max(1, Math.Min(50, trackbarValue));
                    
                    lblSimulationSpeed.Text = $"{_simulationSpeed:F1}x";
                    lblSimulationSpeed.ForeColor = _simulationSpeed == 1.0 ? Color.Green : 
                                                   _simulationSpeed < 1.0 ? Color.Orange : Color.Red;
                }

                // Update status displays
                if (lblProcessVariable != null) lblProcessVariable.Text = _process.Output.ToString("F2");
                if (lblControllerOutput != null) lblControllerOutput.Text = _pidController.Output.ToString("F2");
                if (lblError != null) lblError.Text = _pidController.Error.ToString("F2");
                if (lblSimulationTime != null) lblSimulationTime.Text = _currentTime.ToString("F1");

                // Update button states
                if (btnStart != null) btnStart.Enabled = !_isRunning;
                if (btnStop != null) btnStop.Enabled = _isRunning;
                if (btnReset != null) btnReset.Enabled = !_isRunning;
                
                // Enable/disable setpoint tracking checkbox
                if (chkSetpointTracking != null)
                {
                    chkSetpointTracking.Enabled = !_pidController.AutoMode;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating UI: {ex.Message}");
            }
        }
        #endregion

        #region Event Handlers wired from Designer
        private void BtnStart_Click(object? sender, EventArgs e) => StartSimulation();
        private void BtnStop_Click(object? sender, EventArgs e) => StopSimulation();
        private void BtnReset_Click(object? sender, EventArgs e) => ResetSimulation();

        private void BtnAuto_Click(object? sender, EventArgs e)
        {
            _pidController.AutoMode = true;
            UpdateUI();
        }

        private void BtnManual_Click(object? sender, EventArgs e)
        {
            _pidController.AutoMode = false;
            UpdateUI();
        }

        private void TrkSimulationSpeed_ValueChanged(object? sender, EventArgs e)
        {
            if (trkSimulationSpeed != null)
            {
                _simulationSpeed = Math.Max(0.1, trkSimulationSpeed.Value / 10.0);
                UpdateUI();
            }
        }

        private void CmbPidAlgorithm_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbPidAlgorithm != null)
            {
                _pidController.Algorithm = (PidAlgorithm)cmbPidAlgorithm.SelectedIndex;
                // Light reset for smooth switching
                _pidController.Reset();
                UpdateUI();
            }
        }

        // NumericUpDown handlers (capitalized) mapped from Designer
        private void NumKp_ValueChanged(object? sender, EventArgs e) => numKp_ValueChanged(sender!, e);
        private void NumKi_ValueChanged(object? sender, EventArgs e) => numKi_ValueChanged(sender!, e);
        private void NumKd_ValueChanged(object? sender, EventArgs e) => numKd_ValueChanged(sender!, e);
        private void NumSetpoint_ValueChanged(object? sender, EventArgs e) => numSetpoint_ValueChanged(sender!, e);
        private void NumManualOutput_ValueChanged(object? sender, EventArgs e) => numManualOutput_ValueChanged(sender!, e);

        private void NumProcessGain_ValueChanged(object? sender, EventArgs e)
        {
            if (numProcessGain != null)
                _process.ProcessGain = (double)numProcessGain.Value;
        }

        private void NumTimeConstant_ValueChanged(object? sender, EventArgs e)
        {
            if (numTimeConstant != null)
                _process.TimeConstant = (double)numTimeConstant.Value;
        }

        private void NumDeadTime_ValueChanged(object? sender, EventArgs e)
        {
            if (numDeadTime != null)
                _process.DeadTime = (double)numDeadTime.Value;
        }

        private void NumDisturbance_ValueChanged(object? sender, EventArgs e)
        {
            if (numDisturbance != null)
                _process.Disturbance = (double)numDisturbance.Value;
        }

        private void NumSetpointLow_ValueChanged(object? sender, EventArgs e)
        {
            if (numSetpointLow != null)
            {
                _setpointLow = (double)numSetpointLow.Value;
                if (numSetpointHigh != null && _setpointLow >= (double)numSetpointHigh.Value)
                {
                    _setpointLow = (double)numSetpointHigh.Value - 1.0;
                    numSetpointLow.Value = (decimal)_setpointLow;
                }
            }
        }

        private void NumSetpointHigh_ValueChanged(object? sender, EventArgs e)
        {
            if (numSetpointHigh != null)
            {
                _setpointHigh = (double)numSetpointHigh.Value;
                if (numSetpointLow != null && _setpointHigh <= (double)numSetpointLow.Value)
                {
                    _setpointHigh = (double)numSetpointLow.Value + 1.0;
                    numSetpointHigh.Value = (decimal)_setpointHigh;
                }
            }
        }

        private void NumOutputLow_ValueChanged(object? sender, EventArgs e)
        {
            if (numOutputLow != null)
            {
                _outputLow = (double)numOutputLow.Value;
                if (numOutputHigh != null && _outputLow >= (double)numOutputHigh.Value)
                {
                    _outputLow = (double)numOutputHigh.Value - 1.0;
                    numOutputLow.Value = (decimal)_outputLow;
                }
                _pidController.SetOutputLimits(_outputLow, _outputHigh);
            }
        }

        private void NumOutputHigh_ValueChanged(object? sender, EventArgs e)
        {
            if (numOutputHigh != null)
            {
                _outputHigh = (double)numOutputHigh.Value;
                if (numOutputLow != null && _outputHigh <= (double)numOutputLow.Value)
                {
                    _outputHigh = (double)numOutputLow.Value + 1.0;
                    numOutputHigh.Value = (decimal)_outputHigh;
                }
                _pidController.SetOutputLimits(_outputLow, _outputHigh);
            }
        }

        private void NumXAxisMin_ValueChanged(object? sender, EventArgs e)
        {
            if (numXAxisMin != null && _timeAxis != null)
            {
                _xAxisMin = (double)numXAxisMin.Value;
                _timeAxis.Minimum = _xAxisMin;
                plotView?.InvalidatePlot(true);
            }
        }

        private void NumXAxisMax_ValueChanged(object? sender, EventArgs e)
        {
            if (numXAxisMax != null && _timeAxis != null)
            {
                _xAxisMax = (double)numXAxisMax.Value;
                _timeAxis.Maximum = _xAxisMax;
                plotView?.InvalidatePlot(true);
            }
        }

        private void NumYAxisMin_ValueChanged(object? sender, EventArgs e)
        {
            if (numYAxisMin != null && _valueAxis != null)
            {
                _yAxisMin = (double)numYAxisMin.Value;
                _valueAxis.Minimum = _yAxisMin;
                plotView?.InvalidatePlot(true);
            }
        }

        private void NumYAxisMax_ValueChanged(object? sender, EventArgs e)
        {
            if (numYAxisMax != null && _valueAxis != null)
            {
                _yAxisMax = (double)numYAxisMax.Value;
                _valueAxis.Maximum = _yAxisMax;
                plotView?.InvalidatePlot(true);
            }
        }
        #endregion
    }
}