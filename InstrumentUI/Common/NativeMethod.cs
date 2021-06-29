using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace InstrumentUI_ATK.Common
{
    /// <summary>
    /// Wrapper of some Win32 items
    /// </summary>
    class NativeMethod
    {
        public const int HWND_BROADCAST = 0xffff;
        public static int WM_CLOSE = 0x10; 

        public static readonly int WM_MAKE_ACTIVE = RegisterWindowMessage("WM_MAKE_ACTIVE");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

[DllImport("user32.dll")]

public static extern IntPtr FindWindow(String sClassName, String sAppName);
//Example:

//IntPtr hwnd=FindWindow(null,"Form1");
//"Form1" is the Text property of the window.


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner 
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
            );

        //Add the printer connection for specified pName.        
        [DllImport("winspool.drv")]        
        public static extern bool AddPrinterConnection(string pName);        
        
        //Set the added printer as default printer.        
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]        
        public static extern bool SetDefaultPrinter(string Name);
    }
}
