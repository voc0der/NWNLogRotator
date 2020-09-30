using NWNLogRotator.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NWNLogRotator
{
    /// <summary>
    /// Interaction logic for ExportConfiguration.xaml
    /// </summary>
    public partial class ExportConfiguration : Window
    {
        public Settings _settings;
        public bool _closed = true;
        public ExportConfiguration(Settings __settings)
        {
            InitializeComponent();
            SetupApplication(Settings_Set(__settings));
        }
        public Settings Settings_Get()
        {
            CurrentSettings_Get();
            return _settings;
        }
        public Settings Settings_Set(Settings __settings)
        {
            _settings = __settings;
            return _settings;
        }

        public bool Closed_Get()
        {
            return _closed;
        }
        private void SetupApplication(Settings _settings)
        {
            if (_settings.UseTheme == "light")
            {
                ActivateLightTheme();
            }
            else if (_settings.UseTheme == "dark")
            {
                ActivateDarkTheme();
            }
            LoadSettings_Handler(_settings);
        }

        public void ActivateDarkTheme()
        {
            LinearGradientBrush myBrush = new LinearGradientBrush();
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));

            Grid.Background = myBrush;

            ServerNameTextBox.Background = Brushes.Black;
            ServerNameColorTextBox.Background = Brushes.Black;
            ActorColorTextBox.Background = Brushes.Black;
            PartyColorTextBox.Background = Brushes.Black;
            EmoteColorTextBox.Background = Brushes.Black;
            TimestampColorTextBox.Background = Brushes.Black;
            ShoutColorTextBox.Background = Brushes.Black;
            TellColorTextBox.Background = Brushes.Black;
            WhisperColorTextBox.Background = Brushes.Black;
            BackgroundColorTextBox.Background = Brushes.Black;
            DefaultColorTextBox.Background = Brushes.Black;

            ServerNameTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            ServerNameColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            ServerNameTextBox.Foreground = new SolidColorBrush(Colors.White);
            ServerNameColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            ActorColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            ActorColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            PartyColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            PartyColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            EmoteColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            EmoteColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            TimestampColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            TimestampColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            ShoutColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            ShoutColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            TellColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            TellColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            WhisperColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            WhisperColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            BackgroundColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            BackgroundColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            DefaultColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            DefaultColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            SaveSettingsButton.Background = Brushes.Black;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.White);
        }

        public void ActivateLightTheme()
        {
            ServerNameTextBox.Background = Brushes.White;
            ServerNameColorTextBox.Background = Brushes.White;

            ServerNameTextBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerNameColorTextBox.Foreground = new SolidColorBrush(Colors.Black);

            SaveSettingsButton.Background = Brushes.White;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.Black);
        }
        private void LoadSettings_Handler(Settings _settings)
        {
            ServerNameTextBox.Text = _settings.ServerName;
            ServerNameColorTextBox.Text = _settings.ServerNameColor;
        }

        private Settings CurrentSettings_Get()
        {
            string OutputDirectory = _settings.OutputDirectory;
            string PathToLog = _settings.PathToLog;
            int MinimumRowsToInteger = _settings.MinimumRowsCount;
            string ServerName = ServerNameTextBox.Text;
            string ServerNameColor = ServerNameColorTextBox.Text;
            bool EventText = _settings.EventText;
            bool CombatText = _settings.CombatText;
            string UseTheme = _settings.UseTheme;
            bool Silent = _settings.Silent;
            bool Tray = _settings.Tray;
            bool SaveBackup = _settings.SaveBackup;
            bool Notifications = _settings.Notifications;
            string CustomEmotes = _settings.CustomEmotes;
            string FilterLines = _settings.FilterLines;
            string PathToClient = _settings.PathToClient;
            bool RunClientOnLaunch = _settings.RunClientOnLaunch;
            bool CloseOnLogGenerated = _settings.CloseOnLogGenerated;
            string ServerAddress = _settings.ServerAddress;
            string ServerPassword = _settings.ServerPassword;
            bool DM = _settings.DM;
            bool ServerMode = _settings.ServerMode;
            string BackgroundColor = _settings.BackgroundColor;
            string TimestampColor = _settings.TimestampColor;
            string DefaultColor = _settings.DefaultColor;
            string ActorColor = _settings.ActorColor;
            string PartyColor = _settings.PartyColor;
            string EmoteColor = _settings.EmoteColor;
            string ShoutColor = _settings.ShoutColor;
            string TellColor = _settings.TellColor;
            string WhisperColor = _settings.WhisperColor;

            _settings = new Settings(   OutputDirectory,
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
                                        Notifications,
                                        CustomEmotes,
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
                                        WhisperColor);
            return _settings;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();
            _closed = false;
            this.Close();
        }
    }
}
