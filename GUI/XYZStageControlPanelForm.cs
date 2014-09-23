// Author: Robert Dacunto
// Project: 3D Printer Force Feedback Program
// File: XYZStageControlPanelForm.cs
// Classes: XYZStageControlPanelForm

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using PrintingLogic;

namespace GUI
{
    /// <summary>
    /// This form gives the user access to all 
    /// XYZ-Stage controls.
    /// </summary>
    public partial class XYZStageControlPanelForm : Form
    {
        #region Field Region

        private Logic logic;
        private Thread MoveXYZStageThread;

        private bool stopXYZStage;

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for the XYZStageControlPanelForm
        /// </summary>
        /// <param name="logic"></param>
        public XYZStageControlPanelForm(Logic logic)
        {
            InitializeComponent();

            this.logic = logic;

            this.Load += new EventHandler(XYZStageControlPanelForm_Load);
            this.FormClosing += new FormClosingEventHandler(XYZStageControlPanelForm_FormClosing);

            this.driverPositionUpdateTimer.Tick += driverPositionUpdateTimer_Tick;
        }

        #endregion

        #region Event Handler Region

        /// <summary>
        /// Updates the feedback box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_UpdateFeedback(object sender, UpdateFeedbackEventArgs e)
        {
            feedbackBox.Invoke(new Action(() =>
                 feedbackBox.Text += e.Result));

            feedbackBox.Invoke(new Action(() =>
                feedbackBox.SelectionStart = feedbackBox.TextLength
                ));

            feedbackBox.Invoke(new Action(() =>
                feedbackBox.ScrollToCaret()));
        }

        /// <summary>
        /// Updates each driver position label with the current
        /// position of each driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void driverPositionUpdateTimer_Tick(object sender, EventArgs e)
        {
            ADriverValuePositionDataLabel.Text = logic.GetXYZStageDriverPos(logic.ADriver).ToString();
            bDriverValuePositionDataLabel.Text = logic.GetXYZStageDriverPos(logic.BDriver).ToString();
            cDriverValuePositionDataLabel.Text = logic.GetXYZStageDriverPos(logic.CDriver).ToString(); 
        }

        /// <summary>
        /// On form load, ask the user for the COM port the
        /// XYZ-Stage is connected to. If connection is successful,
        /// begin the timer for updating driver positions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XYZStageControlPanelForm_Load(object sender, EventArgs e)
        {
            if (logic.XYZStageConnected == true)
                return;

            string portName = Interaction.InputBox("Input COM Port for XYZ Stage: ", 
                "XYZ Stage Com Port", "COM#");

            try
            {
                if (logic.ConnectXYZStage(portName))
                {
                    this.logic.UpdateFeedback += logic_UpdateFeedback;

                    logic_UpdateFeedback(this,
                        new UpdateFeedbackEventArgs("\nConnected to XYZ Stage on " + portName + "\n"));

                    logic_UpdateFeedback(this,
                        new UpdateFeedbackEventArgs("\nNum of modules connected: " + 
                            logic.NumXYZStageModules().ToString() + "\n"));

                    xyzStagePanel.Visible = true;     
                    this.driverPositionUpdateTimer.Enabled = true;
                }
            }
            catch (Exception com_error)
            {
                MessageBox.Show(com_error.Message);
                this.Close();
            }
        }

        /// <summary>
        /// On form closing, unsubscribe the updatefeedback event
        /// so that the main form can re-subscribe to the event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XYZStageControlPanelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.logic.UpdateFeedback -= logic_UpdateFeedback;
        }

        #endregion

        #region Enable/Disable Driver Method Region

        /// <summary>
        /// This is the parent function for enabling a driver.
        /// Each driver enable button will call this function, 
        /// passing in each specific driver control. If the
        /// driver is already enabled, it will disable it instead.
        /// </summary>
        /// <param name="driverEnabled"></param>
        /// <param name="driver"></param>
        /// <param name="driverEnableButton"></param>
        /// <param name="driverChannelPanel"></param>
        /// <param name="driverPanel"></param>
        /// <param name="driverDisabledLabel"></param>
        /// <param name="aChannel"></param>
        /// <param name="bChannel"></param>
        /// <param name="cChannel"></param>
        /// <param name="groupButton"></param>
        private void driverEnableButton(bool driverEnabled, 
            byte driver, Button driverEnableButton, Panel driverChannelPanel,
            Panel driverPanel, Label driverDisabledLabel, RadioButton aChannel, RadioButton bChannel,
            RadioButton cChannel, RadioButton groupButton)
        {
            if (driverEnabled == false)
            {
                try
                {
                    logic.EnableDriver(driver, ChannelToUse(aChannel,
                        bChannel, cChannel));
                }
                catch (Exception ex)
                {
                    logic_UpdateFeedback(this, 
                        new UpdateFeedbackEventArgs(ex.Message));

                    return;
                }

                driverEnableButton.Text = "Disable";
                driverChannelPanel.Visible = false;
                driverPanel.Visible = true;
                driverDisabledLabel.Visible = false;
            }
            else
            {
                try
                {
                    logic.DisableDriver(driver);
                }
                catch (Exception ex)
                {
                    logic_UpdateFeedback(this, new UpdateFeedbackEventArgs(ex.Message));

                    return;
                }

                driverEnableButton.Text = "Enable";
                driverChannelPanel.Visible = true;
                driverPanel.Visible = false;
                driverDisabledLabel.Visible = true;

                if (groupButton.Visible == true)
                    groupButton.Visible = false;
            }
        }

        /// <summary>
        /// Enable/disable the A Driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverEnableButton_Click(object sender, EventArgs e)
        {
            driverEnableButton(logic.ADriverEnabled, logic.ADriver, 
                aDriverEnableButton, aDriverChannelPanel,
                aDriverPanel, aDriverDisabledLabel, aDriverChannelAButton,
                aDriverChannelBButton, aDriverChannelCButton,
                aDriverIsLeaderButton);
        }

        /// <summary>
        /// Enable/disable the B Driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverEnableButton_Click(object sender, EventArgs e)
        {
            driverEnableButton(logic.BDriverEnabled, logic.BDriver,
                bDriverEnableButton, bDriverChannelPanel,
                bDriverPanel, bDriverDisabledLabel, bDriverChannelAButton,
                bDriverChannelBButton, bDriverChannelCButton,
                bDriverIsLeaderButton);
        }

        /// <summary>
        /// Enable/disable the C Driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverEnableButton_Click(object sender, EventArgs e)
        {
            driverEnableButton(logic.CDriverEnabled, logic.CDriver,
                cDriverEnableButton, cDriverChannelPanel,
                cDriverPanel, cDriverDisabledLabel, cDriverChannelAButton,
                cDriverChannelBButton, cDriverChannelCButton,
                cDriverIsLeaderButton);
        }

        #endregion

        #region Move Method Region

        /// <summary>
        /// The parent function for moving the driver
        /// to a certain position. Each driver will pass in
        /// their specific controls and movement data.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="speedBox"></param>
        /// <param name="accBox"></param>
        /// <param name="posBox"></param>
        /// <param name="abrupt"></param>
        /// <param name="smooth"></param>
        private void driverMoveButton(byte driver, TextBox speedBox, TextBox accBox, TextBox posBox,
            RadioButton abrupt, RadioButton smooth)
        {
            byte speed = 0;
            byte acc = 0;
            int pos = 0;

            try
            {
                speed = Convert.ToByte(speedBox.Text);
                acc = Convert.ToByte(accBox.Text);
                pos = Convert.ToInt32(posBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Error in inputting speed, acceleration and position data." + 
                " Ensure that the data is in the correct format.", 
                    "XYZ Stage Move Error");
                return;
            }

            byte stop = 0;

            if (abrupt.Checked)
                stop = logic.AbruptStop;
            else
                stop = logic.SmoothStop;

            try
            {
                logic.XYZStageMoveToPos(driver, pos, speed, acc, stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "XYZ Stage Move Error");
            }
        }

        /// <summary>
        /// Move A Driver to a certain position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverMoveButton_Click(object sender, EventArgs e)
        {
            driverMoveButton(logic.ADriver, aDriverSpeedBox, aADriverValueccBox, aDriverPositionBox,
                aDriverStopAbruptButton, aDriverSmoothStopButton);
        }

        /// <summary>
        /// Move B Driver to a certain position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverMoveButton_Click(object sender, EventArgs e)
        {
            driverMoveButton(logic.BDriver, bDriverSpeedBox, bADriverValueccBox, bDriverPositionBox,
                bDriverStopAbruptButton, bDriverStopSmoothButton);
        }

        /// <summary>
        /// Move C Driver to a certain position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverMoveButton_Click(object sender, EventArgs e)
        {
            driverMoveButton(logic.CDriver, cDriverSpeedBox, cADriverValueccBox, cDriverPositionBox,
                cDriverStopAbruptButton, cDriverStopSmoothButton);
        }

        /// <summary>
        /// The parent function for moving a certain driver in a 
        /// certain direction. Direction is determined by the isReverse
        /// bool variable - true will move to the left, false to the right.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="driverSpeedBox"></param>
        /// <param name="driverAccBox"></param>
        /// <param name="driverSmoothStop"></param>
        /// <param name="driverAbruptStop"></param>
        /// <param name="e"></param>
        /// <param name="isReverse"></param>
        private void driverMoveDirectionButton(byte driver, TextBox driverSpeedBox, TextBox driverAccBox,
           RadioButton driverSmoothStop, RadioButton driverAbruptStop, MouseEventArgs e, bool isReverse)
        {
            byte speed = 0;
            byte acc = 0;

            try
            {
                speed = Convert.ToByte(driverSpeedBox.Text);
                acc = Convert.ToByte(driverAccBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("You must input data for speed and acceleration.",
                    "XYZ Stage Move Error");

                return;
            }

            byte stopMode = 0;

            if (driverSmoothStop.Checked)
                stopMode = logic.SmoothStop;
            else if (driverAbruptStop.Checked)
                stopMode = logic.AbruptStop;

            // As long as the left mouse button is held down, it will move the motor
            // in the specified direction.
            if (e.Button == MouseButtons.Left)
            {
                stopXYZStage = false;

                CallMoveXYZStage(driver, speed, acc, stopMode, isReverse);

                MoveXYZStageThread.Start();
            }
        }

        /// <summary>
        /// This thread will move the Z-Stage in the designated direction as long as the
        /// user is holding down the move arrow.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="speed"></param>
        /// <param name="acc"></param>
        /// <param name="stopMode"></param>
        /// <param name="isReverse"></param>
        private void CallMoveXYZStage(byte driver, byte speed, byte acc, byte stopMode, bool isReverse)
        {
            MoveXYZStageThread = new Thread(() =>
            {
                bool started = false;

                // stopXYZStage is normally set to false; it is set to true in the driverStopMoveDirection
                // method when the user releases the left mouse button.
                while (!stopXYZStage)
                {
                    if (!started)
                    {
                        logic.XYZStageMoveToVel(driver, speed, acc, stopMode, isReverse);
                        started = true;
                    }
                }

                started = false;
            });
        }

        /// <summary>
        /// When the user lets go of the arrow button, it will stop the driver from moving
        /// in the designated direction.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="driverSmoothStop"></param>
        /// <param name="driverAbruptStop"></param>
        private void driverStopMoveDirection(byte driver, RadioButton driverSmoothStop, 
            RadioButton driverAbruptStop)
        {
            byte stopMode = 0;

            if (driverAbruptStop.Checked)
                stopMode = logic.AbruptStop;
            if (driverSmoothStop.Checked)
                stopMode = logic.SmoothStop;

            stopXYZStage = true;

            MoveXYZStageThread.Join();

            logic.StopDriver(driver, stopMode);
        }

        /// <summary>
        /// Move the A Driver in the left direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverMoveLeftButton_MouseDown(object sender, MouseEventArgs e)
        {
            driverMoveDirectionButton(logic.ADriver, aDriverSpeedBox, aADriverValueccBox, aDriverSmoothStopButton,
                aDriverStopAbruptButton, e, Logic.xyzStageMoveLeft);
        }

        /// <summary>
        /// Stop moving the A Driver in the left direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverMoveLeftButton_MouseUp(object sender, MouseEventArgs e)
        {
            driverStopMoveDirection(logic.ADriver, aDriverSmoothStopButton, aDriverStopAbruptButton);
        }

        /// <summary>
        /// Move the B driver in the left direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverMoveLeftButton_MouseDown(object sender, MouseEventArgs e)
        {
            driverMoveDirectionButton(logic.BDriver, bDriverSpeedBox, bADriverValueccBox, bDriverStopSmoothButton,
                bDriverStopAbruptButton, e, Logic.xyzStageMoveLeft);
        }

        /// <summary>
        /// Stop moving the B driver in the left direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverMoveLeftButton_MouseUp(object sender, MouseEventArgs e)
        {
            driverStopMoveDirection(logic.BDriver, bDriverStopSmoothButton, bDriverStopAbruptButton);
        }

        /// <summary>
        /// Move the C driver in the left direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverMoveLeftButton_MouseDown(object sender, MouseEventArgs e)
        {
            driverMoveDirectionButton(logic.CDriver, cDriverSpeedBox, cADriverValueccBox, cDriverStopSmoothButton,
                cDriverStopAbruptButton, e, Logic.xyzStageMoveLeft);
        }

        /// <summary>
        /// Stop moving the C driver in the left direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverMoveLeftButton_MouseUp(object sender, MouseEventArgs e)
        {
            driverStopMoveDirection(logic.CDriver, cDriverStopSmoothButton, cDriverStopAbruptButton);
        }

        /// <summary>
        /// Move the A driver in the right direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverMoveRightButton_MouseDown(object sender, MouseEventArgs e)
        {
            driverMoveDirectionButton(logic.ADriver, aDriverSpeedBox, aADriverValueccBox, aDriverSmoothStopButton,
                aDriverStopAbruptButton, e, Logic.xyzStageMoveRight);
        }

        /// <summary>
        /// Stop moving the A driver in the right direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverMoveRightButton_MouseUp(object sender, MouseEventArgs e)
        {
            driverStopMoveDirection(logic.ADriver, aDriverSmoothStopButton, aDriverStopAbruptButton);
        }

        /// <summary>
        /// Move the B driver in the right direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverMoveRightButton_MouseDown(object sender, MouseEventArgs e)
        {
            driverMoveDirectionButton(logic.BDriver, bDriverSpeedBox, bADriverValueccBox, bDriverStopSmoothButton,
                bDriverStopAbruptButton, e, Logic.xyzStageMoveRight);
        }

        /// <summary>
        /// Stop moving the B driver in the right direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverMoveRightButton_MouseUp(object sender, MouseEventArgs e)
        {
            driverStopMoveDirection(logic.BDriver, bDriverStopSmoothButton, bDriverStopAbruptButton);
        }

        /// <summary>
        /// Move the C driver in the right direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverMoveRightButton_MouseDown(object sender, MouseEventArgs e)
        {
            driverMoveDirectionButton(logic.CDriver, cDriverSpeedBox, cADriverValueccBox, cDriverStopSmoothButton,
                cDriverStopAbruptButton, e, Logic.xyzStageMoveRight);
        }

        /// <summary>
        /// Stop moving the C driver in the right direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverMoveRightButton_MouseUp(object sender, MouseEventArgs e)
        {
            driverStopMoveDirection(logic.CDriver, cDriverStopSmoothButton, cDriverStopAbruptButton);
        }

        /// <summary>
        /// Parent function for stopping the driver from moving in position mode.
        /// Each driver will pass in their specific controls.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="driverStopSmooth"></param>
        /// <param name="driverStopAbrupt"></param>
        private void driverStopButton_Click(byte driver, RadioButton driverStopSmooth, RadioButton driverStopAbrupt)
        {
            byte stopMode = 0;

            if (driverStopSmooth.Checked)
                stopMode = logic.SmoothStop;
            if (driverStopAbrupt.Checked)
                stopMode = logic.AbruptStop;

            logic.StopDriver(driver, stopMode);
        }

        /// <summary>
        /// Stops the A driver from moving in position mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverStopButton_Click(object sender, EventArgs e)
        {
            driverStopButton_Click(logic.ADriver, aDriverSmoothStopButton, aDriverStopAbruptButton);
        }

        /// <summary>
        /// Stops the B driver from moving in position mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverStopButton_Click(object sender, EventArgs e)
        {
            driverStopButton_Click(logic.BDriver, bDriverStopSmoothButton, bDriverStopAbruptButton);
        }

        /// <summary>
        /// Stops the C driver from moving in position mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverStopButton_Click(object sender, EventArgs e)
        {
            driverStopButton_Click(logic.CDriver, cDriverStopSmoothButton, cDriverStopAbruptButton);
        }

        #endregion

        #region Channel Method Region

        /// <summary>
        /// Will return the selected channel for the 
        /// associated driver.
        /// </summary>
        /// <param name="aChannel"></param>
        /// <param name="bChannel"></param>
        /// <param name="cChannel"></param>
        /// <returns></returns>
        private byte ChannelToUse(RadioButton aChannel, RadioButton bChannel, 
            RadioButton cChannel)
        {
            byte channel;

            if (aChannel.Checked == true)
                channel = logic.AChannel;
            else if (bChannel.Checked == true)
                channel = logic.BChannel;
            else
                channel = logic.CChannel;

            return channel;
        }

        #endregion

        #region Driver Mode Method Region

        /// <summary>
        /// Enables/disables position mode for the 
        /// associated driver.
        /// </summary>
        /// <param name="driverPositionModeButton"></param>
        /// <param name="driverPositionBox"></param>
        /// <param name="driverMoveButton"></param>
        /// <param name="driverStopButton"></param>
        private void driverPositionModeButton_Changed(RadioButton driverPositionModeButton,
            TextBox driverPositionBox, Button driverMoveButton, Button driverStopButton)
        {
            if (driverPositionModeButton.Checked)
            {
                driverPositionBox.Visible = true;
                driverMoveButton.Visible = true;
                driverStopButton.Visible = true;
            }
            else
            {
                driverPositionBox.Visible = false;
                driverMoveButton.Visible = false;
                driverStopButton.Visible = false;
            }
        }

        /// <summary>
        /// Enables/disables velocity mode for the
        /// associated driver.
        /// </summary>
        /// <param name="driverVelocityModeButton"></param>
        /// <param name="driverMoveLeftButton"></param>
        /// <param name="driverMoveRightButton"></param>
        private void driverVelocityModeButton_Changed(RadioButton driverVelocityModeButton,
            Button driverMoveLeftButton, Button driverMoveRightButton)
        {
            if (driverVelocityModeButton.Checked)
            {
                driverMoveLeftButton.Visible = true;
                driverMoveRightButton.Visible = true;
            }
            else
            {
                driverMoveLeftButton.Visible = false;
                driverMoveRightButton.Visible = false;
            }
        }

        /// <summary>
        /// Enables/disables position mode for the A Driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverPositionModeButton_CheckedChanged(object sender, EventArgs e)
        {
            driverPositionModeButton_Changed(aDriverPositionModeButton, aDriverPositionBox,
                aDriverMoveButton, aDriverStopButton);
        }

        /// <summary>
        /// Enables/disables velocity mode for the A driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverVelocityModeButton_CheckedChanged(object sender, EventArgs e)
        {
            driverVelocityModeButton_Changed(aDriverVelocityModeButton, aDriverMoveLeftButton,
                aDriverMoveRightButton);
        }

        /// <summary>
        /// Enables/disables position mode for the B Driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverPositionModeButton_CheckedChanged(object sender, EventArgs e)
        {
            driverPositionModeButton_Changed(bDriverPositionModeButton, bDriverPositionBox,
                bDriverMoveButton, bDriverStopButton);
        }

        /// <summary>
        /// Enables/disables velocity mode for the B Driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverVelocityModeButton_CheckedChanged(object sender, EventArgs e)
        {
            driverVelocityModeButton_Changed(bDriverVelocityModeButton, bDriverMoveLeftButton,
                bDriverMoveRightButton);
        }

        /// <summary>
        /// Enables/disables position mode for the C driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverPositionModeButton_CheckedChanged(object sender, EventArgs e)
        {
            driverPositionModeButton_Changed(cDriverPositionModeButton, cDriverPositionBox,
                cDriverMoveButton, cDriverStopButton);
        }

        /// <summary>
        /// Enables/disables velocity mode for the C driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverVelocityModeButton_CheckedChanged(object sender, EventArgs e)
        {
            driverVelocityModeButton_Changed(cDriverVelocityModeButton, cDriverMoveLeftButton,
                cDriverMoveRightButton);
        }

        #endregion

        #region Group Method Region


        /// <summary>
        /// This method will add the driver to the driver group, allowing that driver to be
        /// moved at the same time as the other drives in a single group. It will also
        /// remove the driver from the group if checked again.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="driverGroupCheckBox"></param>
        /// <param name="isLeaderButton"></param>
        /// <param name="otherGroupBox1"></param>
        /// <param name="otherGroupBox2"></param>
        private void driverGroupCheckBox_Changed(byte driver, CheckBox driverGroupCheckBox,
            RadioButton isLeaderButton, CheckBox otherGroupBox1, CheckBox otherGroupBox2)
        {
            #region Add to Group Region

            // If added to group, make its "isLeader" button visible. This will allow
            // the user to make this driver the "leader". Add it to the group as a member
            // not the leader. Leader is determined later.
            if (driverGroupCheckBox.Checked)
            {
                isLeaderButton.Visible = true;

                logic.XYZStageSetGroup(driver, false);

                if (!xyzMoveAsGroupButton.Visible)
                {
                    xyzMoveAsGroupButton.Visible = true;
                    chooseDriverLeaderLabel.Visible = true;
                }
            }

            #endregion

            #region Remove from Group Region

            else
            {
                isLeaderButton.Visible = false;

                logic.XYZStageRemoveGroup(driver);

                if (!otherGroupBox1.Checked && !otherGroupBox2.Checked)
                {
                    xyzMoveAsGroupButton.Visible = false;
                    chooseDriverLeaderLabel.Visible = false;
                }
            }

            #endregion
        }

        /// <summary>
        /// Adds/removes the A Driver to the group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverGroupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            driverGroupCheckBox_Changed(logic.ADriver, aDriverGroupCheckBox, aDriverIsLeaderButton,
                bDriverGroupCheckBox, cDriverGroupCheckBox);
        }


        /// <summary>
        /// Adds/removes the B driver to the group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverGroupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            driverGroupCheckBox_Changed(logic.BDriver, bDriverGroupCheckBox, bDriverIsLeaderButton,
                aDriverGroupCheckBox, cDriverGroupCheckBox);
        }


        /// <summary>
        /// Adds/removes the C driver from the group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverGroupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            driverGroupCheckBox_Changed(logic.CDriver, cDriverGroupCheckBox, cDriverIsLeaderButton,
                aDriverGroupCheckBox, bDriverGroupCheckBox);
        }
        
        /// <summary>
        /// Sets the driver leader for the group. 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="driverIsLeaderButton"></param>
        private void driverIsLeaderButton_Changed(byte driver, RadioButton driverIsLeaderButton)
        {
            if (driverIsLeaderButton.Checked)
            {
                logic.XYZStageSetGroup(driver, true);
            }
            else
            {
                logic.XYZStageSetGroup(driver, false);
            }
        }

        /// <summary>
        /// Sets the A Driver as the leader of the group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aDriverIsLeaderButton_CheckedChanged(object sender, EventArgs e)
        {
            driverIsLeaderButton_Changed(logic.ADriver, aDriverIsLeaderButton);
        }

        /// <summary>
        /// Sets the B Driver as the leader of the group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDriverIsLeaderButton_CheckedChanged(object sender, EventArgs e)
        {
            driverIsLeaderButton_Changed(logic.BDriver, bDriverIsLeaderButton);
        }

        /// <summary>
        /// Sets the C driver as the leader of the group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cDriverIsLeaderButton_CheckedChanged(object sender, EventArgs e)
        {
            driverIsLeaderButton_Changed(logic.CDriver, cDriverIsLeaderButton);
        }

        /// <summary>
        /// This button will begin the process of moving the XYZ-Stage
        /// group as one. Each driver in the group will have their own
        /// speed, acceleration and position as determined by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xyzMoveAsGroupButton_Click(object sender, EventArgs e)
        {
            if (!aDriverIsLeaderButton.Checked && !bDriverIsLeaderButton.Checked 
                && !cDriverIsLeaderButton.Checked)
            {
                MessageBox.Show("You must select a group leader first.", 
                    "XYZ Stage Group Move Error");
            }
            else
            {
                bool goodToMove = false;

                // Gather data from each driver in the group, setting position, acceleration
                // and speed, as well as stop mode. Load their data.
                try
                {
                    goodToMove = moveGroup(logic.ADriver, aDriverGroupCheckBox, aDriverSmoothStopButton,
                        aDriverPositionBox, aADriverValueccBox, aDriverSpeedBox);

                    goodToMove = moveGroup(logic.BDriver, bDriverGroupCheckBox, bDriverStopSmoothButton,
                        bDriverPositionBox, bADriverValueccBox, bDriverSpeedBox);

                    goodToMove = moveGroup(logic.CDriver, cDriverGroupCheckBox, cDriverStopSmoothButton,
                        cDriverPositionBox, cADriverValueccBox, cDriverSpeedBox);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);

                    goodToMove = false;
                }

                // Once all data is loaded, move the drivers as a group.
                if (goodToMove)
                {
                    logic.XYZStageMoveGroup();
                }
            }
        }

        /// <summary>
        /// This function will carry out the move operation
        /// for the group of drivers.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="driverGroupCheckBox"></param>
        /// <param name="driverSmoothStopButton"></param>
        /// <param name="driverPositionBox"></param>
        /// <param name="driverAccBox"></param>
        /// <param name="driverSpeedBox"></param>
        /// <returns></returns>
        private bool moveGroup(byte driver, CheckBox driverGroupCheckBox, RadioButton driverSmoothStopButton,
            TextBox driverPositionBox, TextBox driverAccBox, TextBox driverSpeedBox)
        {
            int pos = 0;
            byte speed = 0;
            byte acc = 0;
            byte stopMode;
            bool goodToMove = true;

            if (driverGroupCheckBox.Checked)
            {
                if (driverSmoothStopButton.Checked)
                    stopMode = logic.SmoothStop;
                else
                    stopMode = logic.AbruptStop;

                try
                {
                    pos = Convert.ToInt32(driverPositionBox.Text);
                    acc = Convert.ToByte(driverAccBox.Text);
                    speed = Convert.ToByte(driverSpeedBox.Text);
                }
                catch (Exception)
                {
                    string driverName = "";

                    if (driver == logic.ADriver)
                    {
                        driverName = "A";
                    }
                    else if (driver == logic.BDriver)
                    {
                        driverName = "B";
                    }
                    else
                    {
                        driverName = "C";
                    }

                    throw new Exception("Incorrect parameters for Driver " + driverName + ".");
                }

                if (goodToMove)
                {
                    if (driver == logic.ADriver)
                    {
                        logic.XYZStageMoveGroupSetup(logic.ADriver, stopMode, pos, acc, speed);
                    }
                    else if (driver == logic.BDriver)
                    {
                        logic.XYZStageMoveGroupSetup(logic.BDriver, stopMode, pos, acc, speed);
                    }
                    else
                    {
                        logic.XYZStageMoveGroupSetup(logic.CDriver, stopMode, pos, acc, speed);
                    }
                }
            }

            return goodToMove;
        }

        #endregion
    }
}
