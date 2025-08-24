/*
 * File: FirstOrderProcessWithPidForm.Designer.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Enhanced designer file for First Order Process with PID simulation form
 * Date Created: 2024-12-19
 * Date Modified: 2024-12-19
 * 
 * FIXED: Complete Visual Studio Designer compatible layout
 * FIXED: All controls properly positioned and visible in designer
 * FIXED: Optimized for HD screens with proper scaling
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
        
        // Main plot control
        private PlotView plotView;
        
        // Simulation control buttons
        private Button btnStart;
        private Button btnStop;
        private Button btnReset;
        
        // Auto/Manual control
        private Button btnAuto;
        private Button btnManual;
        
        // PID parameters
        private NumericUpDown numSetpoint;
        private NumericUpDown numKp;
        private NumericUpDown numKi; 
        private NumericUpDown numKd;
        private NumericUpDown numManualOutput;
        private ComboBox cmbPidAlgorithm;
        
        // Process parameters
        private NumericUpDown numProcessGain;
        private NumericUpDown numTimeConstant;
        private NumericUpDown numDeadTime;
        private NumericUpDown numDisturbance;
        
        // Enhanced limits
        private NumericUpDown numSetpointLow;
        private NumericUpDown numSetpointHigh;
        private NumericUpDown numOutputLow;
        private NumericUpDown numOutputHigh;
        
        // Plot axis controls
        private NumericUpDown numXAxisMin;
        private NumericUpDown numXAxisMax;
        private NumericUpDown numYAxisMin;
        private NumericUpDown numYAxisMax;
        private CheckBox chkAutoScale;
        
        // Simulation controls
        private TrackBar trkSimulationSpeed;
        private CheckBox chkSetpointTracking;
        
        // Status displays
        private Label lblProcessVariable;
        private Label lblControllerOutput;
        private Label lblError;
        private Label lblSimulationTime;
        private Label lblSimulationSpeed;
        
        // Group boxes
        private GroupBox grpSimulation;
        private GroupBox grpPidParameters;
        private GroupBox grpProcessParameters;
        private GroupBox grpProcessStatus;
        private GroupBox grpPlotControls;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            plotView = new PlotView();
            grpSimulation = new GroupBox();
            btnStart = new Button();
            btnStop = new Button();
            btnReset = new Button();
            lblSpeedLabel = new Label();
            trkSimulationSpeed = new TrackBar();
            lblSimulationSpeed = new Label();
            lblTimeLabel = new Label();
            lblSimulationTime = new Label();
            grpPidParameters = new GroupBox();
            btnAuto = new Button();
            btnManual = new Button();
            lblAlgorithm = new Label();
            cmbPidAlgorithm = new ComboBox();
            lblSetpoint = new Label();
            numSetpoint = new NumericUpDown();
            lblSPLimits = new Label();
            numSetpointLow = new NumericUpDown();
            numSetpointHigh = new NumericUpDown();
            lblManualOutput = new Label();
            numManualOutput = new NumericUpDown();
            lblOPLimits = new Label();
            numOutputLow = new NumericUpDown();
            numOutputHigh = new NumericUpDown();
            chkSetpointTracking = new CheckBox();
            lblKp = new Label();
            numKp = new NumericUpDown();
            lblKi = new Label();
            numKi = new NumericUpDown();
            lblKd = new Label();
            numKd = new NumericUpDown();
            grpProcessParameters = new GroupBox();
            lblGain = new Label();
            numProcessGain = new NumericUpDown();
            lblTau = new Label();
            numTimeConstant = new NumericUpDown();
            lblTd = new Label();
            numDeadTime = new NumericUpDown();
            lblDisturbance = new Label();
            numDisturbance = new NumericUpDown();
            grpProcessStatus = new GroupBox();
            lblPVLabel = new Label();
            lblProcessVariable = new Label();
            lblMVLabel = new Label();
            lblControllerOutput = new Label();
            lblErrorLabel = new Label();
            lblError = new Label();
            grpPlotControls = new GroupBox();
            lblXAxis = new Label();
            lblXMin = new Label();
            numXAxisMin = new NumericUpDown();
            lblXMax = new Label();
            numXAxisMax = new NumericUpDown();
            lblYAxis = new Label();
            lblYMin = new Label();
            numYAxisMin = new NumericUpDown();
            lblYMax = new Label();
            numYAxisMax = new NumericUpDown();
            chkAutoScale = new CheckBox();
            grpSimulation.SuspendLayout();
            ((ISupportInitialize)trkSimulationSpeed).BeginInit();
            grpPidParameters.SuspendLayout();
            ((ISupportInitialize)numSetpoint).BeginInit();
            ((ISupportInitialize)numSetpointLow).BeginInit();
            ((ISupportInitialize)numSetpointHigh).BeginInit();
            ((ISupportInitialize)numManualOutput).BeginInit();
            ((ISupportInitialize)numOutputLow).BeginInit();
            ((ISupportInitialize)numOutputHigh).BeginInit();
            ((ISupportInitialize)numKp).BeginInit();
            ((ISupportInitialize)numKi).BeginInit();
            ((ISupportInitialize)numKd).BeginInit();
            grpProcessParameters.SuspendLayout();
            ((ISupportInitialize)numProcessGain).BeginInit();
            ((ISupportInitialize)numTimeConstant).BeginInit();
            ((ISupportInitialize)numDeadTime).BeginInit();
            ((ISupportInitialize)numDisturbance).BeginInit();
            grpProcessStatus.SuspendLayout();
            grpPlotControls.SuspendLayout();
            ((ISupportInitialize)numXAxisMin).BeginInit();
            ((ISupportInitialize)numXAxisMax).BeginInit();
            ((ISupportInitialize)numYAxisMin).BeginInit();
            ((ISupportInitialize)numYAxisMax).BeginInit();
            SuspendLayout();
            // 
            // plotView
            // 
            plotView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            plotView.Location = new Point(22, 26);
            plotView.Margin = new Padding(6);
            plotView.Name = "plotView";
            plotView.PanCursor = Cursors.Hand;
            plotView.Size = new Size(1709, 1169);
            plotView.TabIndex = 0;
            plotView.ZoomHorizontalCursor = Cursors.SizeWE;
            plotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // grpSimulation
            // 
            grpSimulation.Controls.Add(btnStart);
            grpSimulation.Controls.Add(btnStop);
            grpSimulation.Controls.Add(btnReset);
            grpSimulation.Controls.Add(lblSpeedLabel);
            grpSimulation.Controls.Add(trkSimulationSpeed);
            grpSimulation.Controls.Add(lblSimulationSpeed);
            grpSimulation.Controls.Add(lblTimeLabel);
            grpSimulation.Controls.Add(lblSimulationTime);
            grpSimulation.Location = new Point(1764, 26);
            grpSimulation.Margin = new Padding(6);
            grpSimulation.Name = "grpSimulation";
            grpSimulation.Padding = new Padding(6);
            grpSimulation.Size = new Size(823, 293);
            grpSimulation.TabIndex = 1;
            grpSimulation.TabStop = false;
            grpSimulation.Text = "Simulation Controls";
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.LightGreen;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Location = new Point(28, 53);
            btnStart.Margin = new Padding(6);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(139, 75);
            btnStart.TabIndex = 0;
            btnStart.Text = "START";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += BtnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.LightCoral;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Location = new Point(186, 53);
            btnStop.Margin = new Padding(6);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(139, 75);
            btnStop.TabIndex = 1;
            btnStop.Text = "STOP";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += BtnStop_Click;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.LightBlue;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Location = new Point(344, 53);
            btnReset.Margin = new Padding(6);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(139, 75);
            btnReset.TabIndex = 2;
            btnReset.Text = "RESET";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += BtnReset_Click;
            // 
            // lblSpeedLabel
            // 
            lblSpeedLabel.AutoSize = true;
            lblSpeedLabel.Location = new Point(28, 151);
            lblSpeedLabel.Margin = new Padding(6, 0, 6, 0);
            lblSpeedLabel.Name = "lblSpeedLabel";
            lblSpeedLabel.Size = new Size(86, 32);
            lblSpeedLabel.TabIndex = 3;
            lblSpeedLabel.Text = "Speed:";
            // 
            // trkSimulationSpeed
            // 
            trkSimulationSpeed.Location = new Point(28, 194);
            trkSimulationSpeed.Margin = new Padding(6);
            trkSimulationSpeed.Maximum = 50;
            trkSimulationSpeed.Minimum = 1;
            trkSimulationSpeed.Name = "trkSimulationSpeed";
            trkSimulationSpeed.Size = new Size(371, 90);
            trkSimulationSpeed.TabIndex = 4;
            trkSimulationSpeed.TickFrequency = 10;
            trkSimulationSpeed.Value = 10;
            trkSimulationSpeed.ValueChanged += TrkSimulationSpeed_ValueChanged;
            // 
            // lblSimulationSpeed
            // 
            lblSimulationSpeed.AutoSize = true;
            lblSimulationSpeed.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            lblSimulationSpeed.ForeColor = Color.Green;
            lblSimulationSpeed.Location = new Point(418, 215);
            lblSimulationSpeed.Margin = new Padding(6, 0, 6, 0);
            lblSimulationSpeed.Name = "lblSimulationSpeed";
            lblSimulationSpeed.Size = new Size(60, 29);
            lblSimulationSpeed.TabIndex = 5;
            lblSimulationSpeed.Text = "1.0x";
            // 
            // lblTimeLabel
            // 
            lblTimeLabel.AutoSize = true;
            lblTimeLabel.Location = new Point(519, 151);
            lblTimeLabel.Margin = new Padding(6, 0, 6, 0);
            lblTimeLabel.Name = "lblTimeLabel";
            lblTimeLabel.Size = new Size(224, 32);
            lblTimeLabel.TabIndex = 6;
            lblTimeLabel.Text = "Simulation Time (s):";
            // 
            // lblSimulationTime
            // 
            lblSimulationTime.AutoSize = true;
            lblSimulationTime.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            lblSimulationTime.ForeColor = Color.Blue;
            lblSimulationTime.Location = new Point(519, 194);
            lblSimulationTime.Margin = new Padding(6, 0, 6, 0);
            lblSimulationTime.Name = "lblSimulationTime";
            lblSimulationTime.Size = new Size(55, 31);
            lblSimulationTime.TabIndex = 7;
            lblSimulationTime.Text = "0.0";
            // 
            // grpPidParameters
            // 
            grpPidParameters.Controls.Add(btnAuto);
            grpPidParameters.Controls.Add(btnManual);
            grpPidParameters.Controls.Add(lblAlgorithm);
            grpPidParameters.Controls.Add(cmbPidAlgorithm);
            grpPidParameters.Controls.Add(lblSetpoint);
            grpPidParameters.Controls.Add(numSetpoint);
            grpPidParameters.Controls.Add(lblSPLimits);
            grpPidParameters.Controls.Add(numSetpointLow);
            grpPidParameters.Controls.Add(numSetpointHigh);
            grpPidParameters.Controls.Add(lblManualOutput);
            grpPidParameters.Controls.Add(numManualOutput);
            grpPidParameters.Controls.Add(lblOPLimits);
            grpPidParameters.Controls.Add(numOutputLow);
            grpPidParameters.Controls.Add(numOutputHigh);
            grpPidParameters.Controls.Add(chkSetpointTracking);
            grpPidParameters.Controls.Add(lblKp);
            grpPidParameters.Controls.Add(numKp);
            grpPidParameters.Controls.Add(lblKi);
            grpPidParameters.Controls.Add(numKi);
            grpPidParameters.Controls.Add(lblKd);
            grpPidParameters.Controls.Add(numKd);
            grpPidParameters.Location = new Point(1764, 355);
            grpPidParameters.Margin = new Padding(6);
            grpPidParameters.Name = "grpPidParameters";
            grpPidParameters.Padding = new Padding(6);
            grpPidParameters.Size = new Size(823, 598);
            grpPidParameters.TabIndex = 2;
            grpPidParameters.TabStop = false;
            grpPidParameters.Text = "PID Parameters";
            // 
            // btnAuto
            // 
            btnAuto.BackColor = Color.LightGreen;
            btnAuto.FlatStyle = FlatStyle.Flat;
            btnAuto.Location = new Point(28, 53);
            btnAuto.Margin = new Padding(6);
            btnAuto.Name = "btnAuto";
            btnAuto.Size = new Size(149, 75);
            btnAuto.TabIndex = 0;
            btnAuto.Text = "AUTO";
            btnAuto.UseVisualStyleBackColor = false;
            btnAuto.Click += BtnAuto_Click;
            // 
            // btnManual
            // 
            btnManual.BackColor = Color.LightGray;
            btnManual.FlatStyle = FlatStyle.Flat;
            btnManual.Location = new Point(195, 53);
            btnManual.Margin = new Padding(6);
            btnManual.Name = "btnManual";
            btnManual.Size = new Size(149, 75);
            btnManual.TabIndex = 1;
            btnManual.Text = "MANUAL";
            btnManual.UseVisualStyleBackColor = false;
            btnManual.Click += BtnManual_Click;
            // 
            // lblAlgorithm
            // 
            lblAlgorithm.AutoSize = true;
            lblAlgorithm.Location = new Point(373, 96);
            lblAlgorithm.Margin = new Padding(6, 0, 6, 0);
            lblAlgorithm.Name = "lblAlgorithm";
            lblAlgorithm.Size = new Size(125, 32);
            lblAlgorithm.TabIndex = 2;
            lblAlgorithm.Text = "Algorithm:";
            // 
            // cmbPidAlgorithm
            // 
            cmbPidAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPidAlgorithm.Items.AddRange(new object[] { "BasicPID", "I-PD", "PI-D" });
            cmbPidAlgorithm.Location = new Point(521, 90);
            cmbPidAlgorithm.Margin = new Padding(6);
            cmbPidAlgorithm.Name = "cmbPidAlgorithm";
            cmbPidAlgorithm.Size = new Size(219, 40);
            cmbPidAlgorithm.TabIndex = 3;
            cmbPidAlgorithm.SelectedIndexChanged += CmbPidAlgorithm_SelectedIndexChanged;
            // 
            // lblSetpoint
            // 
            lblSetpoint.AutoSize = true;
            lblSetpoint.Location = new Point(28, 160);
            lblSetpoint.Margin = new Padding(6, 0, 6, 0);
            lblSetpoint.Name = "lblSetpoint";
            lblSetpoint.Size = new Size(109, 32);
            lblSetpoint.TabIndex = 4;
            lblSetpoint.Text = "Setpoint:";
            // 
            // numSetpoint
            // 
            numSetpoint.DecimalPlaces = 1;
            numSetpoint.Location = new Point(28, 203);
            numSetpoint.Margin = new Padding(6);
            numSetpoint.Name = "numSetpoint";
            numSetpoint.Size = new Size(130, 39);
            numSetpoint.TabIndex = 5;
            numSetpoint.Value = new decimal(new int[] { 50, 0, 0, 0 });
            numSetpoint.ValueChanged += NumSetpoint_ValueChanged;
            // 
            // lblSPLimits
            // 
            lblSPLimits.AutoSize = true;
            lblSPLimits.Location = new Point(28, 304);
            lblSPLimits.Margin = new Padding(6, 0, 6, 0);
            lblSPLimits.Name = "lblSPLimits";
            lblSPLimits.Size = new Size(114, 32);
            lblSPLimits.TabIndex = 6;
            lblSPLimits.Text = "SP Limits:";
            // 
            // numSetpointLow
            // 
            numSetpointLow.DecimalPlaces = 1;
            numSetpointLow.Location = new Point(28, 347);
            numSetpointLow.Margin = new Padding(6);
            numSetpointLow.Name = "numSetpointLow";
            numSetpointLow.Size = new Size(111, 39);
            numSetpointLow.TabIndex = 7;
            numSetpointLow.ValueChanged += NumSetpointLow_ValueChanged;
            // 
            // numSetpointHigh
            // 
            numSetpointHigh.DecimalPlaces = 1;
            numSetpointHigh.Location = new Point(158, 347);
            numSetpointHigh.Margin = new Padding(6);
            numSetpointHigh.Name = "numSetpointHigh";
            numSetpointHigh.Size = new Size(111, 39);
            numSetpointHigh.TabIndex = 8;
            numSetpointHigh.Value = new decimal(new int[] { 100, 0, 0, 0 });
            numSetpointHigh.ValueChanged += NumSetpointHigh_ValueChanged;
            // 
            // lblManualOutput
            // 
            lblManualOutput.AutoSize = true;
            lblManualOutput.Location = new Point(195, 160);
            lblManualOutput.Margin = new Padding(6, 0, 6, 0);
            lblManualOutput.Name = "lblManualOutput";
            lblManualOutput.Size = new Size(95, 32);
            lblManualOutput.TabIndex = 9;
            lblManualOutput.Text = "Output:";
            // 
            // numManualOutput
            // 
            numManualOutput.DecimalPlaces = 1;
            numManualOutput.Enabled = false;
            numManualOutput.Location = new Point(195, 203);
            numManualOutput.Margin = new Padding(6);
            numManualOutput.Name = "numManualOutput";
            numManualOutput.Size = new Size(130, 39);
            numManualOutput.TabIndex = 10;
            numManualOutput.Value = new decimal(new int[] { 50, 0, 0, 0 });
            numManualOutput.ValueChanged += NumManualOutput_ValueChanged;
            // 
            // lblOPLimits
            // 
            lblOPLimits.AutoSize = true;
            lblOPLimits.Location = new Point(332, 304);
            lblOPLimits.Margin = new Padding(6, 0, 6, 0);
            lblOPLimits.Name = "lblOPLimits";
            lblOPLimits.Size = new Size(119, 32);
            lblOPLimits.TabIndex = 11;
            lblOPLimits.Text = "OP Limits:";
            // 
            // numOutputLow
            // 
            numOutputLow.DecimalPlaces = 1;
            numOutputLow.Location = new Point(332, 347);
            numOutputLow.Margin = new Padding(6);
            numOutputLow.Name = "numOutputLow";
            numOutputLow.Size = new Size(111, 39);
            numOutputLow.TabIndex = 12;
            numOutputLow.ValueChanged += NumOutputLow_ValueChanged;
            // 
            // numOutputHigh
            // 
            numOutputHigh.DecimalPlaces = 1;
            numOutputHigh.Location = new Point(462, 347);
            numOutputHigh.Margin = new Padding(6);
            numOutputHigh.Name = "numOutputHigh";
            numOutputHigh.Size = new Size(111, 39);
            numOutputHigh.TabIndex = 13;
            numOutputHigh.Value = new decimal(new int[] { 100, 0, 0, 0 });
            numOutputHigh.ValueChanged += NumOutputHigh_ValueChanged;
            // 
            // chkSetpointTracking
            // 
            chkSetpointTracking.AutoSize = true;
            chkSetpointTracking.Checked = true;
            chkSetpointTracking.CheckState = CheckState.Checked;
            chkSetpointTracking.Location = new Point(373, 206);
            chkSetpointTracking.Margin = new Padding(6);
            chkSetpointTracking.Name = "chkSetpointTracking";
            chkSetpointTracking.Size = new Size(247, 36);
            chkSetpointTracking.TabIndex = 14;
            chkSetpointTracking.Text = "SP Tracks PV (Man)";
            chkSetpointTracking.UseVisualStyleBackColor = true;
            // 
            // lblKp
            // 
            lblKp.AutoSize = true;
            lblKp.Location = new Point(26, 482);
            lblKp.Margin = new Padding(6, 0, 6, 0);
            lblKp.Name = "lblKp";
            lblKp.Size = new Size(116, 32);
            lblKp.TabIndex = 15;
            lblKp.Text = "Gain (Kp):";
            // 
            // numKp
            // 
            numKp.DecimalPlaces = 3;
            numKp.Location = new Point(26, 525);
            numKp.Margin = new Padding(6);
            numKp.Name = "numKp";
            numKp.Size = new Size(149, 39);
            numKp.TabIndex = 16;
            numKp.Value = new decimal(new int[] { 1000, 0, 0, 196608 });
            numKp.ValueChanged += NumKp_ValueChanged;
            // 
            // lblKi
            // 
            lblKi.AutoSize = true;
            lblKi.Location = new Point(221, 482);
            lblKi.Margin = new Padding(6, 0, 6, 0);
            lblKi.Name = "lblKi";
            lblKi.Size = new Size(160, 32);
            lblKi.TabIndex = 17;
            lblKi.Text = "Integral (Ki/s):";
            // 
            // numKi
            // 
            numKi.DecimalPlaces = 3;
            numKi.Location = new Point(221, 525);
            numKi.Margin = new Padding(6);
            numKi.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numKi.Name = "numKi";
            numKi.Size = new Size(149, 39);
            numKi.TabIndex = 18;
            numKi.Value = new decimal(new int[] { 100, 0, 0, 196608 });
            numKi.ValueChanged += NumKi_ValueChanged;
            // 
            // lblKd
            // 
            lblKd.AutoSize = true;
            lblKd.Location = new Point(425, 482);
            lblKd.Margin = new Padding(6, 0, 6, 0);
            lblKd.Name = "lblKd";
            lblKd.Size = new Size(195, 32);
            lblKd.TabIndex = 19;
            lblKd.Text = "Derivative (Kd*s):";
            // 
            // numKd
            // 
            numKd.DecimalPlaces = 3;
            numKd.Location = new Point(425, 525);
            numKd.Margin = new Padding(6);
            numKd.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numKd.Name = "numKd";
            numKd.Size = new Size(149, 39);
            numKd.TabIndex = 20;
            numKd.Value = new decimal(new int[] { 50, 0, 0, 196608 });
            numKd.ValueChanged += NumKd_ValueChanged;
            // 
            // grpProcessParameters
            // 
            grpProcessParameters.Controls.Add(lblGain);
            grpProcessParameters.Controls.Add(numProcessGain);
            grpProcessParameters.Controls.Add(lblTau);
            grpProcessParameters.Controls.Add(numTimeConstant);
            grpProcessParameters.Controls.Add(lblTd);
            grpProcessParameters.Controls.Add(numDeadTime);
            grpProcessParameters.Controls.Add(lblDisturbance);
            grpProcessParameters.Controls.Add(numDisturbance);
            grpProcessParameters.Location = new Point(1764, 982);
            grpProcessParameters.Margin = new Padding(6);
            grpProcessParameters.Name = "grpProcessParameters";
            grpProcessParameters.Padding = new Padding(6);
            grpProcessParameters.Size = new Size(823, 213);
            grpProcessParameters.TabIndex = 3;
            grpProcessParameters.TabStop = false;
            grpProcessParameters.Text = "First Order Process Model";
            // 
            // lblGain
            // 
            lblGain.AutoSize = true;
            lblGain.Location = new Point(28, 53);
            lblGain.Margin = new Padding(6, 0, 6, 0);
            lblGain.Name = "lblGain";
            lblGain.Size = new Size(67, 32);
            lblGain.TabIndex = 0;
            lblGain.Text = "Gain:";
            // 
            // numProcessGain
            // 
            numProcessGain.DecimalPlaces = 2;
            numProcessGain.Location = new Point(28, 96);
            numProcessGain.Margin = new Padding(6);
            numProcessGain.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numProcessGain.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numProcessGain.Name = "numProcessGain";
            numProcessGain.Size = new Size(149, 39);
            numProcessGain.TabIndex = 1;
            numProcessGain.Value = new decimal(new int[] { 100, 0, 0, 131072 });
            numProcessGain.ValueChanged += NumProcessGain_ValueChanged;
            // 
            // lblTau
            // 
            lblTau.AutoSize = true;
            lblTau.Location = new Point(223, 53);
            lblTau.Margin = new Padding(6, 0, 6, 0);
            lblTau.Name = "lblTau";
            lblTau.Size = new Size(86, 32);
            lblTau.TabIndex = 2;
            lblTau.Text = "Tau (s):";
            // 
            // numTimeConstant
            // 
            numTimeConstant.DecimalPlaces = 1;
            numTimeConstant.Location = new Point(223, 96);
            numTimeConstant.Margin = new Padding(6);
            numTimeConstant.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numTimeConstant.Name = "numTimeConstant";
            numTimeConstant.Size = new Size(149, 39);
            numTimeConstant.TabIndex = 3;
            numTimeConstant.Value = new decimal(new int[] { 100, 0, 0, 65536 });
            numTimeConstant.ValueChanged += NumTimeConstant_ValueChanged;
            // 
            // lblTd
            // 
            lblTd.AutoSize = true;
            lblTd.Location = new Point(409, 53);
            lblTd.Margin = new Padding(6, 0, 6, 0);
            lblTd.Name = "lblTd";
            lblTd.Size = new Size(75, 32);
            lblTd.TabIndex = 4;
            lblTd.Text = "Td (s):";
            // 
            // numDeadTime
            // 
            numDeadTime.DecimalPlaces = 1;
            numDeadTime.Location = new Point(409, 96);
            numDeadTime.Margin = new Padding(6);
            numDeadTime.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numDeadTime.Name = "numDeadTime";
            numDeadTime.Size = new Size(149, 39);
            numDeadTime.TabIndex = 5;
            numDeadTime.Value = new decimal(new int[] { 10, 0, 0, 65536 });
            numDeadTime.ValueChanged += NumDeadTime_ValueChanged;
            // 
            // lblDisturbance
            // 
            lblDisturbance.AutoSize = true;
            lblDisturbance.Location = new Point(594, 53);
            lblDisturbance.Margin = new Padding(6, 0, 6, 0);
            lblDisturbance.Name = "lblDisturbance";
            lblDisturbance.Size = new Size(146, 32);
            lblDisturbance.TabIndex = 6;
            lblDisturbance.Text = "Disturbance:";
            // 
            // numDisturbance
            // 
            numDisturbance.DecimalPlaces = 1;
            numDisturbance.Location = new Point(594, 96);
            numDisturbance.Margin = new Padding(6);
            numDisturbance.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numDisturbance.Minimum = new decimal(new int[] { 50, 0, 0, int.MinValue });
            numDisturbance.Name = "numDisturbance";
            numDisturbance.Size = new Size(149, 39);
            numDisturbance.TabIndex = 7;
            numDisturbance.ValueChanged += NumDisturbance_ValueChanged;
            // 
            // grpProcessStatus
            // 
            grpProcessStatus.Controls.Add(lblPVLabel);
            grpProcessStatus.Controls.Add(lblProcessVariable);
            grpProcessStatus.Controls.Add(lblMVLabel);
            grpProcessStatus.Controls.Add(lblControllerOutput);
            grpProcessStatus.Controls.Add(lblErrorLabel);
            grpProcessStatus.Controls.Add(lblError);
            grpProcessStatus.Location = new Point(1764, 1227);
            grpProcessStatus.Margin = new Padding(6);
            grpProcessStatus.Name = "grpProcessStatus";
            grpProcessStatus.Padding = new Padding(6);
            grpProcessStatus.Size = new Size(823, 171);
            grpProcessStatus.TabIndex = 4;
            grpProcessStatus.TabStop = false;
            grpProcessStatus.Text = "Process Status";
            // 
            // lblPVLabel
            // 
            lblPVLabel.AutoSize = true;
            lblPVLabel.Location = new Point(28, 53);
            lblPVLabel.Margin = new Padding(6, 0, 6, 0);
            lblPVLabel.Name = "lblPVLabel";
            lblPVLabel.Size = new Size(88, 32);
            lblPVLabel.TabIndex = 0;
            lblPVLabel.Text = "PV (%):";
            // 
            // lblProcessVariable
            // 
            lblProcessVariable.AutoSize = true;
            lblProcessVariable.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            lblProcessVariable.ForeColor = Color.Green;
            lblProcessVariable.Location = new Point(28, 96);
            lblProcessVariable.Margin = new Padding(6, 0, 6, 0);
            lblProcessVariable.Name = "lblProcessVariable";
            lblProcessVariable.Size = new Size(103, 37);
            lblProcessVariable.TabIndex = 1;
            lblProcessVariable.Text = "50.00";
            // 
            // lblMVLabel
            // 
            lblMVLabel.AutoSize = true;
            lblMVLabel.Location = new Point(279, 53);
            lblMVLabel.Margin = new Padding(6, 0, 6, 0);
            lblMVLabel.Name = "lblMVLabel";
            lblMVLabel.Size = new Size(97, 32);
            lblMVLabel.TabIndex = 2;
            lblMVLabel.Text = "MV (%):";
            // 
            // lblControllerOutput
            // 
            lblControllerOutput.AutoSize = true;
            lblControllerOutput.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            lblControllerOutput.ForeColor = Color.Red;
            lblControllerOutput.Location = new Point(279, 96);
            lblControllerOutput.Margin = new Padding(6, 0, 6, 0);
            lblControllerOutput.Name = "lblControllerOutput";
            lblControllerOutput.Size = new Size(103, 37);
            lblControllerOutput.TabIndex = 3;
            lblControllerOutput.Text = "50.00";
            // 
            // lblErrorLabel
            // 
            lblErrorLabel.AutoSize = true;
            lblErrorLabel.Location = new Point(529, 53);
            lblErrorLabel.Margin = new Padding(6, 0, 6, 0);
            lblErrorLabel.Name = "lblErrorLabel";
            lblErrorLabel.Size = new Size(69, 32);
            lblErrorLabel.TabIndex = 4;
            lblErrorLabel.Text = "Error:";
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            lblError.ForeColor = Color.Orange;
            lblError.Location = new Point(529, 96);
            lblError.Margin = new Padding(6, 0, 6, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(84, 37);
            lblError.TabIndex = 5;
            lblError.Text = "0.00";
            // 
            // grpPlotControls
            // 
            grpPlotControls.Controls.Add(lblXAxis);
            grpPlotControls.Controls.Add(lblXMin);
            grpPlotControls.Controls.Add(numXAxisMin);
            grpPlotControls.Controls.Add(lblXMax);
            grpPlotControls.Controls.Add(numXAxisMax);
            grpPlotControls.Controls.Add(lblYAxis);
            grpPlotControls.Controls.Add(lblYMin);
            grpPlotControls.Controls.Add(numYAxisMin);
            grpPlotControls.Controls.Add(lblYMax);
            grpPlotControls.Controls.Add(numYAxisMax);
            grpPlotControls.Controls.Add(chkAutoScale);
            grpPlotControls.Location = new Point(22, 1227);
            grpPlotControls.Margin = new Padding(6);
            grpPlotControls.Name = "grpPlotControls";
            grpPlotControls.Padding = new Padding(6);
            grpPlotControls.Size = new Size(1709, 171);
            grpPlotControls.TabIndex = 5;
            grpPlotControls.TabStop = false;
            grpPlotControls.Text = "Plot Scale Controls";
            // 
            // lblXAxis
            // 
            lblXAxis.AutoSize = true;
            lblXAxis.Location = new Point(28, 53);
            lblXAxis.Margin = new Padding(6, 0, 6, 0);
            lblXAxis.Name = "lblXAxis";
            lblXAxis.Size = new Size(82, 32);
            lblXAxis.TabIndex = 0;
            lblXAxis.Text = "X Axis:";
            // 
            // lblXMin
            // 
            lblXMin.AutoSize = true;
            lblXMin.Location = new Point(28, 107);
            lblXMin.Margin = new Padding(6, 0, 6, 0);
            lblXMin.Name = "lblXMin";
            lblXMin.Size = new Size(61, 32);
            lblXMin.TabIndex = 1;
            lblXMin.Text = "Min:";
            // 
            // numXAxisMin
            // 
            numXAxisMin.Location = new Point(93, 100);
            numXAxisMin.Margin = new Padding(6);
            numXAxisMin.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numXAxisMin.Name = "numXAxisMin";
            numXAxisMin.Size = new Size(149, 39);
            numXAxisMin.TabIndex = 2;
            numXAxisMin.ValueChanged += NumXAxisMin_ValueChanged;
            // 
            // lblXMax
            // 
            lblXMax.AutoSize = true;
            lblXMax.Location = new Point(260, 107);
            lblXMax.Margin = new Padding(6, 0, 6, 0);
            lblXMax.Name = "lblXMax";
            lblXMax.Size = new Size(64, 32);
            lblXMax.TabIndex = 3;
            lblXMax.Text = "Max:";
            // 
            // numXAxisMax
            // 
            numXAxisMax.Location = new Point(334, 100);
            numXAxisMax.Margin = new Padding(6);
            numXAxisMax.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numXAxisMax.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numXAxisMax.Name = "numXAxisMax";
            numXAxisMax.Size = new Size(149, 39);
            numXAxisMax.TabIndex = 4;
            numXAxisMax.Value = new decimal(new int[] { 300, 0, 0, 0 });
            numXAxisMax.ValueChanged += NumXAxisMax_ValueChanged;
            // 
            // lblYAxis
            // 
            lblYAxis.AutoSize = true;
            lblYAxis.Location = new Point(520, 53);
            lblYAxis.Margin = new Padding(6, 0, 6, 0);
            lblYAxis.Name = "lblYAxis";
            lblYAxis.Size = new Size(81, 32);
            lblYAxis.TabIndex = 5;
            lblYAxis.Text = "Y Axis:";
            // 
            // lblYMin
            // 
            lblYMin.AutoSize = true;
            lblYMin.Location = new Point(520, 107);
            lblYMin.Margin = new Padding(6, 0, 6, 0);
            lblYMin.Name = "lblYMin";
            lblYMin.Size = new Size(61, 32);
            lblYMin.TabIndex = 6;
            lblYMin.Text = "Min:";
            // 
            // numYAxisMin
            // 
            numYAxisMin.Location = new Point(585, 100);
            numYAxisMin.Margin = new Padding(6);
            numYAxisMin.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            numYAxisMin.Name = "numYAxisMin";
            numYAxisMin.Size = new Size(149, 39);
            numYAxisMin.TabIndex = 7;
            numYAxisMin.ValueChanged += NumYAxisMin_ValueChanged;
            // 
            // lblYMax
            // 
            lblYMax.AutoSize = true;
            lblYMax.Location = new Point(752, 107);
            lblYMax.Margin = new Padding(6, 0, 6, 0);
            lblYMax.Name = "lblYMax";
            lblYMax.Size = new Size(64, 32);
            lblYMax.TabIndex = 8;
            lblYMax.Text = "Max:";
            // 
            // numYAxisMax
            // 
            numYAxisMax.Location = new Point(826, 100);
            numYAxisMax.Margin = new Padding(6);
            numYAxisMax.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numYAxisMax.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numYAxisMax.Name = "numYAxisMax";
            numYAxisMax.Size = new Size(149, 39);
            numYAxisMax.TabIndex = 9;
            numYAxisMax.Value = new decimal(new int[] { 100, 0, 0, 0 });
            numYAxisMax.ValueChanged += NumYAxisMax_ValueChanged;
            // 
            // chkAutoScale
            // 
            chkAutoScale.AutoSize = true;
            chkAutoScale.Checked = true;
            chkAutoScale.CheckState = CheckState.Checked;
            chkAutoScale.Location = new Point(1003, 105);
            chkAutoScale.Margin = new Padding(6);
            chkAutoScale.Name = "chkAutoScale";
            chkAutoScale.Size = new Size(180, 36);
            chkAutoScale.TabIndex = 10;
            chkAutoScale.Text = "Auto Scaling";
            chkAutoScale.UseVisualStyleBackColor = true;
            // 
            // FirstOrderProcessWithPidForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2630, 1425);
            Controls.Add(plotView);
            Controls.Add(grpSimulation);
            Controls.Add(grpPidParameters);
            Controls.Add(grpProcessParameters);
            Controls.Add(grpProcessStatus);
            Controls.Add(grpPlotControls);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(6);
            MaximizeBox = false;
            Name = "FirstOrderProcessWithPidForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "First Order Process with PID - Professional Edition";
            grpSimulation.ResumeLayout(false);
            grpSimulation.PerformLayout();
            ((ISupportInitialize)trkSimulationSpeed).EndInit();
            grpPidParameters.ResumeLayout(false);
            grpPidParameters.PerformLayout();
            ((ISupportInitialize)numSetpoint).EndInit();
            ((ISupportInitialize)numSetpointLow).EndInit();
            ((ISupportInitialize)numSetpointHigh).EndInit();
            ((ISupportInitialize)numManualOutput).EndInit();
            ((ISupportInitialize)numOutputLow).EndInit();
            ((ISupportInitialize)numOutputHigh).EndInit();
            ((ISupportInitialize)numKp).EndInit();
            ((ISupportInitialize)numKi).EndInit();
            ((ISupportInitialize)numKd).EndInit();
            grpProcessParameters.ResumeLayout(false);
            grpProcessParameters.PerformLayout();
            ((ISupportInitialize)numProcessGain).EndInit();
            ((ISupportInitialize)numTimeConstant).EndInit();
            ((ISupportInitialize)numDeadTime).EndInit();
            ((ISupportInitialize)numDisturbance).EndInit();
            grpProcessStatus.ResumeLayout(false);
            grpProcessStatus.PerformLayout();
            grpPlotControls.ResumeLayout(false);
            grpPlotControls.PerformLayout();
            ((ISupportInitialize)numXAxisMin).EndInit();
            ((ISupportInitialize)numXAxisMax).EndInit();
            ((ISupportInitialize)numYAxisMin).EndInit();
            ((ISupportInitialize)numYAxisMax).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblSpeedLabel;
        private Label lblTimeLabel;
        private Label lblAlgorithm;
        private Label lblSetpoint;
        private Label lblSPLimits;
        private Label lblManualOutput;
        private Label lblOPLimits;
        private Label lblKp;
        private Label lblKi;
        private Label lblKd;
        private Label lblGain;
        private Label lblTau;
        private Label lblTd;
        private Label lblDisturbance;
        private Label lblPVLabel;
        private Label lblMVLabel;
        private Label lblErrorLabel;
        private Label lblXAxis;
        private Label lblXMin;
        private Label lblXMax;
        private Label lblYAxis;
        private Label lblYMin;
        private Label lblYMax;
    }
}