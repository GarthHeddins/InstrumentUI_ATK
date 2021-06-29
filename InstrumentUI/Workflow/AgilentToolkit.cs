using System.Configuration;
using System.Diagnostics;
using CommonShared;
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using MicroLabData;
using System.Threading;

namespace InstrumentUI_ATK.Workflow
{
    internal class AgilentToolkit : IAgilentToolKit
    {
        public class InstrumentFailedException : System.Exception
        {
            public InstrumentFailedException(int code)
            {
                nInitCode = code;
            }

            public int nInitCode;
        }


            #region constants

            public const double MLInstrumentRangeMaxFrom = 4000.0;
            public const double MLInstrumentRangeMaxTo = 650.0;
            public const int MLCleanRes = 4;

            #endregion

            protected static int m_bInitialized = -1;

            public static int Initialized
            {
                get { return m_bInitialized; }
                set { m_bInitialized = value; }
            }

            protected static bool m_binitializing = false;

            public static bool initializing
            {
                get { return m_binitializing; }
                set { m_binitializing = value; }
            }

            #region protected/private methods

            /// <summary>
            /// Determines if instrument has been successfully initialized and if not, attempts initialization.
            /// </summary>
            /// Each instrument interface function should make a call to this method to verify that the instrument
            /// has been initialized.
            /// <returns>0 if success, non-zero error code otherwise</returns>
            
            public int InitializationCheck()
            {
                int nInitCode = 0; // init as success
                bool bExiting = false;                                

                while (Initialized == -1 && !bExiting) // not yet initialized
                {
                    if (!initializing)
                    {
                        if (Initialized == -1)
                        {
                            initializing = true;
                            nInitCode = Init();                            
                            //nInitCode = -200; // diagnostic (injected error condition)
                            if (nInitCode == 0)
                            {
                                Initialized = 0; // successful initialization
                                initializing = false;
                            }
                            else
                            {
                                bExiting = true;
                                //Application.Exit();
                                initializing = false;
                                InstrumentFailedException anError = new InstrumentFailedException(nInitCode);                                
                                throw anError;
                            }
                        }
                    }
                }
                return nInitCode;
            }

            #endregion

            #region event handlers

            #endregion

            #region instrument interface calls

            public int SetComputeParams(PHASEPOINTS ppoints, PHASETYPE ptype, APODTYPE papod, APODTYPE iapod,
                                               ZFFTYPE zff, OFFSETCORRECTTYPE offset)
                
            {
                InitializationCheck();
                return FTIRInst_SetComputeParams(ppoints, ptype, papod, iapod, zff, offset);
            }

            [DllImport("FTIRInst.dll", SetLastError = true,CallingConvention=CallingConvention.Cdecl)]
            private static extern int FTIRInst_SetComputeParams(PHASEPOINTS ppoints, PHASETYPE ptype, APODTYPE papod,
                                                                APODTYPE iapod, ZFFTYPE zff, OFFSETCORRECTTYPE offset);

            public int StartSingleBeam(int numScans, double from, double to, int res, bool bAutoSetBkg,
                                              bool bAutoSetClean)
            {               
            
                int ret = 0;
                try
                {
                    string sss = "Parameters: numScans=" + numScans.ToString() +
                                 " from=" + from.ToString() +
                                 " to=" + to.ToString() +
                                 " res=" + res.ToString() +
                                 " SetBKG=" + bAutoSetBkg.ToString() +
                                 " SetCLN=" + bAutoSetClean.ToString() +
                                 "\n";
                    Trace.Write("About to StartSingleBeam\n");
                    Trace.Write(sss);
                    ret = FTIRInst_dptrStartSingleBeam(numScans, ref from, ref to, res, (bAutoSetBkg != false) ? 1 : 0,
                                                       (bAutoSetClean != false) ? 1 : 0);                    
                }
                catch (Exception ce)
                {
                    MessageBox.Show(ce.Message, "Exception on StartSingleBeam");
                    Trace.Write("Exception Caught\n");
                }

                int currentUnits = 0;
                int totalUnits = 0;
                FTIR_STATE progress = FTIR_STATE.FTIR_Collecting;
                do
                {
                    progress = FTIRInst_CheckProgress(ref currentUnits, ref totalUnits);
                } while (progress == FTIR_STATE.FTIR_Collecting);

                return ret;

            }

            [DllImport("FTIRInst.dll", SetLastError = true,CallingConvention=CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrStartSingleBeam(int numScans, ref double from, ref double to, int res,
                                                                   int bAutoSetBkg, int bAutoSetClean);

            public FTIR_STATE CheckProgress(ref int currentUnits, ref int totalUnits)
            {
                // Since this method is often called many times in succession, we don't do the
                // initialization check in this function so that the user is not barraged with
                // a mass of message boxes - if the instrument has not yet been initialized
                // when this is called, the user has no doubt already been notified.
                //InitializationCheck();
                return FTIRInst_CheckProgress(ref currentUnits, ref totalUnits);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern FTIR_STATE FTIRInst_CheckProgress(ref int currentUnits, ref int totalUnits);

            public FTIR_STATE CheckProgressEx(ref int currentUnits, ref int totalUnits, ref int rejectedScans)
            {
                try
                {
                    return FTIRInst_CheckProgressEx(ref currentUnits, ref totalUnits, ref rejectedScans);
                }
                catch
                {
                    rejectedScans = 0;
                    return FTIRInst_CheckProgress(ref currentUnits, ref totalUnits);
                }
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern FTIR_STATE FTIRInst_CheckProgressEx(ref int currentUnits, ref int totalUnits, ref int rejectedScans);

        public bool CheckTumbIIR()
        {
            bool stat = false;
            int enEnergyStatus = 0;
            float fBatteryStatus = 0;
            float fSourceCurrentStatus = 0;
            float fSourceVoltageStatus = 0;
            int nLaserStatus = 0;
            float fDetectorStatus = 0;
            int success = FTIRInst_GetStatus(ref enEnergyStatus, ref fBatteryStatus, ref fSourceCurrentStatus, ref fSourceVoltageStatus, ref nLaserStatus, ref fDetectorStatus);
            Thread.Sleep(1000);
            while (enEnergyStatus < 3000)
            {
                success = FTIRInst_GetStatus(ref enEnergyStatus, ref fBatteryStatus, ref fSourceCurrentStatus, ref fSourceVoltageStatus, ref nLaserStatus, ref fDetectorStatus);
                if (enEnergyStatus > 3000)
                    break;
                Trace.WriteLine("Tumbler status = " + enEnergyStatus);
                var result = MessageBox.Show("Check Tumbler", "Tumbler Position", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    break;
                }
            }
            if (enEnergyStatus < 3000)
                stat = false;
            else
                stat = true;

            return stat;
        }

        public FTIR_STATE CheckProgressStruct(ref MLProgress pProgress)
            {
                return pProgress.m_progress.state;                                
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_CheckProgressStruct(ref _progress pProgress);

            public int GetSingleBeamSize()
            {
                double refF = 0;
                double refT = 0;
                int refR = 0;
                InitializationCheck();
                return FTIRInst_dptrGetSingleBeam(null, 0, ref refF, ref refT, ref refR);
            }

            public int GetSingleBeam(double[] array, int size, ref double actualFrom, ref double actualTo,
                                            ref int actualRes)
            {
                InitializationCheck();
                return FTIRInst_dptrGetSingleBeam(array, size, ref actualFrom, ref actualTo, ref actualRes);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrGetSingleBeam(double[] array, int size, ref double actualFrom,
                                                                 ref double actualTo, ref int actualRes);

            // JSA 12/06/07 new functions to retrieve the instrument's stored background spectrum (or its size) - if one exists
            public int GetBackgroundSize()
            {
                double refF = 0;
                double refT = 0;
                int refR = 0;
                InitializationCheck();
                return GetBackground(null, 0, ref refF, ref refT, ref refR);
            }
            
            public int GetBackground(double[] array, int size, ref double actualFrom, ref double actualTo,
                                            ref int actualRes)
            {
                InitializationCheck();
                try
                {
                    return FTIRInst_dptrGetBackground(array, size, ref actualFrom, ref actualTo, ref actualRes);
                }
                catch
                {
                    return FTIRInst_dptrGetSingleBeam(array, size, ref actualFrom, ref actualTo, ref actualRes);
                }

            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrGetBackground(double[] array, int size, ref double actualFrom,
                                                                 ref double actualTo, ref int actualRes);

            // JSA 12/06/07 new functions to retrieve the instrument's stored clean spectrum (or its size) - if one exists
            public int GetCleanSize()
            {
                double refF = 0;
                double refT = 0;
                int refR = 0;
                return GetClean(null, 0, ref refF, ref refT, ref refR);
            }

            public int GetClean(double[] array, int size, ref double actualFrom, ref double actualTo,
                                       ref int actualRes)
            {
                InitializationCheck();
                try
                {
                    return FTIRInst_dptrGetClean(array, size, ref actualFrom, ref actualTo, ref actualRes);
                }
                catch
                {
                    return FTIRInst_dptrGetSingleBeam(array, size, ref actualFrom, ref actualTo, ref actualRes);
                }
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrGetClean(double[] array, int size, ref double actualFrom,
                                                            ref double actualTo, ref int actualRes);

            public int GetRatioSpectrum(double[] bkgarray, double[] smparray, double[] outarray, int size,
                                               DATAYTYPE ytype)
            {
                InitializationCheck();
                try
                {
                    return FTIRInst_dptrGetRatioSpectrum(bkgarray, smparray, outarray, size, ytype);
                }
                catch
                {
                    MessageBox.Show("Older FTIRInst.dll interface found.  Need to update instrument driver.");
                    return -1;
                }
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrGetRatioSpectrum(double[] bkgarray, double[] smparray,
                                                                    double[] outarray, int size, DATAYTYPE ytype);

            // JSA 12/06/07 FTIRInst_dptrSetBackground() is not yet fully implemented in the firmware - do not call it!
            // Currently, the only way to set the background is via FTIRInst_dptrStartSingleBeam() with the bAutoSetBkg set to TRUE
            /*
            public static int SetBackground(double[] array, int size, double from, double to, int res)
            {
                InitializationCheck();
                return FTIRInst_dptrSetBackground(array, size, ref from, ref to, res);
            }
            [DllImport("FTIRInst.dll", SetLastError = true)]
            private static extern int FTIRInst_dptrSetBackground(double[] array, int size, ref double from, ref double to, int res);
            */

            // JSA 12/06/07 FTIRInst_dptrSetClean() is not yet fully implemented in the firmware - do not call it!
            // Currently, the only way to set the background is via FTIRInst_dptrStartSingleBeam() with the bAutoSetClean set to TRUE
            
            public int SetClean(double[] array, int size, double from, double to, int res)
            {
                InitializationCheck();
                return FTIRInst_dptrSetClean(array, size, ref from, ref to, res);
            }
            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrSetClean(double[] array, int size, ref double from, ref double to, int res);
            

            public int StartSpectrum(int numScans, double from, double to, int res, DATAXTYPE xtype,
                                            DATAYTYPE ytype, bool bAutoSetUnknown)
            {
                InitializationCheck();
                return FTIRInst_dptrStartSpectrum(numScans, ref from, ref to, res, xtype, ytype,
                                                  (bAutoSetUnknown != false) ? 1 : 0);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrStartSpectrum(int numScans, ref double from, ref double to, int res,
                                                                 DATAXTYPE xtype, DATAYTYPE ytype, int bAutoSetUnknown);

            public int GetSpectrumSize()
            {
                double refF = 0;
                double refT = 0;
                int refR = 0;
                InitializationCheck();
                return FTIRInst_dptrGetSpectrum(null, 0, ref refF, ref refT, ref refR);
            }

            public int GetSpectrum(double[] array, int size, ref double actualFrom, ref double actualTo,
                                          ref int actualRes)
            {
                InitializationCheck();
                return FTIRInst_dptrGetSpectrum(array, size, ref actualFrom, ref actualTo, ref actualRes);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrGetSpectrum(double[] array, int size, ref double actualFrom,
                                                               ref double actualTo, ref int actualRes);

            public int SetUnknown(double[] array, int size, double from, double to, int res, DATAXTYPE xtype,
                                         DATAYTYPE ytype, bool bIsATR)
            {
                InitializationCheck();
                return FTIRInst_dptrSetUnknown(array, size, ref from, ref to, res, xtype, ytype,
                                               (bIsATR != false) ? 1 : 0);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrSetUnknown(double[] array, int size, ref double from, ref double to,
                                                              int res, DATAXTYPE xtype, DATAYTYPE ytype, int bIsATR);

            public int KillCollection()
            {
                InitializationCheck();
                return FTIRInst_KillCollection();
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_KillCollection();

            public int SoftReset()
            {
                InitializationCheck();
                return FTIRInst_SoftReset();
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_SoftReset();

            public int GetVersion(ref int fwRev, ref int dllRev, ref int spareRev)
            {
                InitializationCheck();
                return FTIRInst_GetVersion(ref fwRev, ref dllRev, ref spareRev);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_GetVersion(ref int fwRev, ref int dllRev, ref int spareRev);

            public int GetStatus(ref int nEnergyStatus, ref float fBatteryStatus, ref float fSourceCurrentStatus,
                                        ref float fSourceVoltageStatus, ref int nLaserStatus, ref float fDetectorStatus)
            {
                InitializationCheck();
                return FTIRInst_GetStatus(ref nEnergyStatus, ref fBatteryStatus, ref fSourceCurrentStatus,
                                          ref fSourceVoltageStatus, ref nLaserStatus, ref fDetectorStatus);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_GetStatus(ref int nEnergyStatus, ref float fBatteryStatus,
                                                         ref float fSourceCurrentStatus, ref float fSourceVoltageStatus,
                                                         ref int nLaserStatus, ref float fDetectorStatus);

            public int GetVersionEx(MLVersion vInfo)
            {
                Trace.WriteLine("Entering GetVersionEx in AgilentToolkit");
                InitializationCheck();
                bool bCalled = false;
                int ret = 0;
                try
                {
                    Trace.WriteLine("Entering try block in GetVersionEx in AgilentToolkit");
                    ExFunctionInvoke ex = new ExFunctionInvoke();
                    ret = ex.GetVersionEx(ref vInfo.m_version);
                    bCalled = true;
                }
                catch(Exception ex)
                {
                    Trace.WriteLine("Entering catch block in GetVersionEx in AgilentToolkit");
                    Helper.LogError("GetVersionEx", "", ex, false);
                }
                if (!bCalled)
                {
                    ret = FTIRInst_GetVersion(ref vInfo.m_version.fwRev, ref vInfo.m_version.dllRev,
                                              ref vInfo.m_version.spareRev);
                    vInfo.m_version.dLaserWN = 7633.0;
                    vInfo.m_version.instrType = 0;
                    vInfo.m_version.sampleTechType = (int) SAMPLINGTECHNOLOGYTYPE.ST_TRANSMISSIONCELL;
                    vInfo.m_version.atrType = 1;
                    vInfo.m_version.dBasePathLength = 0.1;
                    vInfo.m_version.dAdjustPathLength = 0;
                }
                Trace.WriteLine("Exiting GetVersionEx in AgilentToolkit");
                return ret;
            }

            public int GetStatusEx(ref MLDiag dStatus)
            {
                InitializationCheck();
                bool bCalled = false;
                int ret = 0;
                try
                {
                    ExFunctionInvoke ex = new ExFunctionInvoke();
                    ret = ex.GetStatusEx(ref dStatus.m_diag);
                    bCalled = true;
                }
                catch
                {
                }
                if (!bCalled)
                {
                    float fBatStat = 0;
                    ret = FTIRInst_GetStatus(ref dStatus.m_diag.nEnergyStatus, ref fBatStat,
                                             ref dStatus.m_diag.fSourceCurrentStatus,
                                             ref dStatus.m_diag.fSourceVoltageStatus, ref dStatus.m_diag.nLaserStatus,
                                             ref dStatus.m_diag.fTempDetector);
                    dStatus.m_diag.nBatteryMinutes = (int) fBatStat;
                    dStatus.m_diag.nBatteryPct = 0;
                    dStatus.m_diag.nBatteryState = 0;
                    dStatus.m_diag.fTempCPU = 0;
                    dStatus.m_diag.fTempIR = 0;
                    dStatus.m_diag.fTempPower = 0;
                }
                return ret;
            }

            public  int Init()
            {
                // do not put this here - this is one of the few instrument interface functions
                // where you should not do this (would be recursive and would blow the stack)
                //InitializationCheck();                
                int setTarget = FTIRInst_SetTargetDeviceUsb();
                Trace.WriteLine("Set USB Target: " + setTarget.ToString());
                Thread.Sleep(2000);
                return FTIRInst_Init();
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_Init();

            public int Deinit()
            {
                //InitializationCheck();
                return FTIRInst_Deinit();
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_Deinit();

            public int GetLiveSpectrum(double from, double to, int res, DATAXTYPE xtype, DATAYTYPE ytype,
                                                          double[] array, int size, ref double actualFrom, ref double actualTo,
                                                          ref int actualRes)
            {
                InitializationCheck();
                return FTIRInst_dptrGetLiveSpectrum(ref from, ref to, res, xtype, ytype, array, size, ref actualFrom,
                                                    ref actualTo, ref actualRes);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrGetLiveSpectrum(ref double from, ref double to, int res,
                                                                   DATAXTYPE xtype, DATAYTYPE ytype, double[] array,
                                                                   int size, ref double actualFrom, ref double actualTo,
                                                                   ref int actualRes);

            public int SetLaserWN(double newLaser)
            {
                InitializationCheck();
                float fltLaser = (float) newLaser;
                return FTIRInst_SetLaserWaveNumber(ref fltLaser);
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_SetLaserWaveNumber(ref float newLaser);

            /*public double GetLaserWN()
            {
                InitializationCheck();
                float fltLaser = 0;
                FTIRInst_GetLaserWaveNumber(ref fltLaser);
                return (double) fltLaser;
            }*/

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_GetLaserWaveNumber(ref float curLaser);

            public int SetPathlength(double newPathlength, MLVersion _vInfo)
            {
                InitializationCheck();
                float fltPathlength = (float) newPathlength;
                int ret = FTIRInst_SetPathlen(ref fltPathlength);

                // Now force re-getting the vInfo
                GetVersionEx(_vInfo);
                return ret;
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_SetPathlen(ref float newPathlength);

            public double GetPathlength(MLVersion vers)
            {
                InitializationCheck();
                //double fltPathlength = FTIRInst_GetPathlen(ref vers.m_version);
                //double fltPathlength = 0; //FTIRInst_GetPathlen(ref vers.m_version);
                double fltPathlength = 0; //FTIRInst_GetPathlen(ref vers.m_version);
                return (double) fltPathlength;
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern double FTIRInst_GetPathlen(ref _instrumentMLVersionEx _vInfo);

            public int StartCoaddedIGram(int numScans, int nRes, int nPhasePts)
            {
                InitializationCheck();
                int ret = 0;
                try
                {
                    ret = FTIRInst_StartCoaddedIgram(numScans, nRes, nPhasePts);
                }
                catch (Exception ce)
                {
                    MessageBox.Show(ce.Message, "Exception on StartCoaddedIgram");
                }
                return ret;
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_StartCoaddedIgram(int numScans, int nRes, int nPhasePts);

            public int GetCoaddedIgram(double[] pArray, int nArraySize)
            {
                InitializationCheck();
                int ret = 0;
                try
                {
                    ret = FTIRInst_dptrGetCoaddedIgram(pArray, nArraySize);
                }
                catch (Exception ce)
                {
                    MessageBox.Show(ce.Message, "Exception on GetCoaddedIgram");
                }
                return ret;
            }

            [DllImport("FTIRInst.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            private static extern int FTIRInst_dptrGetCoaddedIgram(double[] pArray, int nArraySize);


            #endregion

            #region member variables

            #endregion

            #region enums and nested classes

            #endregion
        //}

        public class ExFunctionInvoke
        {
            public int GetVersionEx(ref _instrumentMLVersionEx _vInfo)
            {                
                return FTIRInst_GetVersionEx(ref _vInfo);
            }

            [DllImport("FTIRInst.dll", SetLastError = true,CallingConvention=CallingConvention.Cdecl)]
            private static extern int FTIRInst_GetVersionEx(ref _instrumentMLVersionEx _vInfo);

            public int GetStatusEx(ref _instrumentMLDiag _dStatus)
            {
                return FTIRInst_GetStatusEx(ref _dStatus);
            }

            [DllImport("FTIRInst.dll", SetLastError = true)]
            private static extern int FTIRInst_GetStatusEx(ref _instrumentMLDiag _dStatus);
        }

        [DllImport("FTIRInst.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int FTIRInst_SetTargetDeviceUsb();

        public bool DiagnosticCheck(string parameters, StringBuilder result)
        {
            Trace.WriteLine("Entering DiagnosticCheck method in AgilentToolkit");
            try
            {
                Trace.WriteLine("Entering try block in DiagnosticCheck method in AgilentToolkit");
                if (!bool.Parse(ConfigurationManager.AppSettings["Simulator"]))
                {
                    //TODO: this function does not exist in the sim dll
                    try
                    {
                        m_binitializing = false;
                        FTIRInst_SetTargetDeviceUsb();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Diagnostic exception: " + ex.ToString());
                    }
                }
                
            }
            catch (Exception e)
            {
                Trace.WriteLine("Entering catch block in DiagnosticCheck method in AgilentToolkit " + e);
                Helper.LogError("DiagnosticCheck", "", e, false);
                return false;
            }


            try
            {
                Trace.WriteLine("Entering try block in DiagnosticCheck method in AgilentToolkit");
                InitializationCheck();
            }
            catch (Exception e)
            {                 
                Trace.WriteLine("Entering catch block in DiagnosticCheck method in AgilentToolkit " + e);
                //Helper.LogError("DiagnosticCheck", "", e, true);
                return false;                
            }
            Trace.WriteLine("Exiting DiagnosticCheck method in AgilentToolkit");
            return true;
        }

        public bool CleanCheck(string peak1, string peak2,string highLimit,string lowLimit, StringBuilder result)
        {            
            double from = 600;
            double to = 4000;                        
            double wave1 = Convert.ToDouble(peak1);
            double wave2 = Convert.ToDouble(peak2);
            double dPeak1 = 0;
            double dPeak2 = 0;
            double dHighLim = Convert.ToDouble(highLimit);
            double dLowLim = Convert.ToDouble(lowLimit);
            
            bool tumbIIR = true;

            if (Helper.CurrentSpectrometer.Name.Equals("Agilent 5500t"))
            {
                tumbIIR = CheckTumbIIR();
            }

            if (tumbIIR == false)
            {
                Trace.WriteLine("ERROR: Check TumbIIR position");
                return false;
            }

            int numScans = Convert.ToInt32(ConfigurationManager.AppSettings["agil_numScans"]);
            int res = Convert.ToInt32(ConfigurationManager.AppSettings["agil_res"]);

            int ret = StartSingleBeam(5, from, to, res, false, false);     
            
            //FTIR_STATE { FTIR_Init = 0, FTIR_Collecting = 1, FTIR_DataReady = 2, FTIR_Aborting = 3, FTIR_Error = 4, FTIR_Transmitting = 5 };                                    

            double[] array1 = new double[2000];
            array1[0] = 0;
            double actualTo = to;
            double actualFrom = from;
            int actualRes = res;
            int size = 0;
            bool bPeak1Set = false;
            bool bPeak2Set = false;

            size = GetBackgroundSize();
            size = GetBackground(array1, size, ref actualFrom, ref actualTo,ref actualRes);
               
            double[] waveNum = new double[size];
            double waveInc = (actualTo - actualFrom) / (size - 1);
            int cnt = 0;
            waveNum[0] = Math.Round(actualFrom, 6);
            waveInc = Math.Round(waveInc, 6);
            for (cnt = 1; cnt < size; cnt++)
            {
                waveNum[cnt] = Math.Round(waveNum[cnt - 1], 6) + waveInc;
            }

            cnt = 0;
            string[] createText = new string[size];            
            string testString = string.Empty;

            for (cnt = 0; cnt < size; cnt++)
            {
                createText[cnt] = waveNum[cnt].ToString() + " " + (Math.Round(array1[cnt], 6)).ToString();
                if (waveNum[cnt] >= wave1 && !bPeak1Set)
                {
                    dPeak1 = Math.Round(array1[cnt], 6);
                    bPeak1Set = true;
                }
                if (waveNum[cnt] >= wave2 && !bPeak2Set)
                {
                    dPeak2 = Math.Round(array1[cnt], 6);
                    bPeak2Set = true;
                }

                testString += createText[cnt] + System.Environment.NewLine;
            }

            //File.WriteAllText("C:\\qta\\test\\CleanBkg.txt", testString);

            try
            {
                Trace.WriteLine("Enter CleanCheck Method ");               
                string cmd = "PeakHeight " + peak1;
                Trace.WriteLine("CleanCheck: Execute command: '" + cmd);               
                
                Trace.WriteLine("CleanCheck Peak1: " + dPeak1.ToString());
                Trace.WriteLine("CleanCheck Peak2: " + dPeak2.ToString());
                double ratio = 0;
                if (dPeak2 != 0)
                    ratio = dPeak1 / dPeak2;

                Trace.WriteLine(dPeak1.ToString() + " " + dPeak2.ToString() + " " + ratio.ToString());

                if (ratio > dHighLim)
                {
                    result.Append("Dirty Reading: " + ratio.ToString());                    
                }
                else if (ratio < dLowLim)
                {
                    result.Append("Dirty Reading: " + ratio.ToString());
                }
                else
                {
                    result.Append("Clean Reading: " + ratio.ToString());
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine("ERROR: CleanCheck Method Application Error: " + ex.Message.ToString());                
                return false;
            }
            finally
            {
                Trace.WriteLine("Exit CleanCheck Method. Clean Check Value:" + result.ToString());
            }

            return true;
        }

        public FTIR_STATE Checking()
        {
            FTIR_STATE fState;
            int currentUnits = 0;
            int totalUnits = 0;
            bool isChecking = true;

            while (isChecking)
            {
                fState = CheckProgress(ref currentUnits, ref totalUnits);
                switch (fState)
                {
                    case FTIR_STATE.FTIR_Init:
                        {
                            isChecking = true;
                            return fState;                            
                        }
                    case FTIR_STATE.FTIR_Collecting:
                        {
                            isChecking = true;
                            return fState;                          
                        }
                    case FTIR_STATE.FTIR_DataReady:
                        {
                            isChecking = false;
                            return fState;                            
                        }
                    case FTIR_STATE.FTIR_Aborting:
                        {
                            isChecking = false;
                            return fState;                            
                        }
                    case FTIR_STATE.FTIR_Error:
                        {
                            isChecking = false;
                            return fState;                           
                        }
                    case FTIR_STATE.FTIR_Transmitting:
                        {
                            isChecking = true;
                            return fState;                          
                        }
                    default:
                        {
                            return fState;
                        }
                }                
            }
            return fState = FTIR_STATE.FTIR_Aborting;
        }

        public bool CleanUp(string parameters, StringBuilder result)
        {
            throw new NotImplementedException();
        }

        public bool SampleScan(string parameters, StringBuilder result)
        {
            throw new NotImplementedException();
        }

    }
}