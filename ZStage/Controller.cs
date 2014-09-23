// Author: Robert Dacunto
// Project: Z-Stage Controller
// File: Controller.cs
// Classes: Controller, ZStageError

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZStage
{
    /// <summary>
    /// Controller handles all logic specific to Z-Stage, 
    /// including connectivity, position reporting and move
    /// operations.
    /// </summary>
    public class Controller
    {
        #region Field Region

        private static readonly Object _lock = new Object();

        /// <summary>
        /// The maximum height the Z-Stage is allowed to 
        /// move to. The actual maximum range is 120 microns,
        /// but this value is in "step size", which roughly
        /// corresponds to a micron.
        /// </summary>
        public const double MaxHeight = 105.4;

        /// <summary>
        /// The minimum height the Z-Stage is allowed to 
        /// move to.
        /// </summary>
        public const double MinHeight = 0.0;

        private bool connected;
        private double stepSize;

        private int ID;
        private int nrDevices;
        private int baud;
        private int idx;
        private int[] serialnrs;
        private char channel;
        private string sAxes;
        private double[] spa;
        private string sspa;
        private int[] svo;

        private static double[] mov;
        private static double[] pos;

        private StringBuilder szDevices;
        private StringBuilder sBuffer;
        private StringBuilder feedbackText;

        #endregion

        #region Property Region

        /// <summary>
        /// Returns whether or not the Z-Stage is connected.
        /// </summary>
        public bool Connected
        {
            get { return connected; }
        }

        /// <summary>
        /// Returns any feedback from Z-Stage to any listening
        /// feedback capture.
        /// </summary>
        public StringBuilder FeedbackText
        {
            get { return feedbackText; }
        }

        /// <summary>
        /// Returns the current stepSize of the Z-Stage.
        /// stepSize is the amount of units the Z-Stage will
        /// move in any given move operation.
        /// </summary>
        public double StepSize
        {
            get { return stepSize; }
            set 
            {
                if (value > MaxHeight)
                    throw new ZStageError("Step size cannot exceed max height.");
                else if (value < 0.1)
                    throw new ZStageError("Step size cannot be less than 0.1.");

                stepSize = value; 
            }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for Z-Stage Controller. NativeMethods is 
        /// DLL associated with the Z-Stage and reveals 
        /// the lower-level functions to the Z-Stage controller.
        /// </summary>
        public Controller()
        {
            stepSize = 1.0;
        }

        #endregion

        #region Connection Method Region

        /// <summary>
        /// Connects the PC to the Z-Stage.
        /// </summary>
        public void Connect()
        {
            szDevices = new StringBuilder(1000);
            nrDevices = NativeMethods.EnumerateUSB(szDevices, 999, null);
            feedbackText = new StringBuilder();

            ID = NativeMethods.ConnectUSB(szDevices.ToString());

            if (ID < 0 || NativeMethods.IsConnected(ID) == 0)
            {
                ShowError(ID, "\nCould not connect to E-816");
            }

            sBuffer = new StringBuilder(256);
            NativeMethods.qIDN(ID, sBuffer, 256);

            feedbackText.Append("\nConnected to E-816: " + sBuffer.ToString());

            baud = 0;
            if (NativeMethods.qBDR(ID, ref baud) == 0)
            {
                ShowError(ID, "\nCould not read qBDR");
            }

            feedbackText.Append("\n with " + baud.ToString() + " baud");

            NativeMethods.qSAI(ID, sBuffer, 256);

            feedbackText.Append("\n\nConnected axes: \"" + sBuffer.ToString() + "\"");

            sAxes = sBuffer.ToString();
            serialnrs = new int[sAxes.Length];

            if (NativeMethods.qSSN(ID, sAxes, serialnrs) == 0)
            {
                ShowError(ID, "\nCould not read SSN");
            }

            idx = 0;
            foreach (char c in sAxes)
            {
                feedbackText.Append('\n' + "   " + c + ": " + serialnrs[idx].ToString());
                idx++;
            }

            channel = '\0';

            if (NativeMethods.qSCH(ID, ref channel) == 0)
            {
                ShowError(ID, "\nCould not read channel name");
            }

            feedbackText.Append("\n\nchannel name: " + channel.ToString());

            spa = new double[10];
            sspa = "AAAAAAAAAA";
            int[] spacmd = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            feedbackText.Append("\n\nSPA TEST ");

            if (NativeMethods.qSPA(ID, sspa, spacmd, spa) == 0)
            {
                ShowError(ID, "\nqSPA failed");
            }

            feedbackText.Append("\nSPA for A:");

            for (int i = 0; i < 10; i++)
            {
                feedbackText.Append('\n' + "   " + (i + 1).ToString() + ": " + spa[i].ToString());
            }

            svo = new int[sAxes.Length];
            if (NativeMethods.qSVO(ID, sAxes, svo) == 0)
            {
                ShowError(ID, "\nqSVO failed");
            }

            feedbackText.Append("\nServo state");

            idx = 0;
            foreach (char c in sAxes)
            {
                feedbackText.Append("   " + c + ": " + svo[idx].ToString());

                svo[idx] = 1;
                idx++;
            }

            if (NativeMethods.SVO(ID, sAxes, svo) == 0)
            {
                ShowError(ID, "\nSVO failed");
            }

            if (NativeMethods.qSVO(ID, sAxes, svo) == 0)
            {
                ShowError(ID, "\nqSVO failed");
            }

            idx = 0;

            mov = new double[sAxes.Length + 1];
            pos = new double[sAxes.Length];

            connected = true;

            MoveToZero();
        }

        /// <summary>
        /// Disconnects the PC from the Z-Stage.
        /// </summary>
        public void Disconnect()
        {
            if (connected)
            {
                connected = false;
                NativeMethods.CloseConnection(ID);
            }
        }

        #endregion

        #region Error Method Region

        /// <summary>
        /// Returns an error message in the form of the 
        /// ZStageError exception.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="sMessage"></param>
        private void ShowError(int ID, string sMessage)
        {
            int err = NativeMethods.GetError(ID);
            StringBuilder sErr = new StringBuilder(1024);
            NativeMethods.TranslateError(err, sErr, 1023);

            string errorMessage = sMessage + "\nError " + err.ToString() + ": \"" + sErr + "\"\n";

            throw new ZStageError(errorMessage);
        }

        #endregion

        #region General Method Region

        /// <summary>
        /// Position feedback function.
        /// </summary>
        /// <returns>Current position of Z-Stage rounded to 
        /// two decimal places.</returns>
        public double Position()
        {
            if (connected != true)
                ShowError(ID, "\nThe Z-Stage is not connected.");

            lock (_lock)
            {
                NativeMethods.qPOS(ID, sAxes, pos);

                pos[0] = (double)Math.Round((decimal)pos[0], 2);
                
                return pos[idx];
            }
        }

        #endregion

        #region Move Method Region

        /// <summary>
        /// Automatically moves the Z-Stage to the default minimum height.
        /// </summary>
        public void MoveToZero()
        {
            if (connected != true)
                ShowError(ID, "\nThe Z-Stage is not connected.");

            mov[0] = MinHeight;

            DoMove();
        }

        /// <summary>
        /// Automatically moves the Z-Stage to the position received.
        /// </summary>
        /// <param name="position"></param>
        public void MoveToPosition(double position)
        {
            if (connected != true)
                ShowError(ID, "\nThe Z-Stage is not connected.");

            mov[0] = position;

            if (position > MaxHeight)
                ShowError(ID, "\nThe Z-Stage cannot move beyond position " + MaxHeight);
            else if (position < MinHeight)
                ShowError(ID, "\nThe Z-Stage cannot move less than position " + MinHeight);

            DoMove();
        }

        /// <summary>
        /// Moves the Z-Stage in the upward direction. Distance moved
        /// based on StepSize and current position.
        /// </summary>
        public void MoveUp()
        {
            if (connected != true)
                ShowError(ID, "\nThe Z-Stage is not connected.");

            if (Position() + StepSize > MaxHeight)
                ShowError(ID, "\nThe Z-Stage cannot move beyond position " + MaxHeight);

            mov[0] = Position() + StepSize;

            DoMove();
        }

        /// <summary>
        /// Moves the Z-Stage in the downward direction. Distance moved
        /// based on StepSize and current position.
        /// </summary>
        public void MoveDown()
        {
            if (connected != true)
                ShowError(ID, "\nThe Z-Stage is not connected.");

            if (Position() - StepSize < MinHeight)
                mov[0] = MinHeight;
            else
                mov[0] = Position() - StepSize;

            DoMove();
        }

        /// <summary>
        /// Performs the move operation stored in the mov variable, as
        /// assigned by MoveUp, MoveDown, MoveToPosition or MoveToZero
        /// </summary>
        public void DoMove()
        {
            lock (_lock)
            {
                NativeMethods.MOV(ID, sAxes, mov);
            }
        }

        #endregion
    }

    /// <summary>
    /// Exception class specific to the Z-Stage.
    /// </summary>
    [Serializable]
    public class ZStageError : Exception
    {
        /// <summary>
        /// Constructor for ZStageError
        /// </summary>
        /// <param name="message"></param>
        public ZStageError(string message)
            : base(message)
        {
        }
    }
}
