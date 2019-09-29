/*  
    *  AUTHOR: Ravenmyst
    *  DATE: 9/21/2019
    *  LICENSE: MIT
*/

using NWNLogRotator.Classes;
using NWNLogRotator.Components;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NWNLogRotator
{
    public partial class MainWindow : Window
    {
        FileHandler instance = new FileHandler();
        Settings _settings;
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();

            SetupApplication();
        }

        private void SetupApplication()
        {
            IterateNWN_Watcher(false);
            LoadSettings_Handler();
            LoadTray_Handler();
        }

        private Settings Settings_Get()
        {
            _settings = instance.InitSettingsIni();
            return _settings;
        }

        private async void IterateNWN_Watcher(bool PreviousStatus)
        {
            var Status = NWNProcessStatus_Get();

            if (Status == true)
            {
                NWNStatusTextBlock.Text = "nwmain is active!";
                NWNStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                await Task.Delay(10000);
                IterateNWN_Watcher(true);
            }
            else
            {
                if (PreviousStatus == true)
                {
                    _settings = CurrentSettings_Get();
                    if (NWNLog_Save(_settings) == true)
                        UpdateResultsPane(1);
                }
                NWNStatusTextBlock.Text = "nwmain not found!";
                NWNStatusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                await Task.Delay(10000);
                IterateNWN_Watcher(false);
            }
        }

        private bool NWNProcessStatus_Get()
        {
            Process[] processlist = Process.GetProcesses();

            foreach (Process theProcess in processlist)
            {
                if (theProcess.ProcessName.IndexOf("nwmain") != -1)
                {
                    return true;
                }
            }
            return false;
        }

        private Settings CurrentSettings_Get()
        {
            string OutputDirectory = OutputDirectoryTextBox.Text;
            string PathToLog = PathToLogTextBox.Text;
            int MinimumRowsToInteger = int.Parse(MinimumRowsCountSlider.Value.ToString());
            string ServerName = "";
            if (ServerNameCheckBox.IsChecked == true && ServerNameTextBox.Text != "")
            {
                ServerName = ServerNameTextBox.Text;
            }
            string ServerNameColor = "";
            if (ServerNameCheckBox.IsChecked == true && ServerNameTextBox.Text != "")
            {
                ServerNameColor = ServerNameColorTextBox.Text;
            }
            bool EventText = EventTextCheckBox.IsChecked.GetValueOrDefault();
            bool CombatText = CombatTextCheckBox.IsChecked.GetValueOrDefault();
            string UseTheme = _settings.UseTheme;
            bool Silent = SilentCheckBox.IsChecked.GetValueOrDefault(); ;
            bool Tray = TrayCheckBox.IsChecked.GetValueOrDefault();

            _settings = new Settings(OutputDirectory,
                                              PathToLog,
                                              MinimumRowsToInteger,
                                              ServerName,
                                              ServerNameColor,
                                              EventText,
                                              CombatText,
                                              UseTheme,
                                              Silent,
                                              Tray
                                            );

            return _settings;
        }

        private void SaveToggle_Event()
        {
            if (StatusBarProgressBar.Visibility == Visibility.Collapsed)
            {
                StatusBarProgressBar.Visibility = Visibility.Visible;
                UpdateResultsPane(4);
            }
            else
            {
                StatusBarProgressBar.Visibility = Visibility.Collapsed;
                UpdateResultsPane(-1);
            }
        }

        private void Tray_Set(bool doMinimize)
        {
            if (_settings.Tray == true)
            {
                if (doMinimize == true)
                {
                    WindowState = WindowState.Minimized;
                    this.Hide();
                }
            }
            else
            {
                ni.Visible = false;
            }
        }

        public async void UpdateResultsPane(int result)
        {
            switch (result)
            {
                case -1:
                    EventStatusTextBlock.Text = "";
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    break;
                case 1:
                    EventStatusTextBlock.Text = "Log Saved Successfully!";
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    await Task.Delay(2000);
                    EventStatusTextBlock.Text = "";
                    break;
                case 2:
                    EventStatusTextBlock.Text = "Settings Saved Successfully!";
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    await Task.Delay(1000);
                    EventStatusTextBlock.Text = "";
                    break;
                case 3:
                    EventStatusTextBlock.Text = "Settings Loaded Successfully!";
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    await Task.Delay(2000);
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    string version = fvi.FileVersion;
                    version = "v" + version.Substring(2, version.Length - 2);
                    EventStatusTextBlock.Text = "Version " + version;
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    await Task.Delay(1000);
                    EventStatusTextBlock.Text = "";
                    break;
                case 4:
                    EventStatusTextBlock.Text = "Loading..";
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    break;
            }
        }

        private void LoadSettings_Handler()
        {
            Settings_Get();

            OutputDirectoryTextBox.Text = _settings.OutputDirectory;
            PathToLogTextBox.Text = _settings.PathToLog;
            MinimumRowsCountSlider.Value = _settings.MinimumRowsCount;
            if (_settings.ServerName != "")
            {
                ServerNameCheckBox.IsChecked = true;
                ServerNameTextBox.Text = _settings.ServerName;
            }
            if (_settings.ServerNameColor != "")
            {
                ServerNameColorCheckBox.IsChecked = true;
                ServerNameColorTextBox.Text = _settings.ServerNameColor;
            }
            EventTextCheckBox.IsChecked = _settings.EventText;
            CombatTextCheckBox.IsChecked = _settings.CombatText;
            SilentCheckBox.IsChecked = _settings.Silent;
            TrayCheckBox.IsChecked = _settings.Tray;

            if (_settings.UseTheme == "light")
            {
                ActivateLightTheme();
            }
            else if (_settings.UseTheme == "dark")
            {
                ActivateDarkTheme();
            }

            UpdateResultsPane(3);
        }

        private void ActivateDarkTheme()
        {
            // purple => black => purple 
            LinearGradientBrush myBrush = new LinearGradientBrush();
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
            Grid.Background = myBrush;

            RunOnceButton.Background = Brushes.Black;
            RunOnceButton.Foreground = new SolidColorBrush(Colors.White);
            TrayCheckBox.Foreground = new SolidColorBrush(Colors.White);
            SilentCheckBox.Foreground = new SolidColorBrush(Colors.White);
            SaveSettingsButton.Background = Brushes.Black;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.White);
            OutputDirectoryTextBox.Background = Brushes.Black;
            PathToLogTextBox.Background = Brushes.Black;
            ServerNameTextBox.Background = Brushes.Black;
            ServerNameColorTextBox.Background = Brushes.Black;
            OutputDirectoryTextBox.Foreground = new SolidColorBrush(Colors.White);
            PathToLogTextBox.Foreground = new SolidColorBrush(Colors.White);
            SettingsTextBlock.Foreground = new SolidColorBrush(Colors.White);
            OutputDirectoryLabel.Foreground = new SolidColorBrush(Colors.White);
            PathToLogLabel.Foreground = new SolidColorBrush(Colors.White);
            ServerNameTextBox.Foreground = new SolidColorBrush(Colors.White);
            ServerNameColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            FlagGroupBox.Foreground = new SolidColorBrush(Colors.White);
            ServerNameCheckBox.Foreground = new SolidColorBrush(Colors.White);
            ServerNameColorCheckBox.Foreground = new SolidColorBrush(Colors.White);
            EventTextCheckBox.Foreground = new SolidColorBrush(Colors.White);
            CombatTextCheckBox.Foreground = new SolidColorBrush(Colors.White);
            MinimumRowsLabel.Foreground = new SolidColorBrush(Colors.White);
            MinimumRowsCountTextBlock.Foreground = new SolidColorBrush(Colors.White);

            _settings.UseTheme = "dark";
        }

        private void ActivateLightTheme()
        {
            Grid.Background = Brushes.White;

            RunOnceButton.Background = Brushes.White;
            RunOnceButton.Foreground = new SolidColorBrush(Colors.Black);
            TrayCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            SilentCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            SaveSettingsButton.Background = Brushes.White;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.Black);
            OutputDirectoryTextBox.Background = Brushes.White;
            PathToLogTextBox.Background = Brushes.White;
            ServerNameTextBox.Background = Brushes.White;
            ServerNameColorTextBox.Background = Brushes.White;
            OutputDirectoryTextBox.Foreground = new SolidColorBrush(Colors.Black);
            PathToLogTextBox.Foreground = new SolidColorBrush(Colors.Black);
            SettingsTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            OutputDirectoryLabel.Foreground = new SolidColorBrush(Colors.Black);
            PathToLogLabel.Foreground = new SolidColorBrush(Colors.Black);
            ServerNameTextBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerNameColorTextBox.Foreground = new SolidColorBrush(Colors.Black);
            FlagGroupBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerNameCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerNameColorCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            EventTextCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            CombatTextCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            MinimumRowsLabel.Foreground = new SolidColorBrush(Colors.Black);
            MinimumRowsCountTextBlock.Foreground = new SolidColorBrush(Colors.Black);

            _settings.UseTheme = "light";
        }

        private bool NWNLog_Save(Settings _settings)
        {
            FileHandler instance = new FileHandler();
            string _filepathandname = instance.ReadNWNLogAndInvokeParser(_settings);

            if (_filepathandname != "")
            {
                if(_settings.Silent == false)
                {
                    MessageBoxResult _messageBoxResult = MessageBox.Show("The log file has been generated successfully. Would you like to open the log file now?",
                            "Success!",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                    if (_messageBoxResult == MessageBoxResult.Yes)
                        System.Diagnostics.Process.Start(_filepathandname);

                    return true;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private void LoadTray_Handler()
        {
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };

            Tray_Set(true);
        }

        private void InvertColorScheme(object sender, MouseButtonEventArgs e)
        {
            if (_settings.UseTheme == "light")
            {
                ActivateDarkTheme();
            }
            else
            {
                ActivateLightTheme();
            }
        }

        private void SettingsTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
        }

        private void SettingsTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void MinimumRowsCountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MinimumRowsCountTextBlock != null)
                MinimumRowsCountTextBlock.Text = e.NewValue.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();

            FileHandler instance = new FileHandler();
            instance.SaveSettingsIni(_settings);

            UpdateResultsPane(2);
        }

        private void ServerNameCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ServerNameTextBox.Visibility = Visibility.Visible;
        }

        private void ServerNameCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ServerNameTextBox.Text = "";
            ServerNameTextBox.Visibility = Visibility.Collapsed;
        }

        private void ServerNameColorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ServerNameColorTextBox.Visibility = Visibility.Visible;
        }

        private void ServerNameColorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ServerNameColorTextBox.Text = "";
            ServerNameColorTextBox.Visibility = Visibility.Collapsed;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();
            bool SavedLogResult = false;
            SaveToggle_Event();

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            await Task.Run(() =>
            {
                SavedLogResult = NWNLog_Save(_settings);
            });
       
            SaveToggle_Event();
            SavedResult_Callback(SavedLogResult);
        }

        private void SavedResult_Callback(bool result)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            if (result == true)
                UpdateResultsPane(1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _settings.Tray = _settings.Tray ? false : true;
            Tray_Set(false);
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
                ni.Visible = true;
            }
                
            if (WindowState == System.Windows.WindowState.Normal)
            {
                ni.Visible = false;
            }
                

            base.OnStateChanged(e);
        }
        private void WindowClosed_Event(object sender, CancelEventArgs e)
        {
            ni.Visible = false;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = _settings.OutputDirectory;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    OutputDirectoryTextBox.Text = dialog.SelectedPath;
            }
        }

        private void Image_MouseDown2(object sender, MouseButtonEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.InitialDirectory = _settings.PathToLog;
            fileDialog.Filter = "txt file (*.txt)|*.txt";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    PathToLogTextBox.Text = file;
                    PathToLogTextBox.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    break;
                default:
                    break;
            }
        }
    }
}
