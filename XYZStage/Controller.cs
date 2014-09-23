// Author: Robert Dacunto
// Project: XYZ-Stage Controller
// File: Controller.cs
// Classes: Controller, Driver, XYZStageError

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYZStage
{
    /// <summary>
    /// Controller handles logic specific to XYZ-Stage as a whole -
    /// including connectivity of the stage, group control, and
    /// enabling/disabling drivers.
    /// </summary>
    public class Controller
    {
        #region Field Region
        
        /// <summary>
        /// The byte value associated with channel A
        /// </summary>
        public const byte ChannelA = NativeMethods.SET_CH_A;

        /// <summary>
        /// The byte value associated with channel B
        /// </summary>
        public const byte ChannelB = NativeMethods.SET_CH_B;

        /// <summary>
        /// The byte value associated with channel C
        /// </summary>
        public const byte ChannelC = NativeMethods.SET_CH_C;

        /// <summary>
        /// Driver A has a value of 1. This is used to determine
        /// which driver is currently being operated on.
        /// </summary>
        public const byte ADriverValue = 1;

        /// <summary>
        /// Driver B has a value of 2. This is used to determine
        /// which driver is currently being operated on.
        /// </summary>
        public const byte BDriverValue = 2;

        /// <summary>
        /// Driver C has a value of 3. This is used to determine
        /// which driver is currently being operated on.
        /// </summary>
        public const byte CDriverValue = 3;

        /// <summary>
        /// The byte value associated with Smooth stop mode.
        /// </summary>
        public const byte SmoothStop = NativeMethods.STOP_SMOOTH;

        /// <summary>
        /// The byte value associated with Abrupt stop mode.
        /// </summary>
        public const byte AbruptStop = NativeMethods.STOP_ABRUPT;

        private const uint baud = 19200;

        private int numModules;
        private char[] port;
        private Driver aDriver;
        private Driver bDriver;
        private Driver cDriver;
        private bool connected;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns whether or not the XYZ-Stage is connected.
        /// </summary>
        public bool Connected
        {
            get { return connected; }
        }

        /// <summary>
        /// Returns the number of modules connected.
        /// </summary>
        public int NumModules
        {
            get { return numModules; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for XYZ-Stage controller.
        /// </summary>
        public Controller()
        {

        }

        #endregion

        #region Connection Method Region

        /// <summary>
        /// Connects the PC to the XYZ-Stage on port
        /// "portName". 
        /// </summary>
        /// <param name="portName"></param>
        /// <returns>Returns true if connection was successful.</returns>
        public bool Connect(string portName)
        {
            port = new char[5];

            int temp = 0;

            foreach (char c in portName)
            {
                port[temp] = c;
                temp++;
            }

            port[4] = '\0';

            if (!EstablishConnection())
            {
                throw new XYZStageError("No Modules found at " + portName);
            }

            if (!FindPicoDriver())
            {
                throw new XYZStageError("Communication Error.");
            }

            connected = true;

            return true;
        }

        /// <summary>
        /// Disconnects the PC from the XYZ-Stage.
        /// </summary>
        public void Disconnect()
        {
            NativeMethods.LdcnShutdown();
            connected = false;
        }

        /// <summary>
        /// If XYZ-Stage is found on designated port, find
        /// the number of modules on that port.
        /// </summary>
        /// <returns>Returns false if no modules are found,
        /// or true if more than zero modules are found.</returns>
        private bool EstablishConnection()
        {
            numModules = NativeMethods.LdcnInit(port, baud);

            if (numModules == 0)
            {
                numModules = NativeMethods.LdcnFullInit(port, baud);
            }

            if (numModules == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Once the module(s) are found in the Connect method,
        /// we now find the drivers connected. Instantiate a
        /// Driver object for each connected driver. Set 
        /// initial params for each driver.
        /// </summary>
        /// <returns>Returns false if unable to set driver
        /// params, or true if all drivers are successfully 
        /// set up.</returns>
        public bool FindPicoDriver()
        {
            NativeMethods.LdcnReadStatus(ADriverValue, NativeMethods.SEND_ID);
            NativeMethods.LdcnReadStatus(BDriverValue, NativeMethods.SEND_ID);
            NativeMethods.LdcnReadStatus(CDriverValue, NativeMethods.SEND_ID);

            aDriver = new Driver(ADriverValue);
            bDriver = new Driver(BDriverValue);
            cDriver = new Driver(CDriverValue);

            aDriver.ModType = NativeMethods.LdcnGetModType(ADriverValue);
            aDriver.ModVersion = NativeMethods.LdcnGetModVer(ADriverValue);

            bDriver.ModType = NativeMethods.LdcnGetModType(BDriverValue);
            bDriver.ModVersion = NativeMethods.LdcnGetModVer(BDriverValue);

            cDriver.ModVersion = NativeMethods.LdcnGetModVer(CDriverValue);
            cDriver.ModType = NativeMethods.LdcnGetModType(CDriverValue);

            if (!aDriver.StepSetParam())
            {
                return false;
            }

            if (!bDriver.StepSetParam())
            {
                return false;
            }

            if (!cDriver.StepSetParam())
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Driver Method Region

        /// <summary>
        /// The passed in driver is enabled on the passed in channel.
        /// A channel is what is connected to the actual motor on the
        /// stage - we tell the driver what motor it will be operating.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="channel"></param>
        /// <returns>Returns true if driver is successfully enabled on the
        /// given channel.</returns>
        public bool EnableDriver(byte driver, byte channel)
        {
            try
            {
                SetOutputs(driver, channel);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Disables the driver that's passed in.
        /// </summary>
        /// <param name="driver"></param>
        public void DisableDriver(byte driver)
        {
            if (driver == ADriverValue)
                aDriver.DisableMotor();
            if (driver == BDriverValue)
                bDriver.DisableMotor();
            if (driver == CDriverValue)
                cDriver.DisableMotor();
        }

        /// <summary>
        /// Set the outputs for the driver on the given channel.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="channel"></param>
        /// <returns>Returns true if the driver's outputs
        /// are successfully established, and false otherwise.</returns>
        public bool SetOutputs(byte driver, byte channel)
        {
            try
            {
                switch (driver)
                {
                    case ADriverValue:
                        aDriver.StepSetOutputs(channel);
                        if (!aDriver.ReadDeviceStatus())
                        {
                            return false;
                        }
                        break;
                    case BDriverValue:
                        bDriver.StepSetOutputs(channel);
                        if (!bDriver.ReadDeviceStatus())
                        {
                            return false;
                        }
                        break;
                    case CDriverValue:
                        cDriver.StepSetOutputs(channel);
                        if (!cDriver.ReadDeviceStatus())
                        {
                            return false;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        #endregion

        #region XYZ-Stage Move Methods

        /// <summary>
        /// Moves the selected driver to a given position.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="pos"></param>
        /// <param name="speed"></param>
        /// <param name="acc"></param>
        /// <param name="stop"></param>
        public void MoveToPos(byte driver, int pos, byte speed, byte acc, byte stop)
        {
            if (driver == ADriverValue)
            {
                aDriver.EnableMotor(stop);
                aDriver.StepLoadTrajPos(pos, acc, speed);
            }
            if (driver == BDriverValue)
            {
                bDriver.EnableMotor(stop);
                bDriver.StepLoadTrajPos(pos, acc, speed);
            }
            if (driver == CDriverValue)
            {
                cDriver.EnableMotor(stop);
                cDriver.StepLoadTrajPos(pos, acc, speed);
            }
        }

        /// <summary>
        /// Moves the selected driver in a certain direction - 
        /// direction is determined by the reverse variable. If reverse is 
        /// true, the motor will spin left, and will spin right otherwise.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="speed"></param>
        /// <param name="acc"></param>
        /// <param name="stop"></param>
        /// <param name="reverse"></param>
        public void MoveToVel(byte driver, byte speed, byte acc, byte stop, bool reverse)
        {
            if (driver == ADriverValue)
            {
                aDriver.EnableMotor(stop);
                aDriver.StepLoadTrajVel(acc, speed, reverse);
            }
            if (driver == BDriverValue)
            {
                bDriver.EnableMotor(stop);
                bDriver.StepLoadTrajVel(acc, speed, reverse);
            }
            if (driver == CDriverValue)
            {
                cDriver.EnableMotor(stop);
                cDriver.StepLoadTrajVel(acc, speed, reverse);
            }
        }

        /// <summary>
        /// This function will stop the selected driver from
        /// moving its active motor.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="stop"></param>
        public void StopMotor(byte driver, byte stop)
        {
            if (driver == ADriverValue)
            {
                aDriver.StopMotor(stop);
            }
            if (driver == BDriverValue)
            {
                bDriver.StopMotor(stop);
            }
            if (driver == CDriverValue)
            {
                cDriver.StopMotor(stop);
            }
        }

        #endregion

        #region Group Methods Region

        /// <summary>
        /// This function will assign the selected driver to
        /// the group. If leader is true, it will make the 
        /// selected driver the leader of the group. The 
        /// leader is necessary for the lower-level group
        /// move functions to operate.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="leader"></param>
        public void SetGroup(byte driver, bool leader)
        {
            if (driver == ADriverValue)
            {
                aDriver.SetGroup(leader);
            }

            if (driver == BDriverValue)
            {
                bDriver.SetGroup(leader);
            }

            if (driver == CDriverValue)
            {
                cDriver.SetGroup(leader);
            }
        }

        /// <summary>
        /// This function will remove the driver from
        /// the group.
        /// </summary>
        /// <param name="driver"></param>
        public void RemoveGroup(byte driver)
        {
            if (driver == ADriverValue)
            {
                aDriver.RemoveGroup();
            }

            if (driver == BDriverValue)
            {
                bDriver.RemoveGroup();
            }

            if (driver == CDriverValue)
            {
                cDriver.RemoveGroup();
            }
        }

        /// <summary>
        /// This function will establish the group settings before
        /// moving the group, enabling the motor on that driver
        /// and loading the trajectory data.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="stop"></param>
        /// <param name="pos"></param>
        /// <param name="acc"></param>
        /// <param name="speed"></param>
        public void MoveGroupSetup(byte driver, byte stop, int pos, 
            byte acc, byte speed)
        {
            Driver tempDriver;

            if (ADriverValue == driver)
            {
                tempDriver = aDriver;
            }
            else if (BDriverValue == driver)
            {
                tempDriver = bDriver;
            }
            else
            {
                tempDriver = cDriver;
            }

            tempDriver.EnableMotor(stop);

            tempDriver.StepLoadTrajGroup(pos, acc, speed);
        }

        /// <summary>
        /// This function will move the group of drivers based on the 
        /// trajectory data loaded in the MoveGroupSetup() function.
        /// </summary>
        public void MoveGroup()
        {
            NativeMethods.ServoStartMotion(NativeMethods.GROUP_ADDR);
        }

        #endregion

        #region General Method Region

        /// <summary>
        /// Report the position of the selected driver.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns>Returns the position of the selected driver 
        /// in long format.</returns>
        public long GetPos(byte driver)
        {
            if (driver == ADriverValue)
                return aDriver.GetPos();
            if (driver == BDriverValue)
                return bDriver.GetPos();
            if (driver == CDriverValue)
                return cDriver.GetPos();

            return 0;
        }

        #endregion
    }

    /// <summary>
    /// Driver handles logic specific to one driver on the XYZ-Stage, 
    /// including moving that driver and enabling/disabling 
    /// driver motors. 
    /// </summary>
    class Driver
    {
        #region Field Region

        private const byte stopAbrupt = NativeMethods.STOP_ABRUPT;
        private const byte stopSmooth = NativeMethods.STOP_SMOOTH;
        private const byte channelA = NativeMethods.SET_CH_A;
        private const byte channelB = NativeMethods.SET_CH_B;
        private const byte channelC = NativeMethods.SET_CH_C;

        private byte picoAddr, outVal, modType, modVersion, mode, minSpeed, 
            runCurrent, hldCurrent, adLimit, emAcc, speed, acc;
        private int pos;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns the module type of the 
        /// driver.
        /// Sets the module type of the 
        /// driver.
        /// </summary>
        public byte ModType
        {
            get { return modType; }
            set { modType = value; }
        }

        /// <summary>
        /// Returns the module version of the
        /// driver.
        /// Sets the module version of the
        /// driver.
        /// </summary>
        public byte ModVersion
        {
            get { return modVersion; }
            set { modVersion = value; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for the Driver class. 
        /// The addr variable corresponds the driver
        /// to one of three possible drivers - a, b or c.
        /// </summary>
        /// <param name="addr"></param>
        public Driver(byte addr)
        {
            picoAddr = addr;
            minSpeed = 1;
            runCurrent = 0;
            hldCurrent = 0;
            adLimit = 0;
            emAcc = 255;
            mode = NativeMethods.SPEED_8X;
            mode |= NativeMethods.IGNORE_LIMITS;
        }

        #endregion

        #region Establish Driver Region

        /// <summary>
        /// Sets the parameters for the driver - 
        /// address, mode, minimum speed, run and hld current,
        /// adlimit, and acceleration.
        /// </summary>
        /// <returns></returns>
        public bool StepSetParam()
        {
            if (!NativeMethods.StepSetParam(picoAddr, mode, minSpeed, runCurrent, 
                hldCurrent, adLimit, emAcc))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the outputs for the driver
        /// </summary>
        /// <param name="channel"></param>
        public void StepSetOutputs(byte channel)
        {
            switch (channel)
            {
                case channelA:
                    outVal = NativeMethods.TYPE_STD | NativeMethods.SET_CH_A;
                    break;
                case channelB:
                    outVal = NativeMethods.TYPE_STD | NativeMethods.SET_CH_B;
                    break;
                case channelC:
                    outVal = NativeMethods.TYPE_STD | NativeMethods.SET_CH_C;
                    break;
                default:
                    outVal = NativeMethods.TYPE_STD | NativeMethods.SET_CH_A;
                    break;
            }

            NativeMethods.StepSetOutputs(picoAddr, outVal);
        }

        /// <summary>
        /// Reads the current status of the device.
        /// </summary>
        /// <returns>Returns true if driver is ready, or
        /// false if there is an error.</returns>
        public bool ReadDeviceStatus()
        {
            NativeMethods.LdcnNoOp(picoAddr);

            if ((NativeMethods.LdcnGetStat(picoAddr) & NativeMethods.POWER_ON) == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Enables the driver motor with the 
        /// given stop type - abrupt or smooth.
        /// </summary>
        /// <param name="stop"></param>
        public void EnableMotor(byte stop)
        {
            byte stopType = stop;

            if (stopType == stopSmooth)
                stopType = NativeMethods.STOP_SMOOTH | NativeMethods.STP_ENABLE_AMP;
            if (stopType == stopAbrupt)
                stopType = NativeMethods.STOP_ABRUPT | NativeMethods.STP_ENABLE_AMP;

            NativeMethods.StepStopMotor(picoAddr, stopType);
        }

        /// <summary>
        /// Disables the driver motor and
        /// resets the position.
        /// </summary>
        public void DisableMotor()
        {
            NativeMethods.StepStopMotor(picoAddr, NativeMethods.STP_DISABLE_AMP);

            ResetPos();
        }

        #endregion

        #region Trajectory Methods

        /// <summary>
        /// Loads the trajectory data for the driver if the
        /// driver is moving in Position mode. Position 
        /// byte is stored in the mode byte.
        /// </summary>
        /// <param name="givenPos"></param>
        /// <param name="givenAcc"></param>
        /// <param name="givenSpeed"></param>
        public void StepLoadTrajPos(int givenPos, byte givenAcc, byte givenSpeed)
        {
            mode = NativeMethods.START_NOW | NativeMethods.LOAD_SPEED | NativeMethods.LOAD_ACC | NativeMethods.LOAD_POS;

            pos = 25 * givenPos;

            speed = givenSpeed;

            acc = givenAcc;

            NativeMethods.StepLoadTraj(picoAddr, mode, pos, speed, acc, 0);
        }

        /// <summary>
        /// Loads the trajectory data for the driver if the driver
        /// is part of a group and the group has received a move request.
        /// </summary>
        /// <param name="givenPos"></param>
        /// <param name="givenAcc"></param>
        /// <param name="givenSpeed"></param>
        public void StepLoadTrajGroup(int givenPos, byte givenAcc, byte givenSpeed)
        {
            mode = NativeMethods.LOAD_SPEED | NativeMethods.LOAD_ACC | NativeMethods.LOAD_POS;

            pos = 25 * givenPos;

            speed = givenSpeed;

            acc = givenAcc;

            NativeMethods.StepLoadTraj(picoAddr, mode, pos, speed, acc, 0);
        }

        /// <summary>
        /// Loads the trajectory data for the driver if the driver
        /// is moving in Velocity mode - no position byte is passed to
        /// the mode.
        /// </summary>
        /// <param name="givenAcc"></param>
        /// <param name="givenSpeed"></param>
        /// <param name="reverse"></param>
        public void StepLoadTrajVel(byte givenAcc, byte givenSpeed, bool reverse)
        {
            if (reverse)
                mode = NativeMethods.LOAD_SPEED | NativeMethods.LOAD_ACC | NativeMethods.STEP_REV;
            else
                mode = NativeMethods.LOAD_SPEED | NativeMethods.LOAD_ACC;

            speed = givenSpeed;
            acc = givenAcc;

            NativeMethods.StepLoadTraj(picoAddr, mode, pos, speed, acc, 0);

            NativeMethods.ServoStartMotion(picoAddr);
        }

        #endregion

        #region General Method Region

        /// <summary>
        /// Resets the position value of the driver - it does not
        /// actually move the driver's motor, merely sets the 
        /// current position to zero.
        /// </summary>
        public void ResetPos()
        {
            NativeMethods.StepResetPos(picoAddr);
        }

        /// <summary>
        /// Gets the current position of the driver's motor.
        /// </summary>
        /// <returns>Returns the position in long format.</returns>
        public long GetPos()
        {
            long returnPos;

            NativeMethods.LdcnReadStatus(picoAddr, NativeMethods.SEND_POS);
            returnPos = NativeMethods.StepGetPos(picoAddr) / 25;

            return returnPos;
        }

        /// <summary>
        /// Stops the motor from moving. Stop
        /// mode is either abrupt or smooth.
        /// </summary>
        /// <param name="stop"></param>
        public void StopMotor(byte stop)
        {
            byte stopMode = 0;

            if (stop == stopSmooth)
                stopMode = NativeMethods.STOP_SMOOTH | NativeMethods.STP_ENABLE_AMP;
            if (stop == stopAbrupt)
                stopMode = NativeMethods.STOP_ABRUPT | NativeMethods.STP_ENABLE_AMP;

            NativeMethods.StepStopMotor(picoAddr, stopMode);
        }

        #endregion

        #region Group Method Region

        /// <summary>
        /// Sets this driver to the group, and
        /// sets it up as a leader if leader is
        /// true.
        /// </summary>
        /// <param name="leader"></param>
        public void SetGroup(bool leader)
        {
            NativeMethods.LdcnSetGroupAddr(picoAddr, NativeMethods.GROUP_ADDR, leader);
        }

        /// <summary>
        /// Removes this driver from the group.
        /// </summary>
        public void RemoveGroup()
        {
            NativeMethods.LdcnSetGroupAddr(picoAddr, NativeMethods.GROUP_ADDR_TWO, true);
        }



        #endregion
    }

    /// <summary>
    /// Exception class specific to the XYZ-Stage.
    /// </summary>
    [Serializable]
    public class XYZStageError : Exception
    {
        /// <summary>
        /// Constructor for XYZStageError.
        /// </summary>
        /// <param name="message"></param>
        public XYZStageError(string message)
            : base(message)
        {
        }
    }
}
