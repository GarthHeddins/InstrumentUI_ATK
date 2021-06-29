using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.Workflow;

namespace InstrumentUI_ATK
{
    /// <summary>
    /// Launches the Instrument UI Application
    /// </summary>
    static class Program
    {
        static Mutex mutex = new Mutex();        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]     
        static void Main()
        {
                        
            try
            {
                if (!IsProcessRunning())
                {
                    System.Diagnostics.Trace.WriteLine("Start App - Main()..");
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    
                    Application.Run(new Main());

                    System.Diagnostics.Trace.WriteLine("Stop App - Main()..");
                    
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    mutex.ReleaseMutex();
                    Application.Exit();                    
                }
            }
            catch (Exception ex)
            {
                Helper.DisplayError("The following Error occurred during start up: " + ex.Message.ToString());
                Helper.LogError("Program.Main", "", ex, false);
            }
        }

        static bool IsProcessRunning()
        {
            bool retvalue = false;
            IntPtr hwnd = new IntPtr();
            try
            {
                try
                {                    
                    hwnd = NativeMethod.FindWindow(null, "Quality Trait Analysis");
                    if ((int)hwnd == 0) // FindWindow() did not find the app running....although there seems to be an issue w/ this working 100%.
                    {
                        if ((int)hwnd==0)
                            return retvalue;
                    }
                }
                    catch(AbandonedMutexException ex)
                {
                    Console.WriteLine("Exception on return from WaitOne." +
                        "\r\n\tMessage: {0}", ex.Message);
                }
                DialogResult response;
                    response = MessageBox.Show(ResourceHelper.InstanceRunningMessage.ToString(), "Verify Closing Process", 
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);   
                if (response == DialogResult.Yes)
                    {
                        hwnd = NativeMethod.FindWindow(null, "Quality Trait Analysis");                        
                        bool result = NativeMethod.PostMessage(
                            hwnd,
                            NativeMethod.WM_CLOSE,
                            IntPtr.Zero,
                            IntPtr.Zero);

                        if (result)
                        {
                            return retvalue;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        // send Win32 message to bring the currently running instance to the front
                        NativeMethod.PostMessage(
                            (IntPtr)NativeMethod.HWND_BROADCAST,
                            NativeMethod.WM_MAKE_ACTIVE,
                            IntPtr.Zero,
                            IntPtr.Zero);
                        return true;
                    }
                }
            catch (Exception ex)
            {
                Helper.DisplayError("The following Error occurred during start up: " + ex.Message.ToString());
                Helper.LogError("Program.Main.IsProcessrunning", "", ex, false);
            }
            return true;
        }

        /// <summary>
        /// It handles any unhandled exception in the application
        /// </summary>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
                Helper.LogError("Program.CurrentDomain_UnhandledException", string.Empty, e.ExceptionObject as Exception, true);
            else
                Helper.LogError("Program.CurrentDomain_UnhandledException", "e.ExceptionObject=" + e.ExceptionObject.ToString(), "An Unhandled exception occurred.", "");
        }
    }
}