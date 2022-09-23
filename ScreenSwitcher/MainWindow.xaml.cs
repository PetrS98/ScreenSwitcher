using ScreenSwitcher.Classes;
using ScreenSwitcher.JDOs;
using ScreenSwitcher.UDTs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ENCRYPTION_KEY = "eoW890%@";
        private const string SETTING_FILE_PATH = "settings.json";

        private SettingsJDO Settings = new();
        
        private JsonHandler JsonHandler = new();
        private WindowSwitchHandler WindowSwitchHandler;

        List<DataGridApplicationUDT> DataGridApplicationItems;

        public MainWindow()
        {
            InitializeComponent();
            SetControlsState(true);

            object? data = JsonHandler.ReadJSONData(SETTING_FILE_PATH, ENCRYPTION_KEY);
            if (data != null) Settings = (SettingsJDO)data;

            WindowSwitchHandler = new WindowSwitchHandler(Settings);

            DataGridApplicationItems = new List<DataGridApplicationUDT>();
            dgApplication.ItemsSource = DataGridApplicationItems;

            MoveSettingsToControls(Settings);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JsonHandler.WriteJSONData(SETTING_FILE_PATH, ENCRYPTION_KEY, Settings);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(tbAppName.Text)) return;

            //lvApps.Items.Add(tbAppName.Text);
            //tbAppName.Clear();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //if(string.IsNullOrWhiteSpace((string)lvApps.SelectedItem)) return;

            //lvApps.Items.Remove(lvApps.SelectedItem);
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

        private void btnManualSwitch_Click(object sender, RoutedEventArgs e)
        {
            WindowSwitchHandler.SwitchScreen();
        }

        private void dgApplication_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            MoveControlsToSettings();
        }

        private void dgApplication_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) MoveControlsToSettings();
        }

        private void MoveControlsToSettings()
        {
            Settings.MonitorsOrder[0] = (short)monitor1.Value;
            Settings.MonitorsOrder[1] = (short)monitor2.Value;

            Settings.MonitorResolutionIndex = cbResolution.SelectedIndex;
            Settings.MonitorResolution = (string)cbResolution.SelectionBoxItem;

            Settings.ChangeInterval = tbInterval.Value;

            Settings.ApplicationInformation = DataGridApplicationItems;
        }

        private void MoveSettingsToControls(SettingsJDO settings)
        {
            monitor1.Value = settings.MonitorsOrder[0];
            monitor2.Value = settings.MonitorsOrder[1];

            cbResolution.SelectedIndex = settings.MonitorResolutionIndex;

            tbInterval.Value = Settings.ChangeInterval;

            foreach (var Information in settings.ApplicationInformation)
            {
                DataGridApplicationItems.Add(Information);
            }

            dgApplication.Items.Refresh();

        }

        private void SetControlsState(bool Enable)
        {
            monitor1.IsEnabled = Enable;
            monitor2.IsEnabled = Enable;

            cbResolution.IsEnabled = Enable;

            tbInterval.IsEnabled = Enable;

            btnApply.IsEnabled = Enable;
            btnStart.IsEnabled = Enable;
            btnStop.IsEnabled = !Enable;

            btnManualSwitch.IsEnabled = Enable;

            dgApplication.IsEnabled = Enable;
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }
    }
}
