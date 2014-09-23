// Author: Robert Dacunto
// Project: 3D Printer Force Feedback Program
// File: ZStageSettingsPanelForm.cs
// Classes: ZStageSettingsPanelForm

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PrintingLogic;

namespace GUI
{
    /// <summary>
    /// ZStageSettingsPanelForm displays form for changing the
    /// settings for the Z-Stage, such as step size, speed, 
    /// dwell time, and offset. 
    /// </summary>
    public partial class ZStageSettingsPanelForm : Form
    {
        #region Field Region

        private Logic logic;
        private bool stopModeChanged;
        private bool returnModeChanged;

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor takes Logic class as parameter 
        /// to allow the form to connect to the logic
        /// layer and change settings in Z-Stage.
        /// </summary>
        /// <param name="logic"></param>
        public ZStageSettingsPanelForm(Logic logic)
        {
            InitializeComponent();

            this.logic = logic;

            this.Load += new EventHandler(ZStageSettingsPanelForm_Load);
            this.FormClosing += 
                new FormClosingEventHandler(ZStageSettingsPanelForm_FormClosing);
        }

        #endregion

        #region Event Handler Region

        /// <summary>
        /// Event fires when form loads. Sets the default text values
        /// of the form's text boxes to values received from the 
        /// logic class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZStageSettingsPanelForm_Load(object sender, EventArgs e)
        {
            stepSizeTextBox.Text = logic.StepSize.ToString();
            offsetTextBox.Text = logic.Offset.ToString();
            speedTextBox.Text = logic.Speed.ToString();
            dwellTimeTextBox.Text = logic.DwellTime.ToString();
            printReturnStepSizeTextBox.Text = logic.PrintingReturnStepSize.ToString();
            printReturnSpeedTextBox.Text = logic.PrintingReturnSpeed.ToString();
            printGoToTextBox.Text = logic.GoToPosition.ToString();
            printSlowDownTextBox.Text = logic.PrintingSlowDownStepSize.ToString();
            forceDetectedPositionTextBox.Text = logic.ForceDetectedPosition.ToString();

            stopModeChanged = false;
            returnModeChanged = false;

            if (logic.PrintStopMode == PrintStopMode.Position)
            {
                positionLimitedRadioButton.Checked = true;
                printSlowDownCheckBox.Enabled = false;
                printSlowDownTextBox.Enabled = false;

                printSlowDownCheckBox.Checked = false;
                logic.PrintSlowDown = false;
            }
            else
            {
                printSlowDownCheckBox.Checked = logic.PrintSlowDown;
                printSlowDownTextBox.Enabled = logic.PrintSlowDown;
            }

            if (logic.PrintReturnMode == PrintReturnMode.Step)
            {
                stepDownRadioButton.Checked = true;
                printSlowDownCheckBox.Enabled = true;
                printSlowDownTextBox.Enabled = true;
                returnToZeroCheckBox.Enabled = true;

                printSlowDownCheckBox.Checked = logic.PrintSlowDown;
            }
        }

        /// <summary>
        /// Event fires when form closes. If the user cancelled, the form is
        /// simply closed and any changes are discarded.
        /// Otherwise, the form has to validate the setting changes (ensure
        /// it is legal numeric values and within the bounds of the settings)
        /// before accepting the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZStageSettingsPanelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                return;

            try
            {
                logic.ValidateInputs(stepSizeTextBox.Text, speedTextBox.Text, offsetTextBox.Text,
                    dwellTimeTextBox.Text, printReturnStepSizeTextBox.Text,
                    printReturnSpeedTextBox.Text, printGoToTextBox.Text,
                    printSlowDownCheckBox.Checked, printSlowDownTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                e.Cancel = true;

                return;
            }

            if (stopModeChanged == true)
                logic.SetZStagePrintStopMode();

            if (returnModeChanged == true)
                logic.SetZStagePrintReturnMode();
        }

        #endregion

        #region Mode Method Region

        /// <summary>
        /// If the user changes the stop mode, set the
        /// change flag to true. Since slow down mode
        /// is valid under force limited mode, allow
        /// the slow down mode checkbox to be 
        /// checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forceLimitedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            stopModeChanged = true;

            if (forceLimitedRadioButton.Checked == true)
            {
                printSlowDownCheckBox.Enabled = true;
            }
        }

        /// <summary>
        /// If the user changes the stop mode,
        /// set the change flag to true. Since slow down
        /// mode is not valid under position limited mode,
        /// disable the slowdown mode check box and 
        /// text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void positionLimitedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            stopModeChanged = true;

            if (positionLimitedRadioButton.Checked == true)
            {
                printSlowDownCheckBox.Checked = false;
                printSlowDownCheckBox.Enabled = false;
                printSlowDownTextBox.Enabled = false;
                logic.PrintSlowDown = false;
            }
        }

        /// <summary>
        /// Tells the program that the user has changed the return
        /// mode of the Z-Stage. The logic class will handle
        /// the changing of the mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jumpDownRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            returnModeChanged = true;

            if (stepDownRadioButton.Checked == true)
                returnToZeroCheckBox.Enabled = true;
            else
                returnToZeroCheckBox.Enabled = false;
        }

        /// <summary>
        /// Tells the program that the user has changed the return 
        /// mode of the Z-Stage. The logic class will handle
        /// the changing of the mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stepDownRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            returnModeChanged = true;

            if (stepDownRadioButton.Checked == true)
                returnToZeroCheckBox.Enabled = true;
            else
                returnToZeroCheckBox.Enabled = false;
        }

        /// <summary>
        /// Enabling the slow down mode will enable the slow down text box
        /// for the user to input the slow down value; disabling it will
        /// disable the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printSlowDownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            printSlowDownTextBox.Enabled = printSlowDownCheckBox.Checked;
        }

        /// <summary>
        /// This feature can only be enabled if Step-Down mode is active. What
        /// it will do is return the Z-Stage to the default minimum position
        /// (0) in a step-down fashion, as opposed to returning to the go-to
        /// position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnToZeroCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                logic.PrintReturnToZero = returnToZeroCheckBox.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Z-Stage Print Error");
            }
        }

        #endregion

    }
}
