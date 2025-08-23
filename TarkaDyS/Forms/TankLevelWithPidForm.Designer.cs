namespace TarkaDyS.Forms
{
    partial class TankLevelWithPidForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // Tank Level specific controls
        private OxyPlot.WindowsForms.PlotView plotTankLevelView;
        private GroupBox groupBoxTankLevelSimulationControls;
        private GroupBox groupBoxTankLevelControl;
        private GroupBox groupBoxTankLevelPidParameters;
        private GroupBox groupBoxTankLevelProcessParameters;
        private GroupBox groupBoxTankLevelProcessStatus;
        private GroupBox groupBoxTankLevelPlotSettings;
        private GroupBox groupBoxTankLevelLegend;
        
        // Tank Level setpoint controls
        private Label lblTankLevelSetpoint;
        private NumericUpDown numTankLevelSetpoint;
        
        // Tank Level PID parameter controls
        private Label lblTankLevelKp;
        private NumericUpDown numTankLevelKp;
        private Label lblTankLevelKi;
        private NumericUpDown numTankLevelKi;
        private Label lblTankLevelKd;
        private NumericUpDown numTankLevelKd;
        
        // Tank Level process parameter controls
        private Label lblTankLevelCrossSectionArea;
        private NumericUpDown numTankLevelCrossSectionArea;
        private Label lblTankLevelOutflowRate;
        private NumericUpDown numTankLevelOutflowRate;
        private Label lblTankLevelMaxLevel;
        private NumericUpDown numTankLevelMaxLevel;
        private Label lblTankLevelDisturbance;
        private NumericUpDown numTankLevelDisturbance;
        
        // Tank Level control mode controls
        private Button btnTankLevelAuto;
        private Button btnTankLevelManual;
        private Label lblTankLevelManualOutput;
        private NumericUpDown numTankLevelManualOutput;
        private CheckBox chkTankLevelSetpointTracking;
        
        // Tank Level simulation controls
        private Button btnTankLevelStart;
        private Button btnTankLevelStop;
        private Button btnTankLevelReset;
        
        // Tank Level status displays
        private Label lblTankLevelSimulationTimeLabel;
        private Label lblTankLevelSimulationTime;
        private Label lblTankLevelProcessVariableLabel;
        private Label lblTankLevelProcessVariable;
        private Label lblTankLevelControllerOutputLabel;
        private Label lblTankLevelControllerOutput;
        private Label lblTankLevelErrorLabel;
        private Label lblTankLevelError;
        
        // Tank Level plot settings
        private Label lblTankLevelTrendDuration;
        private TextBox txtTankLevelTrendDuration;
        private Button btnTankLevelUpdateTrend;
        
        // Tank Level legend
        private Label lblTankLevelSetpointLegend;
        private Label lblTankLevelProcessVariableLegend;
        private Label lblTankLevelControllerOutputLegend;
        private Label lblTankLevelErrorLegend;
        
        private ToolTip toolTipTankLevel;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            plotTankLevelView = new OxyPlot.WindowsForms.PlotView();
            groupBoxTankLevelSimulationControls = new GroupBox();
            lblTankLevelSimulationTime = new Label();
            lblTankLevelSimulationTimeLabel = new Label();
            btnTankLevelReset = new Button();
            btnTankLevelStop = new Button();
            btnTankLevelStart = new Button();
            groupBoxTankLevelControl = new GroupBox();
            chkTankLevelSetpointTracking = new CheckBox();
            numTankLevelSetpoint = new NumericUpDown();
            lblTankLevelSetpoint = new Label();
            numTankLevelManualOutput = new NumericUpDown();
            lblTankLevelManualOutput = new Label();
            btnTankLevelManual = new Button();
            btnTankLevelAuto = new Button();
            groupBoxTankLevelPidParameters = new GroupBox();
            numTankLevelKd = new NumericUpDown();
            lblTankLevelKd = new Label();
            numTankLevelKi = new NumericUpDown();
            lblTankLevelKi = new Label();
            numTankLevelKp = new NumericUpDown();
            lblTankLevelKp = new Label();
            groupBoxTankLevelProcessParameters = new GroupBox();
            numTankLevelDisturbance = new NumericUpDown();
            lblTankLevelDisturbance = new Label();
            numTankLevelMaxLevel = new NumericUpDown();
            lblTankLevelMaxLevel = new Label();
            numTankLevelOutflowRate = new NumericUpDown();
            lblTankLevelOutflowRate = new Label();
            numTankLevelCrossSectionArea = new NumericUpDown();
            lblTankLevelCrossSectionArea = new Label();
            groupBoxTankLevelProcessStatus = new GroupBox();
            lblTankLevelError = new Label();
            lblTankLevelErrorLabel = new Label();
            lblTankLevelControllerOutput = new Label();
            lblTankLevelControllerOutputLabel = new Label();
            lblTankLevelProcessVariable = new Label();
            lblTankLevelProcessVariableLabel = new Label();
            groupBoxTankLevelPlotSettings = new GroupBox();
            btnTankLevelUpdateTrend = new Button();
            txtTankLevelTrendDuration = new TextBox();
            lblTankLevelTrendDuration = new Label();
            groupBoxTankLevelLegend = new GroupBox();
            lblTankLevelErrorLegend = new Label();
            lblTankLevelControllerOutputLegend = new Label();
            lblTankLevelProcessVariableLegend = new Label();
            lblTankLevelSetpointLegend = new Label();
            toolTipTankLevel = new ToolTip(components);
            groupBoxTankLevelSimulationControls.SuspendLayout();
            groupBoxTankLevelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTankLevelSetpoint).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelManualOutput).BeginInit();
            groupBoxTankLevelPidParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTankLevelKd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelKi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelKp).BeginInit();
            groupBoxTankLevelProcessParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTankLevelDisturbance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelMaxLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelOutflowRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelCrossSectionArea).BeginInit();
            groupBoxTankLevelProcessStatus.SuspendLayout();
            groupBoxTankLevelPlotSettings.SuspendLayout();
            groupBoxTankLevelLegend.SuspendLayout();
            SuspendLayout();
            // 
            // plotTankLevelView
            // 
            plotTankLevelView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            plotTankLevelView.Location = new Point(37, 41);
            plotTankLevelView.Margin = new Padding(9, 11, 9, 11);
            plotTankLevelView.Name = "plotTankLevelView";
            plotTankLevelView.PanCursor = Cursors.Hand;
            plotTankLevelView.Size = new Size(1811, 1271);
            plotTankLevelView.TabIndex = 0;
            plotTankLevelView.Text = "plotTankLevelView";
            toolTipTankLevel.SetToolTip(plotTankLevelView, "Real-time plot showing tank level process with PID control response");
            plotTankLevelView.ZoomHorizontalCursor = Cursors.SizeWE;
            plotTankLevelView.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotTankLevelView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // groupBoxTankLevelSimulationControls
            // 
            groupBoxTankLevelSimulationControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxTankLevelSimulationControls.Controls.Add(lblTankLevelSimulationTime);
            groupBoxTankLevelSimulationControls.Controls.Add(lblTankLevelSimulationTimeLabel);
            groupBoxTankLevelSimulationControls.Controls.Add(btnTankLevelReset);
            groupBoxTankLevelSimulationControls.Controls.Add(btnTankLevelStop);
            groupBoxTankLevelSimulationControls.Controls.Add(btnTankLevelStart);
            groupBoxTankLevelSimulationControls.Location = new Point(1902, 41);
            groupBoxTankLevelSimulationControls.Margin = new Padding(9, 11, 9, 11);
            groupBoxTankLevelSimulationControls.Name = "groupBoxTankLevelSimulationControls";
            groupBoxTankLevelSimulationControls.Padding = new Padding(9, 11, 9, 11);
            groupBoxTankLevelSimulationControls.Size = new Size(706, 284);
            groupBoxTankLevelSimulationControls.TabIndex = 1;
            groupBoxTankLevelSimulationControls.TabStop = false;
            groupBoxTankLevelSimulationControls.Text = "Tank Level Simulation Controls";
            toolTipTankLevel.SetToolTip(groupBoxTankLevelSimulationControls, "Controls for tank level process simulation");
            // 
            // lblTankLevelSimulationTime
            // 
            lblTankLevelSimulationTime.AutoSize = true;
            lblTankLevelSimulationTime.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTankLevelSimulationTime.Location = new Point(241, 171);
            lblTankLevelSimulationTime.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelSimulationTime.Name = "lblTankLevelSimulationTime";
            lblTankLevelSimulationTime.Size = new Size(49, 32);
            lblTankLevelSimulationTime.TabIndex = 4;
            lblTankLevelSimulationTime.Text = "0.0";
            toolTipTankLevel.SetToolTip(lblTankLevelSimulationTime, "Current tank level simulation time");
            // 
            // lblTankLevelSimulationTimeLabel
            // 
            lblTankLevelSimulationTimeLabel.AutoSize = true;
            lblTankLevelSimulationTimeLabel.Location = new Point(45, 171);
            lblTankLevelSimulationTimeLabel.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelSimulationTimeLabel.Name = "lblTankLevelSimulationTimeLabel";
            lblTankLevelSimulationTimeLabel.Size = new Size(150, 32);
            lblTankLevelSimulationTimeLabel.TabIndex = 3;
            lblTankLevelSimulationTimeLabel.Text = "Sim Time (s):";
            toolTipTankLevel.SetToolTip(lblTankLevelSimulationTimeLabel, "Current tank level simulation time");
            // 
            // btnTankLevelReset
            // 
            btnTankLevelReset.BackColor = Color.LightBlue;
            btnTankLevelReset.FlatStyle = FlatStyle.Flat;
            btnTankLevelReset.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTankLevelReset.Location = new Point(453, 64);
            btnTankLevelReset.Margin = new Padding(9, 11, 9, 11);
            btnTankLevelReset.Name = "btnTankLevelReset";
            btnTankLevelReset.Size = new Size(204, 85);
            btnTankLevelReset.TabIndex = 2;
            btnTankLevelReset.Text = "RESET";
            toolTipTankLevel.SetToolTip(btnTankLevelReset, "Reset the tank level process simulation");
            btnTankLevelReset.UseVisualStyleBackColor = false;
            btnTankLevelReset.Click += BtnTankLevelReset_Click;
            // 
            // btnTankLevelStop
            // 
            btnTankLevelStop.BackColor = Color.LightCoral;
            btnTankLevelStop.Enabled = false;
            btnTankLevelStop.FlatStyle = FlatStyle.Flat;
            btnTankLevelStop.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTankLevelStop.Location = new Point(249, 64);
            btnTankLevelStop.Margin = new Padding(9, 11, 9, 11);
            btnTankLevelStop.Name = "btnTankLevelStop";
            btnTankLevelStop.Size = new Size(186, 85);
            btnTankLevelStop.TabIndex = 1;
            btnTankLevelStop.Text = "STOP";
            toolTipTankLevel.SetToolTip(btnTankLevelStop, "Stop the tank level process simulation");
            btnTankLevelStop.UseVisualStyleBackColor = false;
            btnTankLevelStop.Click += BtnTankLevelStop_Click;
            // 
            // btnTankLevelStart
            // 
            btnTankLevelStart.BackColor = Color.LightGreen;
            btnTankLevelStart.FlatStyle = FlatStyle.Flat;
            btnTankLevelStart.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTankLevelStart.Location = new Point(45, 64);
            btnTankLevelStart.Margin = new Padding(9, 11, 9, 11);
            btnTankLevelStart.Name = "btnTankLevelStart";
            btnTankLevelStart.Size = new Size(186, 85);
            btnTankLevelStart.TabIndex = 0;
            btnTankLevelStart.Text = "START";
            toolTipTankLevel.SetToolTip(btnTankLevelStart, "Start the tank level process simulation");
            btnTankLevelStart.UseVisualStyleBackColor = false;
            btnTankLevelStart.Click += BtnTankLevelStart_Click;
            // 
            // groupBoxTankLevelControl
            // 
            groupBoxTankLevelControl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxTankLevelControl.Controls.Add(chkTankLevelSetpointTracking);
            groupBoxTankLevelControl.Controls.Add(numTankLevelSetpoint);
            groupBoxTankLevelControl.Controls.Add(lblTankLevelSetpoint);
            groupBoxTankLevelControl.Controls.Add(numTankLevelManualOutput);
            groupBoxTankLevelControl.Controls.Add(lblTankLevelManualOutput);
            groupBoxTankLevelControl.Controls.Add(btnTankLevelManual);
            groupBoxTankLevelControl.Controls.Add(btnTankLevelAuto);
            groupBoxTankLevelControl.Location = new Point(1902, 340);
            groupBoxTankLevelControl.Margin = new Padding(9, 11, 9, 11);
            groupBoxTankLevelControl.Name = "groupBoxTankLevelControl";
            groupBoxTankLevelControl.Padding = new Padding(9, 11, 9, 11);
            groupBoxTankLevelControl.Size = new Size(706, 440);
            groupBoxTankLevelControl.TabIndex = 2;
            groupBoxTankLevelControl.TabStop = false;
            groupBoxTankLevelControl.Text = "Tank Level Control Mode";
            toolTipTankLevel.SetToolTip(groupBoxTankLevelControl, "Tank level control mode selection and settings");
            // 
            // chkTankLevelSetpointTracking
            // 
            chkTankLevelSetpointTracking.AutoSize = true;
            chkTankLevelSetpointTracking.Location = new Point(45, 375);
            chkTankLevelSetpointTracking.Margin = new Padding(9, 11, 9, 11);
            chkTankLevelSetpointTracking.Name = "chkTankLevelSetpointTracking";
            chkTankLevelSetpointTracking.Size = new Size(344, 36);
            chkTankLevelSetpointTracking.TabIndex = 6;
            chkTankLevelSetpointTracking.Text = "Enable Setpoint Tracking PV";
            toolTipTankLevel.SetToolTip(chkTankLevelSetpointTracking, "Tank level setpoint tracking in manual mode");
            chkTankLevelSetpointTracking.UseVisualStyleBackColor = true;
            chkTankLevelSetpointTracking.CheckedChanged += ChkTankLevelSetpointTracking_CheckedChanged;
            // 
            // numTankLevelSetpoint
            // 
            numTankLevelSetpoint.DecimalPlaces = 2;
            numTankLevelSetpoint.Location = new Point(370, 260);
            numTankLevelSetpoint.Margin = new Padding(9, 11, 9, 11);
            numTankLevelSetpoint.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numTankLevelSetpoint.Name = "numTankLevelSetpoint";
            numTankLevelSetpoint.Size = new Size(288, 39);
            numTankLevelSetpoint.TabIndex = 5;
            toolTipTankLevel.SetToolTip(numTankLevelSetpoint, "Target level for tank level process (meters)");
            numTankLevelSetpoint.Value = new decimal(new int[] { 5, 0, 0, 0 });
            numTankLevelSetpoint.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelSetpoint
            // 
            lblTankLevelSetpoint.AutoSize = true;
            lblTankLevelSetpoint.Location = new Point(370, 222);
            lblTankLevelSetpoint.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelSetpoint.Name = "lblTankLevelSetpoint";
            lblTankLevelSetpoint.Size = new Size(151, 32);
            lblTankLevelSetpoint.TabIndex = 4;
            lblTankLevelSetpoint.Text = "Setpoint (m):";
            toolTipTankLevel.SetToolTip(lblTankLevelSetpoint, "Tank level process setpoint");
            // 
            // numTankLevelManualOutput
            // 
            numTankLevelManualOutput.DecimalPlaces = 2;
            numTankLevelManualOutput.Enabled = false;
            numTankLevelManualOutput.Location = new Point(45, 260);
            numTankLevelManualOutput.Margin = new Padding(9, 11, 9, 11);
            numTankLevelManualOutput.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numTankLevelManualOutput.Name = "numTankLevelManualOutput";
            numTankLevelManualOutput.Size = new Size(306, 39);
            numTankLevelManualOutput.TabIndex = 3;
            toolTipTankLevel.SetToolTip(numTankLevelManualOutput, "Tank level manual output value (m³/min)");
            numTankLevelManualOutput.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelManualOutput
            // 
            lblTankLevelManualOutput.AutoSize = true;
            lblTankLevelManualOutput.Location = new Point(45, 222);
            lblTankLevelManualOutput.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelManualOutput.Name = "lblTankLevelManualOutput";
            lblTankLevelManualOutput.Size = new Size(283, 32);
            lblTankLevelManualOutput.TabIndex = 2;
            lblTankLevelManualOutput.Text = "Manual Output (m³/min):";
            toolTipTankLevel.SetToolTip(lblTankLevelManualOutput, "Tank level manual output value");
            // 
            // btnTankLevelManual
            // 
            btnTankLevelManual.BackColor = Color.LightGray;
            btnTankLevelManual.FlatStyle = FlatStyle.Flat;
            btnTankLevelManual.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTankLevelManual.Location = new Point(370, 85);
            btnTankLevelManual.Margin = new Padding(9, 11, 9, 11);
            btnTankLevelManual.Name = "btnTankLevelManual";
            btnTankLevelManual.Size = new Size(288, 102);
            btnTankLevelManual.TabIndex = 1;
            btnTankLevelManual.Text = "MANUAL";
            toolTipTankLevel.SetToolTip(btnTankLevelManual, "Tank level manual mode - direct output control");
            btnTankLevelManual.UseVisualStyleBackColor = false;
            btnTankLevelManual.Click += BtnTankLevelManual_Click;
            // 
            // btnTankLevelAuto
            // 
            btnTankLevelAuto.BackColor = Color.LightGreen;
            btnTankLevelAuto.FlatStyle = FlatStyle.Flat;
            btnTankLevelAuto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTankLevelAuto.Location = new Point(45, 85);
            btnTankLevelAuto.Margin = new Padding(9, 11, 9, 11);
            btnTankLevelAuto.Name = "btnTankLevelAuto";
            btnTankLevelAuto.Size = new Size(306, 102);
            btnTankLevelAuto.TabIndex = 0;
            btnTankLevelAuto.Text = "AUTOMATIC";
            toolTipTankLevel.SetToolTip(btnTankLevelAuto, "Tank level automatic mode - PID controller active");
            btnTankLevelAuto.UseVisualStyleBackColor = false;
            btnTankLevelAuto.Click += BtnTankLevelAuto_Click;
            // 
            // groupBoxTankLevelPidParameters
            // 
            groupBoxTankLevelPidParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxTankLevelPidParameters.Controls.Add(numTankLevelKd);
            groupBoxTankLevelPidParameters.Controls.Add(lblTankLevelKd);
            groupBoxTankLevelPidParameters.Controls.Add(numTankLevelKi);
            groupBoxTankLevelPidParameters.Controls.Add(lblTankLevelKi);
            groupBoxTankLevelPidParameters.Controls.Add(numTankLevelKp);
            groupBoxTankLevelPidParameters.Controls.Add(lblTankLevelKp);
            groupBoxTankLevelPidParameters.Location = new Point(1902, 800);
            groupBoxTankLevelPidParameters.Margin = new Padding(9, 11, 9, 11);
            groupBoxTankLevelPidParameters.Name = "groupBoxTankLevelPidParameters";
            groupBoxTankLevelPidParameters.Padding = new Padding(9, 11, 9, 11);
            groupBoxTankLevelPidParameters.Size = new Size(706, 330);
            groupBoxTankLevelPidParameters.TabIndex = 4;
            groupBoxTankLevelPidParameters.TabStop = false;
            groupBoxTankLevelPidParameters.Text = "Tank Level PID Parameters";
            toolTipTankLevel.SetToolTip(groupBoxTankLevelPidParameters, "Tank level PID controller tuning");
            // 
            // numTankLevelKd
            // 
            numTankLevelKd.DecimalPlaces = 3;
            numTankLevelKd.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numTankLevelKd.Location = new Point(241, 260);
            numTankLevelKd.Margin = new Padding(9, 11, 9, 11);
            numTankLevelKd.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numTankLevelKd.Name = "numTankLevelKd";
            numTankLevelKd.Size = new Size(416, 39);
            numTankLevelKd.TabIndex = 5;
            toolTipTankLevel.SetToolTip(numTankLevelKd, "Tank level derivative gain value");
            numTankLevelKd.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            numTankLevelKd.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelKd
            // 
            lblTankLevelKd.AutoSize = true;
            lblTankLevelKd.Location = new Point(45, 266);
            lblTankLevelKd.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelKd.Name = "lblTankLevelKd";
            lblTankLevelKd.Size = new Size(78, 32);
            lblTankLevelKd.TabIndex = 4;
            lblTankLevelKd.Text = "Kd (s):";
            toolTipTankLevel.SetToolTip(lblTankLevelKd, "Tank level derivative gain");
            // 
            // numTankLevelKi
            // 
            numTankLevelKi.DecimalPlaces = 3;
            numTankLevelKi.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numTankLevelKi.Location = new Point(241, 174);
            numTankLevelKi.Margin = new Padding(9, 11, 9, 11);
            numTankLevelKi.Name = "numTankLevelKi";
            numTankLevelKi.Size = new Size(416, 39);
            numTankLevelKi.TabIndex = 3;
            toolTipTankLevel.SetToolTip(numTankLevelKi, "Tank level integral gain value");
            numTankLevelKi.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            numTankLevelKi.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelKi
            // 
            lblTankLevelKi.AutoSize = true;
            lblTankLevelKi.Location = new Point(45, 180);
            lblTankLevelKi.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelKi.Name = "lblTankLevelKi";
            lblTankLevelKi.Size = new Size(92, 32);
            lblTankLevelKi.TabIndex = 2;
            lblTankLevelKi.Text = "Ki (1/s):";
            toolTipTankLevel.SetToolTip(lblTankLevelKi, "Tank level integral gain");
            // 
            // numTankLevelKp
            // 
            numTankLevelKp.DecimalPlaces = 3;
            numTankLevelKp.Location = new Point(241, 88);
            numTankLevelKp.Margin = new Padding(9, 11, 9, 11);
            numTankLevelKp.Name = "numTankLevelKp";
            numTankLevelKp.Size = new Size(416, 39);
            numTankLevelKp.TabIndex = 1;
            toolTipTankLevel.SetToolTip(numTankLevelKp, "Tank level proportional gain value");
            numTankLevelKp.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numTankLevelKp.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelKp
            // 
            lblTankLevelKp.AutoSize = true;
            lblTankLevelKp.Location = new Point(45, 94);
            lblTankLevelKp.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelKp.Name = "lblTankLevelKp";
            lblTankLevelKp.Size = new Size(47, 32);
            lblTankLevelKp.TabIndex = 0;
            lblTankLevelKp.Text = "Kp:";
            toolTipTankLevel.SetToolTip(lblTankLevelKp, "Tank level proportional gain");
            // 
            // groupBoxTankLevelProcessParameters
            // 
            groupBoxTankLevelProcessParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxTankLevelProcessParameters.Controls.Add(numTankLevelDisturbance);
            groupBoxTankLevelProcessParameters.Controls.Add(lblTankLevelDisturbance);
            groupBoxTankLevelProcessParameters.Controls.Add(numTankLevelMaxLevel);
            groupBoxTankLevelProcessParameters.Controls.Add(lblTankLevelMaxLevel);
            groupBoxTankLevelProcessParameters.Controls.Add(numTankLevelOutflowRate);
            groupBoxTankLevelProcessParameters.Controls.Add(lblTankLevelOutflowRate);
            groupBoxTankLevelProcessParameters.Controls.Add(numTankLevelCrossSectionArea);
            groupBoxTankLevelProcessParameters.Controls.Add(lblTankLevelCrossSectionArea);
            groupBoxTankLevelProcessParameters.Location = new Point(1902, 1150);
            groupBoxTankLevelProcessParameters.Margin = new Padding(9, 11, 9, 11);
            groupBoxTankLevelProcessParameters.Name = "groupBoxTankLevelProcessParameters";
            groupBoxTankLevelProcessParameters.Padding = new Padding(9, 11, 9, 11);
            groupBoxTankLevelProcessParameters.Size = new Size(706, 450);
            groupBoxTankLevelProcessParameters.TabIndex = 5;
            groupBoxTankLevelProcessParameters.TabStop = false;
            groupBoxTankLevelProcessParameters.Text = "Tank Level Process Parameters";
            toolTipTankLevel.SetToolTip(groupBoxTankLevelProcessParameters, "Tank level process model parameters");
            // 
            // numTankLevelDisturbance
            // 
            numTankLevelDisturbance.DecimalPlaces = 2;
            numTankLevelDisturbance.Location = new Point(371, 363);
            numTankLevelDisturbance.Margin = new Padding(9, 11, 9, 11);
            numTankLevelDisturbance.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numTankLevelDisturbance.Minimum = new decimal(new int[] { 10, 0, 0, int.MinValue });
            numTankLevelDisturbance.Name = "numTankLevelDisturbance";
            numTankLevelDisturbance.Size = new Size(286, 39);
            numTankLevelDisturbance.TabIndex = 7;
            toolTipTankLevel.SetToolTip(numTankLevelDisturbance, "Tank level disturbance value");
            numTankLevelDisturbance.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelDisturbance
            // 
            lblTankLevelDisturbance.AutoSize = true;
            lblTankLevelDisturbance.Location = new Point(45, 370);
            lblTankLevelDisturbance.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelDisturbance.Name = "lblTankLevelDisturbance";
            lblTankLevelDisturbance.Size = new Size(247, 32);
            lblTankLevelDisturbance.TabIndex = 6;
            lblTankLevelDisturbance.Text = "Disturbance (m³/min):";
            toolTipTankLevel.SetToolTip(lblTankLevelDisturbance, "Tank level external disturbance");
            // 
            // numTankLevelMaxLevel
            // 
            numTankLevelMaxLevel.DecimalPlaces = 1;
            numTankLevelMaxLevel.Location = new Point(371, 277);
            numTankLevelMaxLevel.Margin = new Padding(9, 11, 9, 11);
            numTankLevelMaxLevel.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numTankLevelMaxLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTankLevelMaxLevel.Name = "numTankLevelMaxLevel";
            numTankLevelMaxLevel.Size = new Size(286, 39);
            numTankLevelMaxLevel.TabIndex = 5;
            toolTipTankLevel.SetToolTip(numTankLevelMaxLevel, "Maximum tank level");
            numTankLevelMaxLevel.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numTankLevelMaxLevel.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelMaxLevel
            // 
            lblTankLevelMaxLevel.AutoSize = true;
            lblTankLevelMaxLevel.Location = new Point(45, 284);
            lblTankLevelMaxLevel.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelMaxLevel.Name = "lblTankLevelMaxLevel";
            lblTankLevelMaxLevel.Size = new Size(168, 32);
            lblTankLevelMaxLevel.TabIndex = 4;
            lblTankLevelMaxLevel.Text = "Max Level (m):";
            toolTipTankLevel.SetToolTip(lblTankLevelMaxLevel, "Tank level maximum level");
            // 
            // numTankLevelOutflowRate
            // 
            numTankLevelOutflowRate.DecimalPlaces = 2;
            numTankLevelOutflowRate.Location = new Point(371, 191);
            numTankLevelOutflowRate.Margin = new Padding(9, 11, 9, 11);
            numTankLevelOutflowRate.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numTankLevelOutflowRate.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numTankLevelOutflowRate.Name = "numTankLevelOutflowRate";
            numTankLevelOutflowRate.Size = new Size(286, 39);
            numTankLevelOutflowRate.TabIndex = 3;
            toolTipTankLevel.SetToolTip(numTankLevelOutflowRate, "Tank outflow rate");
            numTankLevelOutflowRate.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numTankLevelOutflowRate.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelOutflowRate
            // 
            lblTankLevelOutflowRate.AutoSize = true;
            lblTankLevelOutflowRate.Location = new Point(45, 198);
            lblTankLevelOutflowRate.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelOutflowRate.Name = "lblTankLevelOutflowRate";
            lblTankLevelOutflowRate.Size = new Size(259, 32);
            lblTankLevelOutflowRate.TabIndex = 2;
            lblTankLevelOutflowRate.Text = "Outflow Rate (m³/min):";
            toolTipTankLevel.SetToolTip(lblTankLevelOutflowRate, "Tank level outflow rate");
            // 
            // numTankLevelCrossSectionArea
            // 
            numTankLevelCrossSectionArea.DecimalPlaces = 1;
            numTankLevelCrossSectionArea.Location = new Point(371, 105);
            numTankLevelCrossSectionArea.Margin = new Padding(9, 11, 9, 11);
            numTankLevelCrossSectionArea.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTankLevelCrossSectionArea.Name = "numTankLevelCrossSectionArea";
            numTankLevelCrossSectionArea.Size = new Size(286, 39);
            numTankLevelCrossSectionArea.TabIndex = 1;
            toolTipTankLevel.SetToolTip(numTankLevelCrossSectionArea, "Tank cross-sectional area");
            numTankLevelCrossSectionArea.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numTankLevelCrossSectionArea.ValueChanged += TankLevelParameterValue_Changed;
            // 
            // lblTankLevelCrossSectionArea
            // 
            lblTankLevelCrossSectionArea.AutoSize = true;
            lblTankLevelCrossSectionArea.Location = new Point(45, 112);
            lblTankLevelCrossSectionArea.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelCrossSectionArea.Name = "lblTankLevelCrossSectionArea";
            lblTankLevelCrossSectionArea.Size = new Size(173, 32);
            lblTankLevelCrossSectionArea.TabIndex = 0;
            lblTankLevelCrossSectionArea.Text = "Tank Area (m²):";
            toolTipTankLevel.SetToolTip(lblTankLevelCrossSectionArea, "Tank level cross-sectional area");
            // 
            // groupBoxTankLevelProcessStatus
            // 
            groupBoxTankLevelProcessStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            groupBoxTankLevelProcessStatus.Controls.Add(lblTankLevelError);
            groupBoxTankLevelProcessStatus.Controls.Add(lblTankLevelErrorLabel);
            groupBoxTankLevelProcessStatus.Controls.Add(lblTankLevelControllerOutput);
            groupBoxTankLevelProcessStatus.Controls.Add(lblTankLevelControllerOutputLabel);
            groupBoxTankLevelProcessStatus.Controls.Add(lblTankLevelProcessVariable);
            groupBoxTankLevelProcessStatus.Controls.Add(lblTankLevelProcessVariableLabel);
            groupBoxTankLevelProcessStatus.Location = new Point(1902, 1620);
            groupBoxTankLevelProcessStatus.Margin = new Padding(9, 11, 9, 11);
            groupBoxTankLevelProcessStatus.Name = "groupBoxTankLevelProcessStatus";
            groupBoxTankLevelProcessStatus.Padding = new Padding(9, 11, 9, 11);
            groupBoxTankLevelProcessStatus.Size = new Size(706, 277);
            groupBoxTankLevelProcessStatus.TabIndex = 3;
            groupBoxTankLevelProcessStatus.TabStop = false;
            groupBoxTankLevelProcessStatus.Text = "Tank Level Process Status";
            toolTipTankLevel.SetToolTip(groupBoxTankLevelProcessStatus, "Current tank level process values");
            // 
            // lblTankLevelError
            // 
            lblTankLevelError.AutoSize = true;
            lblTankLevelError.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTankLevelError.ForeColor = Color.Orange;
            lblTankLevelError.Location = new Point(409, 190);
            lblTankLevelError.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelError.Name = "lblTankLevelError";
            lblTankLevelError.Size = new Size(63, 32);
            lblTankLevelError.TabIndex = 5;
            lblTankLevelError.Text = "0.00";
            toolTipTankLevel.SetToolTip(lblTankLevelError, "Current tank level error (SP - PV)");
            // 
            // lblTankLevelErrorLabel
            // 
            lblTankLevelErrorLabel.AutoSize = true;
            lblTankLevelErrorLabel.Location = new Point(409, 120);
            lblTankLevelErrorLabel.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelErrorLabel.Name = "lblTankLevelErrorLabel";
            lblTankLevelErrorLabel.Size = new Size(111, 32);
            lblTankLevelErrorLabel.TabIndex = 4;
            lblTankLevelErrorLabel.Text = "Error (m):";
            toolTipTankLevel.SetToolTip(lblTankLevelErrorLabel, "Tank level control error");
            // 
            // lblTankLevelControllerOutput
            // 
            lblTankLevelControllerOutput.AutoSize = true;
            lblTankLevelControllerOutput.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTankLevelControllerOutput.ForeColor = Color.Red;
            lblTankLevelControllerOutput.Location = new Point(204, 190);
            lblTankLevelControllerOutput.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelControllerOutput.Name = "lblTankLevelControllerOutput";
            lblTankLevelControllerOutput.Size = new Size(63, 32);
            lblTankLevelControllerOutput.TabIndex = 3;
            lblTankLevelControllerOutput.Text = "0.00";
            toolTipTankLevel.SetToolTip(lblTankLevelControllerOutput, "Current tank level MV value");
            // 
            // lblTankLevelControllerOutputLabel
            // 
            lblTankLevelControllerOutputLabel.AutoSize = true;
            lblTankLevelControllerOutputLabel.Location = new Point(45, 190);
            lblTankLevelControllerOutputLabel.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelControllerOutputLabel.Name = "lblTankLevelControllerOutputLabel";
            lblTankLevelControllerOutputLabel.Size = new Size(157, 32);
            lblTankLevelControllerOutputLabel.TabIndex = 2;
            lblTankLevelControllerOutputLabel.Text = "MV (m³/min):";
            toolTipTankLevel.SetToolTip(lblTankLevelControllerOutputLabel, "Tank level manipulated variable");
            // 
            // lblTankLevelProcessVariable
            // 
            lblTankLevelProcessVariable.AutoSize = true;
            lblTankLevelProcessVariable.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTankLevelProcessVariable.ForeColor = Color.Green;
            lblTankLevelProcessVariable.Location = new Point(204, 120);
            lblTankLevelProcessVariable.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelProcessVariable.Name = "lblTankLevelProcessVariable";
            lblTankLevelProcessVariable.Size = new Size(63, 32);
            lblTankLevelProcessVariable.TabIndex = 1;
            lblTankLevelProcessVariable.Text = "0.00";
            toolTipTankLevel.SetToolTip(lblTankLevelProcessVariable, "Current tank level PV value");
            // 
            // lblTankLevelProcessVariableLabel
            // 
            lblTankLevelProcessVariableLabel.AutoSize = true;
            lblTankLevelProcessVariableLabel.Location = new Point(45, 120);
            lblTankLevelProcessVariableLabel.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelProcessVariableLabel.Name = "lblTankLevelProcessVariableLabel";
            lblTankLevelProcessVariableLabel.Size = new Size(89, 32);
            lblTankLevelProcessVariableLabel.TabIndex = 0;
            lblTankLevelProcessVariableLabel.Text = "PV (m):";
            toolTipTankLevel.SetToolTip(lblTankLevelProcessVariableLabel, "Tank level process variable");
            // 
            // groupBoxTankLevelPlotSettings
            // 
            groupBoxTankLevelPlotSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupBoxTankLevelPlotSettings.Controls.Add(btnTankLevelUpdateTrend);
            groupBoxTankLevelPlotSettings.Controls.Add(txtTankLevelTrendDuration);
            groupBoxTankLevelPlotSettings.Controls.Add(lblTankLevelTrendDuration);
            groupBoxTankLevelPlotSettings.Location = new Point(108, 1334);
            groupBoxTankLevelPlotSettings.Margin = new Padding(9, 11, 9, 11);
            groupBoxTankLevelPlotSettings.Name = "groupBoxTankLevelPlotSettings";
            groupBoxTankLevelPlotSettings.Padding = new Padding(9, 11, 9, 11);
            groupBoxTankLevelPlotSettings.Size = new Size(604, 273);
            groupBoxTankLevelPlotSettings.TabIndex = 6;
            groupBoxTankLevelPlotSettings.TabStop = false;
            groupBoxTankLevelPlotSettings.Text = "Tank Level Plot Settings";
            toolTipTankLevel.SetToolTip(groupBoxTankLevelPlotSettings, "Configure tank level plot time scale");
            // 
            // btnTankLevelUpdateTrend
            // 
            btnTankLevelUpdateTrend.Location = new Point(318, 160);
            btnTankLevelUpdateTrend.Margin = new Padding(9, 11, 9, 11);
            btnTankLevelUpdateTrend.Name = "btnTankLevelUpdateTrend";
            btnTankLevelUpdateTrend.Size = new Size(227, 96);
            btnTankLevelUpdateTrend.TabIndex = 2;
            btnTankLevelUpdateTrend.Text = "Update";
            toolTipTankLevel.SetToolTip(btnTankLevelUpdateTrend, "Apply tank level trend duration");
            btnTankLevelUpdateTrend.UseVisualStyleBackColor = true;
            // 
            // txtTankLevelTrendDuration
            // 
            txtTankLevelTrendDuration.Location = new Point(45, 164);
            txtTankLevelTrendDuration.Margin = new Padding(9, 11, 9, 11);
            txtTankLevelTrendDuration.Name = "txtTankLevelTrendDuration";
            txtTankLevelTrendDuration.Size = new Size(234, 39);
            txtTankLevelTrendDuration.TabIndex = 1;
            txtTankLevelTrendDuration.Text = "300";
            toolTipTankLevel.SetToolTip(txtTankLevelTrendDuration, "Tank level time window in seconds");
            // 
            // lblTankLevelTrendDuration
            // 
            lblTankLevelTrendDuration.AutoSize = true;
            lblTankLevelTrendDuration.Location = new Point(45, 85);
            lblTankLevelTrendDuration.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelTrendDuration.Name = "lblTankLevelTrendDuration";
            lblTankLevelTrendDuration.Size = new Size(210, 32);
            lblTankLevelTrendDuration.TabIndex = 0;
            lblTankLevelTrendDuration.Text = "Trend Duration (s):";
            toolTipTankLevel.SetToolTip(lblTankLevelTrendDuration, "Tank level plot time window");
            // 
            // groupBoxTankLevelLegend
            // 
            groupBoxTankLevelLegend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupBoxTankLevelLegend.Controls.Add(lblTankLevelErrorLegend);
            groupBoxTankLevelLegend.Controls.Add(lblTankLevelControllerOutputLegend);
            groupBoxTankLevelLegend.Controls.Add(lblTankLevelProcessVariableLegend);
            groupBoxTankLevelLegend.Controls.Add(lblTankLevelSetpointLegend);
            groupBoxTankLevelLegend.Location = new Point(850, 1334);
            groupBoxTankLevelLegend.Margin = new Padding(9, 11, 9, 11);
            groupBoxTankLevelLegend.Name = "groupBoxTankLevelLegend";
            groupBoxTankLevelLegend.Padding = new Padding(9, 11, 9, 11);
            groupBoxTankLevelLegend.Size = new Size(929, 353);
            groupBoxTankLevelLegend.TabIndex = 7;
            groupBoxTankLevelLegend.TabStop = false;
            groupBoxTankLevelLegend.Text = "Tank Level Plot Legend";
            toolTipTankLevel.SetToolTip(groupBoxTankLevelLegend, "Tank level plot legend");
            // 
            // lblTankLevelErrorLegend
            // 
            lblTankLevelErrorLegend.AutoSize = true;
            lblTankLevelErrorLegend.ForeColor = Color.Orange;
            lblTankLevelErrorLegend.Location = new Point(45, 290);
            lblTankLevelErrorLegend.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelErrorLegend.Name = "lblTankLevelErrorLegend";
            lblTankLevelErrorLegend.Size = new Size(271, 32);
            lblTankLevelErrorLegend.TabIndex = 3;
            lblTankLevelErrorLegend.Text = "� � � � � Level Error";
            toolTipTankLevel.SetToolTip(lblTankLevelErrorLegend, "Tank level control error line");
            // 
            // lblTankLevelControllerOutputLegend
            // 
            lblTankLevelControllerOutputLegend.AutoSize = true;
            lblTankLevelControllerOutputLegend.ForeColor = Color.Red;
            lblTankLevelControllerOutputLegend.Location = new Point(45, 222);
            lblTankLevelControllerOutputLegend.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelControllerOutputLegend.Name = "lblTankLevelControllerOutputLegend";
            lblTankLevelControllerOutputLegend.Size = new Size(329, 32);
            lblTankLevelControllerOutputLegend.TabIndex = 2;
            lblTankLevelControllerOutputLegend.Text = "���� Inflow Rate (m³/min)";
            toolTipTankLevel.SetToolTip(lblTankLevelControllerOutputLegend, "Tank level inflow rate line");
            // 
            // lblTankLevelProcessVariableLegend
            // 
            lblTankLevelProcessVariableLegend.AutoSize = true;
            lblTankLevelProcessVariableLegend.ForeColor = Color.Green;
            lblTankLevelProcessVariableLegend.Location = new Point(45, 154);
            lblTankLevelProcessVariableLegend.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelProcessVariableLegend.Name = "lblTankLevelProcessVariableLegend";
            lblTankLevelProcessVariableLegend.Size = new Size(261, 32);
            lblTankLevelProcessVariableLegend.TabIndex = 1;
            lblTankLevelProcessVariableLegend.Text = "���� Tank Level (m)";
            toolTipTankLevel.SetToolTip(lblTankLevelProcessVariableLegend, "Tank level process variable line");
            // 
            // lblTankLevelSetpointLegend
            // 
            lblTankLevelSetpointLegend.AutoSize = true;
            lblTankLevelSetpointLegend.ForeColor = Color.Blue;
            lblTankLevelSetpointLegend.Location = new Point(45, 85);
            lblTankLevelSetpointLegend.Margin = new Padding(9, 0, 9, 0);
            lblTankLevelSetpointLegend.Name = "lblTankLevelSetpointLegend";
            lblTankLevelSetpointLegend.Size = new Size(266, 32);
            lblTankLevelSetpointLegend.TabIndex = 0;
            lblTankLevelSetpointLegend.Text = "� � Level Setpoint (m)";
            toolTipTankLevel.SetToolTip(lblTankLevelSetpointLegend, "Tank level setpoint line");
            // 
            // TankLevelWithPidForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2671, 1707);
            Controls.Add(groupBoxTankLevelLegend);
            Controls.Add(groupBoxTankLevelPlotSettings);
            Controls.Add(groupBoxTankLevelProcessParameters);
            Controls.Add(groupBoxTankLevelPidParameters);
            Controls.Add(groupBoxTankLevelProcessStatus);
            Controls.Add(groupBoxTankLevelControl);
            Controls.Add(groupBoxTankLevelSimulationControls);
            Controls.Add(plotTankLevelView);
            Margin = new Padding(6);
            MinimumSize = new Size(2206, 1200);
            Name = "TankLevelWithPidForm";
            Text = "Tank Level Process with PID Control";
            toolTipTankLevel.SetToolTip(this, "Tank Level Process with PID Control Simulator");
            groupBoxTankLevelSimulationControls.ResumeLayout(false);
            groupBoxTankLevelSimulationControls.PerformLayout();
            groupBoxTankLevelControl.ResumeLayout(false);
            groupBoxTankLevelControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTankLevelSetpoint).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelManualOutput).EndInit();
            groupBoxTankLevelPidParameters.ResumeLayout(false);
            groupBoxTankLevelPidParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTankLevelKd).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelKi).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelKp).EndInit();
            groupBoxTankLevelProcessParameters.ResumeLayout(false);
            groupBoxTankLevelProcessParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTankLevelDisturbance).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelMaxLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelOutflowRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTankLevelCrossSectionArea).EndInit();
            groupBoxTankLevelProcessStatus.ResumeLayout(false);
            groupBoxTankLevelProcessStatus.PerformLayout();
            groupBoxTankLevelPlotSettings.ResumeLayout(false);
            groupBoxTankLevelPlotSettings.PerformLayout();
            groupBoxTankLevelLegend.ResumeLayout(false);
            groupBoxTankLevelLegend.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}