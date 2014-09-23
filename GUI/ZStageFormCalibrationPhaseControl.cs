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
    /// This form controls the settings for each phase
    /// of Z-Stage calibration.
    /// </summary>
    public partial class ZStageFormCalibrationPhaseControl : Form
    {
        #region Field Region

        private Logic logic;

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for ZStageFormCalibrationPhaseControl.
        /// Wire events and instantiate the logic class.
        /// </summary>
        /// <param name="logic"></param>
        public ZStageFormCalibrationPhaseControl(Logic logic)
        {
            InitializeComponent();

            this.logic = logic;

            this.Load += ZStageFormCalibrationPhaseControl_Load;
            this.FormClosing += ZStageFormCalibrationPhaseControl_FormClosing;
        }

        #endregion

        #region Event Handler Region

        /// <summary>
        /// When the settings form loads, fill all the
        /// text box controls with data from the logic class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZStageFormCalibrationPhaseControl_Load(object sender, EventArgs e)
        {
            FillTextBoxControls();
        }

        /// <summary>
        /// When the settings form closes, ensure that the user
        /// has input valid settings for the Z-Stage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZStageFormCalibrationPhaseControl_FormClosing(object sender,
            FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                try
                {
                    CheckSettings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Settings Error");
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region General Method Region

        /// <summary>
        /// Automatically fill the text box controls
        /// with the default values from the logic class.
        /// </summary>
        private void FillTextBoxControls()
        {
            string stepSize = logic.StepSize.ToString();
            string speed = logic.Speed.ToString();
            string offset = logic.Offset.ToString();

            // Phase 1 Controls

            stepSizePhase1TextBox.Text = stepSize;
            speedPhase1TextBox.Text = speed;
            offsetPhase1TextBox.Text = offset;

            // Phase 2 Controls

            stepSizePhase2TextBox.Text = stepSize;
            speedPhase2TextBox.Text = speed;
            offsetPhase2TextBox.Text = offset;

            // Phase 3 Controls

            stepSizePhase3TextBox.Text = stepSize;
            speedPhase3TextBox.Text = speed;
            offsetPhase3TextBox.Text = offset;

            // Phase 4 Controls

            stepSizePhase4TextBox.Text = stepSize;
            speedPhase4TextBox.Text = speed;
        }
        
        /// <summary>
        /// Once the user clicks OK, the program ensures that the values
        /// are valid.
        /// </summary>
        private void CheckSettings()
        {
            try
            {
                logic.ValidateInputs(stepSizePhase1TextBox.Text,
                    speedPhase1TextBox.Text, offsetPhase1TextBox.Text);

                logic.ValidateInputs(stepSizePhase2TextBox.Text,
                    speedPhase2TextBox.Text, offsetPhase2TextBox.Text);

                logic.ValidateInputs(stepSizePhase3TextBox.Text,
                    speedPhase3TextBox.Text, offsetPhase3TextBox.Text);

                logic.ValidateInputs(stepSizePhase4TextBox.Text,
                    speedPhase4TextBox.Text, offsetPhase3TextBox.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
