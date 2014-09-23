using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingLogic
{
    /// <summary>
    /// Class that contains all data pertaining to 
    /// moving the Z-Stage. 
    /// </summary>
    public class MoveZStageThreadParam
    {
        #region Field Region

        private int speed;
        private ZStageMoveDirections direction;
        private bool isCalibrating;
        private double forceSensorStopValue;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns the speed value for the Z-Stage.
        /// </summary>
        public int Speed
        {
            get { return speed; }
        }

        /// <summary>
        /// Returns the direction the Z-Stage is moving.
        /// </summary>
        public ZStageMoveDirections Direction
        {
            get { return direction; }
        }

        /// <summary>
        /// Returns whether the Z-Stage is calibrating or not.
        /// </summary>
        public bool IsCalibrating
        {
            get { return isCalibrating; }
        }

        /// <summary>
        /// Returns the force sensor stop value.
        /// </summary>
        public double ForceSensorStopValue
        {
            get { return forceSensorStopValue; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for MoveZStageThreadParam class.
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="direction"></param>
        /// <param name="isCalibrating"></param>
        /// <param name="forceSensorStopValue"></param>
        public MoveZStageThreadParam(int speed, ZStageMoveDirections direction,
            bool isCalibrating, double forceSensorStopValue)
        {
            this.speed = speed;
            this.direction = direction;
            this.isCalibrating = isCalibrating;
            this.forceSensorStopValue = forceSensorStopValue;
        }

        #endregion
    }
}
