namespace GUI
{
    partial class ZStageFormCalibration
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
                cancellationSource.Dispose();
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
            this.startPositionTextBox = new System.Windows.Forms.TextBox();
            this.forceStopValueTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.forceStopValueLabel = new System.Windows.Forms.Label();
            this.calibrationFeedbackRichTextBox = new System.Windows.Forms.RichTextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.toolTipMasterControl = new System.Windows.Forms.ToolTip(this.components);
            this.stepSizeTextBox = new System.Windows.Forms.TextBox();
            this.speedTextBox = new System.Windows.Forms.TextBox();
            this.offsetTextBox = new System.Windows.Forms.TextBox();
            this.phaseSettingsButton = new System.Windows.Forms.Button();
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.speedLabel = new System.Windows.Forms.Label();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startPositionTextBox
            // 
            this.startPositionTextBox.Location = new System.Drawing.Point(88, 50);
            this.startPositionTextBox.Name = "startPositionTextBox";
            this.startPositionTextBox.Size = new System.Drawing.Size(100, 20);
            this.startPositionTextBox.TabIndex = 9;
            this.toolTipMasterControl.SetToolTip(this.startPositionTextBox, "Set the default starting position of the Z-Stage");
            // 
            // forceStopValueTextBox
            // 
            this.forceStopValueTextBox.Location = new System.Drawing.Point(89, 25);
            this.forceStopValueTextBox.Name = "forceStopValueTextBox";
            this.forceStopValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.forceStopValueTextBox.TabIndex = 8;
            this.toolTipMasterControl.SetToolTip(this.forceStopValueTextBox, "Set the stop value of the force sensor");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Start Position:";
            // 
            // calibrateButton
            // 
            this.calibrateButton.Enabled = false;
            this.calibrateButton.Location = new System.Drawing.Point(70, 212);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(119, 23);
            this.calibrateButton.TabIndex = 14;
            this.calibrateButton.Text = "Begin Calibration";
            this.toolTipMasterControl.SetToolTip(this.calibrateButton, "Begin the calibration process");
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // forceStopValueLabel
            // 
            this.forceStopValueLabel.AutoSize = true;
            this.forceStopValueLabel.Location = new System.Drawing.Point(20, 28);
            this.forceStopValueLabel.Name = "forceStopValueLabel";
            this.forceStopValueLabel.Size = new System.Drawing.Size(62, 13);
            this.forceStopValueLabel.TabIndex = 41;
            this.forceStopValueLabel.Text = "Stop Value:";
            // 
            // calibrationFeedbackRichTextBox
            // 
            this.calibrationFeedbackRichTextBox.Location = new System.Drawing.Point(262, 17);
            this.calibrationFeedbackRichTextBox.Name = "calibrationFeedbackRichTextBox";
            this.calibrationFeedbackRichTextBox.ReadOnly = true;
            this.calibrationFeedbackRichTextBox.Size = new System.Drawing.Size(192, 243);
            this.calibrationFeedbackRichTextBox.TabIndex = 38;
            this.calibrationFeedbackRichTextBox.Text = "";
            // 
            // cancelButton
            // 
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(128, 241);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "Cancel";
            this.toolTipMasterControl.SetToolTip(this.cancelButton, "Cancel calibration");
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(47, 241);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 15;
            this.okButton.Text = "OK";
            this.toolTipMasterControl.SetToolTip(this.okButton, "Accept the calibration values");
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // stepSizeTextBox
            // 
            this.stepSizeTextBox.Location = new System.Drawing.Point(89, 78);
            this.stepSizeTextBox.Name = "stepSizeTextBox";
            this.stepSizeTextBox.ReadOnly = true;
            this.stepSizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.stepSizeTextBox.TabIndex = 10;
            this.toolTipMasterControl.SetToolTip(this.stepSizeTextBox, "Set the number of steps the Z-Stage will take each move");
            // 
            // speedTextBox
            // 
            this.speedTextBox.Location = new System.Drawing.Point(89, 104);
            this.speedTextBox.Name = "speedTextBox";
            this.speedTextBox.ReadOnly = true;
            this.speedTextBox.Size = new System.Drawing.Size(100, 20);
            this.speedTextBox.TabIndex = 11;
            this.toolTipMasterControl.SetToolTip(this.speedTextBox, "Set how many moves the Z-Stage will make per second (input in milliseconds)");
            // 
            // offsetTextBox
            // 
            this.offsetTextBox.Location = new System.Drawing.Point(89, 129);
            this.offsetTextBox.Name = "offsetTextBox";
            this.offsetTextBox.ReadOnly = true;
            this.offsetTextBox.Size = new System.Drawing.Size(100, 20);
            this.offsetTextBox.TabIndex = 12;
            this.toolTipMasterControl.SetToolTip(this.offsetTextBox, "Set the position the Z-Stage will offset itself by after reaching stop value");
            // 
            // phaseSettingsButton
            // 
            this.phaseSettingsButton.Location = new System.Drawing.Point(70, 183);
            this.phaseSettingsButton.Name = "phaseSettingsButton";
            this.phaseSettingsButton.Size = new System.Drawing.Size(119, 23);
            this.phaseSettingsButton.TabIndex = 13;
            this.phaseSettingsButton.Text = "Phase Settings";
            this.toolTipMasterControl.SetToolTip(this.phaseSettingsButton, "Input the settings for the Z-Stage during each stage of calibration.");
            this.phaseSettingsButton.UseVisualStyleBackColor = true;
            this.phaseSettingsButton.Click += new System.EventHandler(this.phaseSettingsButton_Click);
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.Location = new System.Drawing.Point(27, 81);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(55, 13);
            this.stepSizeLabel.TabIndex = 39;
            this.stepSizeLabel.Text = "Step Size:";
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Location = new System.Drawing.Point(41, 107);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(41, 13);
            this.speedLabel.TabIndex = 40;
            this.speedLabel.Text = "Speed:";
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(44, 132);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(38, 13);
            this.offsetLabel.TabIndex = 44;
            this.offsetLabel.Text = "Offset:";
            // 
            // ZStageFormCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(469, 285);
            this.Controls.Add(this.phaseSettingsButton);
            this.Controls.Add(this.startPositionTextBox);
            this.Controls.Add(this.forceStopValueTextBox);
            this.Controls.Add(this.speedTextBox);
            this.Controls.Add(this.stepSizeTextBox);
            this.Controls.Add(this.offsetTextBox);
            this.Controls.Add(this.offsetLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.calibrateButton);
            this.Controls.Add(this.forceStopValueLabel);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.stepSizeLabel);
            this.Controls.Add(this.calibrationFeedbackRichTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.IsMdiContainer = true;
            this.Name = "ZStageFormCalibration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZStageFormCalibration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox startPositionTextBox;
        private System.Windows.Forms.TextBox forceStopValueTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Label forceStopValueLabel;
        private System.Windows.Forms.RichTextBox calibrationFeedbackRichTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ToolTip toolTipMasterControl;
        private System.Windows.Forms.Button phaseSettingsButton;
        private System.Windows.Forms.Label stepSizeLabel;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.TextBox stepSizeTextBox;
        private System.Windows.Forms.TextBox speedTextBox;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.TextBox offsetTextBox;
    }
}