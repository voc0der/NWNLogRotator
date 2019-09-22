using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNLogRotator.classes
{
    class Settings
    {
        public string OutputDirectory;
        public string PathToLog;
        public int MinimumRowsCount;
        public string ServerName;
        public string ServerNameColor;
        public bool? EventText;
        public bool? CombatText;
        public string UseTheme;
        public bool Tray;

        public Settings()
        {
            this.OutputDirectory = "C:\nwnlogs";
            this.PathToLog = "C:\nwnlogs\nwClientLog1.txt";
            this.MinimumRowsCount = 10;
            this.ServerName = "";
            this.ServerNameColor = "";
            this.EventText = false;
            this.CombatText = false;
            this.UseTheme = "";
            this.Tray = false;
        }

        public Settings(string OutputDirectory,
                        string PathToLog, 
                        int MinimumRowsCount,
                        string ServerName,
                        string ServerNameColor,
                        bool? EventText,
                        bool? CombatText,
                        string UseTheme,
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
            this.Tray = Tray;
        }
    }
}
