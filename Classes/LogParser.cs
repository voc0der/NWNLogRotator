using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NWNLogRotator.Classes
{
    class LogParser
    {
        public string ParseLog(Stream inputStream, Settings _run_settings)
        {
            var removeExps = new List<Regex>();

            string[] filterLinesArray = _run_settings.FilterLines.Split(',');
            if (!_run_settings.EventText && _run_settings.FilterLines != "")
            {
                eventLines = new List<String>();
                foreach (string eventString in filterLinesArray)
                {
                    string theEventString = eventString.Trim();
                    eventLines.Add(theEventString);
                }
                _run_settings.EventText = true;
            }
            else if (_run_settings.EventText && _run_settings.FilterLines != "")
            {
                List<String> eventLinesTemp = new List<String>();
                foreach (string theEvent in filterLinesArray)
                {
                    eventLinesTemp.Add(theEvent);
                }
                eventLines.AddRange(eventLinesTemp);
            }

            string text;
            using (var reader = new StreamReader(inputStream, Encoding.GetEncoding("iso-8859-1")))
            {
                if (_run_settings.EventText)
                {
                    StringBuilder cleanTextBuilder = new StringBuilder();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!eventLines.Any(x => line.Contains(x)))
                            cleanTextBuilder.AppendLine(line);
                    }
                    text = cleanTextBuilder.ToString();
                }
                else
                    text = reader.ReadToEnd();
            }

            foreach (var exp in gloabalRemoves)
                text = text.Replace(exp, "");

            // Garbage needs original formatting to be recognized; e.g. in Arelith's case.
            foreach (var exp in garbageLines)
                text = exp.Replace(text, "");

            foreach (var exp in gloabalRemovesTwo)
                text = text.Replace(exp, "");

            char[] newRow = { '\n' };
            string[] textAsList = text.Split(newRow);

            if (_run_settings.CombatText) removeExps.AddRange(combatLines);

            formatReplacesOrdered = formatReplacesWithUserOverride(_run_settings);

            var processedLines = textAsList.AsParallel().Select(line =>
            {
                var lineText = line;
                if (string.IsNullOrWhiteSpace(lineText))
                    return null;

                if (removeExps.Any(x => x.IsMatch(lineText)))
                    return null;

                if (_run_settings.ServerMode == true)
                {
                    foreach (var exp in serverReplacesOrdered)
                        lineText = exp.Item1.Replace(lineText, exp.Item2);
                }

                foreach (var exp in formatReplacesOrdered)
                    lineText = exp.Item1.Replace(lineText, exp.Item2);

                return lineText;
            });

            var parsedText = "";
            try
            {
                parsedText = processedLines.Where(x => x != null).Aggregate((x, y) => x + "<br />" + y);
            }
            catch
            {
                parsedText = "";
            }

            // Stateful crafting parsing if neccessary
            Match HasCrafting = Regex.Match(text, @"^" + timestampExactMatch + @"\[Applying\scrafting\seffects\]\s*?$", RegexOptions.Multiline);
            if (HasCrafting.Length > 0)
            {
                foreach (var exp in craftingLines)
                    parsedText = exp.Replace(parsedText, "");
            }

            text = HTMLPackageLog_Get(parsedText, _run_settings);
            return text;
        }
        public bool LineCount_Get(string ParsedNWNLog, int MinimumRowsCount)
        {
            MatchCollection LineCount = Regex.Matches(ParsedNWNLog, @"\<br\s\/>");

            if (LineCount.Count >= MinimumRowsCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string HTMLPackageLog_Get(string ParsedNWNLog, Settings _run_settings)
        {
            string ServerNameColor = "";
            if (_run_settings.ServerNameColor == "")
            {
                ServerNameColor = "#" + _run_settings.DefaultColor;
            }
            else
            {
                ServerNameColor = "#" + _run_settings.ServerNameColor;
            }

            string OptionalCSSMediaTag = "";
            if (_run_settings.FontSize.Contains("vw") ||
                _run_settings.FontSize.Contains("vh") ||
                _run_settings.FontSize.Contains("vmin") ||
                _run_settings.FontSize.Contains("vmax"))
            {
                OptionalCSSMediaTag = "@media screen and (min-width: 1000px) {" +
                                            ".log-body { font-size: unset !important; }" +
                                       "}";
            }

            string HTMLHeader = "<html>" +
                "<head>" +
                     @"<meta charset=""utf-8"">" +
                     "<style>" +
                        OptionalCSSMediaTag +
                        ".log-body { " +
                            "background-color: #" + _run_settings.BackgroundColor + ";" +
                            "font-family: " + _run_settings.FontName + ";" +
                            "font-size: " + _run_settings.FontSize + ";" +
                            "color: #" + _run_settings.DefaultColor + ";" +
                        "}" +
                        ".log-header { color: " + ServerNameColor + " }" +
                        ".default { color: #" + _run_settings.DefaultColor + "; }" +
                        ".timestamp { color: #" + _run_settings.TimestampColor + "; }" +
                        ".me { color: #" + _run_settings.MyColor + "; }" +
                        ".actors { color: #" + _run_settings.ActorColor + "; }" +
                        ".shouts { color: #" + _run_settings.ShoutColor + "; }" +
                        ".tells { color: #" + _run_settings.TellColor + "; }" +
                        ".whispers { color: #" + _run_settings.WhisperColor + "; }" +
                        ".party { color: #" + _run_settings.PartyColor + "; }" +
                        ".emotes { color: #" + _run_settings.EmoteColor + "; }" +
                        ".custom-emotes-one { color: #" + _run_settings.CustomEmoteOneColor + "; }" +
                        ".custom-emotes-two { color: #" + _run_settings.CustomEmoteTwoColor + "; }" +
                        ".custom-emotes-three { color: #" + _run_settings.CustomEmoteThreeColor + "; }" +
                        ".ooc { color: #" + _run_settings.OOCColor + "; }" +
                    "</style>" +
                "</head>";

            string HTMLLogHeader;
            if (_run_settings.ServerName != "")
            {
                HTMLLogHeader = @"<h4>[<span class=""log-header"">" + _run_settings.ServerName + " Log</span>] ";
            }
            else
            {
                HTMLLogHeader = @"<h4>[<span class=""log-header"">Log</span>] ";
            }
            HTMLLogHeader += @"<span class=""actors"">Date/Time</span>: " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            HTMLLogHeader += "</h4>";
            HTMLLogHeader += @"<body class=""log-body""><span class=""default"">";
            string HTMLPostLogFooter = "</span></body></html>";
            return HTMLHeader + HTMLLogHeader + ParsedNWNLog + HTMLPostLogFooter;
        }

        private List<Tuple<Regex, string>> formatReplacesWithUserOverride(Settings _run_settings)
        {
            string theRegEx;
            List<Tuple<Regex, string>> formatReplacesOrderedReturn = new List<Tuple<Regex, string>>();

            formatReplacesOrderedReturn.AddRange(formatReplacesOrderedOne);
            formatReplacesOrderedReturn.AddRange(formatReplacesOrderedTwo);
            formatReplacesOrderedReturn.AddRange(formatReplacesOrderedThree);

            // Process custom emotes
            List<Tuple<Regex, string>> additionalEmotesList = new List<Tuple<Regex, string>>();
            Tuple<Regex, string> theCustomEmote;

            // OOC emotes
            if (_run_settings.OOCColor.Length != 0)
            {
                theRegEx = "(" + timestampStatefulMatch + nameStatefulMatch + @"\s*?)(\/\/.*)";

                theCustomEmote = new Tuple<Regex, string>(new Regex(@"" + theRegEx, RegexOptions.Compiled), @"<span class=""ooc"">$1$3</span>");
                additionalEmotesList.Add(theCustomEmote);

                formatReplacesOrderedReturn.AddRange(additionalEmotesList);
            }

            // My characters
            if (_run_settings.MyCharacters != "")
            {
                List<Tuple<Regex, string>> MyCharacterLines = new List<Tuple<Regex, string>>();
                theRegEx = "";
                string[] MyCharacters = _run_settings.MyCharacters.Split(',');
                foreach (string CharacterName in MyCharacters)
                {
                    theRegEx = @"(<span class=""actors"">)(\s*?" + CharacterName + @":?.*?)(</span>|\[Whisper\]|\[Tell\]|\[Shout\])";
                    Tuple<Regex, string> theMyCharacterLine = new Tuple<Regex, string>(new Regex(theRegEx, RegexOptions.Compiled), @"$1 <span class=""me"">$2</span> $3");
                    MyCharacterLines.Add(theMyCharacterLine);
                }
                formatReplacesOrderedReturn.AddRange(MyCharacterLines);
            }

            if (_run_settings.CustomEmoteOne.Length != 0 || _run_settings.CustomEmoteTwo.Length != 0 || _run_settings.CustomEmoteThree.Length != 0 || _run_settings.OOCColor.Length != 0)
            {
                string theRegExEscapeCharacter = "\\";
                if (_run_settings.CustomEmoteOne.Length != 0)
                {
                    // Custom emote 1
                    if (_run_settings.CustomEmoteOne.Length == 2)
                    {
                        string tempLeftBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteOne.Substring(0, 1);
                        string tempRightBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteOne.Substring(1, 1);
                        theRegEx = "(" + tempLeftBracket + textStatefulMatch + tempRightBracket + ")";

                        theCustomEmote = new Tuple<Regex, string>(new Regex(theRegEx, RegexOptions.Compiled), @"<span class=""custom-emotes-one"">$1</span>");
                        additionalEmotesList.Add(theCustomEmote);
                    }
                    else if (_run_settings.CustomEmoteOne.Length == 1)
                    {
                        string tempBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteOne.Substring(0, 1);
                        theRegEx = "(" + tempBracket + textStatefulMatch + tempBracket + ")";

                        theCustomEmote = new Tuple<Regex, string>(new Regex(theRegEx, RegexOptions.Compiled), @"<span class=""custom-emotes-one"">$1</span>");
                        additionalEmotesList.Add(theCustomEmote);
                    }
                    formatReplacesOrderedReturn.AddRange(additionalEmotesList);
                }
                // Custom emote 2
                if (_run_settings.CustomEmoteTwo.Length != 0)
                {
                    if (_run_settings.CustomEmoteTwo.Length == 2)
                    {
                        string tempLeftBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteTwo.Substring(0, 1);
                        string tempRightBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteTwo.Substring(1, 1);
                        theRegEx = "(" + tempLeftBracket + textStatefulMatch + tempRightBracket + ")";

                        theCustomEmote = new Tuple<Regex, string>(new Regex(theRegEx, RegexOptions.Compiled), @"<span class=""custom-emotes-two"">$1</span>");
                        additionalEmotesList.Add(theCustomEmote);
                    }
                    else if (_run_settings.CustomEmoteTwo.Length == 1)
                    {
                        string tempBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteTwo.Substring(0, 1);
                        theRegEx = "(" + tempBracket + textStatefulMatch + tempBracket + ")";

                        theCustomEmote = new Tuple<Regex, string>(new Regex(theRegEx, RegexOptions.Compiled), @"<span class=""custom-emotes-two"">$1</span>");
                        additionalEmotesList.Add(theCustomEmote);
                    }
                    formatReplacesOrderedReturn.AddRange(additionalEmotesList);
                }
                // Custom emote 3
                if (_run_settings.CustomEmoteThree.Length != 0)
                {
                    if (_run_settings.CustomEmoteThree.Length == 2)
                    {
                        string tempLeftBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteThree.Substring(0, 1);
                        string tempRightBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteThree.Substring(1, 1);
                        theRegEx = "(" + tempLeftBracket + textStatefulMatch + tempRightBracket + ")";

                        theCustomEmote = new Tuple<Regex, string>(new Regex(theRegEx, RegexOptions.Compiled), @"<span class=""custom-emotes-three"">$1</span>");
                        additionalEmotesList.Add(theCustomEmote);
                    }
                    else if (_run_settings.CustomEmoteThree.Length == 1)
                    {
                        string tempBracket = theRegExEscapeCharacter + _run_settings.CustomEmoteThree.Substring(0, 1);
                        theRegEx = "(" + tempBracket + textStatefulMatch + tempBracket + ")";

                        theCustomEmote = new Tuple<Regex, string>(new Regex(theRegEx, RegexOptions.Compiled), @"<span class=""custom-emotes-three"">$1</span>");
                        additionalEmotesList.Add(theCustomEmote);
                    }
                    formatReplacesOrderedReturn.AddRange(additionalEmotesList);
                }
            }

            // Format fixing for Arelith style language-voice types
            List<Tuple<Regex, string>> additionalFixes = new List<Tuple<Regex, string>>();
            theRegEx = @"(<span class=\\?""actors\\?"">" + nameMatch + @".*?)(\[(?:Whisper|Shout|Tell)\])(.*?\[[A-z0-9]+\].*<\/span>)(.*)";
            Tuple<Regex, string> theCustomLanguageScope = new Tuple<Regex, string>(new Regex(theRegEx, RegexOptions.Compiled), @"$1<span class=""whispers"">$2</span>$3<span class=""whispers"">$4</span>");
            additionalFixes.Add(theCustomLanguageScope);
            formatReplacesOrderedReturn.AddRange(additionalFixes);

            return formatReplacesOrderedReturn;
        }

        private List<Tuple<Regex, string>> formatReplacesOrdered = new List<Tuple<Regex, string>>();

        private List<Tuple<Regex, string>> formatReplacesOrderedOne = new List<Tuple<Regex, string>>
        {
            new Tuple<Regex, string> ( new Regex(@"\[\w{3}\s\w{3}\s*?\d{1,}\s", RegexOptions.Compiled), @"<span class=""timestamp"">[" ),
            new Tuple<Regex, string> ( new Regex(@"\:{1}[0-9]*]{1}",RegexOptions.Compiled), "]</span>" ),
        };

        private List<Tuple<Regex, string>> formatReplacesOrderedTwo = new List<Tuple<Regex, string>>
        {
            // actors
            new Tuple<Regex, string>( new Regex(@"\]<\/span>((...).*: )",RegexOptions.Compiled), @"]</span><span class=""actors"">$1</span>" ),
            // shouts
            new Tuple<Regex, string>( new Regex(@":\s?<\/span>\s?(\[Shout])(.*)",RegexOptions.Compiled), @"</span><span class=""shouts""> $1:$2</span>"),
            // tells
            new Tuple<Regex, string>( new Regex(@":\s?<\/span>\s?(\[Tell])(.*)",RegexOptions.Compiled), @"</span><span class=""tells""> $1:$2</span>"),
            // whispers 
            new Tuple<Regex, string>( new Regex(@":\s?<\/span>\s?(\[Whisper])(.*)",RegexOptions.Compiled), @"</span><span class=""whispers""> $1:$2</span>"),
            // party
            new Tuple<Regex, string>( new Regex(@":\s?<\/span>\s?(\[Party])(.*)",RegexOptions.Compiled), @"</span><span class=""party""> $1:$2</span>"),
        };

        private List<Tuple<Regex, string>> formatReplacesOrderedThree = new List<Tuple<Regex, string>>
        {
            // emotes 
            new Tuple<Regex, string>( new Regex(@"(\*.*?\*)",RegexOptions.Compiled), @"<span class=""emotes"">$1</span>"),
        };

        private List<String> gloabalRemoves = new List<String>
        {
            "\r",
        };

        private List<String> gloabalRemovesTwo = new List<String>
        {
            "[CHAT WINDOW TEXT] ",
        };

        private List<string> eventLines = new List<string>
        {
            "[Event]",
            "[Server]",
            "Minimum Tumble AC Bonus:",
            "No Monk/Shield AC Bonus:",
            "You are light sensitive!",
            "has left as a player..",
            "has joined as a player..",
            "has left as a game master..",
            "has joined as a game master..",
            "You are now in a Party PVP area.",
            "You are now in a No PVP area.",
            "Resting.",
            "Cancelled Rest.",
            "You used a key.",
            "Equipped item swapped out.",
            "You are portalling, you can't do anything right now",
            "Unknown Speaker: You are being moved to your last location, please wait...",
            "You are entering a new area!",
            "This container is persistent.",
            "This container is full",
            "You are too busy to barter now",
            "Player not found",
            "You cannot carry any more items, your inventory is full",
            "This is a trash, its contents may be purged at anytime",
            "Armor/Shield Applies: Skill ",
            "Your character has been saved",
            "New Value:",
            "Quick bar",
            "[ERROR TOO MANY INSTRUCTIONS]",
            "*** ValidateGFFResource sent by user.",
            "Modifying colours doesn't cost gold.",
            "Ignore the crafting roll and gold message for robes.",
            "[NB: this lootbag will be deleted in 2 minutes!]",
            "Unknown Update sub-message",
            "You have multiple items equipped or spell effects active that give an Armor AC bonus and the effects will not stack.",
            "Top Down Camera Activated",
            "Driving Camera Activated",
            "Your journal has been updated.",
            "First login after reset or relog after 1 minute",
            "Your public CDKEY is ",
        };

        private static string lineStartMatch = @"(?<=\n|^)";
        private static string lineChatStartMatch = @"\[CHAT\sWINDOW\sTEXT\]\s";
        private static string timestampMatch = @"^.+?(?=.*)";
        private static string timestampExactMatch = @"\[\w{3}\s\w{3}\s*?\d{1,}\s\d{2}\:\d{2}\:\d{2}\]\s";
        private static string timestampStatefulMatch = @"\<span\sclass\=\""timestamp\""\>\[\d+\:\d+\]\<\/span\>\s*?(\<span\sclass\=\""actors\""\>\s)?";
        private static string nameMatch = @"[A-z0-9\s\.\']+";
        private static string nameStatefulMatch = @"[A-z0-9\s\.\']+\:\s\<\/span\>";
        private static string textStatefulMatch = @"(?!([0-9]{2}\:[0-9]{2}|Whisper|Tell|Party|Shout)).*?";

        private List<Regex> combatLines = new List<Regex>
        {
            new Regex(timestampMatch+@"\*{1}hit\*{1}.*\s\:\s\(\d{1,}\s[+-]\s\d{1,}\s\=\s\d{1,}.*\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"damages\s.*\:\s{1}\d{1,}\s{1}\({1}\d{1,}\s{1}.*\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\*{1}parried\*{1}.*\({1}\d{1,}.*\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}[a-zA-Z]*\:{1}\s{1}Damage\s{1}[a-zA-Z]*\s{1}absorbs\s{1}.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\*{1}target concealed\:{1}.*\:{1}\s{1}\({1}\d{1,}.*\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\*{1}critical hit\*\s{1}\:{1}\s{1}\({1}\d{1,}.*\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\*{1}resisted\*\s{1}\:{1}\s{1}\({1}\d{1,}.*\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"Immune\s{1}to\s{1}Critical\s{1}Hits\.{1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\*{1}miss\*{1}.*\s\:\s\(\d{1,}\s{1}.*\d{1,}\s\=\s\d{1,}\)$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\*{1}success\*{1}\s{1}\:{1}\s{1}\(\d{1,}.*\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\*{1}failure\*{1}.*\s\:\s{1}\({1}.*\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\:\s{1}Initiative\s{1}Roll\s{1}\:\s\d{1,}\s\:\s\(\d{1,}\s[+-]\s{1}\d{1,}\s{1}\={1}\s{1}\d{1,}\){1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\:{1}\s{1}Damage Immunity\s{1}absorbs.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\:{1}\s{1}Immune to Sneak Attacks\.{1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\:{1}\s{1}Immune to Negative Levels\.{1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\:{1}\s{1}Spell Level Absorption absorbs\s{1}\d{1,}.*\:{1}\s{1}\d{1,}.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}[a-zA-Z]*cast.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}[a-zA-Z]*uses.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}[a-zA-Z]*enables.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"[a-zA-Z]*\s{1}attempts\s{1}to\s{1}.*\:\s{1}.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"[a-zA-Z]*\:{1}\s{1}Healed\s{1}\d{1,}\s{1}hit.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"[a-zA-Z]*\:{1}\sImmune to [a-zA-Z]*.*\.{1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}Dispel\s{1}Magic\s{1}\:{1}\s{1}[a-zA-z]*.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}Experience Points Gained\:{1}\s{1,}\d{1,}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"There are signs of recent fighting here...\*{1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"Stale temporary properties detected, cleaning item\.{1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}\[Check for loot\:{1}\s{1}\d{1,}.*\]{1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}You.{1}ve reached your maximum level.\s{1}.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}Devastating Critical Hit!", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1,}Done resting\.{1}.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1,}You triggered a Trap!{1}.*$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}You cannot target a creature you cannot see or do not have a line of sight to\.{1}$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}Weapon equipped as a one-handed weapon.$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}You cannot rest so soon after exerting yourself.$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}Equipping this armor has disabled your monk abilities.$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s{1}No resting is allowed in this area.$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\s[A-z\s]*?enters rage.$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"Overhealed\s\d+\shit\spoints\.$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"\+\d+\sXP$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"Adventuring\sBonus\:\sAdventure\sMode\s\(\+\d+\sdelayed\sXP\)$", RegexOptions.Compiled),
            new Regex(timestampMatch+@".*?\s\:\sEpic\sDodge\s*?\:\sAttack\sevaded$", RegexOptions.Compiled),
            new Regex(timestampMatch+@"You\srested\s\d+\%\sYou\shave\sto\swait\s\d+\sminutes\sbefore\syou\scan\srest\sagain\.$", RegexOptions.Compiled),
        };

        private List<Regex> garbageLines = new List<Regex>
        {
            new Regex(lineStartMatch+nameMatch+@"\:\s\[(?:Talk|Shout|Whisper|Tell)\]\s.*?\n", RegexOptions.Compiled),
            new Regex(lineStartMatch+@"\[(?!CHAT)"+nameMatch+@"\]?\s?.*?\:\s\[(?:Talk|Shout|Whisper|Tell)\]\s.*?\n", RegexOptions.Compiled),
            new Regex(lineStartMatch+lineChatStartMatch+timestampExactMatch+@"\[(?:Talk|Shout|Whisper|Tell)\]\s.*?\n", RegexOptions.Compiled),
            new Regex(lineStartMatch+lineChatStartMatch+timestampExactMatch+@"(Progression\sby\sroleplay\.\.\.|Experience gained\.)\n", RegexOptions.Compiled),
            new Regex(lineStartMatch+lineChatStartMatch+timestampExactMatch+@"Script\s.*?\,\sOID\:\s[A-z\d]+\,\sTag\:\s.*?\,\sERROR\:\sTOO\sMANY\sINSTRUCTIONS\n", RegexOptions.Compiled),
            new Regex(lineStartMatch+@"nwsync\:\s(reconfigured|Migrations|Shard|Storage).*?\n"),
            new Regex(lineStartMatch+@"Game\s?is\s?using\s?local\s?port\:\s.*?\n"),
            new Regex(lineStartMatch+@"GOG\:\s?(Authentication|Error).*?\n"),
            new Regex(lineStartMatch+@"Error\:\s\d+\n"),
        };

        private List<Regex> craftingLines = new List<Regex>
        {
            new Regex(timestampStatefulMatch+@"\[(?:Applying|Removing) crafting effects\]\s*?<br />(?<=<br />)"),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"Jump\s\d+\s(?:part|colou?r)s\s(?:forward|backward)<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"Select\sthe\scolou?r\stype\sthat\syou\swant\sto\schange\.<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+@"Lost\sItem\:\s\<\/span\>[A-z\s\(\)\d]+<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+@"Colou?r\sset\sto\:\s\<\/span\>[A-z\s\(\)\d]+.*?<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+@"Current Colou?r(\stype)?\:\s\<\/span\>[A-z\s\(\)\d]+<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+@"Current\s[Pp]art\:\s\<\/span\>\d+(\(\d+\))?<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"(?:Next|Previous|Change)\sColou?r<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"(?:Next|Previous|Change)\sColou?r<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"Change\s(?:Right|Left)?\s?[A-z]+<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"(Next|Previous) part<br />(?<=<br />)", RegexOptions.Compiled),
            // Try compound statements, then individual lines if they still remain.
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"Change\s(?:Cloth|Metal|Leather)\s\d+<br />"+timestampStatefulMatch+nameStatefulMatch+@"Back<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"Which\spart\sdo\syou\s\want\sto\schange\?<br />("+timestampStatefulMatch+nameStatefulMatch+@"(?:Right|Left)\s?[A-z]*?<br />)?"+timestampStatefulMatch+nameStatefulMatch+@"Back<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"What\sdo\syou\swant\sto\smodify\?<br />("+timestampStatefulMatch+nameStatefulMatch+@"(?:Armour|Weapon)\s?(?:Colours|Appearance)<br />)?"+timestampStatefulMatch+nameStatefulMatch+@"Back<br />(?<=<br />)", RegexOptions.Compiled),
            // Fallback compounds
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"Which\spart\sdo\syou\s\want\sto\schange\?<br />"+timestampStatefulMatch+nameStatefulMatch+@"((?:Right|Left)\s?[A-z]*?|Neck|Back|Belt|Helmet|Armour)<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"Change\s(?:Cloth|Metal|Leather)\s\d+<br />(?<=<br />)", RegexOptions.Compiled),
            new Regex(timestampStatefulMatch+nameStatefulMatch+@"What\sdo\syou\swant\sto\smodify\?<br />"+timestampStatefulMatch+nameStatefulMatch+@"(?:Armour|Weapon)\s?(?:Colours|Appearance)<br />(?<=<br />)", RegexOptions.Compiled),
        };

        private List<Tuple<Regex, string>> serverReplacesOrdered = new List<Tuple<Regex, string>>
        {
            new Tuple<Regex, string> ( new Regex(@"(\-\-\-\-\sServer\sOptions\s\-\-\-\-)([^|]*)(\-\-\-\-\sEnd\sServer\sOptions\s\-\-\-\-)", RegexOptions.Compiled), @"<span class=""whispers"">$1</span><span class=""tells"">$2</span><span class=""whispers"">$3</span>" ),
            new Tuple<Regex, string> ( new Regex(@"\]\s(.*?)\s(Joined\sas\s(?:Game\sMaster|Player)\s\d+)", RegexOptions.Compiled), @"] <span class=""actors"">$1</span> $2" ),
            new Tuple<Regex, string> ( new Regex(@"\]\s(.*?)\s(Left\sas\sa\s(?:Game\sMaster|Player))\s(\(\d+\splayers\sleft\))", RegexOptions.Compiled), @"] <span class=""actors"">$1</span> $2 <span class=""emotes"">$3</span>" ),
            new Tuple<Regex, string> ( new Regex(@"(Your cryptographic public identity is\:\s)(.*?)", RegexOptions.Compiled), @"$1<span class=""emotes"">$2</span>" ),
            new Tuple<Regex, string> ( new Regex(@"(Our\spublic\saddress\sas\sseen\sby\sthe\smasterserver\:)\s(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\:\d{1,5})", RegexOptions.Compiled), @"$1 <span class=""emotes"">$2</span>" ),
            new Tuple<Regex, string> ( new Regex(@"(Connection\sAttempt\smade\sby\s)(.*?)(?=(|$))", RegexOptions.Compiled), @"<span class=""whispers"">$1</span><span class=""actors"">$2</span>" ),
            new Tuple<Regex, string> ( new Regex(@"(SpellLikeAbilityReady\: Could not find valid ability in list.*?)", RegexOptions.Compiled), @"<span class=""whispers"">$1</span>" ),
            new Tuple<Regex, string> ( new Regex(@"(Event\sadded\swhile\spaused\:\s*?EventId\:\s\d\s*?CallerId\:\s\d+\s*?ObjectId\:\s*?\d+)", RegexOptions.Compiled), @"<span class=""emotes"">$1</span>" ),
            new Tuple<Regex, string> ( new Regex(@"(Server Shutting Down)", RegexOptions.Compiled), @"<span class=""whispers"">$1</span>" ),
        };
    }
}
