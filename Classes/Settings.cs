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
        public string CustomEmotes;
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

        // create singleton
        public static Settings _instance = new Settings();

        // prototype
        public Settings()
        {
            this.OutputDirectory = "C:\\nwnlogs";
            this.PathToLog = "C:\\nwnlogs\\nwClientLog1.txt";
            this.MinimumRowsCount = 5;
            this.ServerName = "";
            this.ServerNameColor = "";
            this.EventText = false;
            this.CombatText = false;
            this.UseTheme = "light";
            this.Silent = false;
            this.Tray = false;
            this.SaveBackup = false;
            this.Notifications = false;
            this.CustomEmotes = "";
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
                        string CustomEmotes,
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
                        string WhisperColor)
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
            this.CustomEmotes = CustomEmotes;
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
        }

        // get singleton
        public Settings Instance
        {
            get { return _instance; }
        }
    }
}
