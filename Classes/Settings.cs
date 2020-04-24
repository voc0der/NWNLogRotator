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

        // create singleton
        public static Settings _instance = new Settings();

        // prototype
        public Settings()
        {
            this.OutputDirectory = "C:\\nwnlogs";
            this.PathToLog = "C:\\nwnlogs\\nwClientLog1.txt";
            this.MinimumRowsCount = 10;
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
                        string FilterLines)
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
        }

        // get singleton
        public Settings Instance
        {
            get { return _instance; }
        }
    }
}
