/*  
    *  Author:   © xNetrunner
    *  Project:  NWNLogRotator
    *  GitHub:   https://github.com/xNetrunner/NWNLogRotator
    *  Date:     3/3/2021
    *  License:  MIT
    *  Purpose:  This program is designed used alongside the Neverwinter Nights game, either Enhanced Edition, or 1.69.
    *  This program does not come with a warranty. Any support may be found on the GitHub page.
*/

using NWNLogRotator.Classes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace NWNLogRotator
{
    public partial class MainWindow : Window
    {
        FileHandler FileHandlerInstance = new FileHandler();
        Settings _settings;
        static System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        Notification notification = new Notification();
        private int ClientLauncherState = 0;
        private string ProcessName = "nwmain";

        public MainWindow()
        {
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Length > 1)
            {
                MessageBox.Show("There is already an instance of NWNLogRotator running!");
                ExitEvent();
                Process.GetCurrentProcess().Kill();
            }
            InitializeComponent();
            SetupApplication();
        }

        /*
         * Setters and Getters
        */
        private Settings Settings_Get()
        {
            _settings = FileHandlerInstance.InitSettingsIni();
            return _settings;
        }

        private Settings CurrentSettings_Get()
        {
            string OutputDirectory = OutputDirectoryTextBox.Text;
            string PathToLog = PathToLogTextBox.Text;
            int MinimumRowsToInteger = int.Parse(MinimumRowsCountSlider.Value.ToString());
            string ServerName = _settings.ServerName;
            string ServerNameColor = _settings.ServerNameColor;
            bool EventText = EventTextCheckBox.IsChecked.GetValueOrDefault();
            bool CombatText = CombatTextCheckBox.IsChecked.GetValueOrDefault();
            string UseTheme = _settings.UseTheme;
            bool Silent = SilentCheckBox.IsChecked.GetValueOrDefault();
            bool Tray = TrayCheckBox.IsChecked.GetValueOrDefault();
            bool SaveBackup = SaveBackupCheckBox.IsChecked.GetValueOrDefault();
            bool SaveBackupOnly = SaveBackupOnlyCheckBox.IsChecked.GetValueOrDefault();
            bool SaveOnLaunch = SaveOnLaunchCheckBox.IsChecked.GetValueOrDefault();
            bool Notifications = NotificationsCheckBox.IsChecked.GetValueOrDefault();
            string OOCColor = _settings.OOCColor;
            string FilterLines = "";
            if (FilterLinesCheckBox.IsChecked == true && FilterLinesTextBox.Text != "")
            {
                FilterLines = FilterLinesTextBox.Text;
            }
            string PathToClient = _settings.PathToClient;
            bool RunClientOnLaunch = _settings.RunClientOnLaunch;
            bool CloseOnLogGenerated = _settings.CloseOnLogGenerated;
            string ServerAddress = _settings.ServerAddress;
            string ServerPassword = _settings.ServerPassword;
            bool DM = _settings.DM;
            bool ServerMode = ServerModeRadioButton.IsChecked.GetValueOrDefault();
            string BackgroundColor = _settings.BackgroundColor;
            string TimestampColor = _settings.TimestampColor;
            string DefaultColor = _settings.DefaultColor;
            string ActorColor = _settings.ActorColor;
            string PartyColor = _settings.PartyColor;
            string EmoteColor = _settings.EmoteColor;
            string ShoutColor = _settings.ShoutColor;
            string TellColor = _settings.TellColor;
            string WhisperColor = _settings.WhisperColor;
            string MyColor = _settings.MyColor;
            string MyCharacters = _settings.MyCharacters;
            string FontName = _settings.FontName;
            string FontSize = _settings.FontSize;
            string CustomEmoteOne = _settings.CustomEmoteOne;
            string CustomEmoteOneColor = _settings.CustomEmoteOneColor;
            string CustomEmoteTwo = _settings.CustomEmoteTwo;
            string CustomEmoteTwoColor = _settings.CustomEmoteTwoColor;
            string CustomEmoteThree = _settings.CustomEmoteThree;
            string CustomEmoteThreeColor = _settings.CustomEmoteThreeColor;

            _settings = new Settings(OutputDirectory,
                                                PathToLog,
                                                MinimumRowsToInteger,
                                                ServerName,
                                                ServerNameColor,
                                                EventText,
                                                CombatText,
                                                UseTheme,
                                                Silent,
                                                Tray,
                                                SaveBackup,
                                                SaveBackupOnly,
                                                SaveOnLaunch,
                                                Notifications,
                                                OOCColor,
                                                FilterLines,
                                                PathToClient,
                                                RunClientOnLaunch,
                                                CloseOnLogGenerated,
                                                ServerAddress,
                                                ServerPassword,
                                                DM,
                                                ServerMode,
                                                BackgroundColor,
                                                TimestampColor,
                                                DefaultColor,
                                                ActorColor,
                                                PartyColor,
                                                EmoteColor,
                                                ShoutColor,
                                                TellColor,
                                                WhisperColor,
                                                MyColor,
                                                MyCharacters,
                                                FontName,
                                                FontSize,
                                                CustomEmoteOne,
                                                CustomEmoteOneColor,
                                                CustomEmoteTwo,
                                                CustomEmoteTwoColor,
                                                CustomEmoteThree,
                                                CustomEmoteThreeColor
                                            );

            return _settings;
        }

        private int NWNProcessStatus_Get(string ProcessName)
        {
            if (ClientLauncherState == 1)
            {
                return 2;
            }
            else if (ClientLauncherState == 2)
            {
                return 0;
            }
            else
            {
                Process[] processlist = Process.GetProcesses();

                foreach (Process theProcess in processlist)
                {
                    if (theProcess.ProcessName.IndexOf(ProcessName) != -1)
                    {
                        return 1;
                    }
                }
                return 0;
            }
        }

        private SolidColorBrush Color_Get(string UseTheme, bool OnColor)
        {
            if (OnColor == true)
            {
                if (_settings.UseTheme == "light")
                {
                    return new SolidColorBrush(Colors.DarkGreen);
                }
                else if (_settings.UseTheme == "dark")
                {
                    return new SolidColorBrush(Colors.LawnGreen);
                }
            }
            else
            {
                if (_settings.UseTheme == "light")
                {
                    return new SolidColorBrush(Colors.DarkRed);
                }
                else if (_settings.UseTheme == "dark")
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }

            return new SolidColorBrush(Colors.Gray);
        }

        /*
         * Functions
        */
        private void SetupApplication()
        {
            LoadSettings_Handler();
            IterateNWN_Watcher(false);
        }

        private async void IterateNWN_Watcher(bool PreviousStatus)
        {
            _settings = CurrentSettings_Get();
            var OnColor = Color_Get(_settings.UseTheme, true);
            var OffColor = Color_Get(_settings.UseTheme, false);
            int IterateDelay = 5000;
            ProcessName = _settings.ServerMode == true ? "nwserver" : "nwmain";
            var Status = NWNProcessStatus_Get(ProcessName);

            if (Status > 0)
            {
                NWNStatusTextBlock.Text = ProcessName + " is active!";
                NWNStatusTextBlock.Foreground = OnColor;

                if (Status == 1)
                {
                    await Task.Delay(IterateDelay);
                    IterateNWN_Watcher(true);
                }
            }
            else
            {
                NWNStatusTextBlock.Text = ProcessName + " not found!";
                NWNStatusTextBlock.Foreground = OffColor;
                if (PreviousStatus == true)
                {
                    if(_settings.SaveOnLaunch == false)
                    {
                        if (NWNLog_Save(_settings, true) == true)
                            UpdateResultsPane(1);
                    }
                    else
                    {
                        if(_settings.CloseOnLogGenerated == true )
                        {
                            ExitEvent();
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                        
                }
                await Task.Delay(IterateDelay);
                ClientLauncherState = 0;
                IterateNWN_Watcher(false);
            }
        }

        public async void UpdateResultsPane(int result)
        {
            _settings = CurrentSettings_Get();
            var OnColor = Color_Get(_settings.UseTheme, true);
            var OffColor = Color_Get(_settings.UseTheme, false);

            switch (result)
            {
                case -1:
                    EventStatusTextBlock.Text = "";
                    EventStatusTextBlock.Foreground = OnColor;
                    break;
                case 1:
                    EventStatusTextBlock.Text = "Log Saved Successfully!";
                    EventStatusTextBlock.Foreground = OnColor;
                    await Task.Delay(2000);
                    EventStatusTextBlock.Text = "";
                    break;
                case 2:
                    EventStatusTextBlock.Text = "Settings Saved Successfully!";
                    EventStatusTextBlock.Foreground = OnColor;
                    await Task.Delay(1000);
                    EventStatusTextBlock.Text = "";
                    break;
                case 3:
                    EventStatusTextBlock.Text = "Settings Loaded Successfully!";
                    EventStatusTextBlock.Foreground = OnColor;
                    await Task.Delay(2000);
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    string version = fvi.FileVersion;
                    version = "v" + version.Substring(2, version.Length - 2);
                    EventStatusTextBlock.Text = "Version " + version;
                    EventStatusTextBlock.Foreground = OnColor;
                    await Task.Delay(1000);
                    EventStatusTextBlock.Text = "";
                    break;
                case 4:
                    EventStatusTextBlock.Text = "Loading..";
                    EventStatusTextBlock.Foreground = OnColor;
                    break;
            }
        }

        private void LoadSettings_Handler()
        {
            Settings_Get();

            OutputDirectoryTextBox.Text = _settings.OutputDirectory;
            PathToLogTextBox.Text = _settings.PathToLog;
            MinimumRowsCountSlider.Value = _settings.MinimumRowsCount;
            EventTextCheckBox.IsChecked = _settings.EventText;
            CombatTextCheckBox.IsChecked = _settings.CombatText;
            SilentCheckBox.IsChecked = _settings.Silent;
            TrayCheckBox.IsChecked = _settings.Tray;
            SaveBackupCheckBox.IsChecked = _settings.SaveBackup;

            if(_settings.SaveBackupOnly == true)
            {
                SaveBackupCheckBox.IsChecked = true;
            }
                    
            SaveBackupOnlyCheckBox.IsChecked = _settings.SaveBackupOnly;
            SaveOnLaunchCheckBox.IsChecked = _settings.SaveOnLaunch;
            NotificationsCheckBox.IsChecked = _settings.Notifications;
            if (_settings.FilterLines != "")
            {
                FilterLinesCheckBox.IsChecked = true;
                FilterLinesTextBox.Text = _settings.FilterLines;
            }
            if (_settings.ServerMode == false)
            {
                ClientModeRadioButton.IsChecked = true;
            }
            else
            {
                ServerModeRadioButton.IsChecked = true;
            }

            if (_settings.UseTheme == "light")
            {
                ActivateLightTheme();
            }
            else if (_settings.UseTheme == "dark")
            {
                ActivateDarkTheme();
            }

            UpdateResultsPane(3);

            LoadTray_Listener();

            if (_settings.RunClientOnLaunch)
                LaunchClient();
        }

        private void ActivateDarkTheme()
        {
            /*
             * Purple -> Black -> Purple
            */
            LinearGradientBrush myBrush = new LinearGradientBrush();
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
            Grid.Background = myBrush;

            /*
             * Background
            */
            RunOnceButton.Background = Brushes.Black;
            SaveSettingsButton.Background = Brushes.Black;
            LaunchClientButton.Background = Brushes.Black;
            LauncherConfigurationButton.Background = Brushes.Black;
            ServerConfigurationButton.Background = Brushes.Black;
            OutputDirectoryTextBox.Background = Brushes.Black;
            PathToLogTextBox.Background = Brushes.Black;
            FilterLinesTextBox.Background = Brushes.Black;
            MainStatusBar.Background = Brushes.Black;


            /*
             * Foreground
            */
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.White);
            LaunchClientButton.Foreground = new SolidColorBrush(Colors.White);
            LauncherConfigurationButton.Foreground = new SolidColorBrush(Colors.White);
            ServerConfigurationButton.Foreground = new SolidColorBrush(Colors.White);
            OutputDirectoryTextBox.Foreground = new SolidColorBrush(Colors.White);
            PathToLogTextBox.Foreground = new SolidColorBrush(Colors.White);
            RunOnceButton.Foreground = new SolidColorBrush(Colors.White);
            TrayCheckBox.Foreground = new SolidColorBrush(Colors.White);
            SilentCheckBox.Foreground = new SolidColorBrush(Colors.White);
            SettingsTextBlock.Foreground = new SolidColorBrush(Colors.White);
            OutputDirectoryLabel.Foreground = new SolidColorBrush(Colors.White);
            PathToLogLabel.Foreground = new SolidColorBrush(Colors.White);
            FilterLinesTextBox.Foreground = new SolidColorBrush(Colors.White);
            EventTextCheckBox.Foreground = new SolidColorBrush(Colors.White);
            CombatTextCheckBox.Foreground = new SolidColorBrush(Colors.White);
            MinimumRowsLabel.Foreground = new SolidColorBrush(Colors.White);
            MinimumRowsCountTextBlock.Foreground = new SolidColorBrush(Colors.White);
            SaveBackupCheckBox.Foreground = new SolidColorBrush(Colors.White);
            SaveBackupOnlyCheckBox.Foreground = new SolidColorBrush(Colors.White);
            SaveOnLaunchCheckBox.Foreground = new SolidColorBrush(Colors.White);
            NotificationsCheckBox.Foreground = new SolidColorBrush(Colors.White);
            FilterLinesCheckBox.Foreground = new SolidColorBrush(Colors.White);
            ClientModeRadioButton.Foreground = new SolidColorBrush(Colors.White);
            ServerModeRadioButton.Foreground = new SolidColorBrush(Colors.White);
            HintLabel.Foreground = new SolidColorBrush(Colors.White);
            PathHelperLabel.Foreground = new SolidColorBrush(Colors.White);
            OutputHelperLabel.Foreground = new SolidColorBrush(Colors.White);
            GeneralSettingsLabel.Foreground = new SolidColorBrush(Colors.White);

            /*
             * Transformations
            */
            SettingsTextBlock.Text = "Dark Mode";
            _settings.UseTheme = "dark";
        }

        private void ActivateLightTheme()
        {
            /*
             * Lavender -> White -> Light Gray
            */
            LinearGradientBrush myBrush = new LinearGradientBrush();
            myBrush.GradientStops.Add(new GradientStop(Colors.Lavender, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.LightGray, 1.0));
            Grid.Background = myBrush;

            /*
             * Background
            */
            RunOnceButton.Background = Brushes.White;
            SaveSettingsButton.Background = Brushes.White;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.Black);
            LaunchClientButton.Background = Brushes.White;
            ServerConfigurationButton.Background = Brushes.White;
            LauncherConfigurationButton.Background = Brushes.White;
            OutputDirectoryTextBox.Background = Brushes.White;
            PathToLogTextBox.Background = Brushes.White;
            FilterLinesTextBox.Background = Brushes.White;
            MainStatusBar.Background = Brushes.White;

            /*
             * Foreground
            */
            RunOnceButton.Foreground = new SolidColorBrush(Colors.Black);
            TrayCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            SilentCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            LaunchClientButton.Foreground = new SolidColorBrush(Colors.Black);
            OutputDirectoryTextBox.Foreground = new SolidColorBrush(Colors.Black);
            PathToLogTextBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerConfigurationButton.Foreground = new SolidColorBrush(Colors.Black);
            LauncherConfigurationButton.Foreground = new SolidColorBrush(Colors.Black);
            SettingsTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            OutputDirectoryLabel.Foreground = new SolidColorBrush(Colors.Black);
            PathToLogLabel.Foreground = new SolidColorBrush(Colors.Black);
            FilterLinesTextBox.Foreground = new SolidColorBrush(Colors.Black);
            EventTextCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            CombatTextCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            MinimumRowsLabel.Foreground = new SolidColorBrush(Colors.Black);
            MinimumRowsCountTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            SaveBackupCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            SaveBackupOnlyCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            SaveOnLaunchCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            NotificationsCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            FilterLinesCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            ClientModeRadioButton.Foreground = new SolidColorBrush(Colors.Black);
            ServerModeRadioButton.Foreground = new SolidColorBrush(Colors.Black);
            HintLabel.Foreground = new SolidColorBrush(Colors.Black);
            PathHelperLabel.Foreground = new SolidColorBrush(Colors.Black);
            OutputHelperLabel.Foreground = new SolidColorBrush(Colors.Black);
            GeneralSettingsLabel.Foreground = new SolidColorBrush(Colors.Black);

            /*
             * Transformations
            */
            SettingsTextBlock.Text = "Light Mode";
            _settings.UseTheme = "light";
        }

        private bool NWNLog_Save(Settings _settings, bool _automatic)
        {
            string _filepathandname = FileHandlerInstance.ReadNWNLogAndInvokeParser(_settings);

            if (_filepathandname != "")
            {
                if (_settings.Notifications == true)
                {
                    notification.ShowNotification("Log file generated successfully!");
                }

                if (_settings.SaveOnLaunch == false)
                    if (_automatic && _settings.CloseOnLogGenerated == true)
                    {
                        ExitEvent();
                        Process.GetCurrentProcess().Kill();
                    }
                if (_settings.Silent == false)
                {
                    MessageBoxResult _messageBoxResult = MessageBox.Show("The log file has been generated successfully. Would you like to open the log file now?",
                            "Success!",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                    if (_messageBoxResult == MessageBoxResult.Yes)
                        Process.Start(_filepathandname);

                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (_settings.SaveOnLaunch == false)
                    if (_automatic && _settings.CloseOnLogGenerated == true)
                    {
                        ExitEvent();
                        Process.GetCurrentProcess().Kill();
                    }
            }

            return false;
        }

        private async void LaunchClient()
        {
            _settings = CurrentSettings_Get();

            if(_settings.SaveOnLaunch == true)
                if (NWNLog_Save(_settings, true) == true)
                    UpdateResultsPane(1);

            var theLaunchPath = "";
            try
            {
                theLaunchPath = Path.GetDirectoryName(_settings.PathToClient);
            }
            catch
            {
                theLaunchPath = "";
            }
            var theLaunchParameters = "";

            if (theLaunchPath != "")
            {
                if (_settings.DM == true)
                {
                    theLaunchParameters += " -dmc";
                }
                if (_settings.ServerAddress != "")
                {
                    theLaunchParameters += " +connect " + _settings.ServerAddress;
                    if (_settings.ServerPassword != "")
                    {
                        theLaunchParameters += " +password " + _settings.ServerPassword;
                    }
                }
                if (_settings.PathToClient.ToLower().Contains("steam.exe"))
                {
                    theLaunchParameters = " -gameidlaunch 704450" + theLaunchParameters;
                }
            }
            if (theLaunchPath != "" && File.Exists(_settings.PathToClient))
            {
                try
                {
                    var p = new Process();
                    p.StartInfo.FileName = _settings.PathToClient;
                    p.StartInfo.Arguments = theLaunchParameters;
                    p.StartInfo.WorkingDirectory = theLaunchPath;
                    p.StartInfo.RedirectStandardOutput = false;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.CreateNoWindow = true;
                    if (Path.GetFileName(_settings.PathToClient) == ProcessName + ".exe")
                    {
                        ClientLauncherState = 1;
                        IterateNWN_Watcher(false);
                    }
                    await Task.Run(() =>
                    {
                        p.Start();
                        p.WaitForExit();
                    });
                    if (Path.GetFileName(_settings.PathToClient) == ProcessName + ".exe")
                    {
                        ClientLauncherState = 2;
                        IterateNWN_Watcher(true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxResult _messageBoxResult = MessageBox.Show("An issue occured when trying to open the client: " + ex.Message,
                            "Client Not Found!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBoxResult _messageBoxResult = MessageBox.Show("Please configure the launch options, and verify the client path is correct prior to launching!",
                            "Configure Launch Options",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            }
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

        private void SaveSettings(Settings _settings)
        {
            FileHandlerInstance.SaveSettingsIni(_settings);

            UpdateResultsPane(2);
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

        private void LoadTray_Listener()
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

        /*
         * Event Handlers
        */
        private void SettingsTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void SettingsTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void MinimumRowsCountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MinimumRowsCountTextBlock != null)
                MinimumRowsCountTextBlock.Text = e.NewValue.ToString();
        }

        private void SaveBackupCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveBackupOnlyCheckBox.IsChecked = false;
        }

        private void SaveBackupOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SaveBackupCheckBox.IsChecked = true;
        }

        private void FilterLinesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FilterLinesTextBox.Visibility = Visibility.Visible;
        }

        private void FilterLinesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            FilterLinesTextBox.Text = "";
            FilterLinesTextBox.Visibility = Visibility.Collapsed;
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

        public void WindowClosed_Event(object sender, CancelEventArgs e)
        {
            ExitEvent();
        }

        public static void ExitEvent()
        {
            ni.Visible = false;
            ni.Icon = null;
            ni.Dispose();
            System.Windows.Forms.Application.DoEvents();
        }

        /*
         * Callbacks
        */
        private void SavedResult_Callback(bool result)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            if (result == true)
                UpdateResultsPane(1);
        }

        /*
         * Button Callbacks
        */
        private void PathToLog_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void OutputDirectory_MouseDown(object sender, MouseButtonEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = _settings.OutputDirectory;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    OutputDirectoryTextBox.Text = dialog.SelectedPath;
            }
        }

        private void Button_Click_Save_Settings(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();
            SaveSettings(_settings);
        }

        private void Button_Click_Export_Settings(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();
            ExportConfiguration ExportConfigurationPopUp = new ExportConfiguration(_settings);
            ExportConfigurationPopUp.Owner = Window.GetWindow(this);
            Grid.Effect = new BlurEffect();
            ExportConfigurationPopUp.ShowDialog();
            Settings __settings = ExportConfigurationPopUp.Settings_Get();
            bool __closed = ExportConfigurationPopUp.Closed_Get();
            if (_settings != __settings && !__closed)
            {
                _settings = __settings;
                SaveSettings(_settings);
            }
            Grid.Effect = null;
        }
        private void Button_Click_Launcher_Settings(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();
            LauncherConfiguration LauncherConfigurationPopUp = new LauncherConfiguration(_settings);
            LauncherConfigurationPopUp.Owner = Window.GetWindow(this);
            Grid.Effect = new BlurEffect();
            LauncherConfigurationPopUp.ShowDialog();
            Settings __settings = LauncherConfigurationPopUp.Settings_Get();
            bool __closed = LauncherConfigurationPopUp.Closed_Get();
            if (_settings != __settings && !__closed)
            {
                _settings = __settings;
                SaveSettings(_settings);
            }
            Grid.Effect = null;
        }

        private void Button_Click_Launch_Client(object sender, RoutedEventArgs e)
        {
            LaunchClient();
        }

        private async void Button_Click_Run_Once(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();
            bool SavedLogResult = false;
            SaveToggle_Event();

            Mouse.OverrideCursor = Cursors.Wait;
            await Task.Run(() =>
            {
                SavedLogResult = NWNLog_Save(_settings, false);
            });

            SaveToggle_Event();
            SavedResult_Callback(SavedLogResult);
        }
    }
}
