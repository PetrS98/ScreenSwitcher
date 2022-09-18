using ScreenSwitcher.Classes;
using ScreenSwitcher.JDOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ENCRYPTION_KEY = "eoW890%@";
        private const string SETTING_FILE_PATH = "settings.json";

        private SettingsJDO Settings = new SettingsJDO();

        private JsonHandler JsonHandler = new JsonHandler();
        private WindowSwitchHandler WindowSwitchHandler;

        private int AppIndex = 0;
        AppWindowHandler ggg = new AppWindowHandler();

        public MainWindow()
        {
            InitializeComponent();
            SetControlsState(true);

            object? data = JsonHandler.ReadJSONData(SETTING_FILE_PATH, ENCRYPTION_KEY);
            if (data != null) Settings = (SettingsJDO)data;

            MoveSettingsToControls(Settings);

            WindowSwitchHandler = new WindowSwitchHandler(Settings);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JsonHandler.WriteJSONData(SETTING_FILE_PATH, ENCRYPTION_KEY, Settings);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbAppName.Text)) return;

            lvApps.Items.Add(tbAppName.Text);
            tbAppName.Clear();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace((string)lvApps.SelectedItem)) return;

            lvApps.Items.Remove(lvApps.SelectedItem);
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            MoveControlsToSettings();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            WindowSwitchHandler.StartSwitching();
            SetControlsState(false);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            WindowSwitchHandler.StopSwitching();
            SetControlsState(true);
        }

        private void MoveSettingsToControls(SettingsJDO settings)
        {
            monitor1.Value = settings.MonitorsOrder[0];
            monitor2.Value = settings.MonitorsOrder[1];

            tbOffsetTop.Value = settings.WindowOffset[0];
            tbOffsetLeft.Value = settings.WindowOffset[1];
            tbOffsetBottom.Value = settings.WindowOffset[2];
            tbOffsetRight.Value = settings.WindowOffset[3];

            cbResolution.SelectedIndex = settings.MonitorResolutionIndex;

            tbInterval.Value = Settings.ChangeInterval;

            lvApps.Items.Clear();

            foreach (string app in settings.ApplicationName)
            {
                lvApps.Items.Add(app);
            }
        }

        private void MoveControlsToSettings()
        {
            Settings.MonitorsOrder[0] = (short)monitor1.Value;
            Settings.MonitorsOrder[1] = (short)monitor2.Value;

            Settings.WindowOffset[0] = (short)tbOffsetTop.Value;
            Settings.WindowOffset[1] = (short)tbOffsetLeft.Value;
            Settings.WindowOffset[2] = (short)tbOffsetBottom.Value;
            Settings.WindowOffset[3] = (short)tbOffsetRight.Value;

            Settings.MonitorResolutionIndex = cbResolution.SelectedIndex;
            Settings.MonitorResolution = (string)cbResolution.SelectionBoxItem;

            Settings.ChangeInterval = tbInterval.Value;

            Settings.ApplicationName.Clear();

            foreach (string app in lvApps.Items)
            {
                Settings.ApplicationName.Add(app);
            }
        }

        private void SetControlsState(bool Enable)
        {
            monitor1.IsEnabled = Enable;
            monitor2.IsEnabled = Enable;

            tbOffsetTop.IsEnabled = Enable;
            tbOffsetLeft.IsEnabled = Enable;
            tbOffsetBottom.IsEnabled = Enable;
            tbOffsetRight.IsEnabled = Enable;

            cbResolution.IsEnabled = Enable;

            tbInterval.IsEnabled = Enable;

            btnApply.IsEnabled = Enable;
            btnStart.IsEnabled = Enable;
            btnStop.IsEnabled = !Enable;

            tbAppName.IsEnabled = Enable;
            btnAdd.IsEnabled = Enable;
            btnRemove.IsEnabled = Enable;
        }

        private void btntest_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.ApplicationName.Count == 0 || Settings is null) return;
            Debug.WriteLine("Monitor 1: " + AppIndex);
            ggg.SetWindowPosition(1, Settings.ApplicationName[AppIndex], Settings.MonitorResolution, Settings.MonitorsOrder, Settings.WindowOffset);
            AppIndex = (AppIndex + 1) % Settings.ApplicationName.Count;
            //Thread.Sleep(500);

            Debug.WriteLine("Monitor 2: " + AppIndex);
            ggg.SetWindowPosition(2, Settings.ApplicationName[AppIndex], Settings.MonitorResolution, Settings.MonitorsOrder, Settings.WindowOffset);
        }
    }
}
