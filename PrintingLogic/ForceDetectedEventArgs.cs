using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingLogic
{
    /// <summary>
    /// Event arguments for force detection - must know
    /// the value of the force sensor at detection, the
    /// position of the Z-Stage at detection,
    /// and whether or not the detection occured during
    /// a calibration process or not.
    /// </summary>
    public class ForceDetectedEventArgs : EventArgs
    {
        #region Field Region

        private double forceDetectedValue;
        private double forceDetectedPosition;
        private bool isCalibrating;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns the force value at the time of 
        /// detection.
        /// </summary>
        public double ForceDetectedValue
        {
            get { return forceDetectedValue; }
        }

        /// <summary>
        /// Returns the Z-Stage position at the time of
        /// force value detection.
        /// </summary>
        public double ForceDetectedPosition
        {
            get { return forceDetectedPosition; }
        }

        /// <summary>
        /// Returns whether or not the Z-Stage was calibrating
        /// at the time of detection.
        /// </summary>
        public bool IsCalibrating
        {
            get { return isCalibrating; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for the ForceDetectedEventArgs class.
        /// </summary>
        /// <param name="forceDetectedPosition"></param>
        /// <param name="forceDetectedValue"></param>
        /// <param name="isCalibrating"></param>
        public ForceDetectedEventArgs(double forceDetectedPosition, double forceDetectedValue,
            bool isCalibrating)
        {
            this.forceDetectedValue = forceDetectedValue;
            this.forceDetectedPosition = forceDetectedPosition;
            this.isCalibrating = isCalibrating;
        }

        #endregion
    }
}
