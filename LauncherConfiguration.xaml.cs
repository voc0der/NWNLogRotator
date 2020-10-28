using NWNLogRotator.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace NWNLogRotator
{
    public partial class LauncherConfiguration : Window
    {
        public Settings _settings;
        public bool _closed = true;

        public LauncherConfiguration(Settings __settings)
        {
            InitializeComponent();
            SetupApplication(Settings_Set(__settings));
        }

        /*
         * Setters and Getters
        */
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

        public void ActivateDarkTheme()
        {
            /*
             * Black -> Purple -> Black
            */
            LinearGradientBrush myBrush = new LinearGradientBrush();
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 1.0));
            Grid.Background = myBrush;

            /*
             * Background
            */
            PathToClientTextBox.Background = Brushes.Black;
            ServerAddressTextBox.Background = Brushes.Black;
            ServerPasswordTextBox.Background = Brushes.Black;
            SaveSettingsButton.Background = Brushes.Black;

            /*
             * Foreground
            */
            PathToClientLabel.Foreground = new SolidColorBrush(Colors.White);
            PathToClientTextBox.Foreground = new SolidColorBrush(Colors.White);
            RunClientOnLaunchCheckBox.Foreground = new SolidColorBrush(Colors.White);
            CloseAfterGenerationCheckBox.Foreground = new SolidColorBrush(Colors.White);
            ServerAddressLabel.Foreground = new SolidColorBrush(Colors.White);
            ServerAddressTextBox.Foreground = new SolidColorBrush(Colors.White);
            ServerPasswordLabel.Foreground = new SolidColorBrush(Colors.White);
            ServerPasswordTextBox.Foreground = new SolidColorBrush(Colors.White);
            DMCheckBox.Foreground = new SolidColorBrush(Colors.White);
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.White);
        }

        public void ActivateLightTheme()
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
            PathToClientLabel.Background = Brushes.White;
            PathToClientTextBox.Background = Brushes.White;
            ServerAddressTextBox.Background = Brushes.White;
            ServerPasswordTextBox.Background = Brushes.White;
            SaveSettingsButton.Background = Brushes.White;

            /*
             * Foreground
            */
            PathToClientLabel.Foreground = new SolidColorBrush(Colors.Black);
            PathToClientTextBox.Foreground = new SolidColorBrush(Colors.Black);
            RunClientOnLaunchCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            CloseAfterGenerationCheckBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerAddressLabel.Foreground = new SolidColorBrush(Colors.Black);
            ServerAddressTextBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerPasswordLabel.Foreground = new SolidColorBrush(Colors.Black);
            ServerPasswordTextBox.Foreground = new SolidColorBrush(Colors.Black);
            DMCheckBox.Foreground = new SolidColorBrush(Colors.Black);
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
            string OOCColor = _settings.OOCColor;
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

        /*
         * Functions
        */
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

        /*
         * Button Callbacks
        */
        private void Button_Click_Save_Settings(object sender, RoutedEventArgs e)
        {
            _settings = CurrentSettings_Get();
            _closed = false;
            this.Close();
        }
        private void PathToClient_MouseDown(object sender, MouseButtonEventArgs e)
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
