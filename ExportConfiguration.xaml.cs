using NWNLogRotator.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Black, 1.0));

            Grid.Background = myBrush;

            ServerNameTextBox.Background = Brushes.Black;
            ServerNameColorTextBox.Background = Brushes.Black;
            MyCharactersTextBox.Background = Brushes.Black;
            MyColorTextBox.Background = Brushes.Black;
            FontNameTextBox.Background = Brushes.Black;
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
            ServerNameLabelTwo.Foreground = new SolidColorBrush(Colors.White);
            ServerNameColorLabelTwo.Foreground = new SolidColorBrush(Colors.White);
            MyCharactersTextBox.Foreground = new SolidColorBrush(Colors.White);
            MyCharactersTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            MyColorTextBox.Foreground = new SolidColorBrush(Colors.White);
            MyColorTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            FontNameTextBox.Foreground = new SolidColorBrush(Colors.White);
            FontNameTextBoxLabel.Foreground = new SolidColorBrush(Colors.White);
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
            ResetSettingsButton.Background = Brushes.Black;
            ResetSettingsButton.Foreground = new SolidColorBrush(Colors.White);
            SaveSettingsButton.Background = Brushes.Black;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.White);
        }

        public void ActivateLightTheme()
        {
            ServerNameTextBox.Background = Brushes.White;
            ServerNameColorTextBox.Background = Brushes.White;
            ServerNameTextBox.Foreground = new SolidColorBrush(Colors.Black);
            ServerNameColorTextBox.Foreground = new SolidColorBrush(Colors.Black);

            ResetSettingsButton.Background = Brushes.White;
            ResetSettingsButton.Foreground = new SolidColorBrush(Colors.Black);
            SaveSettingsButton.Background = Brushes.White;
            SaveSettingsButton.Foreground = new SolidColorBrush(Colors.Black);
        }
        private void LoadSettings_Handler(Settings _settings)
        {
            ServerNameTextBox.Text = _settings.ServerName;
            ServerNameColorTextBox.Text = _settings.ServerNameColor;
            BackgroundColorTextBox.Text = _settings.BackgroundColor;
            TimestampColorTextBox.Text = _settings.TimestampColor;
            DefaultColorTextBox.Text = _settings.DefaultColor;
            ActorColorTextBox.Text = _settings.ActorColor;
            PartyColorTextBox.Text = _settings.PartyColor;
            EmoteColorTextBox.Text = _settings.EmoteColor;
            ShoutColorTextBox.Text = _settings.ShoutColor;
            TellColorTextBox.Text = _settings.TellColor;
            WhisperColorTextBox.Text = _settings.WhisperColor;
            MyColorTextBox.Text = _settings.MyColor;
            MyCharactersTextBox.Text = _settings.MyCharacters;
            FontNameTextBox.Text = _settings.FontName;
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
            string BackgroundColor = BackgroundColorTextBox.Text;
            string TimestampColor = TimestampColorTextBox.Text;
            string DefaultColor = DefaultColorTextBox.Text;
            string ActorColor = ActorColorTextBox.Text;
            string PartyColor = PartyColorTextBox.Text;
            string EmoteColor = EmoteColorTextBox.Text;
            string ShoutColor = ShoutColorTextBox.Text;
            string TellColor = TellColorTextBox.Text;
            string WhisperColor = WhisperColorTextBox.Text;
            string MyColor = MyColorTextBox.Text;
            string MyCharacters = MyCharactersTextBox.Text;
            string FontName = FontNameTextBox.Text;

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
            if( _settings.BackgroundColor.Length == 6 &&
               _settings.TimestampColor.Length == 6 &&
               _settings.DefaultColor.Length == 6 &&
               _settings.ActorColor.Length == 6 &&
               _settings.PartyColor.Length == 6 &&
               _settings.EmoteColor.Length == 6 &&
               _settings.ShoutColor.Length == 6 &&
               _settings.TellColor.Length == 6 &&
               _settings.WhisperColor.Length == 6
              )
            {
                _closed = false;
                this.Close();
            } 
            else
            {
                MessageBoxResult _messageBoxResult = MessageBox.Show("Please make sure every color in the Export Options is six characters long.",
                            "Invalid Export Color!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
            }
            
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var __settings = new Settings();
            BackgroundColorTextBox.Text = __settings.BackgroundColor;
            TimestampColorTextBox.Text = __settings.TimestampColor;
            DefaultColorTextBox.Text = __settings.DefaultColor;
            ActorColorTextBox.Text = __settings.ActorColor;
            PartyColorTextBox.Text = __settings.PartyColor;
            EmoteColorTextBox.Text = __settings.EmoteColor;
            ShoutColorTextBox.Text = __settings.ShoutColor;
            TellColorTextBox.Text = __settings.TellColor;
            WhisperColorTextBox.Text = __settings.WhisperColor;
            FontNameTextBox.Text = __settings.FontName;
            __settings = null;
        }
    }
}
