/*  
    *  Author:   © voc0der
    *  Project:  NWNLogRotator
    *  GitHub:   https://github.com/voc0der/NWNLogRotator
    *  Date:     3/3/2021
    *  License:  MIT
    *  Purpose:  This program is designed used alongside the Neverwinter Nights game, either Enhanced Edition, or 1.69.
    *  This program does not come with a warranty. Any support may be found on the GitHub page.
*/

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace NWNLogRotator.Classes
{
    class FileHandler
    {
        Settings _settings;
        int _expectedSettingsCount = 42;
        LogParser LogParserInstance = new LogParser();

        // Used only when no explicit process session bounds are provided (e.g. manual "Run Once")
        private static readonly TimeSpan FallbackSessionWindow = TimeSpan.FromMinutes(45);

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
                                        "SaveBackupOnly=" + _settings.SaveBackupOnly + "\n" +
                                        "SaveOnLaunch=" + _settings.SaveOnLaunch + "\n" +
                                        "Notifications=" + _settings.Notifications + "\n" +
                                        "OOCColor=" + _settings.OOCColor + "\n" +
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
                                        "MyCharacters=" + _settings.MyCharacters + "\n" +
                                        "FontName=" + _settings.FontName + "\n" +
                                        "FontSize=" + _settings.FontSize + "\n" +
                                        "CustomEmoteOne=" + _settings.CustomEmoteOne + "\n" +
                                        "CustomEmoteOneColor=" + _settings.CustomEmoteOneColor + "\n" +
                                        "CustomEmoteTwo=" + _settings.CustomEmoteTwo + "\n" +
                                        "CustomEmoteTwoColor=" + _settings.CustomEmoteTwoColor + "\n" +
                                        "CustomEmoteThree=" + _settings.CustomEmoteThree + "\n" +
                                        "CustomEmoteThreeColor=" + _settings.CustomEmoteThreeColor;

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
            string iniPath = Path.Combine(CurrentProgramDirectory_Get(), "NWNLogRotator.ini");

            string CurrentSettings = SettingsFile_Linter();

            File.WriteAllText(iniPath, CurrentSettings);

            ReadSettingsIni();
        }
        public bool ReadSettingsIni()
        {
            string iniPath = Path.Combine(CurrentProgramDirectory_Get(), "NWNLogRotator.ini");

            // interpret fields
            string OutputDirectory = @"C:\nwnlogs";
            string PathToLog = @"C:\nwnlogs\nwClientLog1.txt";
            int MinimumRowsToInteger = 10;
            string ServerName = "";
            string ServerNameColor = "EECC00";
            bool EventText = false;
            bool CombatText = false;
            string UseTheme = "";
            bool Silent = false;
            bool Tray = false;
            bool SaveBackup = false;
            bool SaveBackupOnly = false;
            bool SaveOnLaunch = false;
            bool Notifications = false;
            string OOCColor = "D70A53";
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
            string MyColor = "D6CEFD";
            string MyCharacters = "";
            string FontName = "Tahoma, Geneva, sans-serif";
            string FontSize = "calc(.7vw + .7vh + .5vmin)";
            string CustomEmoteOne = "";
            string CustomEmoteOneColor = "FF9944";
            string CustomEmoteTwo = "";
            string CustomEmoteTwoColor = "87CEEB";
            string CustomEmoteThree = "";
            string CustomEmoteThreeColor = "FFD8B1";

            int Count = 0;

            foreach (var line in File.ReadLines(iniPath))
            {
                string ParameterValue = line.Split('=').Last();

                if (line.IndexOf("OutputDirectory=") != -1)
                {
                    OutputDirectory = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("PathToLog=") != -1)
                {
                    PathToLog = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("MinimumRows=") != -1)
                {
                    MinimumRowsToInteger = int.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("ServerName=") != -1)
                {
                    ServerName = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("ServerNameColor=") != -1)
                {
                    ServerNameColor = ParameterValue;
                    if (ServerNameColor == "") ServerNameColor = _settings.ServerNameColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("EventText=") != -1)
                {
                    EventText = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("CombatText=") != -1)
                {
                    CombatText = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("UseTheme=") != -1)
                {
                    UseTheme = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("Silent=") != -1)
                {
                    Silent = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("Tray=") != -1)
                {
                    Tray = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("SaveBackup=") != -1)
                {
                    SaveBackup = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("SaveBackupOnly=") != -1)
                {
                    SaveBackupOnly = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("SaveOnLaunch=") != -1)
                {
                    SaveOnLaunch = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("Notifications=") != -1)
                {
                    Notifications = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("OOCColor=") != -1)
                {
                    OOCColor = ParameterValue;
                    if (OOCColor == "") OOCColor = _settings.OOCColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("FilterLines=") != -1)
                {
                    FilterLines = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("PathToClient=") != -1)
                {
                    PathToClient = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("RunClientOnLaunch=") != -1)
                {
                    RunClientOnLaunch = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("CloseOnLogGenerated=") != -1)
                {
                    CloseOnLogGenerated = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("ServerAddress=") != -1)
                {
                    ServerAddress = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("ServerPassword=") != -1)
                {
                    ServerPassword = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("DM=") != -1)
                {
                    DM = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("ServerMode=") != -1)
                {
                    ServerMode = bool.Parse(ParameterValue);
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("BackgroundColor=") != -1)
                {
                    BackgroundColor = ParameterValue;
                    if (BackgroundColor == "") BackgroundColor = _settings.BackgroundColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("TimestampColor=") != -1)
                {
                    TimestampColor = ParameterValue;
                    if (TimestampColor == "") TimestampColor = _settings.TimestampColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("DefaultColor=") != -1)
                {
                    DefaultColor = ParameterValue;
                    if (DefaultColor == "") DefaultColor = _settings.DefaultColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("ActorColor=") != -1)
                {
                    ActorColor = ParameterValue;
                    if (ActorColor == "") ActorColor = _settings.ActorColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("PartyColor=") != -1)
                {
                    PartyColor = ParameterValue;
                    if (PartyColor == "") PartyColor = _settings.PartyColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("EmoteColor=") != -1)
                {
                    EmoteColor = ParameterValue;
                    if (EmoteColor == "") EmoteColor = _settings.EmoteColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("ShoutColor=") != -1)
                {
                    ShoutColor = ParameterValue;
                    if (ShoutColor == "") ShoutColor = _settings.ShoutColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("TellColor=") != -1)
                {
                    TellColor = ParameterValue;
                    if (TellColor == "") TellColor = _settings.TellColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("WhisperColor=") != -1)
                {
                    WhisperColor = ParameterValue;
                    if (WhisperColor == "") WhisperColor = _settings.WhisperColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("MyColor=") != -1)
                {
                    MyColor = ParameterValue;
                    if (MyColor == "") MyColor = _settings.MyColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("MyCharacters=") != -1)
                {
                    MyCharacters = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("FontName=") != -1)
                {
                    FontName = ParameterValue;
                    if (FontName == "") FontName = _settings.FontName;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("FontSize=") != -1)
                {
                    FontSize = ParameterValue;
                    if (FontSize == "") FontSize = _settings.FontSize;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("CustomEmoteOne=") != -1)
                {
                    CustomEmoteOne = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("CustomEmoteOneColor=") != -1)
                {
                    CustomEmoteOneColor = ParameterValue;
                    if (CustomEmoteOneColor == "") CustomEmoteOneColor = _settings.CustomEmoteOneColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("CustomEmoteTwo=") != -1)
                {
                    CustomEmoteTwo = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("CustomEmoteTwoColor=") != -1)
                {
                    CustomEmoteTwoColor = ParameterValue;
                    if (CustomEmoteTwoColor == "") CustomEmoteTwoColor = _settings.CustomEmoteTwoColor;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("CustomEmoteThree=") != -1)
                {
                    CustomEmoteThree = ParameterValue;
                    Count += 1;
                    continue;
                }
                if (line.IndexOf("CustomEmoteThreeColor=") != -1)
                {
                    CustomEmoteThreeColor = ParameterValue;
                    if (CustomEmoteThreeColor == "") CustomEmoteThreeColor = _settings.CustomEmoteThreeColor;
                    Count += 1;
                    continue;
                }
                MessageBox.Show("NWNLogRotator detected that the settings file is outdated. Settings that are still valid have been loaded. Please verify and save the current settings.",
                                "Outdated Settings File!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
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
                                              SaveBackupOnly,
                                              SaveOnLaunch,
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
            if (Count == 0)
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

        private static bool EndsWithDirSep(string p) =>
            !string.IsNullOrEmpty(p) &&
            (p[p.Length - 1] == Path.DirectorySeparatorChar
            || p[p.Length - 1] == Path.AltDirectorySeparatorChar);


        public string FilePath_Get(Settings _run_settings)
        {
            var basePath = _run_settings.OutputDirectory?.Trim() ?? "";
            var p = string.IsNullOrWhiteSpace(_run_settings.ServerName)
                ? basePath
                : Path.Combine(basePath, _run_settings.ServerName);

            return EndsWithDirSep(p) ? p : p + Path.DirectorySeparatorChar;
        }

        public string FileNameGenerator_Get(DateTime _dateTime)
        {
            return "NWNLog_" + _dateTime.ToString("yyyy_MM_ddhhm") + ".html";
        }

        public string LogInputSignature_Get(string configuredPath, DateTime? sessionStartUtc = null, DateTime? sessionEndUtc = null)
        {
            var logPaths = ResolveSessionLogPaths(configuredPath, sessionStartUtc, sessionEndUtc);
            return BuildLogInputSignature(logPaths);
        }

        /// <summary>
        /// Main entry point for reading and parsing NWN logs.
        /// Now supports multi-file sessions where NWN EE rotates logs across nwclientLog1-4.txt
        /// </summary>
        public string ReadNWNLogAndInvokeParser(Settings _run_settings, DateTime? sessionStartUtc = null, DateTime? sessionEndUtc = null)
        {
            // Get all session log files (may be multiple if session spanned log rotation)
            var sessionLogPaths = ResolveSessionLogPaths(_run_settings.PathToLog, sessionStartUtc, sessionEndUtc);
            
            // Wait for the most recent file to stabilize
            if (sessionLogPaths.Length > 0)
            {
                WaitForStableFile(sessionLogPaths[sessionLogPaths.Length - 1]);
            }

            // Re-resolve after stabilization to avoid stale file selection near rollover boundaries
            sessionLogPaths = ResolveSessionLogPaths(_run_settings.PathToLog, sessionStartUtc, sessionEndUtc);

            // If explicit session bounds were provided and no files match, do not fall back to older logs.
            if (sessionLogPaths.Length == 0 && sessionStartUtc.HasValue)
            {
                return "";
            }

            DateTime _dateTime = CurrentDateTime_Get();
            string filepath = FilePath_Get(_run_settings);
            string filename = FileNameGenerator_Get(_dateTime);
            string backupfilename = filename.Remove(filename.Length - 5, 5) + ".txt";
            bool hasenoughlines = false;
            int skippedReadFiles = 0;

            string result;
            try
            {
                // Concatenate all session logs and parse as one
                result = ParseConcatenatedLogs(sessionLogPaths, _run_settings, out skippedReadFiles);
            }
            catch (Exception ex)
            {
                // Fallback: try single file approach for non-EE setups
                try
                {
                    var singlePath = ResolveLatestLogPath(_run_settings.PathToLog);
                    using (var fs = new FileStream(
                        singlePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite | FileShare.Delete,
                        4096,
                        FileOptions.SequentialScan))
                    {
                        result = LogParserInstance.ParseLog(fs, _run_settings);
                    }
                }
                catch
                {
                    MessageBox.Show("NWNLogRotator could not read the Log at PathToLog specified!\n\nError: " + ex.Message,
                                    "Invalid Log Location",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return "";
                }
            }

            if (skippedReadFiles > 0 && _run_settings.Silent == false)
            {
                MessageBox.Show("NWNLogRotator skipped " + skippedReadFiles + " log file(s) while reading this session. Export may be incomplete.",
                                "Partial Session Warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }

            // maintain minimum row lines requirement
            hasenoughlines = LogParserInstance.LineCount_Get(result, _run_settings.MinimumRowsCount);
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
                if (_run_settings.SaveBackupOnly == false)
                    File.WriteAllText(Path.Combine(filepath, filename), result);

                if (_run_settings.SaveBackup == true)
                {
                    // Save concatenated backup of all session logs
                    var theDestinationFile = Path.Combine(filepath, backupfilename);
                    SaveConcatenatedBackup(sessionLogPaths, theDestinationFile);
                }
                return Path.Combine(filepath, filename);
            }
            catch
            {
                string displayDest = Path.Combine(filepath, filename);
                MessageBoxResult _messageBoxResult = MessageBox.Show("The destination file " + displayDest + " is unable to be written to this folder. Would you like NWNLogRotator to try and create the destination folder?",
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
                            File.WriteAllText(Path.Combine(filepath, filename), result);

                            if (_run_settings.SaveBackup == true)
                            {
                                string theDestinationFile = Path.Combine(filepath, backupfilename);
                                SaveConcatenatedBackup(sessionLogPaths, theDestinationFile);
                            }

                            return Path.Combine(filepath, filename);
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

        /// <summary>
        /// Resolves all nwclientLog[1-4].txt files that belong to the current gaming session.
        /// If session bounds are provided, files are selected strictly within that process session.
        /// Otherwise, a narrow fallback window around the newest log is used for manual workflows.
        /// Returns files in chronological order (oldest first) for proper concatenation.
        /// </summary>
        private static string[] ResolveSessionLogPaths(string configuredPath, DateTime? sessionStartUtc = null, DateTime? sessionEndUtc = null)
        {
            try
            {
                var dir = Path.GetDirectoryName(configuredPath);
                if (string.IsNullOrEmpty(dir))
                    return new[] { configuredPath };

                var baseName = Path.GetFileName(configuredPath) ?? string.Empty;

                // Check if this looks like an EE log pattern
                if (!Regex.IsMatch(baseName, @"^nwclientlog\d*\.txt$", RegexOptions.IgnoreCase))
                {
                    // Non-EE setup, just return the configured path
                    return File.Exists(configuredPath) ? new[] { configuredPath } : new string[0];
                }

                // Find the most recent log file to establish session end time
                var allLogs = Directory.EnumerateFiles(dir, "nwclientLog*.txt", SearchOption.TopDirectoryOnly)
                    .Where(p => Regex.IsMatch(Path.GetFileName(p), @"^nwclientlog[1-4]\.txt$", RegexOptions.IgnoreCase))
                    .Select(p => new FileInfo(p))
                    .Where(fi => fi.Exists && fi.Length > 0)
                    .ToList();

                if (allLogs.Count == 0)
                    return File.Exists(configuredPath) ? new[] { configuredPath } : new string[0];

                if (sessionStartUtc.HasValue)
                {
                    var startUtc = sessionStartUtc.Value.AddSeconds(-2);
                    var endUtc = (sessionEndUtc ?? DateTime.UtcNow).AddMinutes(1);
                    var boundedLogs = allLogs
                        .Where(fi => fi.LastWriteTimeUtc >= startUtc && fi.LastWriteTimeUtc <= endUtc)
                        .OrderBy(fi => fi.LastWriteTimeUtc)
                        .ThenBy(fi => fi.Name)
                        .Select(fi => fi.FullName)
                        .ToArray();

                    if (boundedLogs.Length > 0)
                        return boundedLogs;

                    // Explicit session bounds were provided; do not fall back to broad windows
                    return new string[0];
                }

                // Fallback behavior for manual invocations without explicit session boundaries
                var newestLogTime = allLogs.Max(fi => fi.LastWriteTimeUtc);
                var sessionCutoff = newestLogTime - FallbackSessionWindow;

                var sessionLogs = allLogs
                    .Where(fi => fi.LastWriteTimeUtc >= sessionCutoff)
                    .OrderBy(fi => fi.LastWriteTimeUtc)      // Chronological order (oldest first)
                    .ThenBy(fi => fi.Name)                    // Tie-breaker: by name
                    .Select(fi => fi.FullName)
                    .ToArray();

                return sessionLogs.Length > 0 ? sessionLogs : new[] { configuredPath };
            }
            catch
            {
                return File.Exists(configuredPath) ? new[] { configuredPath } : new string[0];
            }
        }

        private static string BuildLogInputSignature(string[] logPaths)
        {
            if (logPaths == null || logPaths.Length == 0)
                return "";

            var sb = new StringBuilder();
            foreach (var logPath in logPaths)
            {
                try
                {
                    var fi = new FileInfo(logPath);
                    if (!fi.Exists)
                        continue;

                    sb.Append(fi.FullName.ToLowerInvariant())
                      .Append('|')
                      .Append(fi.Length)
                      .Append('|')
                      .Append(fi.LastWriteTimeUtc.Ticks)
                      .Append(';');
                }
                catch
                {
                    // Ignore unreadable files while generating a best-effort signature
                    continue;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Parses multiple log files as a single concatenated stream.
        /// Files are read in order and combined before parsing.
        /// </summary>
        private string ParseConcatenatedLogs(string[] logPaths, Settings _run_settings, out int skippedFiles)
        {
            skippedFiles = 0;

            if (logPaths == null || logPaths.Length == 0)
                throw new ArgumentException("No log files to parse");

            // For a single file, use the original approach
            if (logPaths.Length == 1)
            {
                using (var fs = new FileStream(
                    logPaths[0],
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite | FileShare.Delete,
                    4096,
                    FileOptions.SequentialScan))
                {
                    return LogParserInstance.ParseLog(fs, _run_settings);
                }
            }

            // For multiple files, concatenate into a MemoryStream
            using (var combinedStream = new MemoryStream())
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                
                foreach (var logPath in logPaths)
                {
                    if (!File.Exists(logPath))
                        continue;

                    try
                    {
                        using (var fs = new FileStream(
                            logPath,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.ReadWrite | FileShare.Delete,
                            4096,
                            FileOptions.SequentialScan))
                        {
                            fs.CopyTo(combinedStream);
                        }

                        // Ensure there's a newline between concatenated files
                        var newline = encoding.GetBytes("\n");
                        combinedStream.Write(newline, 0, newline.Length);
                    }
                    catch
                    {
                        skippedFiles++;
                        continue;
                    }
                }

                if (combinedStream.Length == 0)
                    throw new InvalidOperationException("No log content could be read");

                // Reset stream position for reading
                combinedStream.Position = 0;

                return LogParserInstance.ParseLog(combinedStream, _run_settings);
            }
        }

        /// <summary>
        /// Saves a concatenated backup of all session log files.
        /// </summary>
        private static void SaveConcatenatedBackup(string[] logPaths, string destinationPath)
        {
            if (logPaths == null || logPaths.Length == 0)
                return;

            // For a single file, just copy it
            if (logPaths.Length == 1)
            {
                File.Copy(logPaths[0], destinationPath, overwrite: true);
                return;
            }

            // For multiple files, concatenate them
            var encoding = Encoding.GetEncoding("iso-8859-1");
            using (var destStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
            {
                bool isFirst = true;
                foreach (var logPath in logPaths)
                {
                    if (!File.Exists(logPath))
                        continue;

                    try
                    {
                        // Add separator between files (except before the first one)
                        if (!isFirst)
                        {
                            var separator = encoding.GetBytes("\n\n--- Log file continued ---\n\n");
                            destStream.Write(separator, 0, separator.Length);
                        }
                        isFirst = false;

                        using (var srcStream = new FileStream(
                            logPath,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.ReadWrite | FileShare.Delete))
                        {
                            srcStream.CopyTo(destStream);
                        }
                    }
                    catch
                    {
                        // Skip files that can't be read
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Legacy method: Returns the single most recently modified nwclientLog file.
        /// Kept for backward compatibility with non-EE setups.
        /// </summary>
        private static string ResolveLatestLogPath(string configuredPath)
        {
            try
            {
                var dir = Path.GetDirectoryName(configuredPath);
                if (string.IsNullOrEmpty(dir)) return configuredPath;
                var baseName = Path.GetFileName(configuredPath) ?? string.Empty;

                var candidates = new List<string>();
                if (File.Exists(configuredPath)) candidates.Add(configuredPath);

                // If it looks like an EE client log, consider 1..4
                if (Regex.IsMatch(baseName, @"^nwclientlog\d+\.txt$", RegexOptions.IgnoreCase))
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        var p = Path.Combine(dir, $"nwclientLog{i}.txt");
                        if (File.Exists(p) && !candidates.Contains(p, StringComparer.OrdinalIgnoreCase))
                            candidates.Add(p);
                    }
                }

                if (candidates.Count == 0) return configuredPath; // non-paginated setups still work

                return candidates
                    .Select(p => new FileInfo(p))
                    .OrderByDescending(fi => fi.LastWriteTimeUtc)
                    .ThenByDescending(fi => fi.Length)
                    .First()
                    .FullName;
            }
            catch { return configuredPath; }
        }

        /// <summary>
        /// Legacy method: Return the second-newest nwclientLog[1-4].txt beside configuredPath.
        /// Kept for backward compatibility but largely superseded by ResolveSessionLogPaths.
        /// </summary>
        private static string SecondNewestEELog(string configuredPath)
        {
            try
            {
                var dir = Path.GetDirectoryName(configuredPath);
                if (string.IsNullOrEmpty(dir)) return configuredPath;

                var files = Directory.EnumerateFiles(dir, "nwclientLog*.txt", SearchOption.TopDirectoryOnly)
                    .Where(p => Regex.IsMatch(Path.GetFileName(p), @"^nwclientlog[1-4]\.txt$", RegexOptions.IgnoreCase))
                    .Select(p => new FileInfo(p))
                    .OrderByDescending(fi => fi.LastWriteTimeUtc)
                    .ThenByDescending(fi => fi.Length)
                    .Select(fi => fi.FullName)
                    .ToArray();

                return files.Length >= 2 ? files[1] : configuredPath;
            }
            catch { return configuredPath; }
        }

        private static void WaitForStableFile(string path, int settleMs = 1200, int timeoutMs = 15000)
        {
            long lastLen = -1;
            DateTime lastWrite = DateTime.MinValue;
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                var fi = new FileInfo(path);
                if (!fi.Exists) { Thread.Sleep(200); continue; }

                if (fi.Length == lastLen && fi.LastWriteTimeUtc == lastWrite)
                {
                    Thread.Sleep(settleMs);
                    var fi2 = new FileInfo(path);
                    if (fi2.Length == lastLen && fi2.LastWriteTimeUtc == lastWrite)
                        return; // stable
                }

                lastLen = fi.Length;
                lastWrite = fi.LastWriteTimeUtc;
                Thread.Sleep(200);
            }
        }
    }
}
