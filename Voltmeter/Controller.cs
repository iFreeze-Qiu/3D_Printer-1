// Author: Robert Dacunto
// Project: Voltmeter Controller
// File: Controller.cs
// Classes: Controller

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USBMeasure;

namespace Voltmeter
{
    /// <summary>
    /// Controller handles all logic specific to 
    /// Voltmeter, including connectivity and
    /// voltage reporting.
    /// </summary>
    public class Controller
    {
        #region Field Region

        private CUSBMeasureMain device;
        private double ADCValue;
        private bool connected;
        
        private string unitTypeText;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns whether or not the Voltmeter is
        /// connected.
        /// </summary>
        public bool Connected
        {
            get { return connected; }
        }

        /// <summary>
        /// Returns the type of the
        /// connected voltmeter.
        /// </summary>
        public string UnitTypeText
        {
            get { return unitTypeText; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for Voltmeter Controller.
        /// Device will instantiate the interface 
        /// specific to the voltmeter, allowing access
        /// to all the lower-level functions specific
        /// to the voltmeter.
        /// </summary>
        public Controller()
        {
            device = new CUSBMeasureMain();
        }

        #endregion

        #region Connection Method Region

        /// <summary>
        /// Will connect the PC to the voltmeter.
        /// </summary>
        /// <returns>Returns true or false, depending on
        /// success of connection.</returns>
        public bool Connect()
        {
            device.USBMOpen();

            if (device.Result == 0)
            {
                unitTypeText = device.UnitTypeText;
                connected = true;

                ClearBuffer();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Will disconnect the PC from the voltmeter.
        /// </summary>
        public void Disconnect()
        {
            device.USBMClose();

            connected = false;
        }

        #endregion

        #region Read Method Region

        /// <summary>
        /// Reads the current value of the voltmeter.
        /// </summary>
        /// <returns>Returns voltage in string format.</returns>
        public string Read()
        {
            device.USBMGetData();
            ADCValue = device.UnitData;

            return ADCValue.ToString("# 00.0000");
        }

        /// <summary>
        /// Previous voltage readings may be locked in buffer, 
        /// simply read multiple values to clear old ones out.
        /// </summary>
        private void ClearBuffer()
        {
            for (int i = 0; i < 5; i++)
                device.USBMGetData();
        }

        #endregion
    }
}
