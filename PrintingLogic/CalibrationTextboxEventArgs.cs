using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingLogic
{
    /// <summary>
    /// Class that contains all data pertaining to
    /// updating the text boxes on the calibration
    /// form in the GUI layer.
    /// </summary>
    public class CalibrationTextboxEventArgs : EventArgs
    {
        #region Field Region

        private double stepSize;
        private int speed;
        private double startPosition;
        private double offset;
        private double stopValue;

        #endregion

        #region Propety Region

        /// <summary>
        /// Returns the stepSize to return to the textbox
        /// control.
        /// </summary>
        public double StepSize
        {
            get { return stepSize; }
        }

        /// <summary>
        /// Returns the speed to return to the textbox control.
        /// </summary>
        public int Speed
        {
            get { return speed; }
        }

        /// <summary>
        /// Returns the startPosition to return to the textbox control.
        /// </summary>
        public double StartPosition
        {
            get { return startPosition; }
        }

        /// <summary>
        /// Returns the stopvalue to return to the textbox control.
        /// </summary>
        public double StopValue
        {
            get { return stopValue; }
        }

        /// <summary>
        /// The amount of steps to offset the force detected position
        /// by at the end of each calibration phase.
        /// </summary>
        public double Offset
        {
            get { return offset; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for the CalibrationTextboxEventArgs class.
        /// </summary>
        /// <param name="stepSize"></param>
        /// <param name="speed"></param>
        /// <param name="startPosition"></param>
        /// <param name="offset"></param>
        /// <param name="stopValue"></param>
        public CalibrationTextboxEventArgs(double stepSize, int speed, double startPosition,
            double offset, double stopValue)
        {
            this.stepSize = stepSize;
            this.speed = speed;
            this.startPosition = startPosition;
            this.offset = offset;
            this.stopValue = stopValue;
        }

        #endregion
    }
}
