// Author: Robert Dacunto
// Project: 3D Printer Force Feedback Program
// File: ZStageFormCalibration.cs
// Classes: ZStageFormCalibration

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PrintingLogic;

namespace GUI
{
    /// <summary>
    /// This form handles calibration for the Z-Stage,
    /// setting up the initial values for the Z-Stage
    /// and finding the approximate position to use
    /// for the force stop value.
    /// </summary>
    public partial class ZStageFormCalibration : Form
    {
        #region Field Region

        private Logic logic;
        private CancellationTokenSource cancellationSource;

        #endregion

        #region Constructor Region

        /// <summary>
        /// The constructor takes a Logic object as a parameter
        /// to allow the calibration process to interact with
        /// the logic class.
        /// </summary>
        /// <param name="logic"></param>
        public ZStageFormCalibration(Logic logic)
        {
            InitializeComponent();

            this.logic = logic; 
            this.Load += new EventHandler(ZStageCalibrationForm_Load);
            this.FormClosing += new FormClosingEventHandler(ZStageCalibrationForm_Closing);

            // These events will wire the feedback box to receive updates
            // from the logic class, and to allow the logic class to 
            // change the text boxes across threads.
            this.logic.UpdateFeedback += logic_UpdateFeedback;
            this.logic.CalibrationTextboxUpdate += logic_CalibrationTextboxUpdate;
            this.logic.FinishCalibration += logic_FinishCalibration;

            logic.ResetZStageSettings();
        }

        #endregion

        #region Event Handler Region

        /// <summary>
        /// This event will fire when the Z-Stage finishes calibration. 
        /// It will simply enable the OK button to be clicked to accept
        /// the calibration values. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_FinishCalibration(object sender, EventArgs e)
        {
            if (logic.CalibrationFinished == true)
            {
                try
                {
                    if (!okButton.IsHandleCreated)
                    {
                        okButton.CreateControl();
                    }

                    okButton.Invoke(new Action(() =>
                        okButton.Enabled = true));
                }
                catch (Exception)
                {
                    okButton.Enabled = true;
                }
            }
            else if (logic.DidMaxHeightReachedDuringCalibration == true)
            {
                UnlockInputControlsAfterCancellation();
            }
            
        }

        /// <summary>
        /// This event fires when the form is closing. If the calibration
        /// process is still underway and the user attempts to accept
        /// the calibration values, it stops the form from closing until
        /// calibration is finished. If the user cancels the calibration
        /// process, cancel the calibration and discard the form.
        /// Unsubscribe the feedback update events to allow the 
        /// main form to re-subscribe and update its feedback box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZStageCalibrationForm_Closing(object sender, FormClosingEventArgs e)
        {
            if (logic.IsCalibrating == true)
            {
                if (DialogResult == DialogResult.OK)
                {
                    MessageBox.Show("Cannot accept calibration while calibration is " +
                        "in process. Please wait for calibration to finish, or " +
                        "press the cancel button.", "Calibration Error");
                    e.Cancel = true;

                    return;
                }
            }

            if (DialogResult == DialogResult.Cancel && logic.CalibrationFinished == true)
            {
                logic.CalibrationFinished = false;
            }

            this.logic.UpdateFeedback -= logic_UpdateFeedback;
            this.logic.CalibrationTextboxUpdate -= logic_CalibrationTextboxUpdate;
        }

        /// <summary>
        /// This event fires when the form is loading. 
        /// Set the text values of each textbox on the form
        /// to the default values received from the logic class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZStageCalibrationForm_Load(object sender, EventArgs e)
        {
            calibrationFeedbackRichTextBox.Text += "Z-Stage Calibration\n";

            this.logic_CalibrationTextboxUpdate(this, new CalibrationTextboxEventArgs(logic.StepSize, logic.Speed,
                logic.StartPosition, logic.Offset, logic.StopValue));

            forceStopValueTextBox.ReadOnly = false;
            startPositionTextBox.ReadOnly = false;
        }

        /// <summary>
        /// This event will update the feedback textbox of the form with data
        /// from the logic class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_UpdateFeedback(object sender, UpdateFeedbackEventArgs e)
        {
            // Use Invoke to allow for thread-safe calls.
            calibrationFeedbackRichTextBox.Invoke(new Action(() =>
                calibrationFeedbackRichTextBox.Text += e.Result));

            // Move the caret of the feedback box to the end of the feedback box
            calibrationFeedbackRichTextBox.Invoke(new Action(() =>
                {
                    calibrationFeedbackRichTextBox.SelectionStart = calibrationFeedbackRichTextBox.TextLength;

                    calibrationFeedbackRichTextBox.ScrollToCaret();
                }
                ));
        }

        /// <summary>
        /// Update each textbox on the form with values received from the logic class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_CalibrationTextboxUpdate(object sender, CalibrationTextboxEventArgs e)
        {
            this.stepSizeTextBox.Invoke(new Action(() =>
                this.stepSizeTextBox.Text = e.StepSize.ToString()));

            this.speedTextBox.Invoke(new Action(() =>
                this.speedTextBox.Text = e.Speed.ToString()));

            this.forceStopValueTextBox.Invoke(new Action(() =>
                this.forceStopValueTextBox.Text = e.StopValue.ToString()));

            this.startPositionTextBox.Invoke(new Action(() =>
                this.startPositionTextBox.Text = e.StartPosition.ToString()));

            this.offsetTextBox.Invoke(new Action(() =>
                this.offsetTextBox.Text = e.Offset.ToString()));
        }

        #endregion

        #region General Method Region

        /// <summary>
        /// Disable each textbox on the form to prevent the user
        /// from modifying their values during calibration.
        /// </summary>
        private void LockInputControls()
        {
            this.forceStopValueTextBox.Invoke(new Action(() =>
                this.forceStopValueTextBox.ReadOnly = true));

            this.startPositionTextBox.Invoke(new Action(() =>
                this.startPositionTextBox.ReadOnly = true));
        }

        /// <summary>
        /// Returns the controls to their normal state after
        /// a calibration cancel event.
        /// </summary>
        private void UnlockInputControlsAfterCancellation()
        {
            this.forceStopValueTextBox.Invoke(new Action(() =>
                this.forceStopValueTextBox.ReadOnly = false));

            this.startPositionTextBox.Invoke(new Action(() =>
                this.startPositionTextBox.ReadOnly = false));

            this.cancelButton.Invoke(new Action(() =>
                this.cancelButton.Enabled = false));

            this.phaseSettingsButton.Invoke(new Action(() =>
                this.phaseSettingsButton.Enabled = true));

            this.calibrateButton.Invoke(new Action(() =>
                this.calibrateButton.Enabled = false));
        }

        #endregion

        #region Calibration Method Region

        /// <summary>
        /// This function will open the phase settings form
        /// to input the settings for the Z-Stage for each
        /// stage of calibration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void phaseSettingsButton_Click(object sender, EventArgs e)
        {
            using (ZStageFormCalibrationPhaseControl phaseControl =
                new ZStageFormCalibrationPhaseControl(logic))
            {
                DialogResult result = phaseControl.ShowDialog();

                if (result == DialogResult.OK)
                {
                    calibrateButton.Enabled = true;
                    phaseSettingsButton.Enabled = false;
                }
            }
        }

        /// <summary>
        /// This button will begin the calibration process if
        /// each value in each textbox is valid. The form passes
        /// control over calibration to the logic class, which handles
        /// all threading and calibration phases.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void calibrateButton_Click(object sender, EventArgs e)
        {
            if (logic.CalibrationFinished)
            {
                MessageBox.Show("Calibration already finished, press OK to accept" +
                    " these values, or CANCEL to perform process again.",
                    "Calibration Finished");

                return;
            }

            if (logic.IsCalibrating)
            {
                MessageBox.Show("Calibration is already underway, please wait.",
                    "Calibration In Process");

                return;
            }

            try
            {
                logic.ValidateInputs(forceStopValueTextBox.Text,
                    startPositionTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Input Validation Error");
                return;
            }

            LockInputControls();

            cancellationSource = new CancellationTokenSource();

            cancelButton.Enabled = true;

            try
            {
                await logic.Calibrate(cancellationSource);
            }
            catch(OperationCanceledException)
            {
                UnlockInputControlsAfterCancellation();
                logic_UpdateFeedback(this, new UpdateFeedbackEventArgs("\nCalibration Cancelled.\n"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calibration Error");
            }
        }

        /// <summary>
        /// Cancels the calibration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            cancellationSource.Cancel();
            cancelButton.Enabled = false;
        }

        #endregion

    }
}
