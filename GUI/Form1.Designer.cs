namespace GUI
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.forceSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enabledForceSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onForceSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offForceSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grossToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zStageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enabledZStageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onZStageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.offZStageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.zStageCalibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zStageSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zStageSettingsPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zStageIgnoreSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.positionsFoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackingModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voltmeterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enabledVoltmeterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onVoltmeterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offVoltmeterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voltageValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xyzStageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voltmeterReadoutLabel = new System.Windows.Forms.Label();
            this.zStageGroupBox = new System.Windows.Forms.GroupBox();
            this.zStageManualPrintButton = new System.Windows.Forms.Button();
            this.zStageCancelPrintButton = new System.Windows.Forms.Button();
            this.zStageMoveToPositionTextBox = new System.Windows.Forms.TextBox();
            this.zStageResetButton = new System.Windows.Forms.Button();
            this.zStageMoveToPositionInputButton = new System.Windows.Forms.Button();
            this.zStageMoveToPositionLabel = new System.Windows.Forms.Label();
            this.zStageMoveUpButton = new System.Windows.Forms.Button();
            this.zStageMoveDownButton = new System.Windows.Forms.Button();
            this.zStagePositionUpdateLabel = new System.Windows.Forms.Label();
            this.zStagePositionTextLabel = new System.Windows.Forms.Label();
            this.forceSensorGroupBox = new System.Windows.Forms.GroupBox();
            this.forceSensorStopValueTextBox = new System.Windows.Forms.TextBox();
            this.forceSensorStopValueInputButton = new System.Windows.Forms.Button();
            this.forceSensorStopValueLabel = new System.Windows.Forms.Label();
            this.forceSensorValueLabel = new System.Windows.Forms.Label();
            this.feedbackRichTextBox = new System.Windows.Forms.RichTextBox();
            this.sensorUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.zStagePrintBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTipMasterControl = new System.Windows.Forms.ToolTip(this.components);
            this.resetCalibrationtoolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.zStageGroupBox.SuspendLayout();
            this.forceSensorGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceSensorToolStripMenuItem,
            this.zStageToolStripMenuItem,
            this.voltmeterToolStripMenuItem,
            this.xyzStageToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(493, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // forceSensorToolStripMenuItem
            // 
            this.forceSensorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enabledForceSensorToolStripMenuItem,
            this.grossToolStripMenuItem,
            this.tareToolStripMenuItem});
            this.forceSensorToolStripMenuItem.Name = "forceSensorToolStripMenuItem";
            this.forceSensorToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.forceSensorToolStripMenuItem.Text = "&Force Sensor";
            // 
            // enabledForceSensorToolStripMenuItem
            // 
            this.enabledForceSensorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onForceSensorToolStripMenuItem,
            this.offForceSensorToolStripMenuItem});
            this.enabledForceSensorToolStripMenuItem.Name = "enabledForceSensorToolStripMenuItem";
            this.enabledForceSensorToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.enabledForceSensorToolStripMenuItem.Text = "&Enabled";
            // 
            // onForceSensorToolStripMenuItem
            // 
            this.onForceSensorToolStripMenuItem.Name = "onForceSensorToolStripMenuItem";
            this.onForceSensorToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.onForceSensorToolStripMenuItem.Text = "&On";
            this.onForceSensorToolStripMenuItem.Click += new System.EventHandler(this.onForceSensorToolStripMenuItem_Click);
            // 
            // offForceSensorToolStripMenuItem
            // 
            this.offForceSensorToolStripMenuItem.Checked = true;
            this.offForceSensorToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.offForceSensorToolStripMenuItem.Name = "offForceSensorToolStripMenuItem";
            this.offForceSensorToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.offForceSensorToolStripMenuItem.Text = "O&ff";
            this.offForceSensorToolStripMenuItem.Click += new System.EventHandler(this.offForceSensorToolStripMenuItem_Click);
            // 
            // grossToolStripMenuItem
            // 
            this.grossToolStripMenuItem.Checked = true;
            this.grossToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.grossToolStripMenuItem.Enabled = false;
            this.grossToolStripMenuItem.Name = "grossToolStripMenuItem";
            this.grossToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.grossToolStripMenuItem.Text = "&Gross";
            this.grossToolStripMenuItem.ToolTipText = "Display the gross value of the force sensor";
            this.grossToolStripMenuItem.Click += new System.EventHandler(this.grossToolStripMenuItem_Click);
            // 
            // tareToolStripMenuItem
            // 
            this.tareToolStripMenuItem.Enabled = false;
            this.tareToolStripMenuItem.Name = "tareToolStripMenuItem";
            this.tareToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.tareToolStripMenuItem.Text = "&Tare";
            this.tareToolStripMenuItem.ToolTipText = "Set the current gross value to zero";
            this.tareToolStripMenuItem.Click += new System.EventHandler(this.tareToolStripMenuItem_Click);
            // 
            // zStageToolStripMenuItem
            // 
            this.zStageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enabledZStageToolStripMenuItem,
            this.toolStripMenuItem1,
            this.zStageCalibrationToolStripMenuItem,
            this.resetCalibrationtoolStripMenuItem2,
            this.zStageSettingsToolStripMenuItem,
            this.trackingModeToolStripMenuItem});
            this.zStageToolStripMenuItem.Enabled = false;
            this.zStageToolStripMenuItem.Name = "zStageToolStripMenuItem";
            this.zStageToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.zStageToolStripMenuItem.Text = "&Z-Stage";
            // 
            // enabledZStageToolStripMenuItem
            // 
            this.enabledZStageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onZStageToolStripMenuItem1,
            this.offZStageToolStripMenuItem1});
            this.enabledZStageToolStripMenuItem.Name = "enabledZStageToolStripMenuItem";
            this.enabledZStageToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.enabledZStageToolStripMenuItem.Text = "&Enabled";
            // 
            // onZStageToolStripMenuItem1
            // 
            this.onZStageToolStripMenuItem1.Name = "onZStageToolStripMenuItem1";
            this.onZStageToolStripMenuItem1.Size = new System.Drawing.Size(91, 22);
            this.onZStageToolStripMenuItem1.Text = "&On";
            this.onZStageToolStripMenuItem1.Click += new System.EventHandler(this.onZStageToolStripMenuItem1_Click);
            // 
            // offZStageToolStripMenuItem1
            // 
            this.offZStageToolStripMenuItem1.Checked = true;
            this.offZStageToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.offZStageToolStripMenuItem1.Name = "offZStageToolStripMenuItem1";
            this.offZStageToolStripMenuItem1.Size = new System.Drawing.Size(91, 22);
            this.offZStageToolStripMenuItem1.Text = "O&ff";
            this.offZStageToolStripMenuItem1.Click += new System.EventHandler(this.offZStageToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(160, 6);
            // 
            // zStageCalibrationToolStripMenuItem
            // 
            this.zStageCalibrationToolStripMenuItem.Enabled = false;
            this.zStageCalibrationToolStripMenuItem.Name = "zStageCalibrationToolStripMenuItem";
            this.zStageCalibrationToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.zStageCalibrationToolStripMenuItem.Text = "&Calibration";
            this.zStageCalibrationToolStripMenuItem.ToolTipText = "Calibrate Z-Stage to find approximate position for printing";
            this.zStageCalibrationToolStripMenuItem.Click += new System.EventHandler(this.zStageCalibrationToolStripMenuItem_Click);
            // 
            // zStageSettingsToolStripMenuItem
            // 
            this.zStageSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zStageSettingsPanelToolStripMenuItem,
            this.zStageIgnoreSensorToolStripMenuItem,
            this.positionsFoundToolStripMenuItem});
            this.zStageSettingsToolStripMenuItem.Enabled = false;
            this.zStageSettingsToolStripMenuItem.Name = "zStageSettingsToolStripMenuItem";
            this.zStageSettingsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.zStageSettingsToolStripMenuItem.Text = "&Settings";
            // 
            // zStageSettingsPanelToolStripMenuItem
            // 
            this.zStageSettingsPanelToolStripMenuItem.Name = "zStageSettingsPanelToolStripMenuItem";
            this.zStageSettingsPanelToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.zStageSettingsPanelToolStripMenuItem.Text = "&Settings Panel";
            this.zStageSettingsPanelToolStripMenuItem.ToolTipText = "Change the settings of the Z-Stage";
            this.zStageSettingsPanelToolStripMenuItem.Click += new System.EventHandler(this.zStageSettingsPanelToolStripMenuItem_Click);
            // 
            // zStageIgnoreSensorToolStripMenuItem
            // 
            this.zStageIgnoreSensorToolStripMenuItem.Name = "zStageIgnoreSensorToolStripMenuItem";
            this.zStageIgnoreSensorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.zStageIgnoreSensorToolStripMenuItem.Text = "&Ignore Sensor";
            this.zStageIgnoreSensorToolStripMenuItem.ToolTipText = "Force the Z-Stage to ignore the sensor stop value";
            this.zStageIgnoreSensorToolStripMenuItem.Click += new System.EventHandler(this.zStageIgnoreSensorToolStripMenuItem_Click);
            // 
            // positionsFoundToolStripMenuItem
            // 
            this.positionsFoundToolStripMenuItem.Name = "positionsFoundToolStripMenuItem";
            this.positionsFoundToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.positionsFoundToolStripMenuItem.Text = "&Positions Found";
            this.positionsFoundToolStripMenuItem.ToolTipText = "This dialog will display the positions found during each phase of Z-Stage calibra" +
    "tion.";
            this.positionsFoundToolStripMenuItem.Click += new System.EventHandler(this.positionsFoundToolStripMenuItem_Click);
            // 
            // trackingModeToolStripMenuItem
            // 
            this.trackingModeToolStripMenuItem.Enabled = false;
            this.trackingModeToolStripMenuItem.Name = "trackingModeToolStripMenuItem";
            this.trackingModeToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.trackingModeToolStripMenuItem.Text = "Tracking Mode";
            this.trackingModeToolStripMenuItem.ToolTipText = "This dialog will open the Z-Stage tracking mode form.";
            this.trackingModeToolStripMenuItem.Click += new System.EventHandler(this.trackingModeToolStripMenuItem_Click);
            // 
            // voltmeterToolStripMenuItem
            // 
            this.voltmeterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enabledVoltmeterToolStripMenuItem,
            this.voltageValueToolStripMenuItem});
            this.voltmeterToolStripMenuItem.Enabled = false;
            this.voltmeterToolStripMenuItem.Name = "voltmeterToolStripMenuItem";
            this.voltmeterToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.voltmeterToolStripMenuItem.Text = "&Voltmeter";
            // 
            // enabledVoltmeterToolStripMenuItem
            // 
            this.enabledVoltmeterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onVoltmeterToolStripMenuItem,
            this.offVoltmeterToolStripMenuItem});
            this.enabledVoltmeterToolStripMenuItem.Name = "enabledVoltmeterToolStripMenuItem";
            this.enabledVoltmeterToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.enabledVoltmeterToolStripMenuItem.Text = "&Enabled";
            // 
            // onVoltmeterToolStripMenuItem
            // 
            this.onVoltmeterToolStripMenuItem.Name = "onVoltmeterToolStripMenuItem";
            this.onVoltmeterToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.onVoltmeterToolStripMenuItem.Text = "&On";
            this.onVoltmeterToolStripMenuItem.Click += new System.EventHandler(this.onVoltmeterToolStripMenuItem_Click);
            // 
            // offVoltmeterToolStripMenuItem
            // 
            this.offVoltmeterToolStripMenuItem.Checked = true;
            this.offVoltmeterToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.offVoltmeterToolStripMenuItem.Name = "offVoltmeterToolStripMenuItem";
            this.offVoltmeterToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.offVoltmeterToolStripMenuItem.Text = "&Off";
            this.offVoltmeterToolStripMenuItem.Click += new System.EventHandler(this.offVoltmeterToolStripMenuItem_Click);
            // 
            // voltageValueToolStripMenuItem
            // 
            this.voltageValueToolStripMenuItem.Enabled = false;
            this.voltageValueToolStripMenuItem.Name = "voltageValueToolStripMenuItem";
            this.voltageValueToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.voltageValueToolStripMenuItem.Text = "&Voltage Change";
            this.voltageValueToolStripMenuItem.ToolTipText = "Alter the voltage change value - the change value is the amount of change in volt" +
    "age to detect to begin the printing process.";
            this.voltageValueToolStripMenuItem.Click += new System.EventHandler(this.voltageValueToolStripMenuItem_Click);
            // 
            // xyzStageToolStripMenuItem
            // 
            this.xyzStageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlPanelToolStripMenuItem});
            this.xyzStageToolStripMenuItem.Name = "xyzStageToolStripMenuItem";
            this.xyzStageToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.xyzStageToolStripMenuItem.Text = "&XYZ-Stage";
            // 
            // controlPanelToolStripMenuItem
            // 
            this.controlPanelToolStripMenuItem.Name = "controlPanelToolStripMenuItem";
            this.controlPanelToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.controlPanelToolStripMenuItem.Text = "&Display";
            this.controlPanelToolStripMenuItem.Click += new System.EventHandler(this.controlPanelToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tutorialToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // tutorialToolStripMenuItem
            // 
            this.tutorialToolStripMenuItem.Name = "tutorialToolStripMenuItem";
            this.tutorialToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.tutorialToolStripMenuItem.Text = "&Tutorial";
            this.tutorialToolStripMenuItem.ToolTipText = "The tutorial associated with this software.";
            this.tutorialToolStripMenuItem.Click += new System.EventHandler(this.tutorialToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // voltmeterReadoutLabel
            // 
            this.voltmeterReadoutLabel.AutoSize = true;
            this.voltmeterReadoutLabel.Font = new System.Drawing.Font("Moire", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voltmeterReadoutLabel.ForeColor = System.Drawing.Color.Red;
            this.voltmeterReadoutLabel.Location = new System.Drawing.Point(22, 343);
            this.voltmeterReadoutLabel.Name = "voltmeterReadoutLabel";
            this.voltmeterReadoutLabel.Size = new System.Drawing.Size(19, 18);
            this.voltmeterReadoutLabel.TabIndex = 101;
            this.voltmeterReadoutLabel.Text = "V";
            this.voltmeterReadoutLabel.Visible = false;
            // 
            // zStageGroupBox
            // 
            this.zStageGroupBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.zStageGroupBox.Controls.Add(this.zStageManualPrintButton);
            this.zStageGroupBox.Controls.Add(this.zStageCancelPrintButton);
            this.zStageGroupBox.Controls.Add(this.zStageMoveToPositionTextBox);
            this.zStageGroupBox.Controls.Add(this.zStageResetButton);
            this.zStageGroupBox.Controls.Add(this.zStageMoveToPositionInputButton);
            this.zStageGroupBox.Controls.Add(this.zStageMoveToPositionLabel);
            this.zStageGroupBox.Controls.Add(this.zStageMoveUpButton);
            this.zStageGroupBox.Controls.Add(this.zStageMoveDownButton);
            this.zStageGroupBox.Controls.Add(this.zStagePositionUpdateLabel);
            this.zStageGroupBox.Controls.Add(this.zStagePositionTextLabel);
            this.zStageGroupBox.Location = new System.Drawing.Point(12, 133);
            this.zStageGroupBox.Name = "zStageGroupBox";
            this.zStageGroupBox.Size = new System.Drawing.Size(209, 193);
            this.zStageGroupBox.TabIndex = 104;
            this.zStageGroupBox.TabStop = false;
            this.zStageGroupBox.Text = "Z-Stage";
            this.zStageGroupBox.Visible = false;
            // 
            // zStageManualPrintButton
            // 
            this.zStageManualPrintButton.Enabled = false;
            this.zStageManualPrintButton.Location = new System.Drawing.Point(12, 138);
            this.zStageManualPrintButton.Name = "zStageManualPrintButton";
            this.zStageManualPrintButton.Size = new System.Drawing.Size(75, 23);
            this.zStageManualPrintButton.TabIndex = 5;
            this.zStageManualPrintButton.Text = "Manual Print";
            this.zStageManualPrintButton.UseVisualStyleBackColor = true;
            this.zStageManualPrintButton.Click += new System.EventHandler(this.zStageManualPrintButton_Click);
            // 
            // zStageCancelPrintButton
            // 
            this.zStageCancelPrintButton.Enabled = false;
            this.zStageCancelPrintButton.Location = new System.Drawing.Point(12, 164);
            this.zStageCancelPrintButton.Name = "zStageCancelPrintButton";
            this.zStageCancelPrintButton.Size = new System.Drawing.Size(75, 23);
            this.zStageCancelPrintButton.TabIndex = 7;
            this.zStageCancelPrintButton.Text = "Cancel Print";
            this.toolTipMasterControl.SetToolTip(this.zStageCancelPrintButton, "Force the Z-Stage to cancel printing");
            this.zStageCancelPrintButton.UseVisualStyleBackColor = true;
            this.zStageCancelPrintButton.Click += new System.EventHandler(this.zStageCancelPrintButton_Click);
            // 
            // zStageMoveToPositionTextBox
            // 
            this.zStageMoveToPositionTextBox.Location = new System.Drawing.Point(62, 105);
            this.zStageMoveToPositionTextBox.Name = "zStageMoveToPositionTextBox";
            this.zStageMoveToPositionTextBox.Size = new System.Drawing.Size(100, 20);
            this.zStageMoveToPositionTextBox.TabIndex = 3;
            // 
            // zStageResetButton
            // 
            this.zStageResetButton.Location = new System.Drawing.Point(92, 138);
            this.zStageResetButton.Name = "zStageResetButton";
            this.zStageResetButton.Size = new System.Drawing.Size(90, 23);
            this.zStageResetButton.TabIndex = 6;
            this.zStageResetButton.Text = "Reset Position";
            this.toolTipMasterControl.SetToolTip(this.zStageResetButton, "Reset the position of the Z-Stage to its minimum position");
            this.zStageResetButton.UseVisualStyleBackColor = true;
            this.zStageResetButton.Click += new System.EventHandler(this.zStageResetButton_Click);
            // 
            // zStageMoveToPositionInputButton
            // 
            this.zStageMoveToPositionInputButton.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zStageMoveToPositionInputButton.Image = global::GUI.Properties.Resources.checkmark;
            this.zStageMoveToPositionInputButton.Location = new System.Drawing.Point(168, 105);
            this.zStageMoveToPositionInputButton.Name = "zStageMoveToPositionInputButton";
            this.zStageMoveToPositionInputButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.zStageMoveToPositionInputButton.Size = new System.Drawing.Size(32, 20);
            this.zStageMoveToPositionInputButton.TabIndex = 4;
            this.toolTipMasterControl.SetToolTip(this.zStageMoveToPositionInputButton, "Move the Z-Stage to the specified position");
            this.zStageMoveToPositionInputButton.UseVisualStyleBackColor = true;
            this.zStageMoveToPositionInputButton.Click += new System.EventHandler(this.zStageMoveToPositionInputButton_Click);
            // 
            // zStageMoveToPositionLabel
            // 
            this.zStageMoveToPositionLabel.AutoSize = true;
            this.zStageMoveToPositionLabel.Location = new System.Drawing.Point(9, 108);
            this.zStageMoveToPositionLabel.Name = "zStageMoveToPositionLabel";
            this.zStageMoveToPositionLabel.Size = new System.Drawing.Size(53, 13);
            this.zStageMoveToPositionLabel.TabIndex = 4;
            this.zStageMoveToPositionLabel.Text = "Move To:";
            // 
            // zStageMoveUpButton
            // 
            this.zStageMoveUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.zStageMoveUpButton.Image = global::GUI.Properties.Resources.uparrow;
            this.zStageMoveUpButton.Location = new System.Drawing.Point(92, 58);
            this.zStageMoveUpButton.Name = "zStageMoveUpButton";
            this.zStageMoveUpButton.Size = new System.Drawing.Size(75, 31);
            this.zStageMoveUpButton.TabIndex = 9;
            this.toolTipMasterControl.SetToolTip(this.zStageMoveUpButton, "Move the Z-Stage up");
            this.zStageMoveUpButton.UseVisualStyleBackColor = true;
            this.zStageMoveUpButton.Click += new System.EventHandler(this.zStageMoveUpButton_Click);
            // 
            // zStageMoveDownButton
            // 
            this.zStageMoveDownButton.FlatAppearance.BorderSize = 2;
            this.zStageMoveDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.zStageMoveDownButton.Image = global::GUI.Properties.Resources.downarrow1;
            this.zStageMoveDownButton.Location = new System.Drawing.Point(10, 58);
            this.zStageMoveDownButton.Name = "zStageMoveDownButton";
            this.zStageMoveDownButton.Size = new System.Drawing.Size(75, 31);
            this.zStageMoveDownButton.TabIndex = 8;
            this.toolTipMasterControl.SetToolTip(this.zStageMoveDownButton, "Move the Z-Stage down");
            this.zStageMoveDownButton.UseVisualStyleBackColor = true;
            this.zStageMoveDownButton.Click += new System.EventHandler(this.zStageMoveDownButton_Click);
            // 
            // zStagePositionUpdateLabel
            // 
            this.zStagePositionUpdateLabel.AutoSize = true;
            this.zStagePositionUpdateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zStagePositionUpdateLabel.ForeColor = System.Drawing.Color.Red;
            this.zStagePositionUpdateLabel.Location = new System.Drawing.Point(60, 15);
            this.zStagePositionUpdateLabel.Name = "zStagePositionUpdateLabel";
            this.zStagePositionUpdateLabel.Size = new System.Drawing.Size(19, 20);
            this.zStagePositionUpdateLabel.TabIndex = 103;
            this.zStagePositionUpdateLabel.Text = "0";
            // 
            // zStagePositionTextLabel
            // 
            this.zStagePositionTextLabel.AutoSize = true;
            this.zStagePositionTextLabel.Location = new System.Drawing.Point(7, 20);
            this.zStagePositionTextLabel.Name = "zStagePositionTextLabel";
            this.zStagePositionTextLabel.Size = new System.Drawing.Size(47, 13);
            this.zStagePositionTextLabel.TabIndex = 0;
            this.zStagePositionTextLabel.Text = "Position:";
            // 
            // forceSensorGroupBox
            // 
            this.forceSensorGroupBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.forceSensorGroupBox.Controls.Add(this.forceSensorStopValueTextBox);
            this.forceSensorGroupBox.Controls.Add(this.forceSensorStopValueInputButton);
            this.forceSensorGroupBox.Controls.Add(this.forceSensorStopValueLabel);
            this.forceSensorGroupBox.Controls.Add(this.forceSensorValueLabel);
            this.forceSensorGroupBox.Location = new System.Drawing.Point(12, 27);
            this.forceSensorGroupBox.Name = "forceSensorGroupBox";
            this.forceSensorGroupBox.Size = new System.Drawing.Size(209, 100);
            this.forceSensorGroupBox.TabIndex = 17;
            this.forceSensorGroupBox.TabStop = false;
            this.forceSensorGroupBox.Text = "Force Sensor";
            this.forceSensorGroupBox.Visible = false;
            // 
            // forceSensorStopValueTextBox
            // 
            this.forceSensorStopValueTextBox.Location = new System.Drawing.Point(67, 72);
            this.forceSensorStopValueTextBox.Name = "forceSensorStopValueTextBox";
            this.forceSensorStopValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.forceSensorStopValueTextBox.TabIndex = 1;
            // 
            // forceSensorStopValueInputButton
            // 
            this.forceSensorStopValueInputButton.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forceSensorStopValueInputButton.Image = global::GUI.Properties.Resources.checkmark;
            this.forceSensorStopValueInputButton.Location = new System.Drawing.Point(168, 72);
            this.forceSensorStopValueInputButton.Name = "forceSensorStopValueInputButton";
            this.forceSensorStopValueInputButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.forceSensorStopValueInputButton.Size = new System.Drawing.Size(32, 20);
            this.forceSensorStopValueInputButton.TabIndex = 2;
            this.toolTipMasterControl.SetToolTip(this.forceSensorStopValueInputButton, "Change the stop value of the force sensor");
            this.forceSensorStopValueInputButton.UseVisualStyleBackColor = true;
            this.forceSensorStopValueInputButton.Click += new System.EventHandler(this.forceSensorStopValueInputButton_Click);
            // 
            // forceSensorStopValueLabel
            // 
            this.forceSensorStopValueLabel.AutoSize = true;
            this.forceSensorStopValueLabel.Location = new System.Drawing.Point(6, 75);
            this.forceSensorStopValueLabel.Name = "forceSensorStopValueLabel";
            this.forceSensorStopValueLabel.Size = new System.Drawing.Size(62, 13);
            this.forceSensorStopValueLabel.TabIndex = 105;
            this.forceSensorStopValueLabel.Text = "Stop Value:";
            // 
            // forceSensorValueLabel
            // 
            this.forceSensorValueLabel.AutoSize = true;
            this.forceSensorValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forceSensorValueLabel.ForeColor = System.Drawing.Color.Red;
            this.forceSensorValueLabel.Location = new System.Drawing.Point(7, 20);
            this.forceSensorValueLabel.Name = "forceSensorValueLabel";
            this.forceSensorValueLabel.Size = new System.Drawing.Size(49, 33);
            this.forceSensorValueLabel.TabIndex = 0;
            this.forceSensorValueLabel.Text = "0g";
            // 
            // feedbackRichTextBox
            // 
            this.feedbackRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.feedbackRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.feedbackRichTextBox.Location = new System.Drawing.Point(243, 31);
            this.feedbackRichTextBox.Name = "feedbackRichTextBox";
            this.feedbackRichTextBox.ReadOnly = true;
            this.feedbackRichTextBox.Size = new System.Drawing.Size(238, 330);
            this.feedbackRichTextBox.TabIndex = 100;
            this.feedbackRichTextBox.Text = "3D Printing System Feedback";
            // 
            // resetCalibrationtoolStripMenuItem2
            // 
            this.resetCalibrationtoolStripMenuItem2.Enabled = false;
            this.resetCalibrationtoolStripMenuItem2.Name = "resetCalibrationtoolStripMenuItem2";
            this.resetCalibrationtoolStripMenuItem2.Size = new System.Drawing.Size(163, 22);
            this.resetCalibrationtoolStripMenuItem2.Text = "Reset Calibration";
            this.resetCalibrationtoolStripMenuItem2.Click += new System.EventHandler(this.resetCalibrationtoolStripMenuItem2_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 370);
            this.Controls.Add(this.feedbackRichTextBox);
            this.Controls.Add(this.voltmeterReadoutLabel);
            this.Controls.Add(this.zStageGroupBox);
            this.Controls.Add(this.forceSensorGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IsMdiContainer = true;
            this.Name = "FormMain";
            this.Text = "3D Printer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.zStageGroupBox.ResumeLayout(false);
            this.zStageGroupBox.PerformLayout();
            this.forceSensorGroupBox.ResumeLayout(false);
            this.forceSensorGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem forceSensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enabledForceSensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onForceSensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offForceSensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grossToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zStageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enabledZStageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onZStageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem offZStageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem zStageCalibrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zStageSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zStageSettingsPanelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zStageIgnoreSensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem voltmeterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enabledVoltmeterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onVoltmeterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offVoltmeterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xyzStageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlPanelToolStripMenuItem;
        private System.Windows.Forms.Label voltmeterReadoutLabel;
        private System.Windows.Forms.GroupBox zStageGroupBox;
        private System.Windows.Forms.Button zStageCancelPrintButton;
        private System.Windows.Forms.TextBox zStageMoveToPositionTextBox;
        private System.Windows.Forms.Button zStageResetButton;
        private System.Windows.Forms.Button zStageMoveToPositionInputButton;
        private System.Windows.Forms.Label zStageMoveToPositionLabel;
        private System.Windows.Forms.Button zStageMoveUpButton;
        private System.Windows.Forms.Button zStageMoveDownButton;
        private System.Windows.Forms.Label zStagePositionUpdateLabel;
        private System.Windows.Forms.Label zStagePositionTextLabel;
        private System.Windows.Forms.GroupBox forceSensorGroupBox;
        private System.Windows.Forms.TextBox forceSensorStopValueTextBox;
        private System.Windows.Forms.Button forceSensorStopValueInputButton;
        private System.Windows.Forms.Label forceSensorStopValueLabel;
        private System.Windows.Forms.Label forceSensorValueLabel;
        private System.Windows.Forms.RichTextBox feedbackRichTextBox;
        private System.Windows.Forms.Timer sensorUpdateTimer;
        private System.ComponentModel.BackgroundWorker zStagePrintBackgroundWorker;
        private System.Windows.Forms.ToolTip toolTipMasterControl;
        private System.Windows.Forms.ToolStripMenuItem positionsFoundToolStripMenuItem;
        private System.Windows.Forms.Button zStageManualPrintButton;
        private System.Windows.Forms.ToolStripMenuItem voltageValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tutorialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trackingModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetCalibrationtoolStripMenuItem2;
    }
}

