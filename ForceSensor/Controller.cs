// Author: Robert Dacunto
// Project: Force Sensor Controller
// File: Controller.cs
// Classes: Controller, SENSOR_ERROR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Futek_USB_DLL;

namespace ForceSensor
{
    /// <summary>
    /// Controller handles all logic specific to the force sensor,
    /// including connectivity and reporting sensor value.
    /// </summary>
    public class Controller
    {
        #region Field Region

        private USB_DLL oFutekUSBDLL;

        private bool connected;

        private string serialNumber;
        private string deviceHandle;
        private double normalData;
        private string deviceStatus;
        private int offSet;
        private int fullScale;
        private string tempValues;
        private int capacity;
        private int decimalPoint;
        private int unitCode;
        private double tare;
        private string deviceIndex;
        private double calculatedReading;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns whether or not the force sensor
        /// is connected.
        /// </summary>
        public bool Connected
        {
            get { return connected; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for force sensor controller. 
        /// Instantiates oFutekUSBDLL which allows access to 
        /// the lower-level force sensor functions.
        /// deviceIndex defaults to 0 in order to retrieve
        /// the serial number of the force sensor
        /// in the first position.
        /// </summary>
        public Controller()
        {
            deviceIndex = "0";
            oFutekUSBDLL = new USB_DLL();
        }

        #endregion

        #region Connection Region

        /// <summary>
        /// Connects the PC to the force sensor
        /// with the given serial number.
        /// </summary>
        /// <param name="serialNumber"></param>
        public void Connect(string serialNumber)
        {
            this.serialNumber = serialNumber;

            oFutekUSBDLL.Open_Device_Connection(this.serialNumber);

            deviceStatus = oFutekUSBDLL.DeviceStatus;

            if (deviceStatus == "0")
            {
                deviceHandle = oFutekUSBDLL.DeviceHandle;
                connected = true;

                GetOffsetValue();
                GetFullscaleValue();
                GetFullscaleLoad();
                GetDecimalPoint();
                GetUnitCode();
            }
            else
            {
                throw new SENSOR_ERROR("\nFailed to connect to sensor. Device error " + deviceStatus + '\n');
            }
        }

        /// <summary>
        /// Disconnects the PC from the force sensor.
        /// </summary>
        public void Disconnect()
        {
            if (connected)
            {
                connected = false;
                oFutekUSBDLL.Close_Device_Connection(deviceHandle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns the default serial number.</returns>
        public string DefaultSerialNumber()
        {
            return oFutekUSBDLL.Get_Device_Serial_Number(deviceIndex);
        }

        #endregion

        #region Setup Method Region

        /// <summary>
        /// Evaluate expression to determine if it is a numeric expression.
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns>Returns true if expression is numeric, false
        /// otherwise.</returns> 
        private static bool IsNumeric(Object Expression)
        {

            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 ||
                    Expression is Int64 || Expression is Decimal ||
                    Expression is Single || Expression is Double ||
                    Expression is Boolean)
            {
                return true;
            }

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }

        /// <summary>
        /// Calculate the device's offset value.
        /// </summary>
        private void GetOffsetValue()
        {
            tempValues = oFutekUSBDLL.Get_Offset_Value(deviceHandle);

            while (IsNumeric(tempValues) == false)
            {
                tempValues = oFutekUSBDLL.Get_Offset_Value(deviceHandle);
            }

            offSet = Int32.Parse(tempValues);
        }

        /// <summary>
        /// Calculate the device's fullscale value
        /// </summary> 
        private void GetFullscaleValue()
        {
            tempValues = oFutekUSBDLL.Get_Fullscale_Value(deviceHandle);

            while (IsNumeric(tempValues) == false)
            {
                tempValues = oFutekUSBDLL.Get_Fullscale_Value(deviceHandle);
            }

            fullScale = Int32.Parse(tempValues);
        }

        /// <summary>
        /// Calculate the device's fullscale load
        /// </summary>
        private void GetFullscaleLoad()
        {
            tempValues = oFutekUSBDLL.Get_Fullscale_Load(deviceHandle);

            while (IsNumeric(tempValues) == false)
            {
                tempValues = oFutekUSBDLL.Get_Fullscale_Load(deviceHandle);
            }

            capacity = Int32.Parse(tempValues);
        }

        /// <summary>
        /// Sets the decimal point for the force sensor, 
        /// used in the calculatedReading function when
        /// returning the force sensor value.
        /// </summary>
        private void GetDecimalPoint()
        {
            tempValues = oFutekUSBDLL.Get_Decimal_Point(deviceHandle);

            while (IsNumeric(tempValues) == false)
            {
                tempValues = oFutekUSBDLL.Get_Decimal_Point(deviceHandle);
            }

            decimalPoint = Int32.Parse(tempValues);

            if (decimalPoint > 3)
                decimalPoint = 0;
        }

        /// <summary>
        /// Sets the unit code for the force sensor.
        /// </summary>
        private void GetUnitCode()
        {
            tempValues = oFutekUSBDLL.Get_Unit_Code(deviceHandle);

            while (IsNumeric(tempValues) == false)
            {
                tempValues = oFutekUSBDLL.Get_Unit_Code(deviceHandle);
            }

            unitCode = Int32.Parse(tempValues);
        }

        #endregion

        #region Sensor Reading Method Region

        /// <summary>
        /// Gets the current sensor value, converts it to
        /// grams, and returns value minus the tare (if any).
        /// </summary>
        /// <returns>Sensor value (tared if set)</returns>
         
        public double UpdateSensor()
        {
            tempValues = oFutekUSBDLL.Normal_Data_Request(deviceHandle);

            while (IsNumeric(tempValues) == false)
            {
                tempValues = oFutekUSBDLL.Normal_Data_Request(deviceHandle);
            }

            normalData = Double.Parse(tempValues);

            calculatedReading = (normalData - offSet);
            calculatedReading = calculatedReading / (fullScale - offSet);
            calculatedReading = calculatedReading * capacity;

            // convert to pounds, then to grams
            calculatedReading = ((calculatedReading / Math.Pow(10, decimalPoint)) / 10000) *
                453.592;

            return calculatedReading - tare;            
        }

        /// <summary>
        /// Sets the tare value to the current sensor value,
        /// and further sensor updates will minus the tare 
        /// value from its reading.
        /// </summary>
        public void Tare()
        {
            tare = calculatedReading;
        }

        /// <summary>
        /// Will undo the tare, if previously set, and 
        /// allow the sensor to display its gross/real value.
        /// </summary>
        public void Gross()
        {
            tare = 0;
        }

        #endregion
    }

    /// <summary>
    /// Exception class specific to the sensor.
    /// </summary>
    [Serializable]
    public class SENSOR_ERROR : Exception
    {
        /// <summary>
        /// Constructor for SENSOR_ERROR, 
        /// simply passes error message to base
        /// </summary>
        /// <param name="message"></param>
        public SENSOR_ERROR(string message)
            : base(message)
        {
        }
    }
}
