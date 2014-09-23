// Author: Robert Dacunto
// Project: 3D Printer Force Feedback Program
// File: Logic.cs
// Classes: Logic, UpdateFeedbackEventArgs, ForceDetectedEventArgs,
// UpdateFeedbackEventArgs, MoveZStageThreadParam, CalibrationTextboxEventArgs,
// WorkItem

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ForceSensor;
using ZStage;
using XYZStage;
using Voltmeter;

namespace PrintingLogic
{
    #region Enum Region

    /// <summary>
    /// The directions the Z-Stage may move
    /// </summary>
    public enum ZStageMoveDirections 
    { 
        /// <summary>
        /// Up direction for Z-Stage movement
        /// </summary>
        Up, 

        /// <summary>
        /// Down direction for Z-Stage movement
        /// </summary>
        Down 
    }

    /// <summary>
    /// The phases of the calibration process
    /// </summary>
    public enum CalibrationStep 
    { 
        /// <summary>
        /// Phase 1 of calibration
        /// </summary>
        Phase1, 

        /// <summary>
        /// Phase 2 of calibration
        /// </summary>
        Phase2, 

        /// <summary>
        /// Phase 3 of calibration
        /// </summary>
        Phase3, 

        /// <summary>
        /// Phase 4, final phase of calibration
        /// </summary>
        Phase4 
    }

    /// <summary>
    /// The two modes for Z-Stage print stop.
    /// </summary>
    public enum PrintStopMode 
    { 
        /// <summary>
        /// Force-limited mode which will stop the Z-Stage
        /// from moving during a print when the force stop
        /// value is detected.
        /// </summary>
        Force, 

        /// <summary>
        /// Position-limited mode which will stop the Z-Stage
        /// from moving during a print when the force detected
        /// position is reached.
        /// </summary>
        Position 
    }

    /// <summary>
    /// The two modes for Z-Stage print return.
    /// </summary>
    public enum PrintReturnMode
    { 
        /// <summary>
        /// Jump-mode for print return - this will
        /// return the Z-Stage to its go-to position
        /// automatically.
        /// </summary>
        Jump, 
        
        /// <summary>
        /// Step-down mode for print return - this will
        /// return the Z-Stage to its go-to position
        /// in step decrements.
        /// </summary>
        Step 
    }

    #endregion

    #region Main Logic Class Region

    /// <summary>
    /// Logic is the main class for handling all 3D printing
    /// logic, interfacing the GUI layer with the 
    /// lower-level hardware-specific classes.
    /// </summary>
    public class Logic
    {
        #region Event Region

        /// <summary>
        /// Event to update the feedback box on the GUI thread.
        /// </summary>
        public event EventHandler<UpdateFeedbackEventArgs> UpdateFeedback;
        
        /// <summary>
        /// Event to update the sensor value on the GUI thread.
        /// </summary>
        public event EventHandler<UpdateFeedbackEventArgs> SensorUpdated;

        /// <summary>
        /// Event to update the Z-Stage position value on the GUI thread.
        /// </summary>
        public event EventHandler<UpdateFeedbackEventArgs> PositionUpdated;

        /// <summary>
        /// Event to update the Voltmeter voltage value on the GUI thread.
        /// </summary>
        public event EventHandler<UpdateFeedbackEventArgs> VoltageUpdated;

        /// <summary>
        /// Event to update the textbox controls on the calibration GUI form.
        /// </summary>
        public event EventHandler<CalibrationTextboxEventArgs> CalibrationTextboxUpdate;

        /// <summary>
        /// Event to fire when the voltage change value is detected.
        /// </summary>
        public event EventHandler<EventArgs> VoltageDetected;

        /// <summary>
        /// Event to play a sound when the force value is detected.
        /// </summary>
        public event EventHandler<EventArgs> ForceDetectedSound;

        /// <summary>
        /// Event to fire when the Z-Stage calibration is finished.
        /// </summary>
        public event EventHandler<EventArgs> FinishCalibration;

        /// <summary>
        /// Event to fire when the force stop value is detected during 
        /// Z-Stage tracking mode.
        /// </summary>
        public event EventHandler<ForceDetectedEventArgs> ForceDetectedDuringTracking;

        /// <summary>
        /// Event to fire when the force value is detected.
        /// </summary>
        public event EventHandler<ForceDetectedEventArgs> ForceDetectedDuringCalibration;

        /// <summary>
        /// Event to fire when the maximum height of the Z-Stage is reached
        /// during calibration.
        /// </summary>
        public event EventHandler<EventArgs> MaxHeightReachedDuringCalibration;

        /// <summary>
        /// Event to fire when the Z-Stage reaches maximum height during tracking. 
        /// </summary>
        public event EventHandler<EventArgs> MaxHeightReachedDuringTracking;

        #endregion

        #region Field Region

        /// <summary>
        /// The default value to detect change in the
        /// voltmeter.
        /// </summary>
        public const double DefaultVoltageChangeValue = 1.0;

        /// <summary>
        /// Numeric value associated with one second converted to
        /// milliseconds.
        /// </summary>
        public const int OneSecondInMillisec = 1000;

        /// <summary>
        /// The minimum force sensor stop value. User cannot
        /// set a force value less than this value.
        /// </summary>
        public const double MinForceSensorStopValue = 0.0;

        /// <summary>
        /// Maximum force sensor stop value. User cannot
        /// set a force value higher than this value.
        /// </summary>
        public const double MaxForceSensorStopValue = 100.0;

        /// <summary>
        /// The conversion of micron to "steps", which is the
        /// value used for moving the Z-Stage.
        /// </summary>
        public const double OneMicronToStep = 1.0;

        /// <summary>
        /// The default force sensor stop value, 2 milligrams,
        /// as determined by project needs.
        /// </summary>
        public const double DefaultForceSensorStopValue = 0.002;

        /// <summary>
        /// The number of decimal places to round the force value
        /// to when read-in from force gauge.
        /// </summary>
        public const int RoundForceSensorValueTo = 4;

        /// <summary>
        /// The number of decimal places to round the Z-Stage
        /// position value to when displayed on GUI.
        /// </summary>
        public const int RoundZStagePositionValueTo = 2;

        /// <summary>
        /// The minimum Z-Stage "step size" - or, .01 micron.
        /// </summary>
        public const double MinimumZStageStepSize = 0.01;

        /// <summary>
        /// The delay, in milliseconds, between moving the Z-Stage to
        /// the next position. This is required as the CPU processes the move
        /// quicker than the Z-Stage physically moves - with a 100 millisecond delay,
        /// we allow the Z-Stage to finish its step move before the CPU reaches the
        /// next move command.
        /// </summary>
        public const int ZStageMoveDelayInMillisec = 100;

        private Thread calibrationThread;
        private Thread voltmeterThread;

        private ForceSensor.Controller forceSensorController;
        private ZStage.Controller zStageController;
        private XYZStage.Controller xyzStageController;
        private Voltmeter.Controller voltmeterController;

        private CalibrationStep phase;
        private double stepSize;
        private int speed;
        private int dwellTime;
        private double stopValue;
        private double startPosition;
        private double forceDetectedPosition;
        private double offset;
        private int printingReturnSpeed;
        private double printingReturnStepSize;
        private double printingSlowDownStepSize;
        private bool isCalibrating;
        private bool calibrationFinished;
        private bool ignoreSensor;
        private double goToPosition;
        private List<double> calibrationPositions;
        private List<double> calibrationForce;
        private List<double> calibrationStepSizes;
        private List<int> calibrationSpeedValues;
        private List<double> calibrationOffsetValues;
        private bool maxHeightReachedDuringCalibration;

        /// <summary>
        /// The force stop value difference to trigger slow-down
        /// mode for the Z-Stage during a print if Slow-Down mode
        /// is activated. When the force value is .0005 less
        /// than the stop value, it will trigger the slow-down
        /// to begin.
        /// </summary>
        public const double PrintSlowDownOffset = .0005;

        private PrintStopMode printStopMode;
        private PrintReturnMode printReturnMode;
        private bool printing;
        private bool printSlowDown;
        private bool printReturnToZero;
        private bool maxHeightReachedDuringPrinting;

        /// <summary>
        /// The default serial number for the force sensor. This
        /// is automatically generated by the program when connecting
        /// to the force sensor.
        /// </summary>
        public readonly string DefaultSerialNumber;

        /// <summary>
        /// Moving a motor on the XYZ-Stage to the left
        /// is associated with "true"
        /// </summary>
        public const bool xyzStageMoveLeft = true;

        /// <summary>
        /// Moving a motor on the XYZ-Stage to the right
        /// is associated with "false"
        /// </summary>
        public const bool xyzStageMoveRight = false;

        private readonly byte aDriver;
        private readonly byte bDriver;
        private readonly byte cDriver;
        private readonly byte aChannel;
        private readonly byte bChannel;
        private readonly byte cChannel;
        private readonly byte smoothStop;
        private readonly byte abruptStop;

        private bool aDriverEnabled;
        private bool bDriverEnabled;
        private bool cDriverEnabled;

        private bool xyzStageConnected;

        private double lastVoltageValue;
        private double currentVoltageValue;
        private double voltageChangeValue;

        /// <summary>
        /// This bool variable is used to prevent
        /// multiple voltage triggers from happening at once.
        /// It is set true when the first voltage trigger is 
        /// detected, and is only set false when a print
        /// is finished.
        /// </summary>
        public bool IsVoltageDetected;

        private List<TrackingData> trackingData;
        private bool isTracking;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns the current Z-Stage step size.
        /// </summary>
        public double StepSize
        {
            get { return stepSize; }
        }

        /// <summary>
        /// Returns the current Z-Stage speed.
        /// </summary>
        public int Speed
        {
            get { return speed; }
        }

        /// <summary>
        /// Returns the current Z-Stage dwell time.
        /// </summary>
        public int DwellTime
        {
            get { return dwellTime; }
        }

        /// <summary>
        /// Returns the current force sensor stop value.
        /// </summary>
        public double StopValue
        {
            get { return stopValue; }
        }

        /// <summary>
        /// Returns the current Z-Stage start position to
        /// use during calibration.
        /// </summary>
        public double StartPosition
        {
            get { return startPosition; }
        }

        /// <summary>
        /// Returns the current Z-Stage offset.
        /// </summary>
        public double Offset
        {
            get { return offset; }
        }

        /// <summary>
        /// Returns whether or not the Z-Stage
        /// is currently calibrating.
        /// </summary>
        public bool IsCalibrating
        {
            get { return isCalibrating; }
        }

        /// <summary>
        /// Returns whether or not the Z-Stage
        /// finished calibration.
        /// </summary>
        public bool CalibrationFinished
        {
            get { return calibrationFinished; }
            set { calibrationFinished = value; }
        }

        /// <summary>
        /// Returns whether or not the sensor is set to ignore
        /// or sets the ignoreSensor flag on/off.
        /// </summary>
        public bool IgnoreSensor
        {
            get { return ignoreSensor; }
            set { ignoreSensor = value; }
        }

        /// <summary>
        /// Returns the byte value associated with the XYZ 
        /// Stage A Driver.
        /// </summary>
        public byte ADriver
        {
            get { return aDriver; }
        }

        /// <summary>
        /// Returns the byte value associated with the XYZ
        /// Stage B Driver.
        /// </summary>
        public byte BDriver
        {
            get { return bDriver; }
        }

        /// <summary>
        /// Returns the byte value associated with the XYZ
        /// Stage C Driver.
        /// </summary>
        public byte CDriver
        {
            get { return cDriver; }
        }

        /// <summary>
        /// Returns the byte value associated with the XYZ
        /// Stage A Channel.
        /// </summary>
        public byte AChannel
        {
            get { return aChannel; }
        }

        /// <summary>
        /// Returns the byte value associated with the XYZ
        /// Stage B Channel.
        /// </summary>
        public byte BChannel
        {
            get { return bChannel; }
        }

        /// <summary>
        /// Returns the byte value associated with the XYZ
        /// Stage C Channel.
        /// </summary>
        public byte CChannel
        {
            get { return cChannel; }
        }

        /// <summary>
        /// Returns whether or not the XYZ Stage A Driver is enabled.
        /// </summary>
        public bool ADriverEnabled
        {
            get { return aDriverEnabled; }
        }


        /// <summary>
        /// Returns whether or not the XYZ Stage B Driver is enabled.
        /// </summary>
        public bool BDriverEnabled
        {
            get { return bDriverEnabled; }
        }

        /// <summary>
        /// Returns whether or not the XYZ Stage C Driver is enabled.
        /// </summary>
        public bool CDriverEnabled
        {
            get { return cDriverEnabled; }
        }

        /// <summary>
        /// Returns the byte value associated with the XYZ Stage
        /// Abrupt Stop mode.
        /// </summary>
        public byte AbruptStop
        {
            get { return abruptStop; }
        }

        /// <summary>
        /// Returns the byte value associated with the XYZ Stage 
        /// Smooth Stop mode.
        /// </summary>
        public byte SmoothStop
        {
            get { return smoothStop; }
        }

        /// <summary>
        /// Returns whether or not the XYZ Stage is connected.
        /// </summary>
        public bool XYZStageConnected
        {
            get { return xyzStageConnected; }
        }

        /// <summary>
        /// Appends each position, force, speed and step size values
        /// during each stage of Z-Stage calibration to a string
        /// and returns the string to the GUI form.
        /// </summary>
        public string CalibrationPositions
        {
            get
            {
                StringBuilder positions = new StringBuilder();

                for (int i = 0; i < calibrationPositions.Count; i++)
                {
                    positions.AppendLine("Position: " + 
                        calibrationPositions[i].ToString() + "\n" +
                        "Force: " + calibrationForce[i].ToString() + "\n" +
                        "Speed: " + calibrationSpeedValues[i].ToString() + "\n" +
                        "Step Size: " + calibrationStepSizes[i].ToString() +
                        "\n\n");
                }

                return positions.ToString();
            }
        }

        /// <summary>
        /// Returns whether or not the Z-Stage is currently engaged in a print.
        /// </summary>
        public bool Printing
        {
            get { return printing; }
        }

        /// <summary>
        /// Returns the Voltmeter change value.
        /// </summary>
        public double VoltageChangeValue
        {
            get { return voltageChangeValue; }
            set
            {
                if (value < 0)
                    throw new Exception
                        ("Invalid voltage change, must be greater than 0.");

                voltageChangeValue = value;
            }
        }

        /// <summary>
        /// Returns the current Z-Stage print stop mode.
        /// </summary>
        public PrintStopMode PrintStopMode
        {
            get { return printStopMode; }
        }

        /// <summary>
        /// Returns the current Z-Stage print return mode.
        /// </summary>
        public PrintReturnMode PrintReturnMode
        {
            get { return printReturnMode; }
        }

        /// <summary>
        /// Returns the Z-Stage print return speed.
        /// </summary>
        public int PrintingReturnSpeed
        {
            get { return printingReturnSpeed; }
        }

        /// <summary>
        /// Returns the Z-Stage print return step size.
        /// </summary>
        public double PrintingReturnStepSize
        {
            get { return printingReturnStepSize; }
        }

        /// <summary>
        /// Returns the Z-Stage printing go to position.
        /// </summary>
        public double GoToPosition
        {
            get { return goToPosition; }
        }

        /// <summary>
        /// Returns the Z-Stage print slow down step size, 
        /// or sets the Z-Stage print slow down step size.
        /// </summary>
        public double PrintingSlowDownStepSize
        {
            get { return printingSlowDownStepSize; }
            set 
            { 
                if (value < 0 || value + zStageController.Position() 
                    > ZStage.Controller.MaxHeight)
                {
                    throw new Exception("Invalid step size.");
                }

                printingSlowDownStepSize = value; 
            }
        }

        /// <summary>
        /// Gets whether printReturnToZero is true or false,
        /// or sets it to true or false, depending on whether
        /// or not the printReturnMode is Step or Jump.
        /// </summary>
        public bool PrintReturnToZero
        {
            get { return printReturnToZero; }
            set
            {
                if (value == true && printReturnMode == PrintReturnMode.Step)
                    printReturnToZero = true;
                else if (value == true && printReturnMode == PrintReturnMode.Jump)
                    throw new Exception("You cannot set the Z-Stage to return to " +
                "the minimum height if return mode is set to Jump");
                else
                    printReturnToZero = false;
            }
        }

        /// <summary>
        /// Returns whether or not Z-Stage print slow down mode is enabled, 
        /// or sets Z-Stage print slow down mode on/off.
        /// </summary>
        public bool PrintSlowDown
        {
            get { return printSlowDown; }
            set { printSlowDown = value; }
        }

        /// <summary>
        /// Returns the current force detected position for the Z-Stage.
        /// </summary>
        public double ForceDetectedPosition
        {
            get { return forceDetectedPosition; }
        }

        /// <summary>
        /// Returns the list of tracking data to use for Z-Stage tracking mode.
        /// </summary>
        public List<TrackingData> TrackingDataList
        {
            get { return trackingData; }
            set
            {
                trackingData = value;
            }
        }

        /// <summary>
        /// Returns whether the Z-Stage is currently in tracking mode.
        /// </summary>
        public bool IsTracking
        {
            get { return isTracking; }
        }

        /// <summary>
        /// Returns whether the Z-Stage reached the maximum height
        /// during calibration.
        /// </summary>
        public bool DidMaxHeightReachedDuringCalibration
        {
            get { return maxHeightReachedDuringCalibration; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for logic class. Establish initial values for
        /// Z-Stage printing and XYZ-Stage driver values
        /// </summary>
        public Logic()
        {
            #region Hardware-Specific Object Instantiation

            // These objects correspond to each individual hardware
            // They contain all logic specific to that hardware
            forceSensorController = new ForceSensor.Controller();
            zStageController = new ZStage.Controller();
            xyzStageController = new XYZStage.Controller();
            voltmeterController = new Voltmeter.Controller();

            // Obtain the serial number of the force sensor connected
            // to the PC. If more than one force sensors are connected,
            // it will retrieve the serial number of the first force
            // sensor it detects
            DefaultSerialNumber = forceSensorController.DefaultSerialNumber();

            #endregion

            #region Z-Stage Settings

            ResetZStageSettings();

            #endregion

            #region XYZ-Stage Driver and Channel Values Region

            aDriver = XYZStage.Controller.ADriverValue;
            bDriver = XYZStage.Controller.BDriverValue;
            cDriver = XYZStage.Controller.CDriverValue;
            smoothStop = XYZStage.Controller.SmoothStop;
            abruptStop = XYZStage.Controller.AbruptStop;
            aChannel = XYZStage.Controller.ChannelA;
            bChannel = XYZStage.Controller.ChannelB;
            cChannel = XYZStage.Controller.ChannelC;

            #endregion

            #region Voltage Settings Region

            voltageChangeValue = DefaultVoltageChangeValue;

            #endregion

            ForceDetectedDuringCalibration += item_ForceDetected;
            MaxHeightReachedDuringCalibration += item_MaxHeightReachedDuringCalibration;
        }

        #endregion

        #region Connection Region

        /// <summary>
        /// Connects to the force sensor with the given
        /// serial number.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>Returns true if connection was successful, or 
        /// throws an exception if error occurred.</returns>
        public bool ConnectForceSensor(string serialNumber)
        {            
            try
            {
                forceSensorController.Connect(serialNumber);
            }
            catch (SENSOR_ERROR)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Connects to the Z-Stage and moves it to 
        /// position 0.
        /// </summary>
        /// <returns>Returns true if connection was successful, or
        /// throws an exception if error occurred.</returns>
        public bool ConnectZStage()
        {
            try
            {
                zStageController.Connect();
            }
            catch (ZStageError)
            {
                throw;
            }

            UpdateFeedbackTextBox(zStageController.FeedbackText.ToString());

            zStageController.MoveToZero();

            return true;
        }

        /// <summary>
        /// Connects to the XYZ-Stage. Calibration
        /// is required in order to select different
        /// channels on each driver. If calibration
        /// is not done, it would not be possible to
        /// select a different channel for a driver
        /// and move.
        /// </summary>
        /// <param name="portName"></param>
        /// <returns>Returns true if connection and
        /// calibration is successful.</returns>
        public bool ConnectXYZStage(string portName)
        {
            try
            {
                xyzStageController.Connect(portName);
            }
            catch (XYZStageError)
            {
                throw; ;
            }

            xyzStageConnected = true;

            BeginXYZStageCalibration();

            return true;
        }

        /// <summary>
        /// Disconnects from the force sensor.
        /// </summary>
        /// <returns>Returns true if disconnection is 
        /// successful, or throws an exception if
        /// an error occurred.</returns>
        public bool DisconnectForceSensor()
        {
            try
            {
                forceSensorController.Disconnect();
            }
            catch (SENSOR_ERROR)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Disconnects from the Z-Stage.
        /// </summary>
        /// <returns>Returns true if disconnection is
        /// successful, or throws an exception if
        /// an error occurred.</returns>
        public bool DisconnectZStage()
        {
            try
            {
                zStageController.Disconnect();
            }
            catch (ZStageError)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Disconnects from all active
        /// hardware. Typically run when application is
        /// closing to ensure that all hardware is 
        /// safely disconnected before program is
        /// closed.
        /// </summary>
        /// <returns>Returns true if all hardware successfuly
        /// disconnects.</returns>
        public bool KillConnections()
        {
            if (zStageController.Connected)
            {
                zStageController.MoveToZero();
                DisconnectZStage();
            }

            if (forceSensorController.Connected)
                DisconnectForceSensor();

            if (voltmeterController.Connected)
                DisconnectVoltmeter();

            if (xyzStageController.Connected)
                DisconnectXYZStage();

            return true;
        }

        /// <summary>
        /// Connects to the voltmeter and automatically
        /// begins thread to monitor voltage.
        /// </summary>
        /// <returns>Returns true if connection is successful,
        /// or false otherwise.</returns>
        public bool ConnectVoltmeter()
        {
            if (voltmeterController.Connect() == true)
            {
                this.voltmeterThread =
                    new Thread(new ThreadStart(this.voltmeterThreadProcSafe));
                this.voltmeterThread.Start();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Disconnects from the voltmeter and
        /// aborts the voltage-monitoring thread.
        /// </summary>
        /// <returns>Returns true if disconnection is
        /// successful.</returns>
        public bool DisconnectVoltmeter()
        {
            this.voltmeterThread.Abort();

            voltmeterController.Disconnect();

            return true;
        }

        /// <summary>
        /// Disconnects the XYZ-Stage.
        /// </summary>
        /// <returns>Returns true if disconnection is
        /// successful.</returns>
        public bool DisconnectXYZStage()
        {
            xyzStageController.Disconnect();

            return true;
        }
        
        #endregion

        #region Z-Stage General Methods

        /// <summary>
        /// This function will restore the Z-Stage settings
        /// to their initial default values.
        /// </summary>
        public void ResetZStageSettings()
        {
            phase = CalibrationStep.Phase1;
            stepSize = OneMicronToStep;
            speed = OneSecondInMillisec;
            printingReturnStepSize = OneMicronToStep;
            printingReturnSpeed = OneSecondInMillisec;
            printSlowDown = false;
            printingSlowDownStepSize = OneMicronToStep;
            dwellTime = OneSecondInMillisec;
            stopValue = DefaultForceSensorStopValue;
            startPosition = ZStage.Controller.MinHeight;
            forceDetectedPosition = ZStage.Controller.MinHeight;
            offset = OneMicronToStep;
            isCalibrating = false;
            calibrationFinished = false;
            printStopMode = PrintStopMode.Force;
            printReturnMode = PrintReturnMode.Jump;
            goToPosition = ZStage.Controller.MinHeight;


            calibrationPositions = new List<double>();
            calibrationForce = new List<double>();
            calibrationStepSizes = new List<double>();
            calibrationSpeedValues = new List<int>();
            calibrationOffsetValues = new List<double>();
        }

        /// <summary>
        /// This function will set the Z-Stage's print stop mode
        /// to whichever value it is currently not set to - ie,
        /// if it is already in force mode, it will then set it
        /// to position mode.
        /// </summary>
        public void SetZStagePrintStopMode()
        {
            if (printStopMode == PrintStopMode.Force)
                printStopMode = PrintStopMode.Position;
            else
                printStopMode = PrintStopMode.Force;
        }

        /// <summary>
        /// This function will set the Z-Stage's print return mode
        /// to whichever value it is currently not set to - ie,
        /// if it is already in jump mode, it will then set it to
        /// step down mode.
        /// </summary>
        public void SetZStagePrintReturnMode()
        {
            if (printReturnMode == PrintReturnMode.Jump)
                printReturnMode = PrintReturnMode.Step;
            else
                printReturnMode = PrintReturnMode.Jump;
        }

        /// <summary>
        /// Returns the Z-Stage position string format,
        /// rounded to two decimal places.
        /// </summary>
        /// <returns></returns>
        public string GetZStagePosition()
        {
            return String.Format("{0:0.00}",
                        zStageController.Position());
        }

        /// <summary>
        /// Async task that is triggered when the voltage
        /// is detected when tracking mode is engaged. 
        /// It will move the Z-Stage until it detects the stop
        /// value (or until the maximum height is reached), and
        /// then fire the appropriate event, passing position
        /// and force value data back to the GUI.
        /// </summary>
        public async Task BeginTracking()
        {
            await Task.Run(() =>
                {
                    isTracking = true;

                    double sensorValue = 
                        Math.Round(forceSensorController.UpdateSensor(), RoundForceSensorValueTo);

                    double position;

                    if (goToPosition < ZStage.Controller.MinHeight)
                        goToPosition = ZStage.Controller.MinHeight;
                    else if (goToPosition > ZStage.Controller.MaxHeight)
                        goToPosition = ZStage.Controller.MaxHeight;
                    
                    try
                    {
                        MoveZStageToPosition(goToPosition);
                    }
                    catch (Exception ex)
                    {
                        UpdateFeedbackTextBox(ex.Message);
                        isTracking = false;
                        return;
                    }

                    OnSensorUpdated(new UpdateFeedbackEventArgs(sensorValue.ToString()));

                    while (GoodToMove(ZStageMoveDirections.Up) 
                        && sensorValue < stopValue)
                    {
                        Thread.Sleep(speed);

                        OnSensorUpdated(new UpdateFeedbackEventArgs(sensorValue.ToString()));

                        sensorValue = 
                            Math.Round(forceSensorController.UpdateSensor(), RoundForceSensorValueTo);

                        position = zStageController.Position();

                        if (sensorValue >= stopValue)
                        {
                            OnSensorUpdated(new UpdateFeedbackEventArgs(sensorValue.ToString()));

                            goToPosition = position - offset;

                            if (goToPosition < ZStage.Controller.MinHeight)
                                goToPosition = ZStage.Controller.MinHeight;
                            else if (goToPosition > ZStage.Controller.MaxHeight)
                                goToPosition = ZStage.Controller.MaxHeight;

                            trackingData.Add(new TrackingData(position, sensorValue));

                            if (ForceDetectedDuringTracking != null)
                                ForceDetectedDuringTracking(this,
                                    new ForceDetectedEventArgs(
                                        position, sensorValue, false));

                            isTracking = false;

                            break;
                        }

                        if (position >= ZStage.Controller.MaxHeight ||
                                position + zStageController.StepSize >= 
                                    ZStage.Controller.MaxHeight)
                        {
                            OnSensorUpdated(new UpdateFeedbackEventArgs(sensorValue.ToString()));

                            if (MaxHeightReachedDuringTracking != null)
                                MaxHeightReachedDuringTracking(this,
                                    null);

                            isTracking = false;

                            break;
                        }

                        try
                        {
                            MoveZStageToPosition(position + zStageController.StepSize);
                        }
                        catch (Exception ex)
                        {
                            UpdateFeedbackTextBox(ex.Message);

                            isTracking = false;
                            break;
                        }
                    }

                    isTracking = false;
                });
        }

        #endregion

        #region Calibration Method Region

        /// <summary>
        /// Entry-point for Z-Stage calibration process. 
        /// First checks to make sure calibration has not already
        /// finished or that it is not calibrating. 
        /// </summary>
        public async Task Calibrate(CancellationTokenSource cancellationSource)
        {
            isCalibrating = true;
            maxHeightReachedDuringCalibration = false;

            try
            {
                await Task.Run(() =>
                    {
                        try
                        {
                            for (phase = CalibrationStep.Phase1; phase <= CalibrationStep.Phase4; phase++)
                            {
                                cancellationSource.Token.ThrowIfCancellationRequested();

                                Thread.Sleep(OneSecondInMillisec * 4);

                                UpdateFeedbackTextBox("\nBeginning " + phase.ToString());

                                ValuesDuringStage(cancellationSource.Token);

                                ExecutePhase(cancellationSource.Token);

                                if (maxHeightReachedDuringCalibration)
                                    break;
                            }

                            goToPosition = forceDetectedPosition - offset;

                            if (goToPosition < ZStage.Controller.MinHeight)
                                goToPosition = ZStage.Controller.MinHeight;

                            if (maxHeightReachedDuringCalibration)
                            {
                                calibrationFinished = false;
                            }
                            else
                            {
                                calibrationFinished = true;

                                UpdateFeedbackTextBox("\nCalibration finished. Press OK to accept these values.\n");
                            }

                            isCalibrating = false;
                            FinishCalibrating();
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (ZStageError)
                        {
                            //handle Z-Stage errors here...
                            throw;
                        }
                    }, cancellationSource.Token);
            }
            catch (OperationCanceledException)
            {
                isCalibrating = false;
                throw;
            }
        }


        /// <summary>
        /// Sets the values of each Z-Stage setting for each
        /// stage of the Z-Stage calibration process.
        /// Will update the GUI layer with the new values
        /// for each setting.
        /// </summary>
        /// <param name="stepSize"></param>
        /// <param name="speed"></param>
        /// <param name="stopValue"></param>
        /// <param name="startPosition"></param>
        /// <param name="offset"></param>
        private void SetValues(double stepSize, int speed, double stopValue, 
            double startPosition, double offset, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.stepSize = stepSize;
            this.speed = speed;
            this.stopValue = stopValue;
            this.startPosition = startPosition;
            this.offset = offset;

            zStageController.StepSize = stepSize;

            if (startPosition < ZStage.Controller.MinHeight)
                startPosition = ZStage.Controller.MinHeight;

            if (CalibrationTextboxUpdate != null)
                CalibrationTextboxUpdate(this, new CalibrationTextboxEventArgs(stepSize, speed,
                    startPosition, offset, stopValue));
        }

        /// <summary>
        /// Once the values for each Z-Stage setting have
        /// been set, the calibration phase may begin. 
        /// First, move the Z-Stage to its new starting position 
        /// and then create a WorkItem object which will 
        /// carry out the bulk of the work for the calibration.
        /// </summary>
        private void ExecutePhase(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                MoveZStageToPosition(startPosition);
            }
            catch (Exception ex)
            {
                UpdateFeedbackTextBox(ex.Message);
            }

            try
            {
                PerformCalibrationWork(new MoveZStageThreadParam(speed,
                    ZStageMoveDirections.Up, true, stopValue), 
                    cancellationToken);
            }
            catch (ZStageError)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        /// <summary>
        /// Main loop to deal with calibration process.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="cancellationToken"></param>
        private void PerformCalibrationWork(MoveZStageThreadParam parameter,
            CancellationToken cancellationToken)
        {
            #region Set Up Region

            double position;
            Stopwatch time = new Stopwatch();
            int elapsedTime = 0;
            ZStageMoveDirections direction = parameter.Direction;
            int speed = parameter.Speed;

            #endregion

            #region Main Loop Region

            // This loop will run until the user cancels, the Z-Stage
            // reaches its max or min height, or until the force value 
            // is detected.
            while (parameter.IsCalibrating == true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Put thread to sleep based on user-defined speed
                // and how long it takes to compute sensor value average.
                CalibrationThreadToSleep(speed, elapsedTime);

                // Update position of Z-Stage on GUI layer
                position = zStageController.Position();

                cancellationToken.ThrowIfCancellationRequested();

                // Check to make sure the move is allowable before processing - 
                // that is, it is within the position bounds of Z-Stage
                if (ValidMoveDuringCalibration(direction, position, parameter) == false)
                    break;

                cancellationToken.ThrowIfCancellationRequested();

                // Check to make sure the sensor is not already at or exceeding
                // its stop value.
                if (CheckSensorValueWithAverage(direction, parameter, ref elapsedTime) == false)
                    break;

                cancellationToken.ThrowIfCancellationRequested();

                // Move the Z-Stage up or down.
                try
                {
                    CompleteMove(direction);
                }
                catch (ZStageError)
                {
                    throw;
                }

                cancellationToken.ThrowIfCancellationRequested();

                // Re-check sensor value without computing average to see if
                // force has been met before proceeding to next move cycle
                if (CheckSensorValueWithoutAverage(direction, parameter) == false)
                    break;

                cancellationToken.ThrowIfCancellationRequested();
            }

            #endregion
        }

        /// <summary>
        /// Puts the thread to sleep during calibration to achieve proper 
        /// speed during move - as the calibration algorithim calculates the average
        /// force value with each move, we need to make up for the time it takes to 
        /// compute the average by subtracting the time to compute from the user input
        /// speed time. Otherwise, the move operation would take much longer (adding
        /// user speed time plus average computation time).
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="elapsedTime"></param>
        private void CalibrationThreadToSleep(int speed, int elapsedTime)
        {
            int sleepTime = 0;

            sleepTime = speed - elapsedTime;

            if (sleepTime <= 0)
                sleepTime = 1;

            Thread.Sleep(sleepTime);
        }

        /// <summary>
        /// This function will determine if the Z-Stage is
        /// able to move up or down before checking for
        /// sensor value. If the Z-Stage is at its min or
        /// max heights, and is trying to go down or up
        /// respectively, then it will not process
        /// the move.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="position"></param>
        /// <param name="parameter"></param>
        /// <returns>Returns true if the move request
        /// is valid, false otherwise.</returns>
        private bool ValidMoveDuringCalibration(ZStageMoveDirections direction,
            double position, MoveZStageThreadParam parameter)
        {
            // If the Z-Stage is at its min height and is trying
            // to move down, cancel the request.
            if (direction == ZStageMoveDirections.Down)
            {
                if ((position - zStageController.StepSize) < ZStage.Controller.MinHeight)
                {
                    return false;
                }
            }
            else if (direction == ZStageMoveDirections.Up)
            {
                // If the Z-Stage is at its max height and is trying to go up,
                // check to see if the Z-Stage is calibrating. If so, report
                // that max height was reached to the GUI layer to inform the user
                // that they need to adjust Z-Stage physical position on platform
                // to reach the force sensor. Cancel the move request in either
                // scenario.
                if ((position + zStageController.StepSize) > ZStage.Controller.MaxHeight)
                {
                    if (parameter.IsCalibrating == true)
                    {
                        if (MaxHeightReachedDuringCalibration != null)
                            MaxHeightReachedDuringCalibration(this, new EventArgs());

                        return false;
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This function will check the sensor value by computing
        /// an average of 50 sensor values to accomodate
        /// fluctuation in air pressure.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="parameter"></param>
        /// <param name="elapsedTime"></param>
        /// <returns>Returns true if the force has not been detected,
        /// false otherwise.</returns>
        private bool CheckSensorValueWithAverage(ZStageMoveDirections direction,
            MoveZStageThreadParam parameter, ref int elapsedTime)
        {
            Stopwatch time = new Stopwatch();

            time.Start();

            double numericSensorValue = Math.Round(GatherSensorAverage(),
                Logic.RoundForceSensorValueTo);

            time.Stop();

            elapsedTime = (int)time.ElapsedMilliseconds;

            return CheckSensorValue(direction, parameter,
                numericSensorValue);
        }

        /// <summary>
        /// Computes one sensor value.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="parameter"></param>
        /// <returns>Returns true if the force has not been detected,
        /// false otherwise.</returns>
        private bool CheckSensorValueWithoutAverage(ZStageMoveDirections direction,
            MoveZStageThreadParam parameter)
        {
            double numericSensorValue =
                Math.Round(forceSensorController.UpdateSensor(), Logic.RoundForceSensorValueTo);

            return CheckSensorValue(direction, parameter,
                numericSensorValue);
        }

        /// <summary>
        /// Checks to see if the force stop value has been detected,
        /// either as an average or as a single sensor value.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="parameter"></param>
        /// <param name="numericSensorValue"></param>
        /// <returns>Returns true if the force has not been detected,
        /// false otherwise.</returns>
        private bool CheckSensorValue(ZStageMoveDirections direction,
            MoveZStageThreadParam parameter, double numericSensorValue)
        {
            double position = zStageController.Position();

            string value = numericSensorValue.ToString();

            this.OnSensorUpdated(new UpdateFeedbackEventArgs(value));

            if ((numericSensorValue >= parameter.ForceSensorStopValue) &&
                direction == ZStageMoveDirections.Up)
            {
                if (ForceDetectedDuringCalibration != null)
                    ForceDetectedDuringCalibration(this,
                       new ForceDetectedEventArgs(position, numericSensorValue,
                           parameter.IsCalibrating));

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function will tell the Z-Stage to move up or down.
        /// </summary>
        /// <param name="direction"></param>
        private void CompleteMove(ZStageMoveDirections direction)
        {
            if (direction == ZStageMoveDirections.Down)
            {
                try
                {
                    zStageController.MoveDown();
                }
                catch (ZStageError)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    zStageController.MoveUp();
                }
                catch (ZStageError)
                {
                    throw;
                }
            }

            Thread.Sleep(ZStageMoveDelayInMillisec);

            this.OnPositionUpdated(
                new UpdateFeedbackEventArgs(zStageController.Position().ToString()));
        }

        /// <summary>
        /// This function will collect fifty sensor values
        /// 2 milliseconds apart and average them to create
        /// the average sensor value to use for force
        /// detection in a move operation. This is to help
        /// mitigate the results of air fluctuation in force
        /// sensor readings.
        /// </summary>
        /// <returns></returns>
        private double GatherSensorAverage()
        {
            Queue<double> temp = new Queue<double>();
            double numericSensorUpdate;
            double average = 0.0;
            int twoMilliseconds = 2;
            int maxLoopAverage = 50;

            for (int i = 0; i < maxLoopAverage; i++)
            {
                numericSensorUpdate = Math.Round(forceSensorController.UpdateSensor(),
                    Logic.RoundForceSensorValueTo);
                temp.Enqueue(numericSensorUpdate);
                Thread.Sleep(twoMilliseconds);
            }

            average = temp.Average();

            return average;
        }

        /// <summary>
        /// This function determines what values to set to the 
        /// Z-Stage for each step of the process. Currently, each
        /// value is hard-coded for each phase.
        /// </summary>
        private void ValuesDuringStage(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            double tempStartPosition = forceDetectedPosition - offset;

            if (tempStartPosition < ZStage.Controller.MinHeight)
                tempStartPosition = ZStage.Controller.MinHeight;

            int listPosition;

            try
            {
                // The list position for the offset in Phase 4 has to be one
                // less than the current position because there are only three
                // offset values in the list, as opposed to four for the rest
                // of the settings.
                switch (phase)
                {
                    case CalibrationStep.Phase1:
                        listPosition = 0;
                        tempStartPosition = startPosition;
                        SetValues(calibrationStepSizes.ElementAt(listPosition),
                            calibrationSpeedValues.ElementAt(listPosition), stopValue,
                            tempStartPosition, calibrationOffsetValues.ElementAt(listPosition),
                            cancellationToken);
                        listPosition++;
                        break;
                    case CalibrationStep.Phase2:
                        listPosition = 1;
                        SetValues(calibrationStepSizes.ElementAt(listPosition),
                            calibrationSpeedValues.ElementAt(listPosition), stopValue,
                            tempStartPosition, calibrationOffsetValues.ElementAt(listPosition),
                            cancellationToken);
                        listPosition++;
                        break;
                    case CalibrationStep.Phase3:
                        listPosition = 2;
                        SetValues(calibrationStepSizes.ElementAt(listPosition),
                            calibrationSpeedValues.ElementAt(listPosition), stopValue,
                            tempStartPosition, calibrationOffsetValues.ElementAt(listPosition),
                            cancellationToken);
                        listPosition++;
                        break;
                    case CalibrationStep.Phase4:
                        listPosition = 3;
                        SetValues(calibrationStepSizes.ElementAt(listPosition),
                            calibrationSpeedValues.ElementAt(listPosition), stopValue,
                            tempStartPosition, calibrationOffsetValues.ElementAt(--listPosition),
                            cancellationToken);
                        break;
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        #endregion

        #region Z-Stage Move Methods
        
        /// <summary>
        /// This function will move the Z-Stage to the
        /// given position, as long as it is within the
        /// bounds of the Z-Stage. 
        /// Once it has moved to its new position, update
        /// the GUI layer with the new position value.
        /// </summary>
        /// <param name="position"></param>
        public void MoveZStageToPosition(double position)
        {
            if (position < ZStage.Controller.MinHeight ||
                position > ZStage.Controller.MaxHeight)
                throw new Exception("Invalid position. Must be within Z-Stage " +
                    "position bounds.");

            try
            {
                zStageController.MoveToPosition(position);
            }
            catch (Exception)
            {
                throw;
            }

            Thread.Sleep(ZStageMoveDelayInMillisec);

            this.OnPositionUpdated(new UpdateFeedbackEventArgs(String.Format("{0:0.00}", 
                zStageController.Position())));
        }

        /// <summary>
        /// This function will simply move the Z-Stage to
        /// its default minimum height.
        /// </summary>
        /// <returns>Returns true if move was successful.</returns>
        public bool ZStageReset()
        {
            try
            {
                MoveZStageToPosition(ZStage.Controller.MinHeight);
            }
            catch (Exception ex)
            {
                UpdateFeedbackTextBox(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if the Z-Stage can move. If direction is down,
        /// we only care about whether or not the Z-Stage has already
        /// reached the min height. If the position is up, we only care
        /// about if the Z-Stage will exceed the maximum height.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>True if the move is allowed, false otherwise.</returns>
        public bool GoodToMove(ZStageMoveDirections direction)
        {
            if (direction == ZStageMoveDirections.Down)
            {
                if (zStageController.Position() <= ZStage.Controller.MinHeight)
                    return false;
            }
            else if (direction == ZStageMoveDirections.Up)
            {
                if (zStageController.Position() >= ZStage.Controller.MaxHeight ||
                    zStageController.Position() + StepSize > ZStage.Controller.MaxHeight)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// This function will move the Z-Stage in a specific drection -
        /// up or down, depending on what button the user will click
        /// on the GUI layer.
        /// The WorkItem object will handle the bulk of the moving work.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveZStage(ZStageMoveDirections direction)
        {           
            if (GoodToMove(direction))
            {
                if (direction == ZStageMoveDirections.Up)
                {
                    double numericSensorValue =
                        Math.Round(forceSensorController.UpdateSensor(),
                        RoundForceSensorValueTo);

                    if (numericSensorValue < stopValue)
                    {
                        zStageController.MoveUp();
                    }
                }
                else
                {
                    zStageController.MoveDown();
                }

                Thread.Sleep(ZStageMoveDelayInMillisec);

                double position = zStageController.Position();

                OnPositionUpdated(new UpdateFeedbackEventArgs(position.ToString()));
            }
        }

        /// <summary>
        /// This function handles the logic for the Z-Stage
        /// printing process.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        public void Print(BackgroundWorker worker, DoWorkEventArgs e)
        {
            double position;
            double sensorValue;
            bool slowedDown = false;

            UpdateFeedback(this, new UpdateFeedbackEventArgs(
                "\nPrinting...\n"));

            printing = true;

            // Move the Z-Stage to its starting position - 
            // the position where the force value was detected
            // during calibration minus the user-defined offset 
            // value.
            MoveZStageToPosition(goToPosition);

            sensorValue = Math.Round(forceSensorController.UpdateSensor(), RoundForceSensorValueTo);

            // This process will continue to run while the Z-Stage
            // is allowed to move up. The PrintLoopCheck will return
            // a value based on the printStopMode set by the user 
            // (either Position or Force Stop Mode).
            while (GoodToMove(ZStageMoveDirections.Up) == true
                && PrintLoopCheck(sensorValue))
            {
                // If the user has cancelled the print, end the process.
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // We put the thread to sleep based on the user-defined
                    // speed setting. This is done to ensure that the Z-Stage
                    // is moving at a uniform pace - the Z-Stage itself 
                    // has no concept of speed, as it simply moves from one
                    // position to another instantaneously. By forcing the thread
                    // to sleep between each "step", we artifically enforce a 
                    // speed upon the Z-Stage.
                    Thread.Sleep(speed);

                    position = zStageController.Position();

                    sensorValue = Math.Round(forceSensorController.UpdateSensor(), RoundForceSensorValueTo);

                    OnSensorUpdated(new UpdateFeedbackEventArgs(sensorValue.ToString()));

                    if (printSlowDown == true && slowedDown == false)
                    {
                        if (sensorValue >= (stopValue - PrintSlowDownOffset) && 
                            sensorValue < stopValue)
                        {
                            UpdateFeedback(this, 
                                new UpdateFeedbackEventArgs("Slowing Z-Stage to " +
                                printingSlowDownStepSize.ToString() + "steps."));

                            zStageController.StepSize = printingSlowDownStepSize;

                            slowedDown = true;
                        }
                    }

                    if (position + zStageController.StepSize > ZStage.Controller.MaxHeight)
                    {
                        maxHeightReachedDuringPrinting = true;
                        OnMaxHeightReachedDuringPrinting();
                        break;
                    }

                    // If the Z-Stage's next move would put it beyond or
                    // equal to the force detected position, move it to the
                    // force detected position instead.  Otherwise, 
                    // move to the next position.
                    if (position + zStageController.StepSize >= forceDetectedPosition
                        && printStopMode == PrintStopMode.Position)
                    {
                        MoveZStageToPosition(forceDetectedPosition);

                        position = zStageController.Position();
                        sensorValue = Math.Round(forceSensorController.UpdateSensor(), RoundForceSensorValueTo);

                        OnSensorUpdated(new UpdateFeedbackEventArgs(sensorValue.ToString()));
                        break;
                    }
                    else if (sensorValue < stopValue
                        && printStopMode == PrintStopMode.Force)
                    {
                        MoveZStageToPosition(position + zStageController.StepSize);
                    }
                    else if (sensorValue >= stopValue 
                        && printStopMode == PrintStopMode.Force)
                    {
                        position = zStageController.Position();
                        sensorValue = 
                            Math.Round(forceSensorController.UpdateSensor(), RoundForceSensorValueTo);
                        forceDetectedPosition = position;
                        goToPosition = forceDetectedPosition - offset;

                        if (goToPosition < ZStage.Controller.MinHeight)
                            goToPosition = ZStage.Controller.MinHeight;

                        OnSensorUpdated(new UpdateFeedbackEventArgs(sensorValue.ToString()));

                        break;
                    }
                    else
                    {
                        MoveZStageToPosition(position + zStageController.StepSize);
                    }

                    // Update the position and sensor values on the GUI
                    // layer.
                    position = zStageController.Position();
                    sensorValue = Math.Round(forceSensorController.UpdateSensor(), RoundForceSensorValueTo);

                    OnSensorUpdated(new UpdateFeedbackEventArgs(sensorValue.ToString()));

                    if (sensorValue >= stopValue && printStopMode == PrintStopMode.Force)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// This function will determine which Stop Mode is
        /// set for printing. If Position mode is set, it will
        /// check to make sure the Z-Stage does not exceed the 
        /// forceDetectedPosition. Else, it will check to make
        /// sure that it does not exceed the force stop value.
        /// </summary>
        /// <param name="sensorValue"></param>
        /// <returns></returns>
        private bool PrintLoopCheck(double sensorValue)
        {
            // Because the Z-Stage's position fluctuates in the second
            // decimal place, we also check for the 
            // force detected position minus 0.01 step. If this is not done,
            // the loop could continue infinitely as the stage would 
            // never reach the full force detected position in
            // certain situations.
            if (printStopMode == PrintStopMode.Position)
                return zStageController.Position() < forceDetectedPosition
                    || zStageController.Position() < forceDetectedPosition - .01;

            // Simply check if the sensorValue is still less than the stopValue.
            return sensorValue < stopValue;
        }

        /// <summary>
        /// This function will finish the printing process. It fires when
        /// the user either cancels the print or the printing process
        /// reaches the force detected position. 
        /// </summary>
        public async Task FinishPrint()
        {
            if (maxHeightReachedDuringPrinting == true)
            {
                maxHeightReachedDuringPrinting = false;
                return;
            }

            // The Z-Stage will dwell once it reaches the correct 
            // position. The dwell-time is user-defined. Once it
            // finishes dwelling, it will return to the initial
            // position (force detected position minus the offset)
            // if Jump Down mode is selected, or it will move
            // the Z-Stage down step-by-step if Step Mode is
            // active.
            UpdateFeedback(this, new UpdateFeedbackEventArgs(
                "\nDwelling for " + dwellTime.ToString() + 
                " milliseconds.\n"));

            if (printSlowDown == true)
                zStageController.StepSize = stepSize;

            Thread.Sleep(dwellTime);

            if (printReturnMode == PrintReturnMode.Jump)
                MoveZStageToPosition(goToPosition);
            else
                await HandlePrintReturnByStep();

            UpdateFeedback(this, new UpdateFeedbackEventArgs(
                "\nPrint finished.\n"));

            Thread.Sleep(OneSecondInMillisec);

            OnPositionUpdated(new UpdateFeedbackEventArgs(String.Format("{0:0.00}", 
                zStageController.Position())));

            OnPositionUpdated(new UpdateFeedbackEventArgs(String.Format("{0:0.00}",
                zStageController.Position())));

            printing = false;
        }

        /// <summary>
        /// This async task fires if the user has set
        /// Step-Down mode for printing. The Z-Stage, upon
        /// finishing printing and completing its dwell 
        /// operation, will return to its gotoposition
        /// or the default minimum position at a step size
        /// and speed determined by the user in Z-Stage settings.
        /// </summary>
        /// <returns></returns>
        private async Task HandlePrintReturnByStep()
        {
            await Task.Run(() =>
                {
                    zStageController.StepSize = printingReturnStepSize;

                    while (PrintStepDownPositionCheck())
                    {
                        if ((zStageController.Position() - zStageController.StepSize) < goToPosition)
                        {
                            MoveZStageToPosition(goToPosition);

                            break;
                        }

                        MoveZStageToPosition(zStageController.Position() -
                            zStageController.StepSize);

                        Thread.Sleep(printingReturnSpeed);
                    }

                    zStageController.StepSize = stepSize;
                });
        }

        /// <summary>
        /// Returns a bool value based on what the
        /// printReturnToZero mode is set to (if 
        /// print return mode is Step-Down). If false,
        /// we return the Z-Stage to the goToPosition,
        /// or else to the Z-Stage's minHeight.
        /// </summary>
        /// <returns></returns>
        private bool PrintStepDownPositionCheck()
        {
            if (printReturnToZero == false)
                return zStageController.Position() > goToPosition;
            else
                return zStageController.Position() > ZStage.Controller.MinHeight;
        }

        #endregion

        #region XYZ Stage Methods

        /// <summary>
        /// This function will carry out XYZ-Stage calibration, 
        /// which occurs automatically after the XYZ-Stage has been
        /// connected. It will test to make sure each driver is
        /// properly connected.
        /// </summary>
        private void BeginXYZStageCalibration()
        {
            byte tempDriver = 0;

            try
            {
                xyzStageController.EnableDriver(aDriver, XYZStage.Controller.ChannelA);

                xyzStageController.EnableDriver(bDriver, XYZStage.Controller.ChannelA);

                xyzStageController.EnableDriver(cDriver, XYZStage.Controller.ChannelA);
            }
            catch (Exception)
            {
                throw;
            }

            for (int i = 0; i < xyzStageController.NumModules; i++)
            {
                if (i == aDriver)
                    tempDriver = aDriver;
                else if (i == bDriver)
                    tempDriver = bDriver;
                else if (i == cDriver)
                    tempDriver = cDriver;

                CalibrateXYZStage(tempDriver, abruptStop);
            }

            xyzStageController.DisableDriver(aDriver);

            xyzStageController.DisableDriver(bDriver);

            xyzStageController.DisableDriver(cDriver);
        }

        /// <summary>
        /// The function that performs the actual
        /// XYZ-Stage calibration on the selected driver, by
        /// simply moving its connected motor one unit to the
        /// right. If the move is successful, the driver
        /// is successfully calibrated.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="abruptStop"></param>
        private void CalibrateXYZStage(byte driver, byte abruptStop)
        {
            byte defaultSpeed = 1;
            byte defaultAcceleration = 1;

            xyzStageController.MoveToVel(driver, defaultSpeed, defaultAcceleration,
                abruptStop, false);

            System.Threading.Thread.Sleep(OneSecondInMillisec / 2);

            xyzStageController.StopMotor(driver, abruptStop);

            System.Threading.Thread.Sleep(OneSecondInMillisec / 2);
        }

        /// <summary>
        /// Queries the XYZStage Controller for the
        /// number of modules connected.
        /// </summary>
        /// <returns>Returns the number of modules connected to
        /// XYZ-Stage.</returns>
        public int NumXYZStageModules()
        {
            return xyzStageController.NumModules;
        }

        /// <summary>
        /// This function will enable the selected driver with the selected 
        /// channel (or motor) on the XYZ-Stage.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="channel"></param>
        public void EnableDriver(byte driver, byte channel)
        {
            try
            {
                xyzStageController.EnableDriver(driver, channel);
            }
            catch (Exception)
            {
                throw;
            }

            switch (driver)
            {
                case XYZStage.Controller.ADriverValue:
                    aDriverEnabled = true;
                    UpdateFeedbackTextBox("Driver A enabled on channel " +
                        channel.ToString() + "\n");
                    break;
                case XYZStage.Controller.BDriverValue:
                    bDriverEnabled = true;
                    UpdateFeedbackTextBox("\nDriver B enabled on channel " +
                        channel.ToString() + "\n");
                    break;
                case XYZStage.Controller.CDriverValue:
                    cDriverEnabled = true;
                    UpdateFeedbackTextBox("\nDriver C enabled on channel " +
                        channel.ToString() + "\n");
                    break;
            }
        }

        /// <summary>
        /// This function will disable the selected driver
        /// on the XYZ-Stage.
        /// </summary>
        /// <param name="driver"></param>
        public void DisableDriver(byte driver)
        {
            try
            {
                xyzStageController.DisableDriver(driver);
            }
            catch (Exception)
            {
                throw;
            }

            switch (driver)
            {
                case XYZStage.Controller.ADriverValue:
                    aDriverEnabled = false;
                    UpdateFeedbackTextBox("\nDriver A disabled\n");
                    break;
                case XYZStage.Controller.BDriverValue:
                    bDriverEnabled = false;
                    UpdateFeedbackTextBox("\nDriver B disabled\n");
                    break;
                case XYZStage.Controller.CDriverValue:
                    cDriverEnabled = false;
                    UpdateFeedbackTextBox("Driver C disabled\n");
                    break;
            }
        }

        /// <summary>
        /// This function will move the selected driver 
        /// (with the motor/channel selected at time of enabling)
        /// to the given position, with the input speed, acceleration
        /// and stop mode.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="pos"></param>
        /// <param name="speed"></param>
        /// <param name="acc"></param>
        /// <param name="stop"></param>
        public void XYZStageMoveToPos(byte driver, int pos, byte speed, byte acc, byte stop)
        {
            try
            {
                xyzStageController.MoveToPos(driver, pos, speed, acc, stop);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This function will query the XYZ-Stage controller
        /// for the current position of the selected driver.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns>The position, as a long value, of the 
        /// selected driver.</returns>
        public long GetXYZStageDriverPos(byte driver)
        {
            return xyzStageController.GetPos(driver);
        }

        /// <summary>
        /// This function will move the selected driver on the
        /// XYZ-Stage in velocity mod, for as long as the user
        /// is holding down the appropriate move-arrow button
        /// on the GUI form. IsReverse is either true or false 
        /// - true is moving to the right, false to the left
        /// (or reverse).
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="speed"></param>
        /// <param name="acc"></param>
        /// <param name="stopMode"></param>
        /// <param name="isReverse"></param>
        public void XYZStageMoveToVel(byte driver, byte speed, byte acc, byte stopMode, bool isReverse)
        {
            xyzStageController.MoveToVel(driver, speed, acc, stopMode, isReverse);
        }

        /// <summary>
        /// This function will stop the selected driver from moving,
        /// either smoothly or abruptly.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="stopMode"></param>
        public void StopDriver(byte driver, byte stopMode)
        {
            xyzStageController.StopMotor(driver, stopMode);
        }

        /// <summary>
        /// This function will add the driver to the group
        /// and set whether it is the leader or not.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="isLeader"></param>
        public void XYZStageSetGroup(byte driver, bool isLeader)
        {
            xyzStageController.SetGroup(driver, isLeader);
        }

        /// <summary>
        /// This function will remove the driver from the group.
        /// </summary>
        /// <param name="driver"></param>
        public void XYZStageRemoveGroup(byte driver)
        {
            xyzStageController.RemoveGroup(driver);
        }

        /// <summary>
        /// This function will set the parameters for the selected
        /// driver in the group - stop mode, position, acceleration
        /// and speed.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="stopMode"></param>
        /// <param name="pos"></param>
        /// <param name="acc"></param>
        /// <param name="speed"></param>
        public void XYZStageMoveGroupSetup(byte driver, byte stopMode, int pos, 
            byte acc, byte speed)
        {
            xyzStageController.MoveGroupSetup(driver, stopMode, pos,
                acc, speed);
        }

        /// <summary>
        /// This function will carry out the move operation for the
        /// group.
        /// </summary>
        public void XYZStageMoveGroup()
        {
            xyzStageController.MoveGroup();
        }

        #endregion

        #region Sensor Method Region

        /// <summary>
        /// Get the current sensor value rounded to four
        /// decimal places.
        /// </summary>
        /// <returns>Returns current sensor value.</returns>
        public double SensorUpdate()
        {
            return Math.Round(forceSensorController.UpdateSensor(), 4);
        }

        /// <summary>
        /// Sets the force sensor to gross setting.
        /// </summary>
        /// <returns>Returns true if successful.</returns>
        public bool SensorSetGross()
        {
            forceSensorController.Gross();

            return true;
        }

        /// <summary>
        /// Sets the force sensor to tare setting.
        /// </summary>
        /// <returns>Returns true if successful.</returns>
        public bool SensorSetTare()
        {
            forceSensorController.Tare();

            return true;
        }

        /// <summary>
        /// Changes the force sensor stop value 
        /// to new valued provided by user.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns true if value is accepted, 
        /// false otherwise.</returns>
        public bool ChangeStopValue(string value)
        {
            double tempValue;

            try
            {
                tempValue = Double.Parse(value);
            }
            catch (Exception)
            {
                throw;
            }

            if (tempValue >= MinForceSensorStopValue && tempValue <= MaxForceSensorStopValue)
            {
                stopValue = tempValue;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region Voltmeter Method Region
        
        /// <summary>
        /// This is the voltage monitoring thread.
        /// Will monitor the voltage reported
        /// by the voltmeter for a change in voltage
        /// greater than the preset "change" value.
        /// If change value is detected, begin the
        /// Z-Stage printing process.
        /// </summary>
        private void voltmeterThreadProcSafe()
        {
            string voltmeterStringValue;
            lastVoltageValue = Double.Parse(voltmeterController.Read());

            while (voltmeterController.Connected == true)
            {
                voltmeterStringValue = voltmeterController.Read();

                this.OnVoltageUpdated(new UpdateFeedbackEventArgs(voltmeterStringValue));

                currentVoltageValue = Double.Parse(voltmeterStringValue);

                if (currentVoltageValue - lastVoltageValue >= voltageChangeValue
                    && IsVoltageDetected != true)
                {
                    if (VoltageDetected != null)
                        VoltageDetected(this, null);
                }

                lastVoltageValue = currentVoltageValue;

                Thread.Sleep(OneSecondInMillisec);
            }
        }

        #endregion

        #region Event Handler Region

        /// <summary>
        /// This function will fire the UpdateFeedback event
        /// which will update the GUI layer's feedback box
        /// </summary>
        /// <param name="feedback"></param>
        private void UpdateFeedbackTextBox(string feedback)
        {
            UpdateFeedbackEventArgs e = new UpdateFeedbackEventArgs(feedback);

            if (UpdateFeedback != null)
                UpdateFeedback(this, e);
        }

        /// <summary>
        /// Event fires when the calibration process is finished.
        /// </summary>
        private void FinishCalibrating()
        {
            if (FinishCalibration != null)
                FinishCalibration(this, new EventArgs());
        }

        /// <summary>
        ///  Event fires when force is detected, playing a sound to notify
        ///  the user.
        /// </summary>
        private void PlayForceDetectedSound()
        {
            if (ForceDetectedSound != null)
                ForceDetectedSound(this, new EventArgs());
        }

        /// <summary>
        /// This event will perform the updating of the GUI layer's
        /// sensor display.
        /// </summary>
        /// <param name="e"></param>
        private void OnSensorUpdated(UpdateFeedbackEventArgs e)
        {
            //EventHandler<UpdateFeedbackEventArgs> temp = this.SensorUpdated;
            if (this.SensorUpdated != null)
            {
                this.SensorUpdated(this, e);
               // temp.Invoke(this, e);
            }
        }

        /// <summary>
        /// This event will perform the updating of the GUI layer's 
        /// sensor display.
        /// </summary>
        /// <param name="e"></param>
        private void OnPositionUpdated(UpdateFeedbackEventArgs e)
        {
            if (this.PositionUpdated != null)
            {
                this.PositionUpdated(this, e);
            }
        }

        /// <summary>
        /// Event fires if the maximum height is reached during the
        /// printing process.
        /// </summary>
        private void OnMaxHeightReachedDuringPrinting()
        {
            UpdateFeedbackEventArgs e =
                new UpdateFeedbackEventArgs("\nMax height reached during printing.\n");

            if (UpdateFeedback != null)
                UpdateFeedback(this, e);
        }

        /// <summary>
        /// This event will perform the updating of the GUI layer's
        /// voltage display.
        /// </summary>
        /// <param name="e"></param>
        private void OnVoltageUpdated(UpdateFeedbackEventArgs e)
        {
            EventHandler<UpdateFeedbackEventArgs> temp = this.VoltageUpdated;
            if (temp != null)
            {
                temp.Invoke(this, e);
            }
        }

        /// <summary>
        /// This event will fire when the force stop value has been detected.
        /// If the force is detected during a calibration process, the program 
        /// passes to the next phase of calibration by beginning a new
        /// calibration thread. If calibration is finished, it ends the
        /// calibration process. If Z-Stage is not calibrating, it will
        /// play a sound to notify the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_ForceDetected(object sender, ForceDetectedEventArgs e)
        {
            UpdateFeedbackTextBox("\nForce of " + e.ForceDetectedValue + 
                " detected at position " + e.ForceDetectedPosition + "\n");

            if (e.IsCalibrating == false)
            {
                PlayForceDetectedSound();
            }

            forceDetectedPosition = e.ForceDetectedPosition;

            if (e.IsCalibrating)
            {
                calibrationPositions.Add(e.ForceDetectedPosition);
                calibrationForce.Add(e.ForceDetectedValue);
            }
        }

        /// <summary>
        /// This event will fire if the Z-Stage reaches the maximum height
        /// during calibration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_MaxHeightReachedDuringCalibration(object sender, EventArgs e)
        {
            UpdateFeedbackTextBox("\nThe Z-Stage reached its maximum height during calibration." +
                "Please cancel the calibration and lower 3D printing arm.\n");

            maxHeightReachedDuringCalibration = true;
        }

        #endregion

        #region Input Validation Method Region

        /// <summary>
        /// This function will validate the user's input for changing
        /// the Z-Stage's settings from the Settings panel.
        /// </summary>
        /// <param name="stepSizeTextBox"></param>
        /// <param name="speedTextBox"></param>
        /// <param name="offsetTextBox"></param>
        /// <param name="dwellTimeTextBox"></param>
        /// <param name="printReturnStepSizeTextBox"></param>
        /// <param name="printSlowDownTextBox"></param>
        /// <param name="printGoToPositionTextBox"></param>
        /// <param name="printReturnSpeedTextBox"></param>
        /// <param name="printSlowDown"></param>
        /// <returns>Returns true if all inputs are valid,
        /// or throws an exception if an error has occurred.</returns>
        public bool ValidateInputs(string stepSizeTextBox, string speedTextBox, 
            string offsetTextBox, string dwellTimeTextBox, string printReturnStepSizeTextBox,
            string printReturnSpeedTextBox, string printGoToPositionTextBox,
            bool printSlowDown, string printSlowDownTextBox)
        {
            dwellTime = Int32.Parse(dwellTimeTextBox);

            if (dwellTime < 0)
            {
                throw new Exception("Dwell time must be greater than 0.");
            }

            printingReturnStepSize = Double.Parse(printReturnStepSizeTextBox);

            if (printingReturnStepSize <= 0.0)
            {
                throw new Exception("Print return step size time must be greater than 0.");
            }

            printingReturnSpeed = Int32.Parse(printReturnSpeedTextBox);

            if (printingReturnSpeed < 0)
            {
                throw new Exception("Print return speed must be greater or equal to 0.");
            }

            goToPosition = Double.Parse(printGoToPositionTextBox);

            if (goToPosition < ZStage.Controller.MinHeight ||
                goToPosition > ZStage.Controller.MaxHeight)
            {
                throw new Exception("Go to position cannot exceed Z-Stage min and max " +
                    "position bounds.");
            }

            this.printSlowDown = printSlowDown;

            if (printSlowDown == true)
            {
                try
                {
                    printingSlowDownStepSize = Double.Parse(printSlowDownTextBox);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            try
            {
                this.ValidateInputs(stepSizeTextBox, speedTextBox, stopValue.ToString(),
                startPosition.ToString(), offsetTextBox, false);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// This function will validate the inputs for the Z-Stage
        /// calibration Phase Settings form.
        /// </summary>
        /// <param name="stepSizeTextBox"></param>
        /// <param name="speedTextBox"></param>
        /// <param name="offsetTextBox"></param>
        /// <returns>Returns true if all inputs are valid, or
        /// throws an exception if an error occurs.</returns>
        public bool ValidateInputs(string stepSizeTextBox, string speedTextBox,
            string offsetTextBox)
        {
            try
            {
                this.ValidateInputs(stepSizeTextBox, speedTextBox, stopValue.ToString(),
                    startPosition.ToString(), offsetTextBox, true);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// This function will validate the inputs on the main Z-Stage calibration form.
        /// </summary>
        /// <param name="stopValueTextBox"></param>
        /// <param name="startPositionTextBox"></param>
        /// <returns>Returns true if all inputs are valid, or throws an exception
        /// if an error occurs.</returns>
        public bool ValidateInputs(string stopValueTextBox, string startPositionTextBox)
        {
            try
            {
                this.ValidateInputs(stepSize.ToString(), speed.ToString(),
                    stopValueTextBox, startPositionTextBox, offset.ToString(),
                    true);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// This function will validate the user's input for all Z-Stage
        /// settings. 
        /// </summary>
        /// <param name="stepSizeTextBox"></param>
        /// <param name="speedTextBox"></param>
        /// <param name="forceStopValueTextBox"></param>
        /// <param name="startPositionTextBox"></param>
        /// <param name="offsetTextBox"></param>
        /// <param name="calibrating"></param>
        /// <returns></returns>
        public bool ValidateInputs(string stepSizeTextBox, string speedTextBox,
            string forceStopValueTextBox, string startPositionTextBox, string offsetTextBox,
            bool calibrating)
        {
            try
            {
                stepSize = Double.Parse(stepSizeTextBox);

                if (stepSize < MinimumZStageStepSize || stepSize > ZStage.Controller.MaxHeight)
                {
                    throw new Exception("Step size must greater than " + 
                        MinimumZStageStepSize.ToString() + " or less than " +
                        ZStage.Controller.MaxHeight.ToString());
                }

                zStageController.StepSize = stepSize;

                if (calibrating)
                    calibrationStepSizes.Add(stepSize);

                speed = Int32.Parse(speedTextBox);

                if (speed < 0)
                {
                    throw new Exception("Speed must be greater than 0.");
                }

                if (calibrating)
                    calibrationSpeedValues.Add(speed);

                stopValue = Double.Parse(forceStopValueTextBox);

                if (stopValue > MaxForceSensorStopValue || stopValue < MinForceSensorStopValue)
                {
                    throw new Exception("Stop value must greater than " + 
                    MinForceSensorStopValue.ToString() + " or less than " + 
                    MaxForceSensorStopValue.ToString() + ".");
                }

                startPosition = Double.Parse(startPositionTextBox);

                if (startPosition < ZStage.Controller.MinHeight || startPosition > ZStage.Controller.MaxHeight)
                {
                    throw new Exception("Start position must be greater than " + 
                        ZStage.Controller.MinHeight + " or less than " +
                        ZStage.Controller.MaxHeight.ToString());
                }

                offset = Double.Parse(offsetTextBox);

                if (calibrating)
                {
                    if (offset < 0)
                    {
                        throw new Exception("Offset must be greater than 0");
                    }

                    calibrationOffsetValues.Add(offset);
                }
                else if (offset < 0 || (forceDetectedPosition - offset < 0))
                {
                    throw new Exception("Offset must be greater than 0, or must not " +
                        " reduce the position below 0.");
                }


            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        #endregion
    }

    #endregion
}
