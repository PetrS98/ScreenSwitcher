using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace ScreenSwitcher.Classes
{
    public class AppWindowHandler
    {
        public event EventHandler<Exception>? ErrorOcurred;

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public extern static bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public bool SetWindowState_Async(string WindowName, WindowState State)
        {
            try
            {
                IntPtr hWnd = _GetProcessHandler(WindowName);
                if (!hWnd.Equals(IntPtr.Zero))
                {
                    ShowWindowAsync(hWnd, (int)State);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                ErrorOcurred?.Invoke(this, ex);
                return false;
            }
        }
        public bool SetWindowState(string WindowName, WindowState State)
        {
            try
            {
                IntPtr hWnd = _GetProcessHandler(WindowName);
                if (!hWnd.Equals(IntPtr.Zero))
                {
                    ShowWindow(hWnd, (int)State);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorOcurred?.Invoke(this, ex);
                return false;
            }
        }

        public bool SetWindowPosition(short ToMonitor, string WindowName, string Resolution, short[] MonitorsOrder, short[] AppOffset)
        {
            int _XPos = 0, _YPos = 0, _Width, _Height;

            string[] _ResolutionStr = Resolution.ToLower().Split('x');
            int[] _ResolutionInt = new int[2];

            if (int.TryParse(_ResolutionStr[0], out _ResolutionInt[0]) == false) return false;
            if (int.TryParse(_ResolutionStr[1], out _ResolutionInt[1]) == false) return false;

            if (ToMonitor == 1)
            {
                _XPos = 0 + AppOffset[1];
                _YPos = 0 + AppOffset[0];
            }
            else if (ToMonitor == 2)
            {
                if (MonitorsOrder[0] == 1) _XPos = _ResolutionInt[0] + AppOffset[1];
                else _XPos = -(_ResolutionInt[0] + AppOffset[1]);
            }

            //_Width = _ResolutionInt[0] + Math.Abs(AppOffset[1]) + Math.Abs(AppOffset[3]);
            //_Height = _ResolutionInt[1] + Math.Abs(AppOffset[0]) + Math.Abs(AppOffset[2]);

            _Width = _ResolutionInt[0] + (-AppOffset[1]) + AppOffset[3];
            _Height = _ResolutionInt[1] + (-AppOffset[0]) + AppOffset[2];

            WINDOWPLACEMENT WindowPlacement = new WINDOWPLACEMENT();
            _GetWindowPlacement(WindowName, ref WindowPlacement);

            if (WindowPlacement.showCmd == 2) SetWindowState(WindowName, WindowState.SHOWNORMAL);

            bool status =_MoveWindow(WindowName, _XPos, _YPos, _Width, _Height, true);

            //SetWindowState(WindowName, WindowState.SHOWMAXIMIZED);

            return status;
        }

        private bool _MoveWindow(string WindowName, int XPos, int YPos, int Width, int Height, bool Repaint)
        {
            try
            {
                IntPtr hWnd = _GetProcessHandler(WindowName);
                if (!hWnd.Equals(IntPtr.Zero))
                {
                    //MoveWindow(hWnd, XPos, YPos, Width, Height, Repaint);
                    SetWindowPos(hWnd, (IntPtr)(-1), XPos, YPos, Width, Height, 0x4000);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorOcurred?.Invoke(this, ex);
                return false;
            }
        }

        private bool _GetWindowPlacement(string WindowName, ref WINDOWPLACEMENT WindowPlacement)
        {
            try
            {
                IntPtr hWnd = _GetProcessHandler(WindowName);
                if (!hWnd.Equals(IntPtr.Zero))
                {
                    GetWindowPlacement(hWnd, ref WindowPlacement);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorOcurred?.Invoke(this, ex);
                return false;
            }
        }

        private IntPtr _GetProcessHandler(string ProccesName)
        {
            Process[] ProcessByName = Process.GetProcessesByName(ProccesName);
            if (ProcessByName == null) return IntPtr.Zero;

            IntPtr hWnd = IntPtr.Zero;

            foreach (Process proc in ProcessByName)
            {
                hWnd = proc.MainWindowHandle;
            }

            return hWnd;
        }
    }
}
