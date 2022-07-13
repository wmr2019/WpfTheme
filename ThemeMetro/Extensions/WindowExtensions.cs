/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Extensions
*   文件名称 ：WindowExtensions.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 4:46:11 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修  改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ThemeMetro.Controls
{
    public static class WindowExtensions
    {
        #region Window Flashing API Stuff

        private const UInt32 FLASHW_STOP = 0; //Stop flashing. The system restores the window to its original state.
        private const UInt32 FLASHW_CAPTION = 1; //Flash the window caption.
        private const UInt32 FLASHW_TRAY = 2; //Flash the taskbar button.
        private const UInt32 FLASHW_ALL = 3; //Flash both the window caption and taskbar button.
        private const UInt32 FLASHW_TIMER = 4; //Flash continuously, until the FLASHW_STOP flag is set.
        private const UInt32 FLASHW_TIMERNOFG = 12; //Flash continuously until the window comes to the foreground.

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public UInt32 cbSize; //The size of the structure in bytes.
            public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.
            public UInt32 dwFlags; //The Flash Status.
            public UInt32 uCount; // number of times to flash the window
            public UInt32 dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        #endregion

        public static void FlashWindow(this Window win, UInt32 count = UInt32.MaxValue, UInt32 interval = 500)
        {
            try
            {
                //Don't flash if the window is active
                if (win.IsActive) return;

                WindowInteropHelper h = new WindowInteropHelper(win);
                FLASHWINFO info = new FLASHWINFO
                {
                    hwnd = h.Handle,
                    dwFlags = FLASHW_TRAY | FLASHW_TIMER,
                    uCount = count,
                    dwTimeout = interval
                };

                info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
                FlashWindowEx(ref info);
            }
            catch { }
        }

        public static void StopFlashingWindow(this Window win)
        {
            try
            {
                WindowInteropHelper h = new WindowInteropHelper(win);
                FLASHWINFO info = new FLASHWINFO();
                info.hwnd = h.Handle;
                info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
                info.dwFlags = FLASHW_STOP;
                info.uCount = UInt32.MaxValue;
                info.dwTimeout = 0;

                FlashWindowEx(ref info);
            }
            catch { }
        }
    }
}
