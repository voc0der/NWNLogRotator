using NWNLogRotator.Classes;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace NWNLogRotator.Components
{
    public partial class FileHandler : Component
    {
        Settings _settings;
        int _expectedSettingsCount = 32;

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
                                        "Silent=" + _settings.Silent + "\n" +
                                        "Tray=" + _settings.Tray + "\n" +
                                        "SaveBackup=" + _settings.SaveBackup + "\n" +
                                        "Notifications=" + _settings.Notifications + "\n" +
                                        "CustomEmotes=" + _settings.CustomEmotes + "\n" +
                                        "FilterLines=" + _settings.FilterLines + "\n" +
                                        "PathToClient=" + _settings.PathToClient + "\n" +
                                        "RunClientOnLaunch=" + _settings.RunClientOnLaunch + "\n" +
                                        "CloseOnLogGenerated=" + _settings.CloseOnLogGenerated + "\n" +
                                        "ServerAddress=" + _settings.ServerAddress + "\n" +
                                        "ServerPassword=" + _settings.ServerPassword + "\n" +
                                        "DM=" + _settings.DM + "\n" +
                                        "ServerMode=" + _settings.ServerMode + "\n" +
                                        "BackgroundColor=" + _settings.BackgroundColor + "\n" +
                                        "TimestampColor=" + _settings.TimestampColor + "\n" +
                                        "DefaultColor=" + _settings.DefaultColor + "\n" +
                                        "ActorColor=" + _settings.ActorColor + "\n" +
                                        "PartyColor=" + _settings.PartyColor + "\n" +
                                        "EmoteColor=" + _settings.EmoteColor + "\n" +
                                        "ShoutColor=" + _settings.ShoutColor + "\n" +
                                        "TellColor=" + _settings.TellColor + "\n" +
                                        "WhisperColor=" + _settings.WhisperColor + "\n" +
                                        "MyColor=" + _settings.MyColor + "\n" +
                                        "MyCharacters=" + _settings.MyCharacters;
            return NewSettingsFile;
        }

        public void SaveSettingsIni(Settings _new_settings)
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

            string CurrentSettings = SettingsFile_Linter();

            File.WriteAllText(iniPath, CurrentSettings);

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
            bool EventText = false;
            bool CombatText = false;
            string UseTheme = "";
            bool Silent = false;
            bool Tray = false;
            bool SaveBackup = false;
            bool Notifications = false;
            string CustomEmotes = "";
            string FilterLines = "";
            string PathToClient = "";
            bool RunClientOnLaunch = false;
            bool CloseOnLogGenerated = false;
            string ServerAddress = "";
            string ServerPassword = "";
            bool DM = false;
            bool ServerMode = false;
            string BackgroundColor = "000000";
            string TimestampColor = "B1A2BD";
            string DefaultColor = "FFFFFF";
            string ActorColor = "8F7FFF";
            string PartyColor = "FFAED6";
            string EmoteColor = "A6DDCE";
            string ShoutColor = "F0DBA5";
            string TellColor = "00FF00";
            string WhisperColor = "808080";
            string MyColor = "";
            string MyCharacters = "";

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
                    MinimumRowsToInteger = int.Parse(ParameterValue);
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
                    EventText = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("CombatText=") != -1)
                {
                    CombatText = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("UseTheme=") != -1)
                {
                    UseTheme = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("Silent=") != -1)
                {
                    Silent = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("Tray=") != -1)
                {
                    Tray = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("SaveBackup=") != -1)
                {
                    SaveBackup = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("Notifications=") != -1)
                {
                    Notifications = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("CustomEmotes=") != -1)
                {
                    CustomEmotes = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("FilterLines=") != -1)
                {
                    FilterLines = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("PathToClient=") != -1)
                {
                    PathToClient = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("RunClientOnLaunch=") != -1)
                {
                    RunClientOnLaunch = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("CloseOnLogGenerated=") != -1)
                {
                    CloseOnLogGenerated = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("ServerAddress=") != -1)
                {
                    ServerAddress = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("ServerPassword=") != -1)
                {
                    ServerPassword = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("DM=") != -1)
                {
                    DM = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("ServerMode=") != -1)
                {
                    ServerMode = bool.Parse(ParameterValue);
                    Count += 1;
                }
                if (line.IndexOf("BackgroundColor=") != -1)
                {
                    BackgroundColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("TimestampColor=") != -1)
                {
                    TimestampColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("DefaultColor=") != -1)
                {
                    DefaultColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("ActorColor=") != -1)
                {
                    ActorColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("PartyColor=") != -1)
                {
                    PartyColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("EmoteColor=") != -1)
                {
                    EmoteColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("ShoutColor=") != -1)
                {
                    ShoutColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("TellColor=") != -1)
                {
                    TellColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("WhisperColor=") != -1)
                {
                    WhisperColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("MyColor=") != -1)
                {
                    MyColor = ParameterValue;
                    Count += 1;
                }
                if (line.IndexOf("MyCharacters=") != -1)
                {
                    MyCharacters = ParameterValue;
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
                                              MyCharacters
                                            );
            if(Count == 0)
            {
                MessageBox.Show("Default Configuration Loaded:\n\nPlease ensure NWNLogRotator.ini is properly formatted, and has " + _expectedSettingsCount + " parameters present.\n\nIf it is deleted, NWNLogRotator will create a new one automatically with the default settings.",
                                "Invalid Settings File!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return false;
            }
            else if (Count != _expectedSettingsCount)
            {
                MessageBox.Show("The application detected a change in configuration.\n\nPlease verify and save the new NWNLogRotator.ini configuration.",
                                "Save Settings!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
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

        public string FilePath_Get(Settings _run_settings)
        {
            string LogBasePath = _run_settings.OutputDirectory.Trim();

            if (!LogBasePath.EndsWith("/"))
            {
                LogBasePath += "\\";
            }

            // has a server listed
            string ServerName = "Server";
            if (_run_settings.ServerName != "")
            {
                ServerName = _run_settings.ServerName + "\\";
                LogBasePath += ServerName;
            }

            return LogBasePath;
        }

        public string FileNameGenerator_Get(DateTime _dateTime)
        {
            return "NWNLog_" + _dateTime.ToString("yyyy_MM_ddhhm") + ".html";
        }

        public string ReadNWNLogAndInvokeParser(Settings _run_settings)
        {
            string result;
            DateTime _dateTime = CurrentDateTime_Get();
            string filepath = FilePath_Get(_run_settings);
            string filename = FileNameGenerator_Get(_dateTime);
            string backupfilename = filename.Remove(filename.Length - 5, 5) + ".txt";
            bool hasenoughlines = false;

            FileStream fs;
            try
            {
                fs = new FileStream(_run_settings.PathToLog, FileMode.Open);
            }
            catch
            {
                MessageBox.Show("NWNLogRotator could not read the Log at PathToLog specified!",
                                "Invalid Log Location",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return "";
            }

            LogParser instance = new LogParser();
            result = instance.ParseLog(fs, _run_settings);
            fs.Close();

            // maintain minimum row lines requirement
            hasenoughlines = instance.LineCount_Get(result, _run_settings.MinimumRowsCount);
            if (hasenoughlines == false)
            {
                if (_run_settings.Silent == false)
                {
                    MessageBox.Show("This NWN Log did not meet the 'Minimum Rows' requirement. The specified log file was not saved!",
                                "Minimum Rows Information",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                }
                return "";
            }
            
            try
            {
                File.WriteAllText(filepath + filename, result);

                if (_run_settings.SaveBackup == true)
                {
                    string theFileToCopy = _run_settings.PathToLog;
                    string theDestinationFile = filepath + backupfilename;

                    File.Copy(theFileToCopy, theDestinationFile);
                }

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
                        try
                        {
                            Directory.CreateDirectory(filepath);
                        }
                        catch
                        {
                            MessageBox.Show("NWNLogRotator was not able create a folder or verify the folder structure of the Output Directory.",
                                            "Invalid Output Directory",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                            return "";
                        }
                        try
                        {
                            File.WriteAllText(filepath + filename, result);

                            if(_run_settings.SaveBackup == true)
                            {
                                string theFileToCopy = _run_settings.PathToLog;
                                string theDestinationFile = filepath + backupfilename;

                                File.Copy(theFileToCopy, theDestinationFile);
                            }

                            return filepath + filename;
                        }
                        catch
                        {
                            MessageBox.Show("NWNLogRotator was not able to write to the entered Output Directory. Please ensure the file structure exists.",
                                "Output Directory Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                }
            }
            return "";
        }
    }
}
