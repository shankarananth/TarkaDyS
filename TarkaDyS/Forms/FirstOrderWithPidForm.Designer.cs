using OxyPlot.WindowsForms;

namespace TarkaDyS.Forms
{
    partial class FirstOrderWithPidForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // First-Order specific controls
        private PlotView plotFirstOrderView;
        private GroupBox groupBoxFirstOrderSimulationControls;
        private GroupBox groupBoxFirstOrderControl;
        private GroupBox groupBoxFirstOrderPidParameters;
        private GroupBox groupBoxFirstOrderProcessParameters;
        private GroupBox groupBoxFirstOrderProcessStatus;
        private GroupBox groupBoxFirstOrderPlotSettings;
        private GroupBox groupBoxFirstOrderLegend;
        
        // First-Order setpoint controls
        private Label lblFirstOrderSetpoint;
        private NumericUpDown numFirstOrderSetpoint;
        
        // First-Order PID parameter controls
        private Label lblFirstOrderKp;
        private NumericUpDown numFirstOrderKp;
        private Label lblFirstOrderKi;
        private NumericUpDown numFirstOrderKi;
        private Label lblFirstOrderKd;
        private NumericUpDown numFirstOrderKd;
        
        // First-Order process parameter controls
        private Label lblFirstOrderProcessGain;
        private NumericUpDown numFirstOrderProcessGain;
        private Label lblFirstOrderTimeConstant;
        private NumericUpDown numFirstOrderTimeConstant;
        private Label lblFirstOrderDeadTime;
        private NumericUpDown numFirstOrderDeadTime;
        private Label lblFirstOrderDisturbance;
        private NumericUpDown numFirstOrderDisturbance;
        
        // First-Order control mode controls
        private Button btnFirstOrderAuto;
        private Button btnFirstOrderManual;
        private Label lblFirstOrderManualOutput;
        private NumericUpDown numFirstOrderManualOutput;
        private CheckBox chkFirstOrderSetpointTracking;
        
        // First-Order simulation controls
        private Button btnFirstOrderStart;
        private Button btnFirstOrderStop;
        private Button btnFirstOrderReset;
        
        // First-Order status displays
        private Label lblFirstOrderSimulationTimeLabel;
        private Label lblFirstOrderSimulationTime;
        private Label lblFirstOrderProcessVariableLabel;
        private Label lblFirstOrderProcessVariable;
        private Label lblFirstOrderControllerOutputLabel;
        private Label lblFirstOrderControllerOutput;
        private Label lblFirstOrderErrorLabel;
        private Label lblFirstOrderError;
        
        // First-Order plot settings
        private Label lblFirstOrderTrendDuration;
        private TextBox txtFirstOrderTrendDuration;
        private Button btnFirstOrderUpdateTrend;
        
        // First-Order legend
        private Label lblFirstOrderSetpointLegend;
        private Label lblFirstOrderProcessVariableLegend;
        private Label lblFirstOrderControllerOutputLegend;
        private Label lblFirstOrderErrorLegend;
        
        private ToolTip toolTipFirstOrder;
        
        // Simulation speed controls
        private TrackBar trkSimulationSpeed;
        private Label lblSimulationSpeedLabel;
        private Label lblSimulationSpeedValue;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            plotFirstOrderView = new PlotView();
            groupBoxFirstOrderSimulationControls = new GroupBox();
            lblSimulationSpeedValue = new Label();
            lblSimulationSpeedLabel = new Label();
            trkSimulationSpeed = new TrackBar();
            lblFirstOrderSimulationTime = new Label();
            lblFirstOrderSimulationTimeLabel = new Label();
            btnFirstOrderReset = new Button();
            btnFirstOrderStop = new Button();
            btnFirstOrderStart = new Button();
            groupBoxFirstOrderControl = new GroupBox();
            chkFirstOrderSetpointTracking = new CheckBox();
            numFirstOrderSetpoint = new NumericUpDown();
            lblFirstOrderSetpoint = new Label();
            numFirstOrderManualOutput = new NumericUpDown();
            lblFirstOrderManualOutput = new Label();
            btnFirstOrderManual = new Button();
            btnFirstOrderAuto = new Button();
            groupBoxFirstOrderPidParameters = new GroupBox();
            numFirstOrderKd = new NumericUpDown();
            lblFirstOrderKd = new Label();
            numFirstOrderKi = new NumericUpDown();
            lblFirstOrderKi = new Label();
            numFirstOrderKp = new NumericUpDown();
            lblFirstOrderKp = new Label();
            groupBoxFirstOrderProcessParameters = new GroupBox();
            numFirstOrderDisturbance = new NumericUpDown();
            lblFirstOrderDisturbance = new Label();
            numFirstOrderDeadTime = new NumericUpDown();
            lblFirstOrderDeadTime = new Label();
            numFirstOrderTimeConstant = new NumericUpDown();
            lblFirstOrderTimeConstant = new Label();
            numFirstOrderProcessGain = new NumericUpDown();
            lblFirstOrderProcessGain = new Label();
            groupBoxFirstOrderProcessStatus = new GroupBox();
            lblFirstOrderError = new Label();
            lblFirstOrderErrorLabel = new Label();
            lblFirstOrderControllerOutput = new Label();
            lblFirstOrderControllerOutputLabel = new Label();
            lblFirstOrderProcessVariable = new Label();
            lblFirstOrderProcessVariableLabel = new Label();
            groupBoxFirstOrderPlotSettings = new GroupBox();
            btnFirstOrderUpdateTrend = new Button();
            txtFirstOrderTrendDuration = new TextBox();
            lblFirstOrderTrendDuration = new Label();
            groupBoxFirstOrderLegend = new GroupBox();
            lblFirstOrderErrorLegend = new Label();
            lblFirstOrderControllerOutputLegend = new Label();
            lblFirstOrderProcessVariableLegend = new Label();
            lblFirstOrderSetpointLegend = new Label();
            toolTipFirstOrder = new ToolTip(components);
            groupBoxFirstOrderSimulationControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkSimulationSpeed).BeginInit();
            groupBoxFirstOrderControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderSetpoint).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderManualOutput).BeginInit();
            groupBoxFirstOrderPidParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderKd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderKi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderKp).BeginInit();
            groupBoxFirstOrderProcessParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderDisturbance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderDeadTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderTimeConstant).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderProcessGain).BeginInit();
            groupBoxFirstOrderProcessStatus.SuspendLayout();
            groupBoxFirstOrderPlotSettings.SuspendLayout();
            groupBoxFirstOrderLegend.SuspendLayout();
            SuspendLayout();
            // 
            // plotFirstOrderView
            // 
            plotFirstOrderView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            plotFirstOrderView.Location = new Point(37, 41);
            plotFirstOrderView.Margin = new Padding(9, 11, 9, 11);
            plotFirstOrderView.Name = "plotFirstOrderView";
            plotFirstOrderView.PanCursor = Cursors.Hand;
            plotFirstOrderView.Size = new Size(1811, 1271);
            plotFirstOrderView.TabIndex = 0;
            plotFirstOrderView.Text = "plotFirstOrderView";
            toolTipFirstOrder.SetToolTip(plotFirstOrderView, "Real-time plot showing first-order process with PID control response");
            plotFirstOrderView.ZoomHorizontalCursor = Cursors.SizeWE;
            plotFirstOrderView.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotFirstOrderView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // groupBoxFirstOrderSimulationControls
            // 
            groupBoxFirstOrderSimulationControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxFirstOrderSimulationControls.Controls.Add(lblSimulationSpeedValue);
            groupBoxFirstOrderSimulationControls.Controls.Add(lblSimulationSpeedLabel);
            groupBoxFirstOrderSimulationControls.Controls.Add(trkSimulationSpeed);
            groupBoxFirstOrderSimulationControls.Controls.Add(lblFirstOrderSimulationTime);
            groupBoxFirstOrderSimulationControls.Controls.Add(lblFirstOrderSimulationTimeLabel);
            groupBoxFirstOrderSimulationControls.Controls.Add(btnFirstOrderReset);
            groupBoxFirstOrderSimulationControls.Controls.Add(btnFirstOrderStop);
            groupBoxFirstOrderSimulationControls.Controls.Add(btnFirstOrderStart);
            groupBoxFirstOrderSimulationControls.Location = new Point(1902, 41);
            groupBoxFirstOrderSimulationControls.Margin = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderSimulationControls.Name = "groupBoxFirstOrderSimulationControls";
            groupBoxFirstOrderSimulationControls.Padding = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderSimulationControls.Size = new Size(706, 384);
            groupBoxFirstOrderSimulationControls.TabIndex = 1;
            groupBoxFirstOrderSimulationControls.TabStop = false;
            groupBoxFirstOrderSimulationControls.Text = "First-Order Simulation Controls";
            toolTipFirstOrder.SetToolTip(groupBoxFirstOrderSimulationControls, "Controls for first-order process simulation");
            // 
            // lblSimulationSpeedValue
            // 
            lblSimulationSpeedValue.AutoSize = true;
            lblSimulationSpeedValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSimulationSpeedValue.ForeColor = Color.Green;
            lblSimulationSpeedValue.Location = new Point(241, 224);
            lblSimulationSpeedValue.Margin = new Padding(6, 0, 6, 0);
            lblSimulationSpeedValue.Name = "lblSimulationSpeedValue";
            lblSimulationSpeedValue.Size = new Size(62, 32);
            lblSimulationSpeedValue.TabIndex = 6;
            lblSimulationSpeedValue.Text = "1.0x";
            // 
            // lblSimulationSpeedLabel
            // 
            lblSimulationSpeedLabel.AutoSize = true;
            lblSimulationSpeedLabel.Location = new Point(45, 224);
            lblSimulationSpeedLabel.Margin = new Padding(6, 0, 6, 0);
            lblSimulationSpeedLabel.Name = "lblSimulationSpeedLabel";
            lblSimulationSpeedLabel.Size = new Size(133, 32);
            lblSimulationSpeedLabel.TabIndex = 5;
            lblSimulationSpeedLabel.Text = "Sim Speed:";
            // 
            // trkSimulationSpeed
            // 
            trkSimulationSpeed.LargeChange = 10;
            trkSimulationSpeed.Location = new Point(45, 277);
            trkSimulationSpeed.Margin = new Padding(6);
            trkSimulationSpeed.Maximum = 100;
            trkSimulationSpeed.Minimum = 1;
            trkSimulationSpeed.Name = "trkSimulationSpeed";
            trkSimulationSpeed.Size = new Size(613, 90);
            trkSimulationSpeed.SmallChange = 5;
            trkSimulationSpeed.TabIndex = 7;
            trkSimulationSpeed.TickFrequency = 10;
            toolTipFirstOrder.SetToolTip(trkSimulationSpeed, "Simulation speed: 1x = 1 second timestep (real-time), 10x = 0.1 second timestep (fastest)");
            trkSimulationSpeed.Value = 10;
            trkSimulationSpeed.ValueChanged += TrkSimulationSpeed_ValueChanged;
            // 
            // lblFirstOrderSimulationTime
            // 
            lblFirstOrderSimulationTime.AutoSize = true;
            lblFirstOrderSimulationTime.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFirstOrderSimulationTime.Location = new Point(241, 171);
            lblFirstOrderSimulationTime.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderSimulationTime.Name = "lblFirstOrderSimulationTime";
            lblFirstOrderSimulationTime.Size = new Size(49, 32);
            lblFirstOrderSimulationTime.TabIndex = 4;
            lblFirstOrderSimulationTime.Text = "0.0";
            toolTipFirstOrder.SetToolTip(lblFirstOrderSimulationTime, "Current first-order simulation time");
            // 
            // lblFirstOrderSimulationTimeLabel
            // 
            lblFirstOrderSimulationTimeLabel.AutoSize = true;
            lblFirstOrderSimulationTimeLabel.Location = new Point(45, 171);
            lblFirstOrderSimulationTimeLabel.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderSimulationTimeLabel.Name = "lblFirstOrderSimulationTimeLabel";
            lblFirstOrderSimulationTimeLabel.Size = new Size(150, 32);
            lblFirstOrderSimulationTimeLabel.TabIndex = 3;
            lblFirstOrderSimulationTimeLabel.Text = "Sim Time (s):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderSimulationTimeLabel, "Current first-order simulation time");
            // 
            // btnFirstOrderReset
            // 
            btnFirstOrderReset.BackColor = Color.LightBlue;
            btnFirstOrderReset.FlatStyle = FlatStyle.Flat;
            btnFirstOrderReset.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFirstOrderReset.Location = new Point(453, 64);
            btnFirstOrderReset.Margin = new Padding(9, 11, 9, 11);
            btnFirstOrderReset.Name = "btnFirstOrderReset";
            btnFirstOrderReset.Size = new Size(204, 85);
            btnFirstOrderReset.TabIndex = 2;
            btnFirstOrderReset.Text = "RESET";
            toolTipFirstOrder.SetToolTip(btnFirstOrderReset, "Reset the first-order process simulation");
            btnFirstOrderReset.UseVisualStyleBackColor = false;
            btnFirstOrderReset.Click += BtnFirstOrderReset_Click;
            // 
            // btnFirstOrderStop
            // 
            btnFirstOrderStop.BackColor = Color.LightCoral;
            btnFirstOrderStop.Enabled = false;
            btnFirstOrderStop.FlatStyle = FlatStyle.Flat;
            btnFirstOrderStop.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFirstOrderStop.Location = new Point(249, 64);
            btnFirstOrderStop.Margin = new Padding(9, 11, 9, 11);
            btnFirstOrderStop.Name = "btnFirstOrderStop";
            btnFirstOrderStop.Size = new Size(186, 85);
            btnFirstOrderStop.TabIndex = 1;
            btnFirstOrderStop.Text = "STOP";
            toolTipFirstOrder.SetToolTip(btnFirstOrderStop, "Stop the first-order process simulation");
            btnFirstOrderStop.UseVisualStyleBackColor = false;
            btnFirstOrderStop.Click += BtnFirstOrderStop_Click;
            // 
            // btnFirstOrderStart
            // 
            btnFirstOrderStart.BackColor = Color.LightGreen;
            btnFirstOrderStart.FlatStyle = FlatStyle.Flat;
            btnFirstOrderStart.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFirstOrderStart.Location = new Point(45, 64);
            btnFirstOrderStart.Margin = new Padding(9, 11, 9, 11);
            btnFirstOrderStart.Name = "btnFirstOrderStart";
            btnFirstOrderStart.Size = new Size(186, 85);
            btnFirstOrderStart.TabIndex = 0;
            btnFirstOrderStart.Text = "START";
            toolTipFirstOrder.SetToolTip(btnFirstOrderStart, "Start the first-order process simulation");
            btnFirstOrderStart.UseVisualStyleBackColor = false;
            btnFirstOrderStart.Click += BtnFirstOrderStart_Click;
            // 
            // groupBoxFirstOrderControl
            // 
            groupBoxFirstOrderControl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxFirstOrderControl.Controls.Add(chkFirstOrderSetpointTracking);
            groupBoxFirstOrderControl.Controls.Add(numFirstOrderSetpoint);
            groupBoxFirstOrderControl.Controls.Add(lblFirstOrderSetpoint);
            groupBoxFirstOrderControl.Controls.Add(numFirstOrderManualOutput);
            groupBoxFirstOrderControl.Controls.Add(lblFirstOrderManualOutput);
            groupBoxFirstOrderControl.Controls.Add(btnFirstOrderManual);
            groupBoxFirstOrderControl.Controls.Add(btnFirstOrderAuto);
            groupBoxFirstOrderControl.Location = new Point(1902, 361);
            groupBoxFirstOrderControl.Margin = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderControl.Name = "groupBoxFirstOrderControl";
            groupBoxFirstOrderControl.Padding = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderControl.Size = new Size(706, 597);
            groupBoxFirstOrderControl.TabIndex = 2;
            groupBoxFirstOrderControl.TabStop = false;
            groupBoxFirstOrderControl.Text = "First-Order Control Mode";
            toolTipFirstOrder.SetToolTip(groupBoxFirstOrderControl, "First-order control mode selection and settings");
            // 
            // chkFirstOrderSetpointTracking
            // 
            chkFirstOrderSetpointTracking.AutoSize = true;
            chkFirstOrderSetpointTracking.Location = new Point(45, 416);
            chkFirstOrderSetpointTracking.Margin = new Padding(9, 11, 9, 11);
            chkFirstOrderSetpointTracking.Name = "chkFirstOrderSetpointTracking";
            chkFirstOrderSetpointTracking.Size = new Size(344, 36);
            chkFirstOrderSetpointTracking.TabIndex = 6;
            chkFirstOrderSetpointTracking.Text = "Enable Setpoint Tracking PV";
            toolTipFirstOrder.SetToolTip(chkFirstOrderSetpointTracking, "First-order setpoint tracking in manual mode");
            chkFirstOrderSetpointTracking.UseVisualStyleBackColor = true;
            chkFirstOrderSetpointTracking.CheckedChanged += ChkFirstOrderSetpointTracking_CheckedChanged;
            // 
            // numFirstOrderSetpoint
            // 
            numFirstOrderSetpoint.DecimalPlaces = 2;
            numFirstOrderSetpoint.Location = new Point(370, 301);
            numFirstOrderSetpoint.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderSetpoint.Name = "numFirstOrderSetpoint";
            numFirstOrderSetpoint.Size = new Size(288, 39);
            numFirstOrderSetpoint.TabIndex = 5;
            toolTipFirstOrder.SetToolTip(numFirstOrderSetpoint, "Target value for first-order process");
            numFirstOrderSetpoint.Value = new decimal(new int[] { 50, 0, 0, 0 });
            numFirstOrderSetpoint.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderSetpoint
            // 
            lblFirstOrderSetpoint.AutoSize = true;
            lblFirstOrderSetpoint.Location = new Point(370, 222);
            lblFirstOrderSetpoint.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderSetpoint.Name = "lblFirstOrderSetpoint";
            lblFirstOrderSetpoint.Size = new Size(150, 32);
            lblFirstOrderSetpoint.TabIndex = 4;
            lblFirstOrderSetpoint.Text = "Setpoint (%):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderSetpoint, "First-order process setpoint");
            // 
            // numFirstOrderManualOutput
            // 
            numFirstOrderManualOutput.DecimalPlaces = 2;
            numFirstOrderManualOutput.Enabled = false;
            numFirstOrderManualOutput.Location = new Point(45, 301);
            numFirstOrderManualOutput.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderManualOutput.Name = "numFirstOrderManualOutput";
            numFirstOrderManualOutput.Size = new Size(306, 39);
            numFirstOrderManualOutput.TabIndex = 3;
            toolTipFirstOrder.SetToolTip(numFirstOrderManualOutput, "First-order manual output value (0-100%)");
            numFirstOrderManualOutput.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderManualOutput
            // 
            lblFirstOrderManualOutput.AutoSize = true;
            lblFirstOrderManualOutput.Location = new Point(45, 222);
            lblFirstOrderManualOutput.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderManualOutput.Name = "lblFirstOrderManualOutput";
            lblFirstOrderManualOutput.Size = new Size(223, 32);
            lblFirstOrderManualOutput.TabIndex = 2;
            lblFirstOrderManualOutput.Text = "Manual Output (%):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderManualOutput, "First-order manual output value");
            // 
            // btnFirstOrderManual
            // 
            btnFirstOrderManual.BackColor = Color.LightGray;
            btnFirstOrderManual.FlatStyle = FlatStyle.Flat;
            btnFirstOrderManual.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFirstOrderManual.Location = new Point(370, 85);
            btnFirstOrderManual.Margin = new Padding(9, 11, 9, 11);
            btnFirstOrderManual.Name = "btnFirstOrderManual";
            btnFirstOrderManual.Size = new Size(288, 102);
            btnFirstOrderManual.TabIndex = 1;
            btnFirstOrderManual.Text = "MANUAL";
            toolTipFirstOrder.SetToolTip(btnFirstOrderManual, "First-order manual mode - direct output control");
            btnFirstOrderManual.UseVisualStyleBackColor = false;
            btnFirstOrderManual.Click += BtnFirstOrderManual_Click;
            // 
            // btnFirstOrderAuto
            // 
            btnFirstOrderAuto.BackColor = Color.LightGreen;
            btnFirstOrderAuto.FlatStyle = FlatStyle.Flat;
            btnFirstOrderAuto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFirstOrderAuto.Location = new Point(45, 85);
            btnFirstOrderAuto.Margin = new Padding(9, 11, 9, 11);
            btnFirstOrderAuto.Name = "btnFirstOrderAuto";
            btnFirstOrderAuto.Size = new Size(306, 102);
            btnFirstOrderAuto.TabIndex = 0;
            btnFirstOrderAuto.Text = "AUTOMATIC";
            toolTipFirstOrder.SetToolTip(btnFirstOrderAuto, "First-order automatic mode - PID controller active");
            btnFirstOrderAuto.UseVisualStyleBackColor = false;
            btnFirstOrderAuto.Click += BtnFirstOrderAuto_Click;
            // 
            // groupBoxFirstOrderPidParameters
            // 
            groupBoxFirstOrderPidParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxFirstOrderPidParameters.Controls.Add(numFirstOrderKd);
            groupBoxFirstOrderPidParameters.Controls.Add(lblFirstOrderKd);
            groupBoxFirstOrderPidParameters.Controls.Add(numFirstOrderKi);
            groupBoxFirstOrderPidParameters.Controls.Add(lblFirstOrderKi);
            groupBoxFirstOrderPidParameters.Controls.Add(numFirstOrderKp);
            groupBoxFirstOrderPidParameters.Controls.Add(lblFirstOrderKp);
            groupBoxFirstOrderPidParameters.Location = new Point(1902, 1278);
            groupBoxFirstOrderPidParameters.Margin = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderPidParameters.Name = "groupBoxFirstOrderPidParameters";
            groupBoxFirstOrderPidParameters.Padding = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderPidParameters.Size = new Size(706, 410);
            groupBoxFirstOrderPidParameters.TabIndex = 4;
            groupBoxFirstOrderPidParameters.TabStop = false;
            groupBoxFirstOrderPidParameters.Text = "First-Order PID Parameters";
            toolTipFirstOrder.SetToolTip(groupBoxFirstOrderPidParameters, "First-order PID controller tuning");
            // 
            // numFirstOrderKd
            // 
            numFirstOrderKd.DecimalPlaces = 3;
            numFirstOrderKd.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numFirstOrderKd.Location = new Point(241, 301);
            numFirstOrderKd.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderKd.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numFirstOrderKd.Name = "numFirstOrderKd";
            numFirstOrderKd.Size = new Size(416, 39);
            numFirstOrderKd.TabIndex = 5;
            toolTipFirstOrder.SetToolTip(numFirstOrderKd, "First-order derivative gain value");
            numFirstOrderKd.Value = new decimal(new int[] { 5, 0, 0, 131072 });
            numFirstOrderKd.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderKd
            // 
            lblFirstOrderKd.AutoSize = true;
            lblFirstOrderKd.Location = new Point(45, 307);
            lblFirstOrderKd.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderKd.Name = "lblFirstOrderKd";
            lblFirstOrderKd.Size = new Size(78, 32);
            lblFirstOrderKd.TabIndex = 4;
            lblFirstOrderKd.Text = "Kd (s):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderKd, "First-order derivative gain");
            // 
            // numFirstOrderKi
            // 
            numFirstOrderKi.DecimalPlaces = 3;
            numFirstOrderKi.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numFirstOrderKi.Location = new Point(241, 198);
            numFirstOrderKi.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderKi.Name = "numFirstOrderKi";
            numFirstOrderKi.Size = new Size(416, 39);
            numFirstOrderKi.TabIndex = 3;
            toolTipFirstOrder.SetToolTip(numFirstOrderKi, "First-order integral gain value");
            numFirstOrderKi.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            numFirstOrderKi.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderKi
            // 
            lblFirstOrderKi.AutoSize = true;
            lblFirstOrderKi.Location = new Point(45, 205);
            lblFirstOrderKi.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderKi.Name = "lblFirstOrderKi";
            lblFirstOrderKi.Size = new Size(92, 32);
            lblFirstOrderKi.TabIndex = 2;
            lblFirstOrderKi.Text = "Ki (1/s):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderKi, "First-order integral gain");
            // 
            // numFirstOrderKp
            // 
            numFirstOrderKp.DecimalPlaces = 3;
            numFirstOrderKp.Location = new Point(241, 96);
            numFirstOrderKp.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderKp.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numFirstOrderKp.Name = "numFirstOrderKp";
            numFirstOrderKp.Size = new Size(416, 39);
            numFirstOrderKp.TabIndex = 1;
            toolTipFirstOrder.SetToolTip(numFirstOrderKp, "First-order proportional gain value");
            numFirstOrderKp.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numFirstOrderKp.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderKp
            // 
            lblFirstOrderKp.AutoSize = true;
            lblFirstOrderKp.Location = new Point(45, 102);
            lblFirstOrderKp.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderKp.Name = "lblFirstOrderKp";
            lblFirstOrderKp.Size = new Size(47, 32);
            lblFirstOrderKp.TabIndex = 0;
            lblFirstOrderKp.Text = "Kp:";
            toolTipFirstOrder.SetToolTip(lblFirstOrderKp, "First-order proportional gain");
            // 
            // groupBoxFirstOrderProcessParameters
            // 
            groupBoxFirstOrderProcessParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxFirstOrderProcessParameters.Controls.Add(numFirstOrderDisturbance);
            groupBoxFirstOrderProcessParameters.Controls.Add(lblFirstOrderDisturbance);
            groupBoxFirstOrderProcessParameters.Controls.Add(numFirstOrderDeadTime);
            groupBoxFirstOrderProcessParameters.Controls.Add(lblFirstOrderDeadTime);
            groupBoxFirstOrderProcessParameters.Controls.Add(numFirstOrderTimeConstant);
            groupBoxFirstOrderProcessParameters.Controls.Add(lblFirstOrderTimeConstant);
            groupBoxFirstOrderProcessParameters.Controls.Add(numFirstOrderProcessGain);
            groupBoxFirstOrderProcessParameters.Controls.Add(lblFirstOrderProcessGain);
            groupBoxFirstOrderProcessParameters.Location = new Point(1902, 1709);
            groupBoxFirstOrderProcessParameters.Margin = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderProcessParameters.Name = "groupBoxFirstOrderProcessParameters";
            groupBoxFirstOrderProcessParameters.Padding = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderProcessParameters.Size = new Size(706, 512);
            groupBoxFirstOrderProcessParameters.TabIndex = 5;
            groupBoxFirstOrderProcessParameters.TabStop = false;
            groupBoxFirstOrderProcessParameters.Text = "First-Order Process Parameters";
            toolTipFirstOrder.SetToolTip(groupBoxFirstOrderProcessParameters, "First-order process model parameters");
            // 
            // numFirstOrderDisturbance
            // 
            numFirstOrderDisturbance.DecimalPlaces = 2;
            numFirstOrderDisturbance.Location = new Point(371, 403);
            numFirstOrderDisturbance.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderDisturbance.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numFirstOrderDisturbance.Minimum = new decimal(new int[] { 50, 0, 0, int.MinValue });
            numFirstOrderDisturbance.Name = "numFirstOrderDisturbance";
            numFirstOrderDisturbance.Size = new Size(286, 39);
            numFirstOrderDisturbance.TabIndex = 7;
            toolTipFirstOrder.SetToolTip(numFirstOrderDisturbance, "First-order disturbance value");
            numFirstOrderDisturbance.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderDisturbance
            // 
            lblFirstOrderDisturbance.AutoSize = true;
            lblFirstOrderDisturbance.Location = new Point(45, 410);
            lblFirstOrderDisturbance.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderDisturbance.Name = "lblFirstOrderDisturbance";
            lblFirstOrderDisturbance.Size = new Size(187, 32);
            lblFirstOrderDisturbance.TabIndex = 6;
            lblFirstOrderDisturbance.Text = "Disturbance (%):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderDisturbance, "First-order external disturbance");
            // 
            // numFirstOrderDeadTime
            // 
            numFirstOrderDeadTime.DecimalPlaces = 1;
            numFirstOrderDeadTime.Location = new Point(279, 301);
            numFirstOrderDeadTime.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderDeadTime.Name = "numFirstOrderDeadTime";
            numFirstOrderDeadTime.Size = new Size(379, 39);
            numFirstOrderDeadTime.TabIndex = 5;
            toolTipFirstOrder.SetToolTip(numFirstOrderDeadTime, "First-order transport delay");
            numFirstOrderDeadTime.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numFirstOrderDeadTime.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderDeadTime
            // 
            lblFirstOrderDeadTime.AutoSize = true;
            lblFirstOrderDeadTime.Location = new Point(45, 307);
            lblFirstOrderDeadTime.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderDeadTime.Name = "lblFirstOrderDeadTime";
            lblFirstOrderDeadTime.Size = new Size(75, 32);
            lblFirstOrderDeadTime.TabIndex = 4;
            lblFirstOrderDeadTime.Text = "Td (s):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderDeadTime, "First-order dead time");
            // 
            // numFirstOrderTimeConstant
            // 
            numFirstOrderTimeConstant.DecimalPlaces = 1;
            numFirstOrderTimeConstant.Location = new Point(279, 198);
            numFirstOrderTimeConstant.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderTimeConstant.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numFirstOrderTimeConstant.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numFirstOrderTimeConstant.Name = "numFirstOrderTimeConstant";
            numFirstOrderTimeConstant.Size = new Size(379, 39);
            numFirstOrderTimeConstant.TabIndex = 3;
            toolTipFirstOrder.SetToolTip(numFirstOrderTimeConstant, "First-order response speed");
            numFirstOrderTimeConstant.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numFirstOrderTimeConstant.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderTimeConstant
            // 
            lblFirstOrderTimeConstant.AutoSize = true;
            lblFirstOrderTimeConstant.Location = new Point(45, 205);
            lblFirstOrderTimeConstant.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderTimeConstant.Name = "lblFirstOrderTimeConstant";
            lblFirstOrderTimeConstant.Size = new Size(86, 32);
            lblFirstOrderTimeConstant.TabIndex = 2;
            lblFirstOrderTimeConstant.Text = "Tau (s):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderTimeConstant, "First-order time constant");
            // 
            // numFirstOrderProcessGain
            // 
            numFirstOrderProcessGain.DecimalPlaces = 2;
            numFirstOrderProcessGain.Location = new Point(279, 96);
            numFirstOrderProcessGain.Margin = new Padding(9, 11, 9, 11);
            numFirstOrderProcessGain.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            numFirstOrderProcessGain.Name = "numFirstOrderProcessGain";
            numFirstOrderProcessGain.Size = new Size(379, 39);
            numFirstOrderProcessGain.TabIndex = 1;
            toolTipFirstOrder.SetToolTip(numFirstOrderProcessGain, "First-order steady-state gain");
            numFirstOrderProcessGain.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numFirstOrderProcessGain.ValueChanged += FirstOrderParameterValue_Changed;
            // 
            // lblFirstOrderProcessGain
            // 
            lblFirstOrderProcessGain.AutoSize = true;
            lblFirstOrderProcessGain.Location = new Point(45, 102);
            lblFirstOrderProcessGain.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderProcessGain.Name = "lblFirstOrderProcessGain";
            lblFirstOrderProcessGain.Size = new Size(67, 32);
            lblFirstOrderProcessGain.TabIndex = 0;
            lblFirstOrderProcessGain.Text = "Gain:";
            toolTipFirstOrder.SetToolTip(lblFirstOrderProcessGain, "First-order process gain");
            // 
            // groupBoxFirstOrderProcessStatus
            // 
            groupBoxFirstOrderProcessStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxFirstOrderProcessStatus.Controls.Add(lblFirstOrderError);
            groupBoxFirstOrderProcessStatus.Controls.Add(lblFirstOrderErrorLabel);
            groupBoxFirstOrderProcessStatus.Controls.Add(lblFirstOrderControllerOutput);
            groupBoxFirstOrderProcessStatus.Controls.Add(lblFirstOrderControllerOutputLabel);
            groupBoxFirstOrderProcessStatus.Controls.Add(lblFirstOrderProcessVariable);
            groupBoxFirstOrderProcessStatus.Controls.Add(lblFirstOrderProcessVariableLabel);
            groupBoxFirstOrderProcessStatus.Location = new Point(1902, 979);
            groupBoxFirstOrderProcessStatus.Margin = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderProcessStatus.Name = "groupBoxFirstOrderProcessStatus";
            groupBoxFirstOrderProcessStatus.Padding = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderProcessStatus.Size = new Size(706, 277);
            groupBoxFirstOrderProcessStatus.TabIndex = 3;
            groupBoxFirstOrderProcessStatus.TabStop = false;
            groupBoxFirstOrderProcessStatus.Text = "First-Order Process Status";
            toolTipFirstOrder.SetToolTip(groupBoxFirstOrderProcessStatus, "Current first-order process values");
            // 
            // lblFirstOrderError
            // 
            lblFirstOrderError.AutoSize = true;
            lblFirstOrderError.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFirstOrderError.ForeColor = Color.Orange;
            lblFirstOrderError.Location = new Point(409, 154);
            lblFirstOrderError.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderError.Name = "lblFirstOrderError";
            lblFirstOrderError.Size = new Size(63, 32);
            lblFirstOrderError.TabIndex = 5;
            lblFirstOrderError.Text = "0.00";
            toolTipFirstOrder.SetToolTip(lblFirstOrderError, "Current first-order error (SP - PV)");
            // 
            // lblFirstOrderErrorLabel
            // 
            lblFirstOrderErrorLabel.AutoSize = true;
            lblFirstOrderErrorLabel.Location = new Point(409, 85);
            lblFirstOrderErrorLabel.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderErrorLabel.Name = "lblFirstOrderErrorLabel";
            lblFirstOrderErrorLabel.Size = new Size(69, 32);
            lblFirstOrderErrorLabel.TabIndex = 4;
            lblFirstOrderErrorLabel.Text = "Error:";
            toolTipFirstOrder.SetToolTip(lblFirstOrderErrorLabel, "First-order control error");
            // 
            // lblFirstOrderControllerOutput
            // 
            lblFirstOrderControllerOutput.AutoSize = true;
            lblFirstOrderControllerOutput.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFirstOrderControllerOutput.ForeColor = Color.Red;
            lblFirstOrderControllerOutput.Location = new Point(204, 154);
            lblFirstOrderControllerOutput.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderControllerOutput.Name = "lblFirstOrderControllerOutput";
            lblFirstOrderControllerOutput.Size = new Size(63, 32);
            lblFirstOrderControllerOutput.TabIndex = 3;
            lblFirstOrderControllerOutput.Text = "0.00";
            toolTipFirstOrder.SetToolTip(lblFirstOrderControllerOutput, "Current first-order MV value");
            // 
            // lblFirstOrderControllerOutputLabel
            // 
            lblFirstOrderControllerOutputLabel.AutoSize = true;
            lblFirstOrderControllerOutputLabel.Location = new Point(45, 154);
            lblFirstOrderControllerOutputLabel.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderControllerOutputLabel.Name = "lblFirstOrderControllerOutputLabel";
            lblFirstOrderControllerOutputLabel.Size = new Size(97, 32);
            lblFirstOrderControllerOutputLabel.TabIndex = 2;
            lblFirstOrderControllerOutputLabel.Text = "MV (%):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderControllerOutputLabel, "First-order manipulated variable");
            // 
            // lblFirstOrderProcessVariable
            // 
            lblFirstOrderProcessVariable.AutoSize = true;
            lblFirstOrderProcessVariable.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFirstOrderProcessVariable.ForeColor = Color.Green;
            lblFirstOrderProcessVariable.Location = new Point(204, 85);
            lblFirstOrderProcessVariable.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderProcessVariable.Name = "lblFirstOrderProcessVariable";
            lblFirstOrderProcessVariable.Size = new Size(63, 32);
            lblFirstOrderProcessVariable.TabIndex = 1;
            lblFirstOrderProcessVariable.Text = "0.00";
            toolTipFirstOrder.SetToolTip(lblFirstOrderProcessVariable, "Current first-order PV value");
            // 
            // lblFirstOrderProcessVariableLabel
            // 
            lblFirstOrderProcessVariableLabel.AutoSize = true;
            lblFirstOrderProcessVariableLabel.Location = new Point(45, 85);
            lblFirstOrderProcessVariableLabel.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderProcessVariableLabel.Name = "lblFirstOrderProcessVariableLabel";
            lblFirstOrderProcessVariableLabel.Size = new Size(88, 32);
            lblFirstOrderProcessVariableLabel.TabIndex = 0;
            lblFirstOrderProcessVariableLabel.Text = "PV (%):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderProcessVariableLabel, "First-order process variable");
            // 
            // groupBoxFirstOrderPlotSettings
            // 
            groupBoxFirstOrderPlotSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupBoxFirstOrderPlotSettings.Controls.Add(btnFirstOrderUpdateTrend);
            groupBoxFirstOrderPlotSettings.Controls.Add(txtFirstOrderTrendDuration);
            groupBoxFirstOrderPlotSettings.Controls.Add(lblFirstOrderTrendDuration);
            groupBoxFirstOrderPlotSettings.Location = new Point(108, 1334);
            groupBoxFirstOrderPlotSettings.Margin = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderPlotSettings.Name = "groupBoxFirstOrderPlotSettings";
            groupBoxFirstOrderPlotSettings.Padding = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderPlotSettings.Size = new Size(604, 273);
            groupBoxFirstOrderPlotSettings.TabIndex = 6;
            groupBoxFirstOrderPlotSettings.TabStop = false;
            groupBoxFirstOrderPlotSettings.Text = "First-Order Plot Settings";
            toolTipFirstOrder.SetToolTip(groupBoxFirstOrderPlotSettings, "Configure first-order plot time scale");
            // 
            // btnFirstOrderUpdateTrend
            // 
            btnFirstOrderUpdateTrend.Location = new Point(318, 160);
            btnFirstOrderUpdateTrend.Margin = new Padding(9, 11, 9, 11);
            btnFirstOrderUpdateTrend.Name = "btnFirstOrderUpdateTrend";
            btnFirstOrderUpdateTrend.Size = new Size(227, 96);
            btnFirstOrderUpdateTrend.TabIndex = 2;
            btnFirstOrderUpdateTrend.Text = "Update";
            toolTipFirstOrder.SetToolTip(btnFirstOrderUpdateTrend, "Apply first-order trend duration");
            btnFirstOrderUpdateTrend.UseVisualStyleBackColor = true;
            btnFirstOrderUpdateTrend.Click += BtnFirstOrderUpdateTrend_Click;
            // 
            // txtFirstOrderTrendDuration
            // 
            txtFirstOrderTrendDuration.Location = new Point(45, 164);
            txtFirstOrderTrendDuration.Margin = new Padding(9, 11, 9, 11);
            txtFirstOrderTrendDuration.Name = "txtFirstOrderTrendDuration";
            txtFirstOrderTrendDuration.Size = new Size(234, 39);
            txtFirstOrderTrendDuration.TabIndex = 1;
            txtFirstOrderTrendDuration.Text = "300";
            toolTipFirstOrder.SetToolTip(txtFirstOrderTrendDuration, "First-order time window in seconds");
            // 
            // lblFirstOrderTrendDuration
            // 
            lblFirstOrderTrendDuration.AutoSize = true;
            lblFirstOrderTrendDuration.Location = new Point(45, 85);
            lblFirstOrderTrendDuration.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderTrendDuration.Name = "lblFirstOrderTrendDuration";
            lblFirstOrderTrendDuration.Size = new Size(210, 32);
            lblFirstOrderTrendDuration.TabIndex = 0;
            lblFirstOrderTrendDuration.Text = "Trend Duration (s):";
            toolTipFirstOrder.SetToolTip(lblFirstOrderTrendDuration, "First-order plot time window");
            // 
            // groupBoxFirstOrderLegend
            // 
            groupBoxFirstOrderLegend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupBoxFirstOrderLegend.Controls.Add(lblFirstOrderErrorLegend);
            groupBoxFirstOrderLegend.Controls.Add(lblFirstOrderControllerOutputLegend);
            groupBoxFirstOrderLegend.Controls.Add(lblFirstOrderProcessVariableLegend);
            groupBoxFirstOrderLegend.Controls.Add(lblFirstOrderSetpointLegend);
            groupBoxFirstOrderLegend.Location = new Point(850, 1334);
            groupBoxFirstOrderLegend.Margin = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderLegend.Name = "groupBoxFirstOrderLegend";
            groupBoxFirstOrderLegend.Padding = new Padding(9, 11, 9, 11);
            groupBoxFirstOrderLegend.Size = new Size(929, 353);
            groupBoxFirstOrderLegend.TabIndex = 7;
            groupBoxFirstOrderLegend.TabStop = false;
            groupBoxFirstOrderLegend.Text = "First-Order Plot Legend";
            toolTipFirstOrder.SetToolTip(groupBoxFirstOrderLegend, "First-order plot legend");
            // 
            // lblFirstOrderErrorLegend
            // 
            lblFirstOrderErrorLegend.AutoSize = true;
            lblFirstOrderErrorLegend.ForeColor = Color.Orange;
            lblFirstOrderErrorLegend.Location = new Point(45, 290);
            lblFirstOrderErrorLegend.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderErrorLegend.Name = "lblFirstOrderErrorLegend";
            lblFirstOrderErrorLegend.Size = new Size(124, 32);
            lblFirstOrderErrorLegend.TabIndex = 3;
            lblFirstOrderErrorLegend.Text = "· · · · · Error";
            toolTipFirstOrder.SetToolTip(lblFirstOrderErrorLegend, "First-order control error line");
            // 
            // lblFirstOrderControllerOutputLegend
            // 
            lblFirstOrderControllerOutputLegend.AutoSize = true;
            lblFirstOrderControllerOutputLegend.ForeColor = Color.Red;
            lblFirstOrderControllerOutputLegend.Location = new Point(45, 222);
            lblFirstOrderControllerOutputLegend.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderControllerOutputLegend.Name = "lblFirstOrderControllerOutputLegend";
            lblFirstOrderControllerOutputLegend.Size = new Size(364, 32);
            lblFirstOrderControllerOutputLegend.TabIndex = 2;
            lblFirstOrderControllerOutputLegend.Text = "———— Controller Output (MV)";
            toolTipFirstOrder.SetToolTip(lblFirstOrderControllerOutputLegend, "First-order manipulated variable line");
            // 
            // lblFirstOrderProcessVariableLegend
            // 
            lblFirstOrderProcessVariableLegend.AutoSize = true;
            lblFirstOrderProcessVariableLegend.ForeColor = Color.Green;
            lblFirstOrderProcessVariableLegend.Location = new Point(45, 154);
            lblFirstOrderProcessVariableLegend.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderProcessVariableLegend.Name = "lblFirstOrderProcessVariableLegend";
            lblFirstOrderProcessVariableLegend.Size = new Size(336, 32);
            lblFirstOrderProcessVariableLegend.TabIndex = 1;
            lblFirstOrderProcessVariableLegend.Text = "———— Process Variable (PV)";
            toolTipFirstOrder.SetToolTip(lblFirstOrderProcessVariableLegend, "First-order process variable line");
            // 
            // lblFirstOrderSetpointLegend
            // 
            lblFirstOrderSetpointLegend.AutoSize = true;
            lblFirstOrderSetpointLegend.ForeColor = Color.Blue;
            lblFirstOrderSetpointLegend.Location = new Point(45, 85);
            lblFirstOrderSetpointLegend.Margin = new Padding(9, 0, 9, 0);
            lblFirstOrderSetpointLegend.Name = "lblFirstOrderSetpointLegend";
            lblFirstOrderSetpointLegend.Size = new Size(213, 32);
            lblFirstOrderSetpointLegend.TabIndex = 0;
            lblFirstOrderSetpointLegend.Text = "— — Setpoint (SP)";
            toolTipFirstOrder.SetToolTip(lblFirstOrderSetpointLegend, "First-order setpoint line");
            // 
            // FirstOrderWithPidForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2671, 1707);
            Controls.Add(groupBoxFirstOrderLegend);
            Controls.Add(groupBoxFirstOrderPlotSettings);
            Controls.Add(groupBoxFirstOrderProcessParameters);
            Controls.Add(groupBoxFirstOrderPidParameters);
            Controls.Add(groupBoxFirstOrderProcessStatus);
            Controls.Add(groupBoxFirstOrderControl);
            Controls.Add(groupBoxFirstOrderSimulationControls);
            Controls.Add(plotFirstOrderView);
            Margin = new Padding(6);
            MinimumSize = new Size(2206, 1200);
            Name = "FirstOrderWithPidForm";
            Text = "First-Order Process with PID Control - Single Timer Architecture";
            toolTipFirstOrder.SetToolTip(this, "First-Order Process with PID Control Simulator");
            groupBoxFirstOrderSimulationControls.ResumeLayout(false);
            groupBoxFirstOrderSimulationControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkSimulationSpeed).EndInit();
            groupBoxFirstOrderControl.ResumeLayout(false);
            groupBoxFirstOrderControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderSetpoint).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderManualOutput).EndInit();
            groupBoxFirstOrderPidParameters.ResumeLayout(false);
            groupBoxFirstOrderPidParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderKd).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderKi).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderKp).EndInit();
            groupBoxFirstOrderProcessParameters.ResumeLayout(false);
            groupBoxFirstOrderProcessParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderDisturbance).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderDeadTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderTimeConstant).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFirstOrderProcessGain).EndInit();
            groupBoxFirstOrderProcessStatus.ResumeLayout(false);
            groupBoxFirstOrderProcessStatus.PerformLayout();
            groupBoxFirstOrderPlotSettings.ResumeLayout(false);
            groupBoxFirstOrderPlotSettings.PerformLayout();
            groupBoxFirstOrderLegend.ResumeLayout(false);
            groupBoxFirstOrderLegend.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}