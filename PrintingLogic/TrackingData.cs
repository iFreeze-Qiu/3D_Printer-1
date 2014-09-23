using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingLogic
{
    /// <summary>
    /// Class for tracking data for Z-Stage
    /// tracking mode. When the force is detected, it will
    /// pass this class with the position and force value
    /// to the tracking list.
    /// </summary>
    public class TrackingData
    {
        #region Field Region

        private double position;
        private double forceValue;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns the position the Z-Stage was at when
        /// force was detected during tracking.
        /// </summary>
        public double Position
        {
            get { return position; }
        }

        /// <summary>
        /// Returns the sensor value at the time force
        /// was detected during tracking.
        /// </summary>
        public double ForceValue
        {
            get { return forceValue; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for the TrackingData class.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forceValue"></param>
        public TrackingData(double position, double forceValue)
        {
            this.position = position;
            this.forceValue = forceValue;
        }

        #endregion
    }
}
