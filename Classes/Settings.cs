namespace NWNLogRotator.Classes
{
    public class Settings
    {
        public string OutputDirectory;
        public string PathToLog;
        public int MinimumRowsCount;
        public string ServerName;
        public string ServerNameColor;
        public bool EventText;
        public bool CombatText;
        public string UseTheme;
        public bool Silent;
        public bool Tray;
        public bool SaveBackup;
        public bool Notifications;
        public string OOCColor;
        public string FilterLines;
        public string PathToClient;
        public bool RunClientOnLaunch;
        public bool CloseOnLogGenerated;
        public string ServerAddress;
        public string ServerPassword;
        public bool DM;
        public bool ServerMode;
        public string BackgroundColor;
        public string TimestampColor;
        public string DefaultColor;
        public string ActorColor;
        public string PartyColor;
        public string EmoteColor;
        public string ShoutColor;
        public string TellColor;
        public string WhisperColor;
        public string MyColor;
        public string MyCharacters;
        public string FontName;
        public string FontSize;
        public string CustomEmoteOne;
        public string CustomEmoteOneColor;
        public string CustomEmoteTwo;
        public string CustomEmoteTwoColor;
        public string CustomEmoteThree;
        public string CustomEmoteThreeColor;

        // create singleton
        public static Settings _instance = new Settings();

        // prototype
        public Settings()
        {
            this.OutputDirectory = "C:\\nwnlogs";
            this.PathToLog = "C:\\nwnlogs\\nwClientLog1.txt";
            this.MinimumRowsCount = 5;
            this.ServerName = "";
            this.ServerNameColor = "EECC00";
            this.EventText = false;
            this.CombatText = false;
            this.UseTheme = "light";
            this.Silent = false;
            this.Tray = false;
            this.SaveBackup = false;
            this.Notifications = false;
            this.OOCColor = "D70A53";
            this.FilterLines = "";
            this.PathToClient = "";
            this.RunClientOnLaunch = false;
            this.CloseOnLogGenerated = false;
            this.ServerAddress = "";
            this.ServerPassword = "";
            this.DM = false;
            this.ServerMode = false;
            this.BackgroundColor = "000000";
            this.TimestampColor = "B1A2BD";
            this.DefaultColor = "FFFFFF";
            this.ActorColor = "8F7FFF";
            this.PartyColor = "FFAED6";
            this.EmoteColor = "A6DDCE";
            this.ShoutColor = "F0DBA5";
            this.TellColor = "00FF00";
            this.WhisperColor = "808080";
            this.MyColor = "D6CEFD";
            this.MyCharacters = "";
            this.FontName = "Tahoma, Geneva, sans-serif";
            this.FontSize = "calc(.7vw + .7vh + .5vmin)";
            this.CustomEmoteOne = "";
            this.CustomEmoteOneColor = "FF9944";
            this.CustomEmoteTwo = "";
            this.CustomEmoteTwoColor = "87CEEB";
            this.CustomEmoteThree = "";
            this.CustomEmoteThreeColor = "FFD8B1";
        }

        // binding
        public Settings(string OutputDirectory,
                        string PathToLog,
                        int MinimumRowsCount,
                        string ServerName,
                        string ServerNameColor,
                        bool EventText,
                        bool CombatText,
                        string UseTheme,
                        bool Silent,
                        bool Tray,
                        bool SaveBackup,
                        bool Notifications,
                        string OOCColor,
                        string FilterLines,
                        string PathToClient,
                        bool RunClientOnLaunch,
                        bool CloseOnLogGenerated,
                        string ServerAddress,
                        string ServerPassword,
                        bool DM,
                        bool ServerMode,
                        string BackgroundColor,
                        string TimestampColor,
                        string DefaultColor,
                        string ActorColor,
                        string PartyColor,
                        string EmoteColor,
                        string ShoutColor,
                        string TellColor,
                        string WhisperColor,
                        string MyColor,
                        string MyCharacters,
                        string FontName,
                        string FontSize,
                        string CustomEmoteOne,
                        string CustomEmoteOneColor,
                        string CustomEmoteTwo,
                        string CustomEmoteTwoColor,
                        string CustomEmoteThree,
                        string CustomEmoteThreeColor)
        {
            this.OutputDirectory = OutputDirectory;
            this.PathToLog = PathToLog;
            this.MinimumRowsCount = MinimumRowsCount;
            this.ServerName = ServerName;
            this.ServerNameColor = ServerNameColor;
            this.EventText = EventText;
            this.CombatText = CombatText;
            this.UseTheme = UseTheme;
            this.Silent = Silent;
            this.Tray = Tray;
            this.SaveBackup = SaveBackup;
            this.Notifications = Notifications;
            this.OOCColor = OOCColor;
            this.FilterLines = FilterLines;
            this.PathToClient = PathToClient;
            this.RunClientOnLaunch = RunClientOnLaunch;
            this.CloseOnLogGenerated = CloseOnLogGenerated;
            this.ServerAddress = ServerAddress;
            this.ServerPassword = ServerPassword;
            this.DM = DM;
            this.ServerMode = ServerMode;
            this.BackgroundColor = BackgroundColor;
            this.TimestampColor = TimestampColor;
            this.DefaultColor = DefaultColor;
            this.ActorColor = ActorColor;
            this.PartyColor = PartyColor;
            this.EmoteColor = EmoteColor;
            this.ShoutColor = ShoutColor;
            this.TellColor = TellColor;
            this.WhisperColor = WhisperColor;
            this.MyColor = MyColor;
            this.MyCharacters = MyCharacters;
            this.FontName = FontName;
            this.FontSize = FontSize;
            this.CustomEmoteOne = CustomEmoteOne;
            this.CustomEmoteOneColor = CustomEmoteOneColor;
            this.CustomEmoteTwo = CustomEmoteTwo;
            this.CustomEmoteTwoColor = CustomEmoteTwoColor;
            this.CustomEmoteThree = CustomEmoteThree;
            this.CustomEmoteThreeColor = CustomEmoteThreeColor;
        }

        // get singleton
        public Settings Instance
        {
            get { return _instance; }
        }
    }
}
