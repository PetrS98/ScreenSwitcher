using ScreenSwitcher.JDOs;
using ScreenSwitcher.UDTs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ScreenSwitcher.Classes
{
    public class WindowSwitchHandler
    {
        SettingsJDO _settings;
        AppWindowHandler _appWindowHandler;

        private System.Timers.Timer SwitchScreenTimer = new System.Timers.Timer();

        private int AppIndex = 0;

        public WindowSwitchHandler(SettingsJDO settings)
        {
            _settings = settings;

            _appWindowHandler = new AppWindowHandler();

            SwitchScreenTimer.Interval = _settings.ChangeInterval;
            SwitchScreenTimer.Elapsed += _SwitchScreen;
        }

        private void _SwitchScreen(object? sender, ElapsedEventArgs e)
        {
            SwitchScreen();
        }

        public void StartSwitching()
        {
            SwitchScreenTimer.Interval = _settings.ChangeInterval;
            SwitchScreenTimer.Start();
        }

        public void StopSwitching()
        {
            SwitchScreenTimer.Stop();
        }

        public void SwitchScreen()
        {
            if (_settings.ApplicationInformation.Count < 2 || _settings is null) return;

            List<DataGridApplicationUDT> EnabledApps = new();

            foreach (DataGridApplicationUDT App in _settings.ApplicationInformation)
            {
                if (App.Used) EnabledApps.Add(App);
            }

            short[] WindowOffsets = new short[4]; 

            WindowOffsets[0] = (short)EnabledApps[AppIndex].OffsetTop!;
            WindowOffsets[1] = (short)EnabledApps[AppIndex].OffsetLeft!;
            WindowOffsets[2] = (short)EnabledApps[AppIndex].OffsetBot!;
            WindowOffsets[3] = (short)EnabledApps[AppIndex].OffsetRight!;

            _appWindowHandler.SetWindowPosition(1, EnabledApps[AppIndex].Name!, _settings.MonitorResolution, _settings.MonitorsOrder, WindowOffsets);

            AppIndex = (AppIndex + 1) % EnabledApps.Count;

            WindowOffsets[0] = (short)EnabledApps[AppIndex].OffsetTop!;
            WindowOffsets[1] = (short)EnabledApps[AppIndex].OffsetLeft!;
            WindowOffsets[2] = (short)EnabledApps[AppIndex].OffsetBot!;
            WindowOffsets[3] = (short)EnabledApps[AppIndex].OffsetRight!;

            _appWindowHandler.SetWindowPosition(2, EnabledApps[AppIndex].Name!, _settings.MonitorResolution, _settings.MonitorsOrder, WindowOffsets);
        }
    }
}
