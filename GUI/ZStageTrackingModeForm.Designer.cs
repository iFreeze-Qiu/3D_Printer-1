namespace GUI
{
    partial class ZStageTrackingModeForm
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
            this.feedbackRichTextBox = new System.Windows.Forms.RichTextBox();
            this.beginTrackingButton = new System.Windows.Forms.Button();
            this.stopTrackingButton = new System.Windows.Forms.Button();
            this.exportDataButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // feedbackRichTextBox
            // 
            this.feedbackRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.feedbackRichTextBox.Location = new System.Drawing.Point(13, 13);
            this.feedbackRichTextBox.Name = "feedbackRichTextBox";
            this.feedbackRichTextBox.Size = new System.Drawing.Size(398, 254);
            this.feedbackRichTextBox.TabIndex = 0;
            this.feedbackRichTextBox.Text = "";
            // 
            // beginTrackingButton
            // 
            this.beginTrackingButton.Location = new System.Drawing.Point(13, 273);
            this.beginTrackingButton.Name = "beginTrackingButton";
            this.beginTrackingButton.Size = new System.Drawing.Size(107, 23);
            this.beginTrackingButton.TabIndex = 1;
            this.beginTrackingButton.Text = "Begin Tracking";
            this.beginTrackingButton.UseVisualStyleBackColor = true;
            this.beginTrackingButton.Click += new System.EventHandler(this.beginTrackingButton_Click);
            // 
            // stopTrackingButton
            // 
            this.stopTrackingButton.Enabled = false;
            this.stopTrackingButton.Location = new System.Drawing.Point(126, 273);
            this.stopTrackingButton.Name = "stopTrackingButton";
            this.stopTrackingButton.Size = new System.Drawing.Size(88, 23);
            this.stopTrackingButton.TabIndex = 2;
            this.stopTrackingButton.Text = "Stop Tracking";
            this.stopTrackingButton.UseVisualStyleBackColor = true;
            this.stopTrackingButton.Click += new System.EventHandler(this.stopTrackingButton_Click);
            // 
            // exportDataButton
            // 
            this.exportDataButton.Location = new System.Drawing.Point(220, 273);
            this.exportDataButton.Name = "exportDataButton";
            this.exportDataButton.Size = new System.Drawing.Size(75, 23);
            this.exportDataButton.TabIndex = 3;
            this.exportDataButton.Text = "Export Data";
            this.exportDataButton.UseVisualStyleBackColor = true;
            this.exportDataButton.Click += new System.EventHandler(this.exportDataButton_Click);
            // 
            // ZStageTrackingModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 307);
            this.Controls.Add(this.exportDataButton);
            this.Controls.Add(this.stopTrackingButton);
            this.Controls.Add(this.beginTrackingButton);
            this.Controls.Add(this.feedbackRichTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ZStageTrackingModeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ZStageTrackingModeForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox feedbackRichTextBox;
        private System.Windows.Forms.Button beginTrackingButton;
        private System.Windows.Forms.Button stopTrackingButton;
        private System.Windows.Forms.Button exportDataButton;
    }
}