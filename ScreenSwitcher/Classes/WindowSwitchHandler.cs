using ScreenSwitcher.JDOs;
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
            SwitchScreenTimer.Elapsed += SwitchScreen;
        }

        private void SwitchScreen(object? sender, ElapsedEventArgs e)
        {
            if (_settings.ApplicationName.Count == 0 || _settings is null) return;

            Debug.WriteLine("Monitor 1: " + AppIndex);
            _appWindowHandler.SetWindowPosition(1, _settings.ApplicationName[AppIndex], _settings.MonitorResolution, _settings.MonitorsOrder, _settings.WindowOffset);

            AppIndex = (AppIndex + 1) % _settings.ApplicationName.Count;
            //Thread.Sleep(2000);

            Debug.WriteLine("Monitor 2: " + AppIndex);
            _appWindowHandler.SetWindowPosition(2, _settings.ApplicationName[AppIndex], _settings.MonitorResolution, _settings.MonitorsOrder, _settings.WindowOffset);
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
    }
}
