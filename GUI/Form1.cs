// Author: Robert Dacunto
// Project: 3D Printer Force Feedback Program
// File: Form1.cs
// Classes: FormMain

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Media;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using PrintingLogic;

namespace GUI
{
    /// <summary>
    /// FormMain class is the main GUI form for the project. 
    /// Controls to connect to force sensor, Z-Stage, voltmeter 
    /// and XYZ-Stage; controls to move Z-Stage manually or
    /// automatically; controls to position XYZ-Stage;
    /// controls to enable automatic printing via voltmeter.
    /// </summary>
    public partial class FormMain : Form
    {
        #region Field Region

        private Logic logic;
        private XYZStageControlPanelForm xyzStageForm;

        private bool userCancelledPrint;
        private bool manualPrint;
        private int printAmount;

        private SoundPlayer soundPlayer =
            new SoundPlayer(global::GUI.Properties.Resources.tada);

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for FormMain
        /// </summary>
        public FormMain()
        {
            #region Component Region

            InitializeComponent();

            #endregion

            #region Class Instantiation Region

            // Logic class handles all interaction with hardware
            logic = new Logic();

            // XYZStageControlPanelForm is a child form of FormMain
            // that gives user control over XYZ-Stage positioning
            xyzStageForm = new XYZStageControlPanelForm(logic);

            #endregion

            #region Form Event Wiring Region

            this.FormClosing += FormMain_FormClosing;
            this.Load += FormMain_Load;

            // These events handle feedback from logic class
            // to user interface
            logic.UpdateFeedback += logic_UpdateFeedback;
            logic.SensorUpdated += sensorUpdated;
            logic.PositionUpdated += positionUpdated;
            logic.VoltageUpdated += voltageUpdated;
            logic.ForceDetectedSound += forceDetectedPlaySound;

            sensorUpdateTimer.Tick += sensorUpdateTimer_Tick;

            #endregion

            #region Background Worker Event Wiring Region

            // zStagePrintBackgroundWorker handles the processing of 
            // the Z-Stage printing process
            zStagePrintBackgroundWorker.DoWork += new DoWorkEventHandler(zStagePrintBackgroundWorker_DoWork);
            zStagePrintBackgroundWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(zStagePrintBackgroundWorker_RunWorkerCompleted);
            zStagePrintBackgroundWorker.WorkerSupportsCancellation = true;

            #endregion
        }

        #endregion

        #region Event Handler Region

        /// <summary>
        /// Event will fire upon loading of FormMain. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormMain_Load(object sender, EventArgs e)
        {
            // Fill the force sensor stop value text box with default value
            forceSensorStopValueTextBox.Text = logic.StopValue.ToString();
        }

        /// <summary>
        /// Form will fire upon closing of FormMain.
        /// Handles the closing of all active connections
        /// to PC to ensure proper disconnect.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool canClose = true;

            sensorUpdateTimer.Enabled = false;

            // Call KillConnections() to ensure all connected hardware 
            // are properly and cleanly disconnected.
            logic.KillConnections();

            if (canClose != true)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Updates the feedbackRichTextbox with updates
        /// from logic class, giving user real-time event
        /// driven information about status of active
        /// connections and processes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_UpdateFeedback(object sender, UpdateFeedbackEventArgs e)
        {
            // Use Invoke to safely update feedbackbox from across threads
            feedbackRichTextBox.Invoke(new Action(() =>
                feedbackRichTextBox.Text += e.Result + "\n"));

            // Using thread-safe invoke, move the caret to the bottom of the
            // feedback box.
            feedbackRichTextBox.Invoke(new Action(() =>
               {
                   feedbackRichTextBox.SelectionStart = feedbackRichTextBox.TextLength;
                   feedbackRichTextBox.ScrollToCaret();
               }));
        }

        /// <summary>
        /// Updates the sensor display label with real-time sensor value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sensorUpdated(object sender, UpdateFeedbackEventArgs e)
        {
            // Use Invoke to ensure thread-safety updating of the control.
            forceSensorValueLabel.Invoke(new Action(() =>
                forceSensorValueLabel.Text = e.Result));
        }

        /// <summary>
        /// Updates the Z-Stage position display label with real-time position
        /// value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void positionUpdated(object sender, UpdateFeedbackEventArgs e)
        {
            // Use Invoke to ensure thread-safety updating of the control.
            zStagePositionUpdateLabel.Invoke(new Action(() =>
               zStagePositionUpdateLabel.Text = e.Result));
        }

        /// <summary>
        /// Updates the voltage display label with real-time voltage value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void voltageUpdated(object sender, UpdateFeedbackEventArgs e)
        {
            // Use Invoke to ensure thread-safety updating of the control.
            voltmeterReadoutLabel.Invoke(new Action(() =>
                voltmeterReadoutLabel.Text = e.Result));
        }

        /// <summary>
        /// On each "tick" of the sensorUpdateTimer, update the sensor
        /// label with real-time sensor value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sensorUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!zStagePrintBackgroundWorker.IsBusy)
            {
                sensorUpdated(this,
                    new UpdateFeedbackEventArgs(logic.SensorUpdate().ToString()));
            }
        }

        /// <summary>
        /// Event to fire when the selected voltage value is detected 
        /// by voltmeter. When detected, stop the sensorUpdateTimer to
        /// ensure no thread-conflicts with logic.SensorUpdate() and
        /// begin the Z-Stage printing process via the backgroundworker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_VoltageDetected(object sender, EventArgs e)
        {
            logic.IsVoltageDetected = true;

            if (logic.Printing == false)
            {
                logic_UpdateFeedback(this,
                new UpdateFeedbackEventArgs("\nVoltage Detected, beginning print operation\n"));

                zStagePrintBackgroundWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Plays a sound notification when the force stop value has been 
        /// detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forceDetectedPlaySound(object sender, EventArgs e)
        {
            soundPlayer.Play();
        }

        #endregion

        #region Z-Stage BackgroundWorker Region

        /// <summary>
        /// Begins Z-Stage printing process. Disable the Z-Stage reset and Up/Down
        /// move buttons, enable the Cancel Print button. Logic class will handle
        /// execution of printing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStagePrintBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            zStageCancelPrintButton.Invoke(new Action(() =>
                zStageCancelPrintButton.Enabled = true));

            zStageResetButton.Invoke(new Action(() =>
                zStageResetButton.Enabled = false));

            zStageMoveUpButton.Invoke(new Action(() =>
                zStageMoveUpButton.Enabled = false));

            zStageMoveDownButton.Invoke(new Action(() =>
                zStageMoveDownButton.Enabled = false));

            zStageManualPrintButton.Invoke(new Action(() =>
                zStageManualPrintButton.Enabled = false));

            menuStrip1.Invoke(new Action(() =>
                zStageToolStripMenuItem.Enabled = false));

            menuStrip1.Invoke(new Action(() =>
                forceSensorToolStripMenuItem.Enabled = false));

            menuStrip1.Invoke(new Action(() =>
                voltmeterToolStripMenuItem.Enabled = false));

            menuStrip1.Invoke(new Action(() =>
                xyzStageToolStripMenuItem.Enabled = false));

            logic.Print(worker, e);
        }

        /// <summary>
        /// Executes when Z-Stage print background process reaches force sensor stop value
        /// or when user manually cancels print process. If user cancelled, restore
        /// controls to normal. If not, finalize printing process in logic.FinishPrint().
        /// Regardless, resume sensor updating and reset voltage detection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void zStagePrintBackgroundWorker_RunWorkerCompleted(object sender, 
            RunWorkerCompletedEventArgs e)
        {
            if (userCancelledPrint == true)
            {
                return;
            }
            else
            {
                await logic.FinishPrint();
            }

            zStageCancelPrintButton.Invoke(new Action(() =>
                zStageCancelPrintButton.Enabled = false));

            zStageMoveUpButton.Invoke(new Action(() =>
                zStageMoveUpButton.Enabled = true));

            zStageMoveDownButton.Invoke(new Action(() =>
                zStageMoveDownButton.Enabled = true));

            zStageResetButton.Invoke(new Action(() =>
                zStageResetButton.Enabled = true));

            zStageManualPrintButton.Invoke(new Action(() =>
                zStageManualPrintButton.Enabled = true));

            menuStrip1.Invoke(new Action(() =>
                zStageToolStripMenuItem.Enabled = true));

            menuStrip1.Invoke(new Action(() =>
                forceSensorToolStripMenuItem.Enabled = true));

            menuStrip1.Invoke(new Action(() =>
                voltmeterToolStripMenuItem.Enabled = true));

            menuStrip1.Invoke(new Action(() =>
                xyzStageToolStripMenuItem.Enabled = true));

            if (logic.IsVoltageDetected == true)
                logic.IsVoltageDetected = false;

            printAmount--;

            zStagePositionUpdateLabel.Invoke(new Action(() =>
                zStagePositionUpdateLabel.Text =
                logic.GetZStagePosition()));

            // Repeat printing if manual print mode is on and
            // there's still prints left to finish

            if (manualPrint == true && printAmount > 0)
            { 
                zStagePrintBackgroundWorker.RunWorkerAsync();
            }
            else if (manualPrint == true && printAmount <= 0)
                manualPrint = false;
        }

        #endregion

        #region Force Sensor Menu Item Event Handler Region

        /// <summary>
        /// Turns the force sensor on. It will query the user for the serial number 
        /// of the force sensor, and then attempt connection. If successful, 
        /// allow for Z-Stage connectivity and begin sensorUpdateTimer to display
        /// sensor value to form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onForceSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (onForceSensorToolStripMenuItem.Checked == true)
                return;

            string serialNumber = Interaction.InputBox("Input serial number: ", "Serial Number Input",
                logic.DefaultSerialNumber);

            try
            {
                if (logic.ConnectForceSensor(serialNumber) == true)
                {
                    onForceSensorToolStripMenuItem.Checked = true;
                    offForceSensorToolStripMenuItem.Checked = false;

                    zStageToolStripMenuItem.Enabled = true;
                    forceSensorGroupBox.Visible = true;

                    grossToolStripMenuItem.Enabled = true;
                    tareToolStripMenuItem.Enabled = true;

                    sensorUpdateTimer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sensor Connection Error");
            }
        }

        /// <summary>
        /// Turns the force sensor off. If the Z-Stage is still connected, it will
        /// prevent force sensor disconnection until Z-Stage is first disconnected.
        /// Once disconnected, it will stop the sensorUpdateTimer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void offForceSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (offForceSensorToolStripMenuItem.Checked == true)
                return;

            if (onZStageToolStripMenuItem1.Checked == true)
            {
                MessageBox.Show("You must disconnect the Z-Stage before you can " +
                    "disconnect the Force Sensor.", "Sensor Connection Error");
                return;
            }

            try
            {
                if (logic.DisconnectForceSensor() == true)
                {
                    offForceSensorToolStripMenuItem.Checked = true;
                    onForceSensorToolStripMenuItem.Checked = false;

                    zStageToolStripMenuItem.Enabled = false;
                    forceSensorGroupBox.Visible = false;

                    grossToolStripMenuItem.Enabled = false;
                    tareToolStripMenuItem.Enabled = false;

                    sensorUpdateTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sensor Connection Error");
            }
        }

        /// <summary>
        /// Will enable the gross setting for the force sensor. When set to gross,
        /// the force sensor will display its current reading as it is determined by 
        /// the force sensor. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grossToolStripMenuItem.Checked)
                return;

            logic.SensorSetGross();

            grossToolStripMenuItem.Checked = true;
            tareToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// WIll enable the tare setting for the force sensor. When set to tare,
        /// it will set the current gross value to 0, and any changes in sensor
        /// value will be compared to that tared value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tareToolStripMenuItem.Checked)
                return;

            logic.SensorSetTare();
            tareToolStripMenuItem.Checked = true;
            grossToolStripMenuItem.Checked = false;
        }

        #endregion

        #region Z-Stage Menu Item Event Handler Region

        /// <summary>
        /// Will turn the Z-Stage on, but only if the force sensor is 
        /// connected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onZStageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (onZStageToolStripMenuItem1.Checked == true)
                return;

            try
            {
                if (logic.ConnectZStage() == true)
                {
                    onZStageToolStripMenuItem1.Checked = true;
                    offZStageToolStripMenuItem1.Checked = false;

                    zStageCalibrationToolStripMenuItem.Enabled = true;

                    zStageGroupBox.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Z-Stage Connection Error");
            }
        }

        /// <summary>
        /// Turns the Z-Stage off 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void offZStageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (offZStageToolStripMenuItem1.Checked == true)
                return;

            try
            {
                if (logic.DisconnectZStage() == true)
                {
                    offZStageToolStripMenuItem1.Checked = true;
                    onZStageToolStripMenuItem1.Checked = false;
                    zStageCalibrationToolStripMenuItem.Enabled = false;
                    zStageSettingsToolStripMenuItem.Enabled = false;

                    logic.ResetZStageSettings();

                    zStageGroupBox.Visible = false;

                    if (onVoltmeterToolStripMenuItem.Checked == true)
                    {
                        if (logic.DisconnectVoltmeter() == true)
                        {
                            DisconnectVoltmeterControls();
                        }
                    }

                    voltmeterToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Z-Stage Connection Error");
            }
        }

        /// <summary>
        /// Opens the calibration panel for the Z-Stage. Unsubscribe the UpdateFeedback
        /// event to allow the calibration form's feedback panel to subscribe, and stop
        /// the sensorUpdateTimer as the calibration function will manually update the 
        /// sensor. If calibration is accepted, enable the voltmeter to be connected.
        /// Regardless of outcome of calibration, re-subscribe the UpdateFeedback event
        /// and resume the sensorUpdateTimer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logic.UpdateFeedback -= logic_UpdateFeedback;
            sensorUpdateTimer.Stop();

            using (ZStageFormCalibration frmCalibration = new ZStageFormCalibration(logic))
            {
                DialogResult result = frmCalibration.ShowDialog();

                if (result == DialogResult.OK)
                {
                    zStageIgnoreSensorToolStripMenuItem.Enabled = true;
                    zStageSettingsToolStripMenuItem.Enabled = true;
                    voltmeterToolStripMenuItem.Enabled = true;

                    MessageBox.Show("Calibration finished. You may begin printing process now, or " +
                        "you can alter settings in the Z-Stage Settings Panel.", "Z-Stage Calibration");

                    logic.ZStageReset();

                    zStageManualPrintButton.Enabled = true;
                    zStageCalibrationToolStripMenuItem.Enabled = false;
                    resetCalibrationtoolStripMenuItem2.Enabled = true;

                    forceSensorStopValueTextBox.Text = logic.StopValue.ToString();
                }
            }

            logic.UpdateFeedback += logic_UpdateFeedback;
            sensorUpdateTimer.Start();
        }

        /// <summary>
        /// Resets Z-Stage calibration data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetCalibrationtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (logic.CalibrationFinished == false)
                return;

            logic.CalibrationFinished = false;
            resetCalibrationtoolStripMenuItem2.Enabled = false;
            zStageCalibrationToolStripMenuItem.Enabled = true;

            logic.ResetZStageSettings();

            MessageBox.Show("Calibration data reset.");
        }

        /// <summary>
        /// Opens the Z-Stage settings panel. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageSettingsPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ZStageSettingsPanelForm frmSettings =
                new ZStageSettingsPanelForm(logic))
            {
                DialogResult result = frmSettings.ShowDialog();

                if (result == DialogResult.OK)
                {
                    MessageBox.Show("Settings changed.", "Z-Stage Settings");
                }
            }
        }

        /// <summary>
        /// This event will turn the ignore sensor variable on or off - if on, 
        /// it will force the Z-Stage to ignore the force sensor stop value,
        /// allowing it to continue moving regardless of sensor value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageIgnoreSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (zStageIgnoreSensorToolStripMenuItem.Checked == true)
            {
                logic.IgnoreSensor = false;
                zStageIgnoreSensorToolStripMenuItem.Checked = false;
                MessageBox.Show("Z-Stage will no longer ignore sensor.", "Z-Stage Settings");
                return;
            }

            logic.IgnoreSensor = true;
            zStageIgnoreSensorToolStripMenuItem.Checked = true;

            MessageBox.Show("Z-Stage will now ignore sensor.", "Z-Stage Settings");
        }

        /// <summary>
        /// This function will show the user the list of positions the Z-Stage
        /// detected the force value at during calibration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void positionsFoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logic.CalibrationFinished == false)
            {
                MessageBox.Show("You have not finished Z-Stage Calibration",
                    "Z-Stage Calibration");

                return;
            }

            MessageBox.Show("Calibration detected the force value at these positions:\n" +
                logic.CalibrationPositions, "Z-Stage Calibration Positions");
        }

        /// <summary>
        /// This function will open the Z-Stage tracking mode 
        /// form. This mode will track the position of the Z-Stage
        /// at the pre-determined force value for as long as it is 
        /// active.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackingModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ZStageTrackingModeForm trackingModeForm =
                new ZStageTrackingModeForm(logic))
            {
                logic.VoltageDetected -= logic_VoltageDetected;
                logic.UpdateFeedback -= logic_UpdateFeedback;
                sensorUpdateTimer.Stop();

                trackingModeForm.ShowDialog();

                logic.VoltageDetected += logic_VoltageDetected;
                logic.UpdateFeedback += logic_UpdateFeedback;
                sensorUpdateTimer.Start();
            }
        }

        #endregion

        #region XYZ Stage Menu Item Event Handler Region

        /// <summary>
        /// This event will open the control panel for the XYZ-Stage. The
        /// XYZ-Stage is independent of the Z-Stage, force sensor and
        /// voltmeter, and thus does not depend on them. We instantiate the
        /// XYZStageForm in FormMain constructor so as to keep XYZ-Stage
        /// settings and position the same throughout lifetime of program
        /// execution, allowing user to open and close the XYZ-Stage panel
        /// without fear of losing data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.logic.UpdateFeedback -= logic_UpdateFeedback;

            xyzStageForm.ShowDialog();

            this.logic.UpdateFeedback += logic_UpdateFeedback;
        }

        #endregion

        #region Voltmeter Menu Item Event Handler Region

        /// <summary>
        /// Turns the voltmeter on and subscribe to the VoltageDetected event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onVoltmeterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (onVoltmeterToolStripMenuItem.Checked == true)
                return;

            if (logic.ConnectVoltmeter() == true)
            {
                offVoltmeterToolStripMenuItem.Checked = false;
                onVoltmeterToolStripMenuItem.Checked = true;
                voltmeterReadoutLabel.Visible = true;
                voltageValueToolStripMenuItem.Enabled = true;
                trackingModeToolStripMenuItem.Enabled = true;

                logic.VoltageDetected += logic_VoltageDetected;
            }
            else
            {
                MessageBox.Show("Failed to connect to voltmeter. Check connection cables.",
                    "Voltmeter");
            }
        }

        /// <summary>
        /// Disconnect the voltmeter and unsubscribe from the VoltageDetected event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void offVoltmeterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (offVoltmeterToolStripMenuItem.Checked == true)
                return;

            if (logic.DisconnectVoltmeter() == true)
            {
                DisconnectVoltmeterControls();
            }
            else
            {
                MessageBox.Show("Failed to disconnect from voltmeter.", "Voltmeter");
            }
        }

        /// <summary>
        /// This function will disable all the controls related
        /// to the voltmeter once it has been disconnected.
        /// </summary>
        private void DisconnectVoltmeterControls()
        {
            onVoltmeterToolStripMenuItem.Checked = false;
            offVoltmeterToolStripMenuItem.Checked = true;
            voltmeterReadoutLabel.Visible = false;
            voltageValueToolStripMenuItem.Enabled = false;
            trackingModeToolStripMenuItem.Enabled = false;
            logic.VoltageDetected -= logic_VoltageDetected;
        }

        /// <summary>
        /// This function changes the voltage detection value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void voltageValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string voltageValue =
                Interaction.InputBox("Change the voltage change value", 
                "Voltage Change Detection",
                logic.VoltageChangeValue.ToString());

            try
            {
                double voltageNumericValue = Double.Parse(voltageValue);
                logic.VoltageChangeValue = voltageNumericValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Voltage Change Error");

                return;
            }

            MessageBox.Show("Voltage change value has been changed to "
                + logic.VoltageChangeValue.ToString(), "Voltage Value");
        }

        #endregion

        #region Z-Stage Move Method Region

        /// <summary>
        /// One click will move the Z-Stage up 
        /// to a position stepSize + current position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageMoveUpButton_Click(object sender, EventArgs e)
        {
            sensorUpdateTimer.Stop();

            logic.MoveZStage(ZStageMoveDirections.Up);

            sensorUpdateTimer.Start();
        }

        /// <summary>
        /// One click will move the Z-Stage down to a position 
        /// current position - stepSize.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageMoveDownButton_Click(object sender, EventArgs e)
        {
            logic.MoveZStage(ZStageMoveDirections.Down);
        }


        /// <summary>
        /// Resets the Z-Stage's position to the default minimum height.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageResetButton_Click(object sender, EventArgs e)
        {
            logic.ZStageReset();
        }

        /// <summary>
        /// Will automatically move the Z-Stage to the position input by the user,
        /// moving instantly to the designated position, regardless of Z-Stage speed 
        /// settings or force sensor stop value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageMoveToPositionInputButton_Click(object sender, EventArgs e)
        {
            double position;

            try
            {
                position = Double.Parse(zStageMoveToPositionTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid format for position. Use a number.", 
                    "Z-Stage Move Error");
                return;
            }

            try
            {
                logic.MoveZStageToPosition(position);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Z-Stage Move Error");
            }
        }

        /// <summary>
        /// This event will cancel the Z-Stage print process if it is currently
        /// undergoing a print.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageCancelPrintButton_Click(object sender, EventArgs e)
        {
            if (zStagePrintBackgroundWorker.IsBusy == true)
            {
                zStagePrintBackgroundWorker.CancelAsync();
                userCancelledPrint = true;
            }

            userCancelledPrint = false;

            zStageToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// This function will manually begin a print operation. It will
        /// query the user for how many times they wish to execute the 
        /// print operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zStageManualPrintButton_Click(object sender, EventArgs e)
        {
            string printAmountText = Interaction.InputBox("How many prints to execute: ",
                "Z-Stage Print Execution", "1");

            try
            {
                printAmount = Int32.Parse(printAmountText);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid print amount. Use a numeric value.",
                    "Print Error");

                return;
            }

            if (logic.Printing == false)
            {
                logic_UpdateFeedback(this,
                    new UpdateFeedbackEventArgs("\nManual printing mode engaged...\n" +
                        "Beginning print operation. Number of prints: " + 
                        printAmount.ToString() + "\n"));

                manualPrint = true;

                zStageToolStripMenuItem.Enabled = false;

                zStagePrintBackgroundWorker.RunWorkerAsync();
            }
        }

        #endregion

        #region Force Sensor Method Region

        /// <summary>
        /// This event will change the stop value for the force sensor to a value
        /// determined by the user. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forceSensorStopValueInputButton_Click(object sender, EventArgs e)
        {
            DialogResult result =
                MessageBox.Show("Are you sure you want to change stop value?",
                "Sensor Settings",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                forceSensorStopValueTextBox.Text = logic.StopValue.ToString();
                return;
            }

            try
            {
                if (logic.ChangeStopValue(forceSensorStopValueTextBox.Text) == true)
                {
                    MessageBox.Show("Stop value has been changed to " + forceSensorStopValueTextBox.Text,
                        "Sensor Settings");
                }
                else
                {
                    MessageBox.Show("Invalid stop value. Ensure stop value is between " +
                        Logic.MinForceSensorStopValue.ToString() + " and " +
                        Logic.MaxForceSensorStopValue.ToString(),
                        "Sensor Settings");

                    forceSensorStopValueTextBox.Text = logic.StopValue.ToString();
                }
            }
            catch (Exception ex)
            {
                forceSensorStopValueTextBox.Text = logic.StopValue.ToString();
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Miscellaneous Menu Item Event Handler Region

        /// <summary>
        /// This will open the About menu dialog box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("3D Printer Force Feedback System\n\nProgrammer: " +
                "Robert Dacunto\nProject Head: Zhantong Mao\n" +
                "Project Supervisor: Dr. Alan Lyons",
                "About");
        }

        /// <summary>
        /// This will load the tutorial for the program. The 
        /// tutorial is written in HTML and displayed in a web container
        /// form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (UserDocForm tutorialForm = new UserDocForm())
            {
                tutorialForm.ShowDialog();
            }
        }

        #endregion        
    }
}
