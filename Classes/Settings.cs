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
                        bool Tray)
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
        }

        // get singleton
        public Settings Instance
        {
            get { return _instance; }
        }
    }
}
