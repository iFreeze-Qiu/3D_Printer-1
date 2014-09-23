namespace GUI
{
    partial class ZStageSettingsPanelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZStageSettingsPanelForm));
            this.dwellTimeTextBox = new System.Windows.Forms.TextBox();
            this.dwellTimeLabel = new System.Windows.Forms.Label();
            this.offsetTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.speedTextBox = new System.Windows.Forms.TextBox();
            this.stepSizeTextBox = new System.Windows.Forms.TextBox();
            this.speedLabel = new System.Windows.Forms.Label();
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.toolTipMasterControl = new System.Windows.Forms.ToolTip(this.components);
            this.jumpDownRadioButton = new System.Windows.Forms.RadioButton();
            this.stepDownRadioButton = new System.Windows.Forms.RadioButton();
            this.positionLimitedRadioButton = new System.Windows.Forms.RadioButton();
            this.forceLimitedRadioButton = new System.Windows.Forms.RadioButton();
            this.printGoToTextBox = new System.Windows.Forms.TextBox();
            this.printReturnSpeedTextBox = new System.Windows.Forms.TextBox();
            this.printReturnStepSizeTextBox = new System.Windows.Forms.TextBox();
            this.printSlowDownCheckBox = new System.Windows.Forms.CheckBox();
            this.printSlowDownTextBox = new System.Windows.Forms.TextBox();
            this.forceDetectedPositionTextBox = new System.Windows.Forms.TextBox();
            this.printSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.zStageReturnModePanel = new System.Windows.Forms.Panel();
            this.zStagePrintReturnModeLabel = new System.Windows.Forms.Label();
            this.printStopModePanel = new System.Windows.Forms.Panel();
            this.printStopModeLabel = new System.Windows.Forms.Label();
            this.printGoToLabel = new System.Windows.Forms.Label();
            this.printReturnSpeedLabel = new System.Windows.Forms.Label();
            this.printReturnStepSizeLabel = new System.Windows.Forms.Label();
            this.forceDetectedPositionLabel = new System.Windows.Forms.Label();
            this.returnToZeroCheckBox = new System.Windows.Forms.CheckBox();
            this.printSettingsGroupBox.SuspendLayout();
            this.zStageReturnModePanel.SuspendLayout();
            this.printStopModePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dwellTimeTextBox
            // 
            this.dwellTimeTextBox.Location = new System.Drawing.Point(132, 79);
            this.dwellTimeTextBox.Name = "dwellTimeTextBox";
            this.dwellTimeTextBox.Size = new System.Drawing.Size(79, 20);
            this.dwellTimeTextBox.TabIndex = 20;
            this.toolTipMasterControl.SetToolTip(this.dwellTimeTextBox, "Set dwell time for Z-Stage during print");
            // 
            // dwellTimeLabel
            // 
            this.dwellTimeLabel.AutoSize = true;
            this.dwellTimeLabel.Location = new System.Drawing.Point(64, 82);
            this.dwellTimeLabel.Name = "dwellTimeLabel";
            this.dwellTimeLabel.Size = new System.Drawing.Size(62, 13);
            this.dwellTimeLabel.TabIndex = 22;
            this.dwellTimeLabel.Text = "Dwell Time:";
            // 
            // offsetTextBox
            // 
            this.offsetTextBox.Location = new System.Drawing.Point(132, 54);
            this.offsetTextBox.Name = "offsetTextBox";
            this.offsetTextBox.Size = new System.Drawing.Size(79, 20);
            this.offsetTextBox.TabIndex = 19;
            this.toolTipMasterControl.SetToolTip(this.offsetTextBox, "Set the position the Z-Stage will offset itself by after reaching stop value");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 20;
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(88, 57);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(38, 13);
            this.offsetLabel.TabIndex = 19;
            this.offsetLabel.Text = "Offset:";
            // 
            // speedTextBox
            // 
            this.speedTextBox.Location = new System.Drawing.Point(132, 30);
            this.speedTextBox.Name = "speedTextBox";
            this.speedTextBox.Size = new System.Drawing.Size(80, 20);
            this.speedTextBox.TabIndex = 18;
            this.toolTipMasterControl.SetToolTip(this.speedTextBox, "Set how many moves the Z-Stage will take per second (input in milliseconds)");
            // 
            // stepSizeTextBox
            // 
            this.stepSizeTextBox.Location = new System.Drawing.Point(132, 8);
            this.stepSizeTextBox.Name = "stepSizeTextBox";
            this.stepSizeTextBox.Size = new System.Drawing.Size(80, 20);
            this.stepSizeTextBox.TabIndex = 17;
            this.toolTipMasterControl.SetToolTip(this.stepSizeTextBox, "Set how many steps the Z-Stage will take per move");
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Location = new System.Drawing.Point(85, 33);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(41, 13);
            this.speedLabel.TabIndex = 16;
            this.speedLabel.Text = "Speed:";
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.Location = new System.Drawing.Point(71, 11);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(55, 13);
            this.stepSizeLabel.TabIndex = 15;
            this.stepSizeLabel.Text = "Step Size:";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(132, 238);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 26;
            this.cancelButton.Text = "Cancel";
            this.toolTipMasterControl.SetToolTip(this.cancelButton, "Cancel any changes");
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(51, 238);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 25;
            this.okButton.Text = "OK";
            this.toolTipMasterControl.SetToolTip(this.okButton, "Accept new settings");
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // jumpDownRadioButton
            // 
            this.jumpDownRadioButton.AutoSize = true;
            this.jumpDownRadioButton.Checked = true;
            this.jumpDownRadioButton.Location = new System.Drawing.Point(6, 25);
            this.jumpDownRadioButton.Name = "jumpDownRadioButton";
            this.jumpDownRadioButton.Size = new System.Drawing.Size(81, 17);
            this.jumpDownRadioButton.TabIndex = 23;
            this.jumpDownRadioButton.TabStop = true;
            this.jumpDownRadioButton.Text = "Jump Down";
            this.toolTipMasterControl.SetToolTip(this.jumpDownRadioButton, "Jump down mode for printing will cause the Z-Stage to return to the go to positio" +
        "n immediately after it finishes dwelling.");
            this.jumpDownRadioButton.UseVisualStyleBackColor = true;
            this.jumpDownRadioButton.CheckedChanged += new System.EventHandler(this.jumpDownRadioButton_CheckedChanged);
            // 
            // stepDownRadioButton
            // 
            this.stepDownRadioButton.AutoSize = true;
            this.stepDownRadioButton.Location = new System.Drawing.Point(6, 48);
            this.stepDownRadioButton.Name = "stepDownRadioButton";
            this.stepDownRadioButton.Size = new System.Drawing.Size(78, 17);
            this.stepDownRadioButton.TabIndex = 24;
            this.stepDownRadioButton.Text = "Step Down";
            this.toolTipMasterControl.SetToolTip(this.stepDownRadioButton, "Step down mode for printing will cause the Z-Stage to return at its return step s" +
        "ize and return speed to the go to position once it finishes dwelling.");
            this.stepDownRadioButton.UseVisualStyleBackColor = true;
            this.stepDownRadioButton.CheckedChanged += new System.EventHandler(this.stepDownRadioButton_CheckedChanged);
            // 
            // positionLimitedRadioButton
            // 
            this.positionLimitedRadioButton.AutoSize = true;
            this.positionLimitedRadioButton.Location = new System.Drawing.Point(6, 45);
            this.positionLimitedRadioButton.Name = "positionLimitedRadioButton";
            this.positionLimitedRadioButton.Size = new System.Drawing.Size(98, 17);
            this.positionLimitedRadioButton.TabIndex = 22;
            this.positionLimitedRadioButton.Text = "Position-Limited";
            this.toolTipMasterControl.SetToolTip(this.positionLimitedRadioButton, "Position-limited mode for printing will cause the Z-Stage to stop printing when i" +
        "t reaches the current force detected position.");
            this.positionLimitedRadioButton.UseVisualStyleBackColor = true;
            this.positionLimitedRadioButton.CheckedChanged += new System.EventHandler(this.positionLimitedRadioButton_CheckedChanged);
            // 
            // forceLimitedRadioButton
            // 
            this.forceLimitedRadioButton.AutoSize = true;
            this.forceLimitedRadioButton.Checked = true;
            this.forceLimitedRadioButton.Location = new System.Drawing.Point(6, 22);
            this.forceLimitedRadioButton.Name = "forceLimitedRadioButton";
            this.forceLimitedRadioButton.Size = new System.Drawing.Size(88, 17);
            this.forceLimitedRadioButton.TabIndex = 21;
            this.forceLimitedRadioButton.TabStop = true;
            this.forceLimitedRadioButton.Text = "Force-Limited";
            this.toolTipMasterControl.SetToolTip(this.forceLimitedRadioButton, "Force-limited mode for printing will cause the Z-Stage to stop printing when it r" +
        "eaches the force stop value.");
            this.forceLimitedRadioButton.UseVisualStyleBackColor = true;
            this.forceLimitedRadioButton.CheckedChanged += new System.EventHandler(this.forceLimitedRadioButton_CheckedChanged);
            // 
            // printGoToTextBox
            // 
            this.printGoToTextBox.Location = new System.Drawing.Point(132, 105);
            this.printGoToTextBox.Name = "printGoToTextBox";
            this.printGoToTextBox.Size = new System.Drawing.Size(79, 20);
            this.printGoToTextBox.TabIndex = 28;
            this.toolTipMasterControl.SetToolTip(this.printGoToTextBox, resources.GetString("printGoToTextBox.ToolTip"));
            // 
            // printReturnSpeedTextBox
            // 
            this.printReturnSpeedTextBox.Location = new System.Drawing.Point(132, 151);
            this.printReturnSpeedTextBox.Name = "printReturnSpeedTextBox";
            this.printReturnSpeedTextBox.Size = new System.Drawing.Size(79, 20);
            this.printReturnSpeedTextBox.TabIndex = 30;
            this.toolTipMasterControl.SetToolTip(this.printReturnSpeedTextBox, "If step down mode is activated, the Z-Stage will return at this speed.");
            // 
            // printReturnStepSizeTextBox
            // 
            this.printReturnStepSizeTextBox.Location = new System.Drawing.Point(132, 128);
            this.printReturnStepSizeTextBox.Name = "printReturnStepSizeTextBox";
            this.printReturnStepSizeTextBox.Size = new System.Drawing.Size(79, 20);
            this.printReturnStepSizeTextBox.TabIndex = 32;
            this.toolTipMasterControl.SetToolTip(this.printReturnStepSizeTextBox, "If step down mode is activated, the Z-Stage will return at this step size.");
            // 
            // printSlowDownCheckBox
            // 
            this.printSlowDownCheckBox.AutoSize = true;
            this.printSlowDownCheckBox.Location = new System.Drawing.Point(40, 180);
            this.printSlowDownCheckBox.Name = "printSlowDownCheckBox";
            this.printSlowDownCheckBox.Size = new System.Drawing.Size(86, 17);
            this.printSlowDownCheckBox.TabIndex = 34;
            this.printSlowDownCheckBox.Text = "Slow Down?";
            this.toolTipMasterControl.SetToolTip(this.printSlowDownCheckBox, "If checked, the Z-Stage will proceed during printing at its normal step size but " +
        "then slow down and resume printing at the slow down step size once it reaches .0" +
        "005 less than the stop value.");
            this.printSlowDownCheckBox.UseVisualStyleBackColor = true;
            this.printSlowDownCheckBox.CheckedChanged += new System.EventHandler(this.printSlowDownCheckBox_CheckedChanged);
            // 
            // printSlowDownTextBox
            // 
            this.printSlowDownTextBox.Enabled = false;
            this.printSlowDownTextBox.Location = new System.Drawing.Point(132, 177);
            this.printSlowDownTextBox.Name = "printSlowDownTextBox";
            this.printSlowDownTextBox.Size = new System.Drawing.Size(80, 20);
            this.printSlowDownTextBox.TabIndex = 35;
            this.toolTipMasterControl.SetToolTip(this.printSlowDownTextBox, "The speed to slow down to once it reaches .0005 less than the stop value.");
            // 
            // forceDetectedPositionTextBox
            // 
            this.forceDetectedPositionTextBox.Enabled = false;
            this.forceDetectedPositionTextBox.Location = new System.Drawing.Point(132, 207);
            this.forceDetectedPositionTextBox.Name = "forceDetectedPositionTextBox";
            this.forceDetectedPositionTextBox.ReadOnly = true;
            this.forceDetectedPositionTextBox.Size = new System.Drawing.Size(80, 20);
            this.forceDetectedPositionTextBox.TabIndex = 37;
            this.toolTipMasterControl.SetToolTip(this.forceDetectedPositionTextBox, "The position the Z-Stage reached when it detected the force stop value.");
            // 
            // printSettingsGroupBox
            // 
            this.printSettingsGroupBox.Controls.Add(this.zStageReturnModePanel);
            this.printSettingsGroupBox.Controls.Add(this.printStopModePanel);
            this.printSettingsGroupBox.Location = new System.Drawing.Point(240, 11);
            this.printSettingsGroupBox.Name = "printSettingsGroupBox";
            this.printSettingsGroupBox.Size = new System.Drawing.Size(169, 227);
            this.printSettingsGroupBox.TabIndex = 24;
            this.printSettingsGroupBox.TabStop = false;
            this.printSettingsGroupBox.Text = "Print-Specific Settings";
            // 
            // zStageReturnModePanel
            // 
            this.zStageReturnModePanel.Controls.Add(this.returnToZeroCheckBox);
            this.zStageReturnModePanel.Controls.Add(this.jumpDownRadioButton);
            this.zStageReturnModePanel.Controls.Add(this.stepDownRadioButton);
            this.zStageReturnModePanel.Controls.Add(this.zStagePrintReturnModeLabel);
            this.zStageReturnModePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.zStageReturnModePanel.Location = new System.Drawing.Point(3, 95);
            this.zStageReturnModePanel.Name = "zStageReturnModePanel";
            this.zStageReturnModePanel.Size = new System.Drawing.Size(163, 129);
            this.zStageReturnModePanel.TabIndex = 1;
            // 
            // zStagePrintReturnModeLabel
            // 
            this.zStagePrintReturnModeLabel.AutoSize = true;
            this.zStagePrintReturnModeLabel.Location = new System.Drawing.Point(3, 9);
            this.zStagePrintReturnModeLabel.Name = "zStagePrintReturnModeLabel";
            this.zStagePrintReturnModeLabel.Size = new System.Drawing.Size(113, 13);
            this.zStagePrintReturnModeLabel.TabIndex = 3;
            this.zStagePrintReturnModeLabel.Text = "Z-Stage Return Mode:";
            // 
            // printStopModePanel
            // 
            this.printStopModePanel.Controls.Add(this.printStopModeLabel);
            this.printStopModePanel.Controls.Add(this.positionLimitedRadioButton);
            this.printStopModePanel.Controls.Add(this.forceLimitedRadioButton);
            this.printStopModePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.printStopModePanel.Location = new System.Drawing.Point(3, 16);
            this.printStopModePanel.Name = "printStopModePanel";
            this.printStopModePanel.Size = new System.Drawing.Size(163, 73);
            this.printStopModePanel.TabIndex = 0;
            // 
            // printStopModeLabel
            // 
            this.printStopModeLabel.AutoSize = true;
            this.printStopModeLabel.Location = new System.Drawing.Point(3, 6);
            this.printStopModeLabel.Name = "printStopModeLabel";
            this.printStopModeLabel.Size = new System.Drawing.Size(86, 13);
            this.printStopModeLabel.TabIndex = 2;
            this.printStopModeLabel.Text = "Print Stop Mode:";
            // 
            // printGoToLabel
            // 
            this.printGoToLabel.AutoSize = true;
            this.printGoToLabel.Location = new System.Drawing.Point(22, 106);
            this.printGoToLabel.Name = "printGoToLabel";
            this.printGoToLabel.Size = new System.Drawing.Size(104, 13);
            this.printGoToLabel.TabIndex = 27;
            this.printGoToLabel.Text = "Print Go To Position:";
            // 
            // printReturnSpeedLabel
            // 
            this.printReturnSpeedLabel.AutoSize = true;
            this.printReturnSpeedLabel.Location = new System.Drawing.Point(26, 154);
            this.printReturnSpeedLabel.Name = "printReturnSpeedLabel";
            this.printReturnSpeedLabel.Size = new System.Drawing.Size(100, 13);
            this.printReturnSpeedLabel.TabIndex = 29;
            this.printReturnSpeedLabel.Text = "Print Return Speed:";
            // 
            // printReturnStepSizeLabel
            // 
            this.printReturnStepSizeLabel.AutoSize = true;
            this.printReturnStepSizeLabel.Location = new System.Drawing.Point(12, 131);
            this.printReturnStepSizeLabel.Name = "printReturnStepSizeLabel";
            this.printReturnStepSizeLabel.Size = new System.Drawing.Size(114, 13);
            this.printReturnStepSizeLabel.TabIndex = 31;
            this.printReturnStepSizeLabel.Text = "Print Return Step Size:";
            // 
            // forceDetectedPositionLabel
            // 
            this.forceDetectedPositionLabel.AutoSize = true;
            this.forceDetectedPositionLabel.Location = new System.Drawing.Point(2, 210);
            this.forceDetectedPositionLabel.Name = "forceDetectedPositionLabel";
            this.forceDetectedPositionLabel.Size = new System.Drawing.Size(124, 13);
            this.forceDetectedPositionLabel.TabIndex = 36;
            this.forceDetectedPositionLabel.Text = "Force Detected Position:";
            // 
            // returnToZeroCheckBox
            // 
            this.returnToZeroCheckBox.AutoSize = true;
            this.returnToZeroCheckBox.Enabled = false;
            this.returnToZeroCheckBox.Location = new System.Drawing.Point(26, 71);
            this.returnToZeroCheckBox.Name = "returnToZeroCheckBox";
            this.returnToZeroCheckBox.Size = new System.Drawing.Size(105, 17);
            this.returnToZeroCheckBox.TabIndex = 25;
            this.returnToZeroCheckBox.Text = "Return To Zero?";
            this.returnToZeroCheckBox.UseVisualStyleBackColor = true;
            this.returnToZeroCheckBox.CheckedChanged += new System.EventHandler(this.returnToZeroCheckBox_CheckedChanged);
            // 
            // ZStageSettingsPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 286);
            this.ControlBox = false;
            this.Controls.Add(this.forceDetectedPositionTextBox);
            this.Controls.Add(this.forceDetectedPositionLabel);
            this.Controls.Add(this.printSlowDownTextBox);
            this.Controls.Add(this.printSlowDownCheckBox);
            this.Controls.Add(this.printReturnStepSizeTextBox);
            this.Controls.Add(this.printReturnStepSizeLabel);
            this.Controls.Add(this.printReturnSpeedTextBox);
            this.Controls.Add(this.printReturnSpeedLabel);
            this.Controls.Add(this.printGoToTextBox);
            this.Controls.Add(this.printGoToLabel);
            this.Controls.Add(this.printSettingsGroupBox);
            this.Controls.Add(this.dwellTimeTextBox);
            this.Controls.Add(this.dwellTimeLabel);
            this.Controls.Add(this.offsetTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.offsetLabel);
            this.Controls.Add(this.speedTextBox);
            this.Controls.Add(this.stepSizeTextBox);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.stepSizeLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ZStageSettingsPanelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Z-Stage Settings Panel";
            this.printSettingsGroupBox.ResumeLayout(false);
            this.zStageReturnModePanel.ResumeLayout(false);
            this.zStageReturnModePanel.PerformLayout();
            this.printStopModePanel.ResumeLayout(false);
            this.printStopModePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dwellTimeTextBox;
        private System.Windows.Forms.Label dwellTimeLabel;
        private System.Windows.Forms.TextBox offsetTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.TextBox speedTextBox;
        private System.Windows.Forms.TextBox stepSizeTextBox;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.Label stepSizeLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ToolTip toolTipMasterControl;
        private System.Windows.Forms.GroupBox printSettingsGroupBox;
        private System.Windows.Forms.RadioButton stepDownRadioButton;
        private System.Windows.Forms.RadioButton jumpDownRadioButton;
        private System.Windows.Forms.Label zStagePrintReturnModeLabel;
        private System.Windows.Forms.Label printStopModeLabel;
        private System.Windows.Forms.RadioButton positionLimitedRadioButton;
        private System.Windows.Forms.RadioButton forceLimitedRadioButton;
        private System.Windows.Forms.Panel zStageReturnModePanel;
        private System.Windows.Forms.Panel printStopModePanel;
        private System.Windows.Forms.Label printGoToLabel;
        private System.Windows.Forms.TextBox printGoToTextBox;
        private System.Windows.Forms.Label printReturnSpeedLabel;
        private System.Windows.Forms.TextBox printReturnSpeedTextBox;
        private System.Windows.Forms.Label printReturnStepSizeLabel;
        private System.Windows.Forms.TextBox printReturnStepSizeTextBox;
        private System.Windows.Forms.CheckBox printSlowDownCheckBox;
        private System.Windows.Forms.TextBox printSlowDownTextBox;
        private System.Windows.Forms.Label forceDetectedPositionLabel;
        private System.Windows.Forms.TextBox forceDetectedPositionTextBox;
        private System.Windows.Forms.CheckBox returnToZeroCheckBox;
    }
}