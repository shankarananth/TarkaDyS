/*
 * File: FirstOrderProcessWithPidForm.Designer.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Enhanced designer file for First Order Process with PID simulation form
 * Date Created: 2025-08-23
 * Date Modified: 2025-08-23
 */

using OxyPlot.WindowsForms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TarkaDyS.Forms
{
    partial class FirstOrderProcessWithPidForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // Controls - Enhanced with professional features
        private PlotView plotView;
        private Button btnStart, btnStop, btnReset, btnAuto, btnManual;
        private NumericUpDown numSetpoint, numKp, numKi, numKd;
        private NumericUpDown numProcessGain, numTimeConstant, numDeadTime, numDisturbance;
        private NumericUpDown numManualOutput;
        private ComboBox cmbPidAlgorithm;
        private TrackBar trkSimulationSpeed;
        private Label lblSimulationSpeed, lblProcessVariable, lblControllerOutput, lblError, lblSimulationTime;
        private GroupBox grpSimulation, grpPidParameters, grpProcessParameters, grpProcessStatus, grpPlotControls;
        
        // Enhanced controls for professional features
        private NumericUpDown numSetpointLow, numSetpointHigh, numOutputLow, numOutputHigh;
        private NumericUpDown numXAxisMin, numXAxisMax, numYAxisMin, numYAxisMax;
        private CheckBox chkSetpointTracking, chkAutoScale;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            this.SuspendLayout();
            
            // 
            // FirstOrderProcessWithPidForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1600, 900);
            this.Name = "FirstOrderProcessWithPidForm";
            this.Text = "First Order Process with PID - Professional Edition";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1400, 800);
            
            // 
            // plotView
            // 
            this.plotView = new PlotView();
            this.plotView.Location = new Point(12, 12);
            this.plotView.Name = "plotView";
            this.plotView.Size = new Size(950, 650);
            this.plotView.TabIndex = 0;
            this.plotView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            
            // 
            // grpSimulation - Simulation Controls
            // 
            this.grpSimulation = new GroupBox();
            this.grpSimulation.Location = new Point(980, 12);
            this.grpSimulation.Name = "grpSimulation";
            this.grpSimulation.Size = new Size(600, 120);
            this.grpSimulation.TabIndex = 1;
            this.grpSimulation.TabStop = false;
            this.grpSimulation.Text = "Simulation Controls";
            this.grpSimulation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            // Start/Stop/Reset buttons
            this.btnStart = new Button();
            this.btnStart.BackColor = Color.LightGreen;
            this.btnStart.Location = new Point(15, 25);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new Size(80, 35);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new EventHandler(this.BtnStart_Click);
            
            this.btnStop = new Button();
            this.btnStop.BackColor = Color.LightCoral;
            this.btnStop.Location = new Point(105, 25);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new Size(80, 35);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new EventHandler(this.BtnStop_Click);
            
            this.btnReset = new Button();
            this.btnReset.BackColor = Color.LightBlue;
            this.btnReset.Location = new Point(195, 25);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new Size(80, 35);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "RESET";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new EventHandler(this.BtnReset_Click);
            
            // Simulation Speed Control
            var lblSpeedLabel = new Label();
            lblSpeedLabel.Location = new Point(15, 70);
            lblSpeedLabel.Name = "lblSpeedLabel";
            lblSpeedLabel.Size = new Size(80, 23);
            lblSpeedLabel.Text = "Sim Speed:";

            this.trkSimulationSpeed = new TrackBar();
            this.trkSimulationSpeed.Location = new Point(100, 65);
            this.trkSimulationSpeed.Maximum = 50;
            this.trkSimulationSpeed.Minimum = 1;
            this.trkSimulationSpeed.Name = "trkSimulationSpeed";
            this.trkSimulationSpeed.Size = new Size(300, 56);
            this.trkSimulationSpeed.TabIndex = 3;
            this.trkSimulationSpeed.TickFrequency = 10;
            this.trkSimulationSpeed.Value = 10;
            this.trkSimulationSpeed.ValueChanged += new EventHandler(this.TrkSimulationSpeed_ValueChanged);
            
            this.lblSimulationSpeed = new Label();
            this.lblSimulationSpeed.ForeColor = Color.Green;
            this.lblSimulationSpeed.Location = new Point(410, 70);
            this.lblSimulationSpeed.Name = "lblSimulationSpeed";
            this.lblSimulationSpeed.Size = new Size(50, 23);
            this.lblSimulationSpeed.TabIndex = 4;
            this.lblSimulationSpeed.Text = "1.0x";
            
            // Simulation Time Display
            var lblTimeLabel = new Label();
            lblTimeLabel.Location = new Point(470, 25);
            lblTimeLabel.Name = "lblTimeLabel";
            lblTimeLabel.Size = new Size(60, 23);
            lblTimeLabel.Text = "Time (s):";
            
            this.lblSimulationTime = new Label();
            this.lblSimulationTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblSimulationTime.ForeColor = Color.Blue;
            this.lblSimulationTime.Location = new Point(470, 48);
            this.lblSimulationTime.Name = "lblSimulationTime";
            this.lblSimulationTime.Size = new Size(80, 30);
            this.lblSimulationTime.TabIndex = 5;
            this.lblSimulationTime.Text = "0.0";
            
            // Add controls to simulation group
            this.grpSimulation.Controls.Add(this.btnStart);
            this.grpSimulation.Controls.Add(this.btnStop);
            this.grpSimulation.Controls.Add(this.btnReset);
            this.grpSimulation.Controls.Add(lblSpeedLabel);
            this.grpSimulation.Controls.Add(this.trkSimulationSpeed);
            this.grpSimulation.Controls.Add(this.lblSimulationSpeed);
            this.grpSimulation.Controls.Add(lblTimeLabel);
            this.grpSimulation.Controls.Add(this.lblSimulationTime);
            
            // 
            // grpPidParameters - Enhanced PID Parameters
            // 
            this.grpPidParameters = new GroupBox();
            this.grpPidParameters.Location = new Point(980, 145);
            this.grpPidParameters.Name = "grpPidParameters";
            this.grpPidParameters.Size = new Size(600, 220);
            this.grpPidParameters.TabIndex = 2;
            this.grpPidParameters.TabStop = false;
            this.grpPidParameters.Text = "PID Parameters";
            this.grpPidParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            // Initialize PID controls
            this.InitializePidControls();
            
            // 
            // grpProcessParameters
            // 
            this.grpProcessParameters = new GroupBox();
            this.grpProcessParameters.Location = new Point(980, 380);
            this.grpProcessParameters.Name = "grpProcessParameters";
            this.grpProcessParameters.Size = new Size(600, 100);
            this.grpProcessParameters.TabIndex = 3;
            this.grpProcessParameters.TabStop = false;
            this.grpProcessParameters.Text = "First Order Process Parameters";
            this.grpProcessParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            this.InitializeProcessControls();
            
            // 
            // grpProcessStatus
            // 
            this.grpProcessStatus = new GroupBox();
            this.grpProcessStatus.Location = new Point(980, 495);
            this.grpProcessStatus.Name = "grpProcessStatus";
            this.grpProcessStatus.Size = new Size(600, 80);
            this.grpProcessStatus.TabIndex = 4;
            this.grpProcessStatus.TabStop = false;
            this.grpProcessStatus.Text = "Process Status";
            this.grpProcessStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            this.InitializeStatusControls();
            
            // 
            // grpPlotControls - New Plot Controls Group
            // 
            this.grpPlotControls = new GroupBox();
            this.grpPlotControls.Location = new Point(980, 590);
            this.grpPlotControls.Name = "grpPlotControls";
            this.grpPlotControls.Size = new Size(600, 100);
            this.grpPlotControls.TabIndex = 5;
            this.grpPlotControls.TabStop = false;
            this.grpPlotControls.Text = "Plot Scale Controls";
            this.grpPlotControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            this.InitializePlotControls();
            
            // Add main controls to form
            this.Controls.Add(this.plotView);
            this.Controls.Add(this.grpSimulation);
            this.Controls.Add(this.grpPidParameters);
            this.Controls.Add(this.grpProcessParameters);
            this.Controls.Add(this.grpProcessStatus);
            this.Controls.Add(this.grpPlotControls);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void InitializePidControls()
        {
            // Auto/Manual buttons
            this.btnAuto = new Button();
            this.btnAuto.BackColor = Color.LightGreen;
            this.btnAuto.Location = new Point(15, 25);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new Size(90, 35);
            this.btnAuto.TabIndex = 0;
            this.btnAuto.Text = "AUTO";
            this.btnAuto.UseVisualStyleBackColor = false;
            this.btnAuto.Click += new EventHandler(this.BtnAuto_Click);
            
            this.btnManual = new Button();
            this.btnManual.BackColor = Color.LightGray;
            this.btnManual.Location = new Point(115, 25);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new Size(90, 35);
            this.btnManual.TabIndex = 1;
            this.btnManual.Text = "MANUAL";
            this.btnManual.UseVisualStyleBackColor = false;
            this.btnManual.Click += new EventHandler(this.BtnManual_Click);
            
            // PID Algorithm - Fixed width to prevent truncation
            var lblAlgorithm = new Label();
            lblAlgorithm.Location = new Point(220, 25);
            lblAlgorithm.Name = "lblAlgorithm";
            lblAlgorithm.Size = new Size(80, 23);
            lblAlgorithm.Text = "Algorithm:";
            
            this.cmbPidAlgorithm = new ComboBox();
            this.cmbPidAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPidAlgorithm.Items.AddRange(new[] { "BasicPID", "I-PD", "PI-D" });
            this.cmbPidAlgorithm.Location = new Point(220, 48);
            this.cmbPidAlgorithm.Name = "cmbPidAlgorithm";
            this.cmbPidAlgorithm.SelectedIndex = 0;
            this.cmbPidAlgorithm.Size = new Size(100, 28);
            this.cmbPidAlgorithm.TabIndex = 2;
            this.cmbPidAlgorithm.SelectedIndexChanged += new EventHandler(this.CmbPidAlgorithm_SelectedIndexChanged);
            
            // Setpoint with limits
            var lblSetpoint = new Label();
            lblSetpoint.Location = new Point(15, 85);
            lblSetpoint.Name = "lblSetpoint";
            lblSetpoint.Size = new Size(70, 23);
            lblSetpoint.Text = "Setpoint:";
            
            this.numSetpoint = new NumericUpDown();
            this.numSetpoint.DecimalPlaces = 1;
            this.numSetpoint.Location = new Point(15, 105);
            this.numSetpoint.Maximum = 100M;
            this.numSetpoint.Name = "numSetpoint";
            this.numSetpoint.Size = new Size(70, 27);
            this.numSetpoint.TabIndex = 3;
            this.numSetpoint.Value = 50M;
            this.numSetpoint.ValueChanged += new EventHandler(this.NumSetpoint_ValueChanged);
            
            // SP Range - Changed from "SP Limits" to "SP Range" for better fit
            var lblSPRange = new Label();
            lblSPRange.Location = new Point(95, 85);
            lblSPRange.Name = "lblSPRange";
            lblSPRange.Size = new Size(70, 23);
            lblSPRange.Text = "SP Range:";
            
            var lblSPLo = new Label();
            lblSPLo.Location = new Point(95, 108);
            lblSPLo.Name = "lblSPLo";
            lblSPLo.Size = new Size(25, 20);
            lblSPLo.Text = "Lo:";
            
            this.numSetpointLow = new NumericUpDown();
            this.numSetpointLow.DecimalPlaces = 1;
            this.numSetpointLow.Location = new Point(120, 105);
            this.numSetpointLow.Maximum = 100M;
            this.numSetpointLow.Name = "numSetpointLow";
            this.numSetpointLow.Size = new Size(60, 27);
            this.numSetpointLow.TabIndex = 4;
            this.numSetpointLow.Value = 0M;
            this.numSetpointLow.ValueChanged += new EventHandler(this.NumSetpointLow_ValueChanged);
            
            var lblSPHi = new Label();
            lblSPHi.Location = new Point(185, 108);
            lblSPHi.Name = "lblSPHi";
            lblSPHi.Size = new Size(25, 20);
            lblSPHi.Text = "Hi:";
            
            this.numSetpointHigh = new NumericUpDown();
            this.numSetpointHigh.DecimalPlaces = 1;
            this.numSetpointHigh.Location = new Point(210, 105);
            this.numSetpointHigh.Maximum = 100M;
            this.numSetpointHigh.Name = "numSetpointHigh";
            this.numSetpointHigh.Size = new Size(60, 27);
            this.numSetpointHigh.TabIndex = 5;
            this.numSetpointHigh.Value = 100M;
            this.numSetpointHigh.ValueChanged += new EventHandler(this.NumSetpointHigh_ValueChanged);
            
            // Controller Output - Changed from "Manual Out:" to "Controller Output:"
            var lblControllerOut = new Label();
            lblControllerOut.Location = new Point(280, 85);
            lblControllerOut.Name = "lblControllerOut";
            lblControllerOut.Size = new Size(110, 23);
            lblControllerOut.Text = "Controller Output:";
            
            this.numManualOutput = new NumericUpDown();
            this.numManualOutput.DecimalPlaces = 1;
            this.numManualOutput.Enabled = false;
            this.numManualOutput.Location = new Point(280, 105);
            this.numManualOutput.Maximum = 100M;
            this.numManualOutput.Name = "numManualOutput";
            this.numManualOutput.Size = new Size(70, 27);
            this.numManualOutput.TabIndex = 6;
            this.numManualOutput.Value = 50M;
            this.numManualOutput.ValueChanged += new EventHandler(this.NumManualOutput_ValueChanged);
            
            // OP Range - Changed from "OP Limits" and adjusted positioning
            var lblOPRange = new Label();
            lblOPRange.Location = new Point(360, 85);
            lblOPRange.Name = "lblOPRange";
            lblOPRange.Size = new Size(70, 23);
            lblOPRange.Text = "OP Range:";
            
            var lblOPLo = new Label();
            lblOPLo.Location = new Point(360, 108);
            lblOPLo.Name = "lblOPLo";
            lblOPLo.Size = new Size(25, 20);
            lblOPLo.Text = "Lo:";
            
            this.numOutputLow = new NumericUpDown();
            this.numOutputLow.DecimalPlaces = 1;
            this.numOutputLow.Location = new Point(385, 105);
            this.numOutputLow.Maximum = 100M;
            this.numOutputLow.Name = "numOutputLow";
            this.numOutputLow.Size = new Size(60, 27);
            this.numOutputLow.TabIndex = 7;
            this.numOutputLow.Value = 0M;
            this.numOutputLow.ValueChanged += new EventHandler(this.NumOutputLow_ValueChanged);
            
            var lblOPHi = new Label();
            lblOPHi.Location = new Point(450, 108);
            lblOPHi.Name = "lblOPHi";
            lblOPHi.Size = new Size(25, 20);
            lblOPHi.Text = "Hi:";
            
            this.numOutputHigh = new NumericUpDown();
            this.numOutputHigh.DecimalPlaces = 1;
            this.numOutputHigh.Location = new Point(475, 105);
            this.numOutputHigh.Maximum = 100M;
            this.numOutputHigh.Name = "numOutputHigh";
            this.numOutputHigh.Size = new Size(60, 27);
            this.numOutputHigh.TabIndex = 8;
            this.numOutputHigh.Value = 100M;
            this.numOutputHigh.ValueChanged += new EventHandler(this.NumOutputHigh_ValueChanged);
            
            // PID Gains - Adjusted spacing to prevent truncation
            var lblKp = new Label();
            lblKp.Location = new Point(15, 145);
            lblKp.Name = "lblKp";
            lblKp.Size = new Size(90, 23);
            lblKp.Text = "Gain (Kp):";
            
            this.numKp = new NumericUpDown();
            this.numKp.DecimalPlaces = 3;
            this.numKp.Location = new Point(15, 165);
            this.numKp.Maximum = 100M;
            this.numKp.Name = "numKp";
            this.numKp.Size = new Size(90, 27);
            this.numKp.TabIndex = 9;
            this.numKp.Value = 1M;
            this.numKp.ValueChanged += new EventHandler(this.NumKp_ValueChanged);
            
            var lblKi = new Label();
            lblKi.Location = new Point(120, 145);
            lblKi.Name = "lblKi";
            lblKi.Size = new Size(100, 23);
            lblKi.Text = "Integral (Ki/s):";
            
            this.numKi = new NumericUpDown();
            this.numKi.DecimalPlaces = 3;
            this.numKi.Location = new Point(120, 165);
            this.numKi.Maximum = 10M;
            this.numKi.Name = "numKi";
            this.numKi.Size = new Size(90, 27);
            this.numKi.TabIndex = 10;
            this.numKi.Value = 0.1M;
            this.numKi.ValueChanged += new EventHandler(this.NumKi_ValueChanged);
            
            var lblKd = new Label();
            lblKd.Location = new Point(230, 145);
            lblKd.Name = "lblKd";
            lblKd.Size = new Size(110, 23);
            lblKd.Text = "Derivative (Kd*s):";
            
            this.numKd = new NumericUpDown();
            this.numKd.DecimalPlaces = 3;
            this.numKd.Location = new Point(230, 165);
            this.numKd.Maximum = 10M;
            this.numKd.Name = "numKd";
            this.numKd.Size = new Size(90, 27);
            this.numKd.TabIndex = 11;
            this.numKd.Value = 0.05M;
            this.numKd.ValueChanged += new EventHandler(this.NumKd_ValueChanged);
            
            // Setpoint Tracking checkbox - Adjusted positioning
            this.chkSetpointTracking = new CheckBox();
            this.chkSetpointTracking.Location = new Point(350, 165);
            this.chkSetpointTracking.Name = "chkSetpointTracking";
            this.chkSetpointTracking.Size = new Size(200, 24);
            this.chkSetpointTracking.TabIndex = 12;
            this.chkSetpointTracking.Text = "SP Tracks PV (Manual)";
            this.chkSetpointTracking.UseVisualStyleBackColor = true;
            
            // Add all controls to PID Parameters group
            this.grpPidParameters.Controls.Add(this.btnAuto);
            this.grpPidParameters.Controls.Add(this.btnManual);
            this.grpPidParameters.Controls.Add(lblAlgorithm);
            this.grpPidParameters.Controls.Add(this.cmbPidAlgorithm);
            this.grpPidParameters.Controls.Add(lblSetpoint);
            this.grpPidParameters.Controls.Add(this.numSetpoint);
            this.grpPidParameters.Controls.Add(lblSPRange);
            this.grpPidParameters.Controls.Add(lblSPLo);
            this.grpPidParameters.Controls.Add(this.numSetpointLow);
            this.grpPidParameters.Controls.Add(lblSPHi);
            this.grpPidParameters.Controls.Add(this.numSetpointHigh);
            this.grpPidParameters.Controls.Add(lblControllerOut);
            this.grpPidParameters.Controls.Add(this.numManualOutput);
            this.grpPidParameters.Controls.Add(lblOPRange);
            this.grpPidParameters.Controls.Add(lblOPLo);
            this.grpPidParameters.Controls.Add(this.numOutputLow);
            this.grpPidParameters.Controls.Add(lblOPHi);
            this.grpPidParameters.Controls.Add(this.numOutputHigh);
            this.grpPidParameters.Controls.Add(lblKp);
            this.grpPidParameters.Controls.Add(this.numKp);
            this.grpPidParameters.Controls.Add(lblKi);
            this.grpPidParameters.Controls.Add(this.numKi);
            this.grpPidParameters.Controls.Add(lblKd);
            this.grpPidParameters.Controls.Add(this.numKd);
            this.grpPidParameters.Controls.Add(this.chkSetpointTracking);
        }

        private void InitializeProcessControls()
        {
            // Process Gain
            var lblGain = new Label();
            lblGain.Location = new Point(15, 30);
            lblGain.Name = "lblGain";
            lblGain.Size = new Size(40, 23);
            lblGain.Text = "Gain:";
            
            this.numProcessGain = new NumericUpDown();
            this.numProcessGain.DecimalPlaces = 2;
            this.numProcessGain.Location = new Point(15, 50);
            this.numProcessGain.Maximum = 10M;
            this.numProcessGain.Minimum = 0.1M;
            this.numProcessGain.Name = "numProcessGain";
            this.numProcessGain.Size = new Size(80, 27);
            this.numProcessGain.TabIndex = 0;
            this.numProcessGain.Value = 1M;
            this.numProcessGain.ValueChanged += new EventHandler(this.NumProcessGain_ValueChanged);
            
            // Time Constant
            var lblTau = new Label();
            lblTau.Location = new Point(110, 30);
            lblTau.Name = "lblTau";
            lblTau.Size = new Size(60, 23);
            lblTau.Text = "Tau (s):";
            
            this.numTimeConstant = new NumericUpDown();
            this.numTimeConstant.DecimalPlaces = 1;
            this.numTimeConstant.Location = new Point(110, 50);
            this.numTimeConstant.Maximum = 100M;
            this.numTimeConstant.Minimum = 0.1M;
            this.numTimeConstant.Name = "numTimeConstant";
            this.numTimeConstant.Size = new Size(80, 27);
            this.numTimeConstant.TabIndex = 1;
            this.numTimeConstant.Value = 10M;
            this.numTimeConstant.ValueChanged += new EventHandler(this.NumTimeConstant_ValueChanged);
            
            // Dead Time
            var lblTd = new Label();
            lblTd.Location = new Point(205, 30);
            lblTd.Name = "lblTd";
            lblTd.Size = new Size(60, 23);
            lblTd.Text = "Td (s):";
            
            this.numDeadTime = new NumericUpDown();
            this.numDeadTime.DecimalPlaces = 1;
            this.numDeadTime.Location = new Point(205, 50);
            this.numDeadTime.Maximum = 50M;
            this.numDeadTime.Name = "numDeadTime";
            this.numDeadTime.Size = new Size(80, 27);
            this.numDeadTime.TabIndex = 2;
            this.numDeadTime.Value = 1M;
            this.numDeadTime.ValueChanged += new EventHandler(this.NumDeadTime_ValueChanged);
            
            // Disturbance
            var lblDist = new Label();
            lblDist.Location = new Point(300, 30);
            lblDist.Name = "lblDist";
            lblDist.Size = new Size(80, 23);
            lblDist.Text = "Disturbance:";
            
            this.numDisturbance = new NumericUpDown();
            this.numDisturbance.DecimalPlaces = 1;
            this.numDisturbance.Location = new Point(300, 50);
            this.numDisturbance.Maximum = 50M;
            this.numDisturbance.Minimum = -50M;
            this.numDisturbance.Name = "numDisturbance";
            this.numDisturbance.Size = new Size(80, 27);
            this.numDisturbance.TabIndex = 3;
            this.numDisturbance.ValueChanged += new EventHandler(this.NumDisturbance_ValueChanged);
            
            // Add controls to process parameters group
            this.grpProcessParameters.Controls.Add(lblGain);
            this.grpProcessParameters.Controls.Add(this.numProcessGain);
            this.grpProcessParameters.Controls.Add(lblTau);
            this.grpProcessParameters.Controls.Add(this.numTimeConstant);
            this.grpProcessParameters.Controls.Add(lblTd);
            this.grpProcessParameters.Controls.Add(this.numDeadTime);
            this.grpProcessParameters.Controls.Add(lblDist);
            this.grpProcessParameters.Controls.Add(this.numDisturbance);
        }

        private void InitializeStatusControls()
        {
            // Process Variable
            var lblPVLabel = new Label();
            lblPVLabel.Location = new Point(15, 25);
            lblPVLabel.Name = "lblPVLabel";
            lblPVLabel.Size = new Size(60, 23);
            lblPVLabel.Text = "PV (%):";
            
            this.lblProcessVariable = new Label();
            this.lblProcessVariable.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblProcessVariable.ForeColor = Color.Green;
            this.lblProcessVariable.Location = new Point(15, 45);
            this.lblProcessVariable.Name = "lblProcessVariable";
            this.lblProcessVariable.Size = new Size(80, 30);
            this.lblProcessVariable.Text = "50.00";
            
            // Controller Output
            var lblMVLabel = new Label();
            lblMVLabel.Location = new Point(120, 25);
            lblMVLabel.Name = "lblMVLabel";
            lblMVLabel.Size = new Size(60, 23);
            lblMVLabel.Text = "MV (%):";
            
            this.lblControllerOutput = new Label();
            this.lblControllerOutput.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblControllerOutput.ForeColor = Color.Red;
            this.lblControllerOutput.Location = new Point(120, 45);
            this.lblControllerOutput.Name = "lblControllerOutput";
            this.lblControllerOutput.Size = new Size(80, 30);
            this.lblControllerOutput.Text = "50.00";
            
            // Error
            var lblErrorLabel = new Label();
            lblErrorLabel.Location = new Point(220, 25);
            lblErrorLabel.Name = "lblErrorLabel";
            lblErrorLabel.Size = new Size(50, 23);
            lblErrorLabel.Text = "Error:";
            
            this.lblError = new Label();
            this.lblError.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblError.ForeColor = Color.Orange;
            this.lblError.Location = new Point(220, 45);
            this.lblError.Name = "lblError";
            this.lblError.Size = new Size(80, 30);
            this.lblError.Text = "0.00";
            
            // Add controls to process status group
            this.grpProcessStatus.Controls.Add(lblPVLabel);
            this.grpProcessStatus.Controls.Add(this.lblProcessVariable);
            this.grpProcessStatus.Controls.Add(lblMVLabel);
            this.grpProcessStatus.Controls.Add(this.lblControllerOutput);
            this.grpProcessStatus.Controls.Add(lblErrorLabel);
            this.grpProcessStatus.Controls.Add(this.lblError);
        }

        private void InitializePlotControls()
        {
            // X Axis Controls - Widened labels to prevent truncation
            var lblXAxis = new Label();
            lblXAxis.Location = new Point(15, 25);
            lblXAxis.Name = "lblXAxis";
            lblXAxis.Size = new Size(80, 23);
            lblXAxis.Text = "X Axis:";
            
            var lblXMin = new Label();
            lblXMin.Location = new Point(15, 50);
            lblXMin.Name = "lblXMin";
            lblXMin.Size = new Size(35, 20);
            lblXMin.Text = "Min:";
            
            this.numXAxisMin = new NumericUpDown();
            this.numXAxisMin.DecimalPlaces = 0;
            this.numXAxisMin.Location = new Point(50, 47);
            this.numXAxisMin.Maximum = 1000M;
            this.numXAxisMin.Name = "numXAxisMin";
            this.numXAxisMin.Size = new Size(80, 27);
            this.numXAxisMin.TabIndex = 0;
            this.numXAxisMin.ValueChanged += new EventHandler(this.NumXAxisMin_ValueChanged);
            
            var lblXMax = new Label();
            lblXMax.Location = new Point(140, 50);
            lblXMax.Name = "lblXMax";
            lblXMax.Size = new Size(35, 20);
            lblXMax.Text = "Max:";
            
            this.numXAxisMax = new NumericUpDown();
            this.numXAxisMax.DecimalPlaces = 0;
            this.numXAxisMax.Location = new Point(175, 47);
            this.numXAxisMax.Maximum = 3600M;
            this.numXAxisMax.Minimum = 10M;
            this.numXAxisMax.Name = "numXAxisMax";
            this.numXAxisMax.Size = new Size(80, 27);
            this.numXAxisMax.TabIndex = 1;
            this.numXAxisMax.Value = 300M;
            this.numXAxisMax.ValueChanged += new EventHandler(this.NumXAxisMax_ValueChanged);
            
            // Y Axis Controls
            var lblYAxis = new Label();
            lblYAxis.Location = new Point(275, 25);
            lblYAxis.Name = "lblYAxis";
            lblYAxis.Size = new Size(80, 23);
            lblYAxis.Text = "Y Axis:";
            
            var lblYMin = new Label();
            lblYMin.Location = new Point(275, 50);
            lblYMin.Name = "lblYMin";
            lblYMin.Size = new Size(35, 20);
            lblYMin.Text = "Min:";
            
            this.numYAxisMin = new NumericUpDown();
            this.numYAxisMin.DecimalPlaces = 0;
            this.numYAxisMin.Location = new Point(310, 47);
            this.numYAxisMin.Maximum = 100M;
            this.numYAxisMin.Minimum = -100M;
            this.numYAxisMin.Name = "numYAxisMin";
            this.numYAxisMin.Size = new Size(80, 27);
            this.numYAxisMin.TabIndex = 2;
            this.numYAxisMin.ValueChanged += new EventHandler(this.NumYAxisMin_ValueChanged);
            
            var lblYMax = new Label();
            lblYMax.Location = new Point(400, 50);
            lblYMax.Name = "lblYMax";
            lblYMax.Size = new Size(35, 20);
            lblYMax.Text = "Max:";
            
            this.numYAxisMax = new NumericUpDown();
            this.numYAxisMax.DecimalPlaces = 0;
            this.numYAxisMax.Location = new Point(435, 47);
            this.numYAxisMax.Maximum = 200M;
            this.numYAxisMax.Minimum = 10M;
            this.numYAxisMax.Name = "numYAxisMax";
            this.numYAxisMax.Size = new Size(80, 27);
            this.numYAxisMax.TabIndex = 3;
            this.numYAxisMax.Value = 100M;
            this.numYAxisMax.ValueChanged += new EventHandler(this.NumYAxisMax_ValueChanged);
            
            // Auto Scale checkbox
            this.chkAutoScale = new CheckBox();
            this.chkAutoScale.Checked = true;
            this.chkAutoScale.Location = new Point(525, 48);
            this.chkAutoScale.Name = "chkAutoScale";
            this.chkAutoScale.Size = new Size(70, 24);
            this.chkAutoScale.TabIndex = 4;
            this.chkAutoScale.Text = "Auto";
            this.chkAutoScale.UseVisualStyleBackColor = true;
            
            // Add controls to plot controls group
            this.grpPlotControls.Controls.Add(lblXAxis);
            this.grpPlotControls.Controls.Add(lblXMin);
            this.grpPlotControls.Controls.Add(this.numXAxisMin);
            this.grpPlotControls.Controls.Add(lblXMax);
            this.grpPlotControls.Controls.Add(this.numXAxisMax);
            this.grpPlotControls.Controls.Add(lblYAxis);
            this.grpPlotControls.Controls.Add(lblYMin);
            this.grpPlotControls.Controls.Add(this.numYAxisMin);
            this.grpPlotControls.Controls.Add(lblYMax);
            this.grpPlotControls.Controls.Add(this.numYAxisMax);
            this.grpPlotControls.Controls.Add(this.chkAutoScale);
        }

        #endregion
    }
}