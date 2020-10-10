using NWNLogRotator.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace NWNLogRotator
{
    /// <summary>
    /// Interaction logic for LauncherConfiguration.xaml
    /// </summary>
    public partial class LauncherConfiguration : Window
    {
        public Settings _settings;
        public bool _closed = true;
        public LauncherConfiguration(Settings __settings)
        {
            InitializeComponent();
            SetupApplication( Settings_Set(__settings) );
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
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 1.0));
            Grid.Background = myBrush;

            PathToClientTextBox.Background = Brushes.Black;
            ServerAddressTextBox.Background = Brushes.Black;
            ServerPasswordTextBox.Background = Brushes.Black;

            PathToClientLabel.Foreground = new SolidColorBrush(Colors.White);
            PathToClientTextBox.Foreground = new SolidColorBrush(Colors.White);
            RunClientOnLaunchCheckBox.Foreground = new SolidColorBrush(Colors.White);
            CloseAfterGenerationCheckBox.Foreground = new SolidColorBrush(Colors.White);
            ServerAddressLabel.Foreground = new SolidColorBrush(Colors.White);
            ServerAddressTextBox.Foreground = new SolidColorBrush(Colors.White);
            ServerPasswordLabel.Foreground = new SolidColorBrush(Colors.White);
            ServerPasswordTextBox.Foreground = new SolidColorBrush(Colors.White);
            DMCheckBox.Foreground = new SolidColorBrush(Colors.White);
            SaveSettingsButton.Background = Brushes.Black;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.White);
        }

        public void ActivateLightTheme()
        {
            PathToClientLabel.Background = Brushes.White;
            PathToClientTextBox.Background = Brushes.White;
            ServerAddressTextBox.Background = Brushes.White;
            ServerPasswordTextBox.Background = Brushes.White;

            PathToClientLabel.Foreground = new SolidColorBrush(Colors.Black);
            PathToClientTextBox.Foreground = new SolidColorBrush(Colors.Black);
            RunClientOnLaunchCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            CloseAfterGenerationCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerAddressLabel.Foreground = new SolidColorBrush(Colors.Black);
            ServerAddressTextBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerPasswordLabel.Foreground = new SolidColorBrush(Colors.Black);
            ServerPasswordTextBox.Foreground = new SolidColorBrush(Colors.Black);
            DMCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            SaveSettingsButton.Background = Brushes.White;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void LoadSettings_Handler(Settings _settings)
        {
            PathToClientTextBox.Text = _settings.PathToClient;
            RunClientOnLaunchCheckBox.IsChecked = _settings.RunClientOnLaunch;
            CloseAfterGenerationCheckBox.IsChecked = _settings.CloseOnLogGenerated;
            ServerAddressTextBox.Text = _settings.ServerAddress;
            ServerPasswordTextBox.Text = _settings.ServerPassword;
            DMCheckBox.IsChecked = _settings.DM;
        }

        private Settings CurrentSettings_Get()
        {
            string OutputDirectory = _settings.OutputDirectory;
            string PathToLog = _settings.PathToLog;
            int MinimumRowsToInteger = _settings.MinimumRowsCount;
            string ServerName = _settings.ServerName;
            string ServerNameColor = _settings.ServerNameColor;
            bool EventText = _settings.EventText;
            bool CombatText = _settings.CombatText;
            string UseTheme = _settings.UseTheme;
            bool Silent = _settings.Silent;
            bool Tray = _settings.Tray;
            bool SaveBackup = _settings.SaveBackup;
            bool Notifications = _settings.Notifications;
            string CustomEmotes = _settings.CustomEmotes;
            string FilterLines = _settings.FilterLines;
            string PathToClient = PathToClientTextBox.Text;
            bool RunClientOnLaunch = RunClientOnLaunchCheckBox.IsChecked.GetValueOrDefault();
            bool CloseOnLogGenerated = CloseAfterGenerationCheckBox.IsChecked.GetValueOrDefault();
            string ServerAddress = ServerAddressTextBox.Text;
            string ServerPassword = ServerPasswordTextBox.Text;
            bool DM = DMCheckBox.IsChecked.GetValueOrDefault();
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
            string MyColor = _settings.MyColor;
            string MyCharacters = _settings.MyCharacters;
            string FontName = _settings.FontName;

            _settings = new Settings(         OutputDirectory,
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
                                              WhisperColor,
                                              MyColor,
                                              MyCharacters,
                                              FontName
                                            );

            return _settings;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();
            _closed = false;
            this.Close();
        }
        private void Image_MouseDown3(object sender, MouseButtonEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.InitialDirectory = _settings.PathToLog;
            fileDialog.Filter = "exe file (*.exe)|*.exe";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    PathToClientTextBox.Text = file;
                    PathToClientTextBox.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    break;
                default:
                    break;
            }
        }
    }
}
