using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace spinjitzuu3
{
    class Input
    {
        const UInt32 WM_KEYDOWN = 0x0100;
        const int VK_1 = 0x31;
        const int VK_D = 0x44;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);



        public void SendD()
        {
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("League of Legends (TM) Client"); //search for process notepad
            if (p.Length > 0) //check if window was found
            {
                SetForegroundWindow(p[0].MainWindowHandle); //bring notepad to foreground
            }

            //SendKeys.SendWait(0x31); //send key "a" to notepad
        }


        const int VK_RIGHT = 0x27;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        //const int VK_D = 0x44;

        //[DllImport("user32.dll")]
        //public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        public void Click1()
        {
            keybd_event(VK_D, 0, 0, 0);
        }

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }
        public void pressQSS()
        {
            Process[] processes = Process.GetProcessesByName("League of Legends (TM) Client");

            foreach (Process proc in processes)
                PostMessage(proc.MainWindowHandle, WM_KEYDOWN, VK_D, 0);
        }

        public int keyPress()
        {
            //Press the key
            keybd_event((byte)0x31, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
            return 0;
        }

        public void leftClick()
        {

            //Cursor.Position = p;
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            Thread.Sleep(2);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }

        public void rightClick()
        {

            //Cursor.Position = p;
            mouse_event((int)(MouseEventFlags.RIGHTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.RIGHTUP), 0, 0, 0, 0);
        }

        public void middleClick()
        {
            //Cursor.Position = p;
            mouse_event((int)(MouseEventFlags.MIDDLEDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.MIDDLEUP), 0, 0, 0, 0);
        }

        public void SetPosition(int a, int b)
        {
            SetCursorPos(a, b);
        }
    }
}
