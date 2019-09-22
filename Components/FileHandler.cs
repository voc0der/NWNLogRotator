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
        Settings _settings;
        int _expectedSettingsCount = 9;

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
            return AppDomain.CurrentDomain.BaseDirectory;
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

        public string SettingsFile_Linter()
        {
            string NewSettingsFile = "OutputDirectory=" + _settings.OutputDirectory + "\n" +
                                        "PathToLog=" + _settings.PathToLog + "\n" +
                                        "MinimumRows=" + _settings.MinimumRowsCount + "\n" +
                                        "ServerName=" + _settings.ServerName + "\n" +
                                        "ServerNameColor=" + _settings.ServerNameColor + "\n" +
                                        "EventText=" + _settings.EventText + "\n" +
                                        "CombatText=" + _settings.CombatText + "\n" +
                                        "UseTheme=" + _settings.UseTheme + "\n" +
                                        "Tray=" + _settings.Tray;
            return NewSettingsFile;
        }

        public void SaveSettingsIni( Settings _new_settings )
        {
            _settings = _new_settings;
            CreateSettingsIni();
        }

        public DateTime CurrentDateTime_Get()
        {
            return DateTime.Now;
        }

        public void CreateSettingsIni()
        {
            string iniPath = CurrentProgramDirectory_Get() + "NWNLogRotator.ini";

            string DefaultSettings = SettingsFile_Linter();

            File.WriteAllText(iniPath, DefaultSettings);

            ReadSettingsIni();
        }
        public bool ReadSettingsIni()
        {
            string iniPath = CurrentProgramDirectory_Get() + "NWNLogRotator.ini";

            // interpret fields
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

            _settings = new Settings(OutputDirectory,
                                              PathToLog,
                                              MinimumRowsToInteger,
                                              ServerName,
                                              ServerNameColor,
                                              EventText,
                                              CombatText,
                                              UseTheme,
                                              Tray
                                            );

            if (Count != _expectedSettingsCount)
            {
                MessageBox.Show("Default Configuration Loaded:\n\nPlease ensure NWNLogRotator.ini is properly formatted, and has " + _expectedSettingsCount + " parameters present.\n\nIf it is deleted, NWNLogRotator will create a new one automatically with the default settings.", "Invalid Settings File!");
                return false;
            }

            return true;
        }

        public Settings InitSettingsIni()
        {
            _settings = new Settings();

            if (SettingsExist_Get() == false)
            {
                CreateSettingsIni();
            }
            else
            {
                ReadSettingsIni();
            }

            return _settings;
        }

        public string FilePath_Get( Settings _run_settings )
        {
            string LogBasePath = _run_settings.OutputDirectory.Trim();

            if (!LogBasePath.EndsWith("/"))
            {
                LogBasePath += "/";
            }

            // has a server listed
            string ServerName = "Server";
            if (_run_settings.ServerName != "")
            {
                ServerName = _run_settings.ServerName + "/";
                LogBasePath += ServerName;
            }

            return LogBasePath;
        }

        public string FileNameGenerator_Get( DateTime _dateTime )
        {
            return "NWNLog_" + _dateTime.ToString("yyyy_MM_ddhhm") + ".html";
        }

        public string ReadNWNLogAndInvokeParser( Settings _run_settings )
        { 
            string result;
            DateTime _dateTime = CurrentDateTime_Get();
            string filepath = FilePath_Get(_run_settings);
            string filename = FileNameGenerator_Get(_dateTime);

            using (StreamReader streamReader = new StreamReader(_run_settings.PathToLog, Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
            }

            Parser instance = new Parser();
            result = instance.ParseNWNLog( result, _run_settings, _dateTime );

            try
            {
                File.WriteAllText(filepath + filename, result);
                return filepath + filename;
            }
            catch
            {
                MessageBoxResult _messageBoxResult = MessageBox.Show("The destination file " + filepath + filename + " is unable to be written to this folder. Would you like NNWNLogRotator to try and create the destination folder?", 
                                                                    "Output Directory Invalid",
                                                                    MessageBoxButton.YesNo,
                                                                    MessageBoxImage.Question);
                if (_messageBoxResult == MessageBoxResult.Yes)
                {
                    // create folder
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                        try
                        {
                            File.WriteAllText(filepath + filename, result);
                            return filepath + filename;
                        }
                        catch
                        {
                            MessageBox.Show("NWNLogRotator was not able to write to the entered Output Directory. Please ensure the file structure exists.", "Output Directory Error");
                        }
                    }
                }
            }
            return "";
        }
    }
}
