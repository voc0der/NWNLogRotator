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
        public string ParseLog(Stream inputStream, bool removeCombat, bool removeEvents, string ServerName, string ServerNameColor, string CustomEmotes, string FilterLines)
        {
            var reader = new StreamReader(inputStream, Encoding.GetEncoding("iso-8859-1"));
            var removeExps = new List<Regex>();

            string[] filterLinesArray = FilterLines.Split(',');
            if (!removeEvents && FilterLines != "")
            {
                eventLines = new List<String>();
                foreach (string theEvent in eventLines)
                {
                    eventLines.Add(theEvent);
                }
                removeEvents = true;
            }
            else if (removeEvents && FilterLines != "")
            {
                List<String> eventLinesTemp = new List<String>();
                foreach (string theEvent in eventLines)
                {
                    eventLinesTemp.Add(theEvent);
                }
                eventLines.AddRange(eventLinesTemp);
            }
            
            string text;
            if (removeEvents)
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

            foreach (var exp in coreRemoves)
                text = text.Replace(exp, "");

            removeExps.AddRange(garbageLines);
            if (removeCombat) removeExps.AddRange(combatLines);
            foreach (var exp in removeExps)
                text = exp.Replace(text, "");


            formatReplacesOrdered = formatReplacesWithUserOverride( CustomEmotes );

            foreach (var exp in formatReplacesOrdered)
                text = exp.Item1.Replace(text, exp.Item2);

            reader.Close();

            text = HTMLPackageLog_Get(text, ServerName, ServerNameColor);
            return text;
        }
        public bool ActorOccurences_Get(string ParsedNWNLog, int MinimumRowsCount)
        {
            Match ActorOccurences = Regex.Match(ParsedNWNLog, @"<span class=\'actors\'>");

            if (ActorOccurences.Length >= MinimumRowsCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string HTMLPackageLog_Get(string ParsedNWNLog, string ServerName, string ServerNameColor)
        {
            string HTMLHeader = "<head>" +
                "<style>" +
                    ".logbody { background-color: #000000; font-family: Tahoma, Geneva, sans-serif; color: #FFFFFF; }";
                    HTMLHeader += ".logheader { color: #";
                    if (ServerNameColor != "")
                    {
                        HTMLHeader += ServerNameColor;
                    }
                    HTMLHeader += " }" +
                    ".default { color: #FFFFFF }" +
                    ".timestamp { color: #B1A2BD; }" +
                    ".actors { color: #8F7FFF; }" +
                    ".tells { color: #0F0; }" +
                    ".whispers { color: #808080; }" +
                    ".emotes { color: #ffaed6; }" +
                "</style>" +
            "</head>";

            string logTitle = "<h4>[<span class='logheader'>" + ServerName + " Log</span>] "
           + "<span class='actors'>Date/Time</span>: " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt")
           + "</h4>";
            string postLog = "</span></body></html>";
            return "<html>" + HTMLHeader + logTitle + ParsedNWNLog + "<body class='logbody'><span class='default'>" + postLog;
        }

        private List<Tuple<Regex, string>> formatReplacesWithUserOverride( string CustomEmotes )
        {
            string[] emotesArray = CustomEmotes.Split(',');
            List<Tuple<Regex, string>> formatReplacesOrderedReturn = new List<Tuple<Regex, string>>();

            formatReplacesOrderedReturn.AddRange(formatReplacesOrderedOne);
            formatReplacesOrderedReturn.AddRange(formatReplacesOrderedTwo);
            if (CustomEmotes.Length != 0)
            {
                List<Tuple<Regex, string>> additionalActorsList = new List<Tuple<Regex, string>>();
                foreach (string emotePair in emotesArray)
                {
                    string theEmotePair = emotePair.Trim();
                    if (theEmotePair.Length == 2)
                    {
                        string tempLeftBracket = theEmotePair.Substring(0,1);
                        string tempRightBracket = theEmotePair.Substring(1,1);
                        string theRegEx;
                        theRegEx = "\\" + tempLeftBracket + "(?!([0-9]{2}\\:[0-9]{2}|Whisper|Tell)).*\\" + tempRightBracket;

                        Tuple<Regex, string> theCustomEmote = new Tuple<Regex, string>(new Regex(@"(" + theRegEx + ")", RegexOptions.Compiled | RegexOptions.Multiline), "<span class='emotes'>$1</span>");
                        additionalActorsList.Add(theCustomEmote);
                    }
                    else if (theEmotePair.Length == 1)
                    {
                        string tempBracket = theEmotePair.Substring(0, 1);
                        string theRegEx;
                        theRegEx = "\\" + tempBracket + "(?!([0-9]{2}\\:[0-9]{2}|Whisper|Tell)).*\\" + tempBracket;

                        Tuple<Regex, string> theCustomEmote = new Tuple<Regex, string>(new Regex(@"(" + theRegEx + ")", RegexOptions.Compiled | RegexOptions.Multiline), "<span class='emotes'>$1</span>");
                        additionalActorsList.Add(theCustomEmote);
                    }
                }
                formatReplacesOrderedReturn.AddRange(additionalActorsList);
            }
       
            formatReplacesOrderedReturn.AddRange(formatReplacesOrderedThree);

            return formatReplacesOrderedReturn;
        }

        private List<Tuple<Regex, string>> formatReplacesOrdered = new List<Tuple<Regex, string>>();

        private List<Tuple<Regex, string>> formatReplacesOrderedOne = new List<Tuple<Regex, string>>
        {
            new Tuple<Regex, string> ( new Regex(@"\[{1}[A-z]{3}\s[A-z]{3}\s[0-9]{2}\s", RegexOptions.Compiled | RegexOptions.Multiline ), "<span class='timestamp'>[" ),
            new Tuple<Regex, string> ( new Regex(@"\:{1}[0-9]*]{1}",RegexOptions.Compiled | RegexOptions.Multiline ), "]</span>" ),
            // actors
            new Tuple<Regex, string>( new Regex(@"\]<\/span>((...).*: )",RegexOptions.Compiled | RegexOptions.Multiline ), "]</span><span class='actors'>$1</span>" ),
            // tells
            new Tuple<Regex, string>( new Regex(@":\s?<\/span>\s?(\[Tell])(.*.*)",RegexOptions.Compiled | RegexOptions.Multiline ), "</span><span class='tells'> $1:$2</span><br />"),
            // whispers 
            new Tuple<Regex, string>( new Regex(@":\s?<\/span>\s?(\[Whisper])(.*.*)",RegexOptions.Compiled | RegexOptions.Multiline ), "</span><span class='whispers'> $1:$2</span><br />"),
        };


        private List<Tuple<Regex, string>> formatReplacesOrderedTwo = new List<Tuple<Regex, string>>
        {
            // emotes 
            new Tuple<Regex, string>( new Regex(@"(\*.*\*)",RegexOptions.Compiled | RegexOptions.Multiline ), "<span class='emotes'>$1</span>")
        };

        private List<Tuple<Regex, string>> formatReplacesOrderedThree = new List<Tuple<Regex, string>>
        {
            // html formatting
            new Tuple<Regex, string>( new Regex(@"\r\n",RegexOptions.Compiled | RegexOptions.Multiline ), "<br />")
        };

        private List<string> coreRemoves = new List<string>
        {
            "[CHAT WINDOW TEXT] ",
        };

        private List<string> eventLines = new List<string>
        {
            "[Event]",
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
        };

        private static string timestampMatch = @".+?(?=.*)"; //@"^\[\w\w\w\s\w\w\w\s\d\d\s\d\d:\d\d:\d\d\]\s";

        private List<Regex> combatLines = new List<Regex>
        {
            new Regex( timestampMatch+@"\*{1}hit\*{1}.*\s\:\s\(\d{1,}\s[+-]\s\d{1,}\s\=\s\d{1,}.*\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"damages\s.*\:\s{1}\d{1,}\s{1}\({1}\d{1,}\s{1}.*\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\*{1}parried\*{1}.*\({1}\d{1,}.*\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}[a-zA-Z]*\:{1}\s{1}Damage\s{1}[a-zA-Z]*\s{1}absorbs\s{1}.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\*{1}target concealed\:{1}.*\:{1}\s{1}\({1}\d{1,}.*\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\*{1}critical hit\*\s{1}\:{1}\s{1}\({1}\d{1,}.*\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\*{1}resisted\*\s{1}\:{1}\s{1}\({1}\d{1,}.*\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"Immune\s{1}to\s{1}Critical\s{1}Hits\.{1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\*{1}miss\*{1}.*\s\:\s\(\d{1,}\s{1}.*\d{1,}\s\=\s\d{1,}\)\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\*{1}success\*{1}\s{1}\:{1}\s{1}\(\d{1,}.*\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\*{1}failure\*{1}.*\s\:\s{1}\({1}.*\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\:\s{1}Initiative\s{1}Roll\s{1}\:\s\d{1,}\s\:\s\(\d{1,}\s[+-]\s{1}\d{1,}\s{1}\={1}\s{1}\d{1,}\){1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\:{1}\s{1}Damage Immunity\s{1}absorbs.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\:{1}\s{1}Immune to Sneak Attacks\.{1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\:{1}\s{1}Immune to Negative Levels\.{1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\:{1}\s{1}Spell Level Absorption absorbs\s{1}\d{1,}.*\:{1}\s{1}\d{1,}.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}[a-zA-Z]*cast.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}[a-zA-Z]*uses.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}[a-zA-Z]*enables.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"[a-zA-Z]*\s{1}attempts\s{1}to\s{1}.*\:\s{1}.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"[a-zA-Z]*\:{1}\s{1}Healed\s{1}\d{1,}\s{1}hit.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"[a-zA-Z]*\:{1}\sImmune to [a-zA-Z]*.*\.{1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}Dispel\s{1}Magic\s{1}\:{1}\s{1}[a-zA-z]*.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}Experience Points Gained\:{1}\s{1,}\d{1,}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"There are signs of recent fighting here...\*{1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"Stale temporary properties detected, cleaning item\.{1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}\[Check for loot\:{1}\s{1}\d{1,}.*\]{1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}You.{1}ve reached your maximum level.\s{1}.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}Devastating Critical Hit!\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1,}Done resting\.{1}.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1,}You triggered a Trap!{1}.*\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}You cannot target a creature you cannot see or do not have a line of sight to\.{1}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}Weapon equipped as a one-handed weapon.\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}You cannot rest so soon after exerting yourself.\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}Equipping this armor has disabled your monk abilities.\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( timestampMatch+@"\s{1}No resting is allowed in this area.\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
        };
        private List<Regex> garbageLines = new List<Regex>
        {
            new Regex( @"nwsync\:\s?storage\s?at\s?[0-9:A-z\,= ]{10,}\r\n", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase ),
            new Regex( @"nwsync\:\s?migrations\s?currently\s?applied\:\s?\d{1,}\r\n", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase ),
            new Regex( @"nwsync\:\s?shard\s?\d{1,}\s?available,\sspace\sused\:\s?\d{1,}\skb\r\n", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase ),
            new Regex( @"game\s?is\s?using\s?local\s?port\:\s?\d{1,}\r\n", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase ),
            new Regex( @"Error\:\s?\d{1,}\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
            new Regex( @"gog\:\s?authentication\s?failed\:\s?\d{1,}\r\n", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase ),
            new Regex( timestampMatch+@"Script\s.*,\sOID\:.*,\sTag\:\s.*,\sERROR\:\sTOO MANY INSTRUCTIONS\r\n", RegexOptions.Compiled | RegexOptions.Multiline ),
        };
    }
}
