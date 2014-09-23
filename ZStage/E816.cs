using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;

namespace ZStage
{
    internal static class NativeMethods
    {
        /////////////////////////////////////////////////////////////////////////////
        // DLL initialization and comm functions

        [DllImport("E816_DLL.dll", EntryPoint = "E816_ConnectRS232")]
        public static extern int ConnectRS232(int nPortNr, int nBaudRate);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_CloseConnection")]
        public static extern void CloseConnection(int iId);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_GetError")]
        public static extern int GetError(int iId);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_TranslateError", BestFitMapping=false, 
            ThrowOnUnmappableChar=true)]
        public static extern int TranslateError(int errNr, [MarshalAs(UnmanagedType.LPStr)]StringBuilder sBuffer, int iMaxlen);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_IsConnected")]
        public static extern int IsConnected(int ID);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_SetTimeout")]
        public static extern int SetTimeout(int ID, int timeout);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_EnumerateUSB", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int EnumerateUSB([MarshalAs(UnmanagedType.LPStr)]StringBuilder sBuffer, int iBufferSize, [MarshalAs(UnmanagedType.LPStr)]string sFilter);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_ConnectUSB", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int ConnectUSB([MarshalAs(UnmanagedType.LPStr)]string sDescription);

        /////////////////////////////////////////////////////////////////////////////
        // general
        [DllImport("E816_DLL.dll", EntryPoint = "E816_qERR")]
        public static extern int qERR(int iId, ref int nError);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qIDN", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qIDN(int iId, [MarshalAs(UnmanagedType.LPStr)]StringBuilder buffer, int iMaxlen);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qHLP", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qHLP(int ID, [MarshalAs(UnmanagedType.LPStr)]StringBuilder sBuffer, int maxlen);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qPOS", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qPOS(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, double[] pdValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qONT", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qONT(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbOnTarget);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MOV", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MOV(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, double[] pdValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qMOV", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qMOV(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, double[] pdValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MVR", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MVR(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, double[] pdValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_SVO", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int SVO(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qSVO", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qSVO(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_DCO", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int DCO(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qDCO", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qDCO(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_SVA", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int SVA(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, double[] pdValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qSVA", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qSVA(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, double[] pdValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_SVR", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int SVR(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, double[] pdValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qVOL", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qVOL(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, double[] pdValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qOVF", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qOVF(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbOverflow);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_AVG")]
        public static extern int AVG(int ID, int nAverage);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qAVG")]
        public static extern int qAVG(int ID, ref int pnAverage);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_SPA", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int SPA(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] iCmdarray, double[] dValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qSPA", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qSPA(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] iCmdarray, double[] dValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_WPA", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int WPA(int ID, [MarshalAs(UnmanagedType.LPStr)]string swPassword);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qSAI", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qSAI(int ID, [MarshalAs(UnmanagedType.LPStr)]StringBuilder axes, int maxlen);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qSSN", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qSSN(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] piValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qSCH")]
        public static extern int qSCH(int ID, ref char pcChannelName);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_SCH")]
        public static extern int SCH(int ID, char cChannelName);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_RST")]
        public static extern int RST(int ID);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_BDR")]
        public static extern int BDR(int ID, int nBaudRate);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qBDR")]
        public static extern int qBDR(int ID, ref int pnBaudRate);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qI2C")]
        public static extern int qI2C(int ID, ref int errorCode, ref char pcChannel);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_WTO")]
        public static extern int WTO(int ID, char cAxis, int nNumber);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_WTOTimer")]
        public static extern int WTOTimer(int ID, char cAxis, int nNumber, int timer);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_SWT")]
        public static extern int SWT(int ID, char cAxis, int nIndex, double dValue);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qSWT")]
        public static extern int qSWT(int ID, char cAxis, int nIndex, ref double pdValue);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MVT", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MVT(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qMVT", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qMVT(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbValarray);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qDIP", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qDIP(int ID, [MarshalAs(UnmanagedType.LPStr)]string sAxes, int[] pbValarray);

        /////////////////////////////////////////////////////////////////////////////

        [DllImport("E816_DLL.dll", EntryPoint = "E816_IsRecordingMacro")]
        public static extern int IsRecordingMacro(int ID, ref int pbRecordingMacro);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_IsRunningMacro")]
        public static extern int IsRunningMacro(int ID, ref int pbRunningMacro);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MAC_DEL", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MAC_DEL(int ID, [MarshalAs(UnmanagedType.LPStr)]string sName);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MAC_START", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MAC_START(int ID, [MarshalAs(UnmanagedType.LPStr)]string sName);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MAC_NSTART", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MAC_NSTART(int ID, [MarshalAs(UnmanagedType.LPStr)]string sName, int nrRuns);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_qMAC", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int qMAC(int ID, [MarshalAs(UnmanagedType.LPStr)]string sName, [MarshalAs(UnmanagedType.LPStr)]StringBuilder sBuffer, int maxlen);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MAC_BEG", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MAC_BEG(int ID, [MarshalAs(UnmanagedType.LPStr)]string sName);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MAC_END")]
        public static extern int MAC_END(int ID);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MAC_qFREE")]
        public static extern int MAC_qFREE(int ID, ref int pNumberChars);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MAC_DEF", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MAC_DEF(int ID, [MarshalAs(UnmanagedType.LPStr)]string sName);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_MAC_qDEF", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int MAC_qDEF(int ID, [MarshalAs(UnmanagedType.LPStr)]StringBuilder sBuffer, int maxlen);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_SaveMacroToFile", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int SaveMacroToFile(int ID, [MarshalAs(UnmanagedType.LPStr)]string sFileName, [MarshalAs(UnmanagedType.LPStr)]string sMacroName);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_LoadMacroFromFile", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int LoadMacroFromFile(int ID, [MarshalAs(UnmanagedType.LPStr)]string sFileName, [MarshalAs(UnmanagedType.LPStr)]string sMacroName);

        /////////////////////////////////////////////////////////////////////////////

        [DllImport("E816_DLL.dll", EntryPoint = "E816_GcsCommandset", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int GcsCommandset(int ID, [MarshalAs(UnmanagedType.LPStr)]string sCommand);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_GcsGetAnswer", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int GcsGetAnswer(int ID, [MarshalAs(UnmanagedType.LPStr)]StringBuilder sAnswer, int bufsize);

        [DllImport("E816_DLL.dll", EntryPoint = "E816_GcsGetAnswerSize", BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern int GcsGetAnswerSize(int ID, ref int iAnswerSize);
    }
}
