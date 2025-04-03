using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using static spinjitzuu3.NativeImport;


namespace spinjitzuu3
{
    class Input
    {
        //---------- Consts ----------

        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const UInt32 WM_KEYDOWN = 0x0100;
        const int VK_1 = 0x31;
        const int VK_D = 0x44;


        //---------- Flags ----------

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

        public void Click1()
        {
            keybd_event(VK_D, 0, 0, 0);
        }


        //---------- Keyboard and Mouse ----------

        /// <summary>
        /// Use Keyboard's Key
        /// </summary>
        public int keyPress()
        {
            keybd_event((byte)0x31, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
            return 0;
        }

        /// <summary>
        /// Use Mouse's Left Button
        /// </summary>
        public void leftClick()
        {
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            Thread.Sleep(2);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }

        /// <summary>
        /// Use Mouse's Right Button
        /// </summary>
        public void rightClick()
        {
            mouse_event((int)(MouseEventFlags.RIGHTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.RIGHTUP), 0, 0, 0, 0);
        }

        /// <summary>
        /// Use Mouse's Middle Button
        /// </summary>
        public void middleClick()
        {
            mouse_event((int)(MouseEventFlags.MIDDLEDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.MIDDLEUP), 0, 0, 0, 0);
        }

        public void SetPosition(int a, int b)
        {
            SetCursorPos(a, b);
        }
    }
}
