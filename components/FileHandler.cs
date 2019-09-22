using NWNLogRotator.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NWNLogRotator.Components
{
    public partial class FileHandler : Component
    {
        public FileHandler()
        {
            InitializeComponent();
        }

        public FileHandler(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public string CurrentProgramDirectory_Get()
        {
            string theWorkingDirectory = "";

            //get the current working directory
            theWorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return theWorkingDirectory;
        }

        public bool SettingsExist_Get()
        {
            string[] files =
                Directory.GetFiles(CurrentProgramDirectory_Get(), "*NWNLogRotator.ini", SearchOption.TopDirectoryOnly);

            if (files.Length == 1)
            {
                return true;
            }
            return false;
        }

        public void CreateSettingsIni()
        {
            string iniPath = CurrentProgramDirectory_Get() + "NWNLogRotator.ini";

            string DefaultSettings =    "OutputDirectory=C:/nwnlogs/\n" +
                                        "PathToLog=C:/nwnlogs/nwClientLog1.txt\n" +
                                        "MinimumRows=10\n" +
                                        "ServerName=\n" +
                                        "ServerNameColor=\n" +
                                        "EventText=false\n" +
                                        "CombatText=false\n" +
                                        "UseTheme=false\n" +
                                        "Tray=false";

            File.WriteAllText(iniPath, DefaultSettings);

            ReadSettingsIni();
        }
        public bool ReadSettingsIni()
        {
            string iniPath = CurrentProgramDirectory_Get() + "NWNLogRotator.ini";

            string OutputDirectory = "C:\nwnlogs";
            string PathToLog = "C:\nwnlogs\nwClientLog1.txt";
            int MinimumRowsToInteger = 10;
            string ServerName = "";
            string ServerNameColor = "";
            bool? EventText = false;
            bool? CombatText = false;
            string UseTheme = "";
            bool Tray = false;

            int Count = 0;

            foreach (var line in File.ReadLines(iniPath))
            {
                string ParameterValue = line.Split('=').Last();

                if (line.IndexOf("OutputDirectory=") != -1)
                {
                    OutputDirectory = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("PathToLog=") != -1)
                {
                    PathToLog = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("MinimumRows=") != -1)
                {
                    MinimumRowsToInteger = int.Parse( ParameterValue );
                    Count += 1;
                }
                if (line.IndexOf("ServerName=") != -1)
                {
                    ServerName = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("ServerNameColor=") != -1)
                {
                    ServerNameColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("EventText=") != -1)
                {
                    EventText = bool.Parse( ParameterValue );
                    Count += 1;
                }
                if (line.IndexOf("CombatText=") != -1)
                {
                    CombatText = bool.Parse( ParameterValue );
                    Count += 1;
                }
                if (line.IndexOf("UseTheme=") != -1)
                {
                    UseTheme = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("Tray=") != -1)
                {
                    Tray = bool.Parse( ParameterValue );
                    Count += 1;
                }
            }

            var SavedSettings = new Settings();/*new Settings(OutputDirectory,
                                              PathToLog,
                                              MinimumRowsToInteger,
                                              ServerName,
                                              ServerNameColor,
                                              EventText,
                                              CombatText,
                                              UseTheme,
                                              Tray
                                            );*/

            if (Count != 9)
            {
                MessageBox.Show("Default Configuration Loaded:\n\nPlease ensure NWNLogRotator.ini is properly formatted, and has 9 parameters present.\n\nIf it is deleted, NWNLogRotator will create a new one automatically with the default settings.", "Invalid Settings File!");
                return false;
            }

            EnactSettings();
            return true;
        }

        public void EnactSettings( )
        {

        }

        public void InitSettingsIni()
        {
            if (SettingsExist_Get() == false)
            {
                CreateSettingsIni();
            }
            else
            {
                ReadSettingsIni();
            }
        }
    }
}
