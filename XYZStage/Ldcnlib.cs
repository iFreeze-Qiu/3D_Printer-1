using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XYZStage
{
    internal static class NativeMethods
    {
        #region Definitions
        /// TYPE_TINY -> 0x10
        public const int TYPE_TINY = 16;

        /// TYPE_STD -> 0x00
        public const int TYPE_STD = 0;

        /// SET_CH_A -> 0x00
        public const byte SET_CH_A = 0;

        /// SET_CH_B -> 0x01
        public const byte SET_CH_B = 1;

        /// SET_CH_C -> 0x02
        public const byte SET_CH_C = 2;

        /// GROUP_ADDR -> 0xFF
        public const int GROUP_ADDR = 255;

        /// GROUP_ADDR_TWO -> 0xBB
        public const int GROUP_ADDR_TWO = 187;

        /// ESCAPE -> 0x51
        public const int ESCAPE = 81;

        /// RESET_POS -> 0x00
        public const int RESET_POS = 0;

        /// SET_ADDR -> 0x01
        public const int SET_ADDR = 1;

        /// DEF_STAT -> 0x02
        public const int DEF_STAT = 2;

        /// READ_STAT -> 0x03
        public const int READ_STAT = 3;

        /// LOAD_TRAJ -> 0x04
        public const int LOAD_TRAJ = 4;

        /// START_MOVE -> 0x05
        public const int START_MOVE = 5;

        /// SET_PARAM -> 0x06
        public const int SET_PARAM = 6;

        /// STOP_MOTOR -> 0x07
        public const int STOP_MOTOR = 7;

        /// SET_OUTPUTS -> 0x08
        public const int SET_OUTPUTS = 8;

        /// SET_HOMING -> 0x09
        public const int SET_HOMING = 9;

        /// SET_BAUD -> 0x0A
        public const int SET_BAUD = 10;

        /// RESERVED -> 0x0B
        public const int RESERVED = 11;

        /// SAVE_AS_HOME -> 0x0C
        public const int SAVE_AS_HOME = 12;

        /// NOT_USED -> 0x0D
        public const int NOT_USED = 13;

        /// NOP -> 0x0E
        public const int NOP = 14;

        /// HARD_RESET -> 0x0F
        public const int HARD_RESET = 15;

        /// SEND_POS -> 0x01
        public const int SEND_POS = 1;

        /// SEND_AD -> 0x02
        public const int SEND_AD = 2;

        /// SEND_ST -> 0x04
        public const int SEND_ST = 4;

        /// SEND_INBYTE -> 0x08
        public const int SEND_INBYTE = 8;

        /// SEND_HOME -> 0x10
        public const int SEND_HOME = 16;

        /// SEND_ID -> 0x20
        public const int SEND_ID = 32;

        /// SEND_OUT -> 0x40
        public const int SEND_OUT = 64;

        /// LOAD_POS -> 0x01
        public const int LOAD_POS = 1;

        /// LOAD_SPEED -> 0x02
        public const int LOAD_SPEED = 2;

        /// LOAD_ACC -> 0x04
        public const int LOAD_ACC = 4;

        /// LOAD_ST -> 0x08
        public const int LOAD_ST = 8;

        /// STEP_REV -> 0x10
        public const int STEP_REV = 16;

        /// START_NOW -> 0x80
        public const int START_NOW = 128;

        /// SPEED_8X -> 0x00
        public const int SPEED_8X = 0;

        /// SPEED_4X -> 0x01
        public const int SPEED_4X = 1;

        /// SPEED_2X -> 0x02
        public const int SPEED_2X = 2;

        /// SPEED_1X -> 0x03
        public const int SPEED_1X = 3;

        /// IGNORE_LIMITS -> 0x04
        public const int IGNORE_LIMITS = 4;

        /// MOFF_LIMIT -> 0x08
        public const int MOFF_LIMIT = 8;

        /// MOFF_STOP -> 0x10
        public const int MOFF_STOP = 16;

        /// STP_ENABLE_AMP -> 0x01
        public const int STP_ENABLE_AMP = 1;

        public const int STP_DISABLE_AMP = 0;

        /// STOP_ABRUPT -> 0x04
        public const byte STOP_ABRUPT = 4;

        /// STOP_SMOOTH -> 0x08
        public const byte STOP_SMOOTH = 8;

        /// ON_LIMIT1 -> 0x01
        public const int ON_LIMIT1 = 1;

        /// ON_LIMIT2 -> 0x02
        public const int ON_LIMIT2 = 2;

        /// HOME_MOTOR_OFF -> 0x04
        public const int HOME_MOTOR_OFF = 4;

        /// ON_HOMESW -> 0x08
        public const int ON_HOMESW = 8;

        /// HOME_STOP_ABRUPT -> 0x10
        public const int HOME_STOP_ABRUPT = 16;

        /// HOME_STOP_SMOOTH -> 0x20
        public const int HOME_STOP_SMOOTH = 32;

        /// MOTOR_MOVING -> 0x01
        public const int MOTOR_MOVING = 1;

        /// CKSUM_ERROR -> 0x02
        public const int CKSUM_ERROR = 2;

        /// STP_AMP_ENABLED -> 0x04
        public const int STP_AMP_ENABLED = 4;

        /// POWER_ON -> 0x08
        public const int POWER_ON = 8;

        /// AT_SPEED -> 0x10
        public const int AT_SPEED = 16;

        /// VEL_MODE -> 0x20
        public const int VEL_MODE = 32;

        /// TRAP_MODE -> 0x40
        public const int TRAP_MODE = 64;

        /// HOME_IN_PROG -> 0x80
        public const int HOME_IN_PROG = 128;

        /// ESTOP -> 0x01
        public const int ESTOP = 1;

        /// AUX_IN1 -> 0x02
        public const int AUX_IN1 = 2;

        /// AUX_IN2 -> 0x04
        public const int AUX_IN2 = 4;

        /// FWD_LIMIT -> 0x08
        public const int FWD_LIMIT = 8;

        /// REV_LIMIT -> 0x10
        public const int REV_LIMIT = 16;

        /// HOME_SWITCH -> 0x20
        public const int HOME_SWITCH = 32;

        /// MAXSIOERROR -> 3
        public const int MAXSIOERROR = 3;

        /// PB9600 -> 129
        public const int PB9600 = 129;

        /// PB19200 -> 63
        public const int PB19200 = 63;

        /// PB57600 -> 20
        public const int PB57600 = 20;

        /// PB115200 -> 10
        public const int PB115200 = 10;

        /// SERVOMODTYPE -> 0
        public const int SERVOMODTYPE = 0;

        /// SERVOHYBTYPE -> 90
        public const int SERVOHYBTYPE = 90;

        /// IOMODTYPE -> 2
        public const int IOMODTYPE = 2;

        /// STEPMODTYPE -> 3
        public const int STEPMODTYPE = 3;

        /// SET_GAIN -> 0x06
        public const int SET_GAIN = 6;

        /// IO_CTRL -> 0x08
        public const int IO_CTRL = 8;

        /// CLEAR_BITS -> 0x0B
        public const int CLEAR_BITS = 11;

        /// EEPROM_CTRL -> 0x0D
        public const int EEPROM_CTRL = 13;

        /// ADD_PATHPOINT -> 0x0D
        public const int ADD_PATHPOINT = 13;

        /// REL_HOME -> 0x01
        public const int REL_HOME = 1;

        /// SEND_VEL -> 0x04
        public const int SEND_VEL = 4;

        /// SEND_AUX -> 0x08
        public const int SEND_AUX = 8;

        /// SEND_PERROR -> 0x40
        public const int SEND_PERROR = 64;

        /// SEND_INPORTS -> 0x80
        public const int SEND_INPORTS = 128;

        /// SEND_NPOINTS -> 0x80
        public const int SEND_NPOINTS = 128;

        /// LOAD_VEL -> 0x02
        public const int LOAD_VEL = 2;

        /// LOAD_PWM -> 0x08
        public const int LOAD_PWM = 8;

        /// ENABLE_SERVO -> 0x10
        public const int ENABLE_SERVO = 16;

        /// REVERSE -> 0x40
        public const int REVERSE = 64;

        /// MOVE_REL -> 0x40
        public const int MOVE_REL = 64;

        /// SRV_ENABLE_AMP -> 0x01
        public const int SRV_ENABLE_AMP = 1;

        /// MOTOR_OFF -> 0x02
        public const int MOTOR_OFF = 2;

        /// STOP_HERE -> 0x10
        public const int STOP_HERE = 16;

        /// ADV_MODE -> 0x20
        public const int ADV_MODE = 32;

        /// SET_OUT1 -> 0x01
        public const int SET_OUT1 = 1;

        /// SET_OUT2 -> 0x02
        public const int SET_OUT2 = 2;

        /// IO1_IN -> 0x04
        public const int IO1_IN = 4;

        /// IO2_IN -> 0x08
        public const int IO2_IN = 8;

        /// WR_OUT1 -> 0x10
        public const int WR_OUT1 = 16;

        /// WR_OUT2 -> 0x20
        public const int WR_OUT2 = 32;

        /// WR_OUT3 -> 0x40
        public const int WR_OUT3 = 64;

        /// FAST_PATH -> 0x40
        public const int FAST_PATH = 64;

        /// WR_OUT4 -> 0x80
        public const int WR_OUT4 = 128;

        /// ON_INDEX -> 0x08
        public const int ON_INDEX = 8;

        /// ON_POS_ERR -> 0x40
        public const int ON_POS_ERR = 64;

        /// ON_CUR_ERR -> 0x80
        public const int ON_CUR_ERR = 128;

        /// STORE_GAINS -> 0x01
        public const int STORE_GAINS = 1;

        /// FETCH_GAINS -> 0x02
        public const int FETCH_GAINS = 2;

        /// STORE_VA -> 0x04
        public const int STORE_VA = 4;

        /// FETCH_VA -> 0x08
        public const int FETCH_VA = 8;

        /// STORE_OUTPUTS -> 0x10
        public const int STORE_OUTPUTS = 16;

        /// FETCH_OUTPUTS -> 0x20
        public const int FETCH_OUTPUTS = 32;

        /// STORE_SI_BIT -> 0x40
        public const int STORE_SI_BIT = 64;

        /// INIT_SERVO -> 0x80
        public const int INIT_SERVO = 128;

        /// P_30HZ -> 30
        public const int P_30HZ = 30;

        /// P_60HZ -> 60
        public const int P_60HZ = 60;

        /// P_120HZ -> 120
        public const int P_120HZ = 120;

        /// MOVE_DONE -> 0x01
        public const int MOVE_DONE = 1;

        /// OVERCURRENT -> 0x04
        public const int OVERCURRENT = 4;

        /// POS_ERR -> 0x10
        public const int POS_ERR = 16;

        /// LIMIT1 -> 0x20
        public const int LIMIT1 = 32;

        /// LIMIT2 -> 0x40
        public const int LIMIT2 = 64;

        /// INDEX -> 0x01
        public const int INDEX = 1;

        /// POS_WRAP -> 0x02
        public const int POS_WRAP = 2;

        /// SERVO_ON -> 0x04
        public const int SERVO_ON = 4;

        /// ACCEL_DONE -> 0x08
        public const int ACCEL_DONE = 8;

        /// SLEW_DONE -> 0x10
        public const int SLEW_DONE = 16;

        /// SERVO_OVERRUN -> 0x20
        public const int SERVO_OVERRUN = 32;

        /// PATH_MODE -> 0x40
        public const int PATH_MODE = 64;

        /// MAXNUMMOD -> 33
        public const int MAXNUMMOD = 33;

        #endregion

        #region Structs

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct LDCNMOD
        {

            /// byte
            public byte modtype;

            /// byte
            public byte modver;

            /// byte
            public byte statusitems;

            /// byte
            public byte stat;

            /// byte
            public byte groupaddr;

            /// boolean
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.I1)]
            public bool groupleader;

            /// void*
            public System.IntPtr p;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct GAINVECT
        {

            /// short
            public short kp;

            /// short
            public short kd;

            /// short
            public short ki;

            /// short
            public short il;

            /// byte
            public byte ol;

            /// byte
            public byte cl;

            /// short
            public short el;

            /// byte
            public byte sr;

            /// byte
            public byte dc;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct SERVOMOD
        {

            /// int
            public int pos;

            /// short
            public short ad;

            /// short
            public short vel;

            /// byte
            public byte aux;

            /// int
            public int home;

            /// short
            public short perror;

            /// byte
            public byte inport1;

            /// byte
            public byte inport2;

            /// byte
            public byte inport3;

            /// int
            public int cmdpos;

            /// int
            public int cmdvel;

            /// int
            public int cmdacc;

            /// byte
            public byte cmdpwm;

            /// short
            public short cmdadc;

            /// GAINVECT->_GAINVECT
            public GAINVECT gain;

            /// int
            public int stoppos;

            /// byte
            public byte outport1;

            /// byte
            public byte outport2;

            /// byte
            public byte outport3;

            /// byte
            public byte outport4;

            /// byte
            public byte stopctrl;

            /// byte
            public byte movectrl;

            /// byte
            public byte ioctrl;

            /// byte
            public byte homectrl;

            /// byte
            public byte servoinit;

            /// byte
            public byte stp_dir_mode;

            /// byte
            public byte ph_adv;

            /// byte
            public byte ph_off;

            /// byte
            public byte advmode;

            /// byte
            public byte npoints;

            /// int
            public int last_ppoint;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct STEPMOD
        {

            /// int
            public int pos;

            /// byte
            public byte ad;

            /// unsigned short
            public ushort st;

            /// byte
            public byte inbyte;

            /// int
            public int home;

            /// int
            public int cmdpos;

            /// byte
            public byte cmdspeed;

            /// byte
            public byte cmdacc;

            /// short
            public short cmdst;

            /// double
            public double st_period;

            /// byte
            public byte move_mode;

            /// byte
            public byte min_speed;

            /// byte
            public byte stopctrl;

            /// byte
            public byte outbyte;

            /// byte
            public byte homectrl;

            /// byte
            public byte ctrlmode;

            /// byte
            public byte run_pwm;

            /// byte
            public byte hold_pwm;

            /// byte
            public byte therm_limit;

            /// byte
            public byte emergency_acc;

            /// byte
            public byte stat_io;
        }

        #endregion

        #region Methods
        /// Return Type: int
        ///portname: char*
        ///baudrate: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnInit")]
        public static extern int LdcnInit(char[] portname, uint baudrate);


        /// Return Type: int
        ///portname: char*
        ///baudrate: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnFullInit")]
        public static extern int LdcnFullInit(char[] portname, uint baudrate);


        /// Return Type: int
        ///portname: char*
        ///baudrate: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnRestoreNetwork")]
        public static extern int LdcnRestoreNetwork(System.IntPtr portname, uint baudrate);


        /// Return Type: BOOL->int
        ///addr: byte
        ///cmd: byte
        ///datastr: char*
        ///n: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnSendCmd")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnSendCmd(byte addr, byte cmd, System.IntPtr datastr, byte n);


        /// Return Type: void
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "FixSioError")]
        public static extern void FixSioError();


        /// Return Type: void
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnShutdown")]
        public static extern void LdcnShutdown();


        /// Return Type: BOOL->int
        ///addr: byte
        ///groupaddr: byte
        ///leader: boolean
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnSetGroupAddr")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnSetGroupAddr(byte addr, byte groupaddr, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.I1)] bool leader);


        /// Return Type: BOOL->int
        ///addr: byte
        ///statusitems: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnDefineStatus")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnDefineStatus(byte addr, byte statusitems);


        /// Return Type: BOOL->int
        ///addr: byte
        ///statusitems: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnReadStatus")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnReadStatus(byte addr, byte statusitems);


        /// Return Type: BOOL->int
        ///groupaddr: byte
        ///baudrate: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnChangeBaud")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnChangeBaud(byte groupaddr, uint baudrate);


        /// Return Type: BOOL->int
        ///groupaddr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnSynchInput")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnSynchInput(byte groupaddr);


        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnNoOp")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnNoOp(byte addr);


        /// Return Type: BOOL->int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnHardReset")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnHardReset();


        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnResetDevice")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnResetDevice(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnGetStat")]
        public static extern byte LdcnGetStat(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnGetStatItems")]
        public static extern byte LdcnGetStatItems(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnGetModType")]
        public static extern byte LdcnGetModType(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnGetModVer")]
        public static extern byte LdcnGetModVer(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnGetGroupAddr")]
        public static extern byte LdcnGetGroupAddr(byte addr);


        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "LdcnGroupLeader")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool LdcnGroupLeader(byte addr);


        /// Return Type: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "GetComPort")]
        public static extern System.IntPtr GetComPort();


        /// Return Type: int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "GetSioError")]
        public static extern int GetSioError();


        /// Return Type: int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "IsBusy")]
        public static extern int IsBusy();


        /// Return Type: int
        ///msgstr: char*
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "SimpleMsgBox")]
        public static extern int SimpleMsgBox(System.IntPtr msgstr);


        /// Return Type: HANDLE->void*
        ///name: char*
        ///baudrate: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "SioOpen")]
        public static extern System.IntPtr SioOpen(System.IntPtr name, uint baudrate);


        /// Return Type: BOOL->int
        ///ComPort: HANDLE->void*
        ///baudrate: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "SioChangeBaud")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SioChangeBaud(System.IntPtr ComPort, uint baudrate);


        /// Return Type: BOOL->int
        ///ComPort: HANDLE->void*
        ///stuff: char*
        ///n: int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "SioPutChars")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SioPutChars(System.IntPtr ComPort, System.IntPtr stuff, int n);


        /// Return Type: DWORD->unsigned int
        ///ComPort: HANDLE->void*
        ///stuff: char*
        ///n: int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "SioGetChars")]
        public static extern uint SioGetChars(System.IntPtr ComPort, System.IntPtr stuff, int n);


        /// Return Type: DWORD->unsigned int
        ///ComPort: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "SioTest")]
        public static extern uint SioTest(System.IntPtr ComPort);


        /// Return Type: BOOL->int
        ///ComPort: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "SioClrInbuf")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SioClrInbuf(System.IntPtr ComPort);


        /// Return Type: BOOL->int
        ///ComPort: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "SioClose")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SioClose(System.IntPtr ComPort);

        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetStat")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoGetStat(byte addr);


        /// Return Type: int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetPos")]
        public static extern int ServoGetPos(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetAD")]
        public static extern byte ServoGetAD(byte addr);


        /// Return Type: short
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetAD10")]
        public static extern short ServoGetAD10(byte addr);


        /// Return Type: short
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetVel")]
        public static extern short ServoGetVel(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetAux")]
        public static extern byte ServoGetAux(byte addr);


        /// Return Type: int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetHome")]
        public static extern int ServoGetHome(byte addr);


        /// Return Type: short
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetPError")]
        public static extern short ServoGetPError(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetInport1")]
        public static extern byte ServoGetInport1(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetInport2")]
        public static extern byte ServoGetInport2(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetInport3")]
        public static extern byte ServoGetInport3(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetOutport1")]
        public static extern byte ServoGetOutport1(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetOutport2")]
        public static extern byte ServoGetOutport2(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetOutport3")]
        public static extern byte ServoGetOutport3(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetOutport4")]
        public static extern byte ServoGetOutport4(byte addr);


        /// Return Type: int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetCmdPos")]
        public static extern int ServoGetCmdPos(byte addr);


        /// Return Type: int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetCmdVel")]
        public static extern int ServoGetCmdVel(byte addr);


        /// Return Type: int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetCmdAcc")]
        public static extern int ServoGetCmdAcc(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetCmdPwm")]
        public static extern byte ServoGetCmdPwm(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetCmdAdc")]
        public static extern byte ServoGetCmdAdc(byte addr);


        /// Return Type: short
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetCmdAdc10")]
        public static extern short ServoGetCmdAdc10(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetMoveCtrl")]
        public static extern byte ServoGetMoveCtrl(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetStopCtrl")]
        public static extern byte ServoGetStopCtrl(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetHomeCtrl")]
        public static extern byte ServoGetHomeCtrl(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetIoCtrl")]
        public static extern byte ServoGetIoCtrl(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetServoInit")]
        public static extern byte ServoGetServoInit(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetSDMode")]
        public static extern byte ServoGetSDMode(byte addr);


        /// Return Type: void
        ///addr: byte
        ///kp: short*
        ///kd: short*
        ///ki: short*
        ///il: short*
        ///ol: byte*
        ///cl: byte*
        ///el: short*
        ///sr: byte*
        ///dc: byte*
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetGain")]
        public static extern void ServoGetGain(byte addr, ref short kp, ref short kd, ref short ki, ref short il, ref byte ol, ref byte cl, ref short el, ref byte sr, ref byte dc);


        /// Return Type: BOOL->int
        ///addr: byte
        ///kp: short
        ///kd: short
        ///ki: short
        ///il: short
        ///ol: byte
        ///cl: byte
        ///el: short
        ///sr: byte
        ///dc: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoSetGain")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoSetGain(byte addr, short kp, short kd, short ki, short il, byte ol, byte cl, short el, byte sr, byte dc);


        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoResetPos")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoResetPos(byte addr);


        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoClearBits")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoClearBits(byte addr);


        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoStopMotor")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoStopMotor(byte addr, byte mode);



        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        ///pos: int
        ///vel: int
        ///acc: int
        ///pwm: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoLoadTraj")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoLoadTraj(byte addr, byte mode, int pos, int vel, int acc, byte pwm);


        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        ///pos: int
        ///vel: int
        ///acc: int
        ///pwm: short
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoLoadTraj10")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoLoadTraj10(byte addr, byte mode, int pos, int vel, int acc, short pwm);


        /// Return Type: BOOL->int
        ///groupaddr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoStartMotion")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoStartMotion(byte groupaddr);


        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoSetHoming")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoSetHoming(byte addr, byte mode);


        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        ///out1: byte
        ///out2: byte
        ///out3: byte
        ///out4: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoSetOutputs")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoSetOutputs(byte addr, byte mode, byte out1, byte out2, byte out3, byte out4);


        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        ///out1: byte
        ///out2: byte
        ///out3: byte
        ///out4: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoEEPROMCtrl")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoEEPROMCtrl(byte addr, byte mode, byte out1, byte out2, byte out3, byte out4);


        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoResetRelHome")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoResetRelHome(byte addr);


        /// Return Type: BOOL->int
        ///addr: byte
        ///fast: boolean
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoSetFastPath")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoSetFastPath(byte addr, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.I1)] bool fast);


        /// Return Type: void
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoInitPath")]
        public static extern void ServoInitPath(byte addr);


        /// Return Type: BOOL->int
        ///addr: byte
        ///npoints: int
        ///path: int*
        ///high_freq: boolean
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoAddPathPoints")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoAddPathPoints(byte addr, int npoints, ref int path, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.I1)] bool high_freq);


        /// Return Type: BOOL->int
        ///groupaddr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoStartPathMode")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ServoStartPathMode(byte groupaddr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "ServoGetNPoints")]
        public static extern byte ServoGetNPoints(byte addr);


        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetStat")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool StepGetStat(byte addr);


        /// Return Type: int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetPos")]
        public static extern int StepGetPos(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetAD")]
        public static extern byte StepGetAD(byte addr);


        /// Return Type: unsigned short
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetStepTime")]
        public static extern ushort StepGetStepTime(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetInbyte")]
        public static extern byte StepGetInbyte(byte addr);


        /// Return Type: int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetHome")]
        public static extern int StepGetHome(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetIObyte")]
        public static extern byte StepGetIObyte(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetMvMode")]
        public static extern byte StepGetMvMode(byte addr);


        /// Return Type: int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetCmdPos")]
        public static extern int StepGetCmdPos(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetCmdSpeed")]
        public static extern byte StepGetCmdSpeed(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetCmdAcc")]
        public static extern byte StepGetCmdAcc(byte addr);


        /// Return Type: unsigned short
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetCmdST")]
        public static extern ushort StepGetCmdST(byte addr);


        /// Return Type: double
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetStepPeriod")]
        public static extern double StepGetStepPeriod(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetCtrlMode")]
        public static extern byte StepGetCtrlMode(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetMinSpeed")]
        public static extern byte StepGetMinSpeed(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetRunCurrent")]
        public static extern byte StepGetRunCurrent(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetHoldCurrent")]
        public static extern byte StepGetHoldCurrent(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetThermLimit")]
        public static extern byte StepGetThermLimit(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetEmAcc")]
        public static extern byte StepGetEmAcc(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetOutputs")]
        public static extern byte StepGetOutputs(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetHomeCtrl")]
        public static extern byte StepGetHomeCtrl(byte addr);


        /// Return Type: byte
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepGetStopCtrl")]
        public static extern byte StepGetStopCtrl(byte addr);

        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepSetParam")]
        public static extern bool StepSetParam(byte addr, byte mode, byte minspeed, byte runcur, byte holdcur,
            byte thermlim, byte em_acc = 255);

        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        ///pos: int
        ///speed: byte
        ///acc: byte
        ///steptime: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepLoadTraj")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool StepLoadTraj(byte addr, byte mode, int pos, byte speed, byte acc, ushort steptime);


        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        ///pos: int
        ///step_period: double
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepLoadUnprofiledTraj")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool StepLoadUnprofiledTraj(byte addr, byte mode, int pos, double step_period);


        /// Return Type: BOOL->int
        ///addr: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepResetPos")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool StepResetPos(byte addr);


        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepStopMotor")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool StepStopMotor(byte addr, byte mode);


        /// Return Type: BOOL->int
        ///addr: byte
        ///outbyte: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepSetOutputs")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool StepSetOutputs(byte addr, byte outbyte);


        /// Return Type: BOOL->int
        ///addr: byte
        ///mode: byte
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepSetHoming")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool StepSetHoming(byte addr, byte mode);


        /// Return Type: int
        ///StepsPerSecond: double
        ///SpeedFactor: int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepsPerSec2StepTime")]
        public static extern int StepsPerSec2StepTime(double StepsPerSecond, int SpeedFactor);


        /// Return Type: double
        ///InitialTimerCount: int
        ///SpeedFactor: int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepTime2StepsPerSec")]
        public static extern double StepTime2StepsPerSec(int InitialTimerCount, int SpeedFactor);


        /// Return Type: double
        ///StepsPerSecond: double
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "StepsPerSec2mSecPerStep")]
        public static extern double StepsPerSec2mSecPerStep(double StepsPerSecond);


        /// Return Type: double
        ///mSecPerStep: double
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "mSecPerStep2StepsPerSec")]
        public static extern double mSecPerStep2StepsPerSec(double mSecPerStep);


        /// Return Type: double
        ///SpeedFactor: int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "MinStepPeriod")]
        public static extern double MinStepPeriod(int SpeedFactor);


        /// Return Type: double
        ///SpeedFactor: int
        [System.Runtime.InteropServices.DllImportAttribute("Ldcnlib.dll", EntryPoint = "MaxStepPeriod")]
        public static extern double MaxStepPeriod(int SpeedFactor);

        #endregion
    }
}
