using System.Text;
using CommonShared;
using MicroLabData;

namespace InstrumentUI_ATK.Workflow
{
    interface IAgilentToolKit
    {
        FTIR_STATE CheckProgress(ref int currentUnits, ref int totalUnits);
        FTIR_STATE CheckProgressEx(ref int currentUnits, ref int totalUnits, ref int rejectedScans);
        FTIR_STATE CheckProgressStruct(ref MLProgress pProgress);
        bool CleanCheck(string peak1, string peak2, string highLimit, string lowLimit, StringBuilder result);
        bool CleanUp(string parameters, StringBuilder result);
        bool DiagnosticCheck(string parameters, StringBuilder result);
        int Deinit();
        int GetBackground(double[] array, int size, ref double actualFrom, ref double actualTo, ref int actualRes);
        int GetBackgroundSize();
        int GetClean(double[] array, int size, ref double actualFrom, ref double actualTo, ref int actualRes);
        int GetCleanSize();
        int GetCoaddedIgram(double[] pArray, int nArraySize);

        int GetLiveSpectrum(double from, double to, int res, DATAXTYPE xtype, DATAYTYPE ytype,
                            double[] array, int size, ref double actualFrom, ref double actualTo,
                            ref int actualRes);
        double GetPathlength(MLVersion vers);
        int GetSingleBeam(double[] array, int size, ref double actualFrom, ref double actualTo,
                          ref int actualRes);

        int GetSingleBeamSize();

        int GetSpectrum(double[] array, int size, ref double actualFrom, ref double actualTo, ref int actualRes);
        int SetClean(double[] array, int size, double from, double to, int res);

        int GetSpectrumSize();
        int GetStatus(ref int nEnergyStatus, ref float fBatteryStatus, ref float fSourceCurrentStatus,
                      ref float fSourceVoltageStatus, ref int nLaserStatus, ref float fDetectorStatus);
        int GetVersion(ref int fwRev, ref int dllRev, ref int spareRev);
        int GetVersionEx(MLVersion vInfo);
        int Init();
        int InitializationCheck();
        int SetComputeParams(PHASEPOINTS ppoints, PHASETYPE ptype, APODTYPE papod, APODTYPE iapod,
                                               ZFFTYPE zff, OFFSETCORRECTTYPE offset);
        int StartSingleBeam(int numScans, double from, double to, int res, bool bAutoSetBkg,
                            bool bAutoSetClean);
        int GetRatioSpectrum(double[] bkgarray, double[] smparray, double[] outarray, int size,
                             DATAYTYPE ytype);

        int SetUnknown(double[] array, int size, double from, double to, int res, DATAXTYPE xtype,
                       DATAYTYPE ytype, bool bIsATR);
        int KillCollection();
        int SoftReset();
        int GetStatusEx(ref MLDiag dStatus);
        int SetLaserWN(double newLaser);
        int SetPathlength(double newPathlength, MLVersion _vInfo);
        int StartCoaddedIGram(int numScans, int nRes, int nPhasePts);
        bool CheckTumbIIR();
    }

}
