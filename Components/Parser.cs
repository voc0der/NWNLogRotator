using NWNLogRotator.classes;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace NWNLogRotator.Components
{
    public partial class Parser : Component
    {
        public Parser()
        {
            InitializeComponent();
        }

        public Parser(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public string ParseNWNLog(string NWNLog, Settings _run_settings, DateTime _dateTime)
        {
            string ParsedNWNLog = "<head>" +
                "<style>" +
                    ".logbody { background-color: #000000; font-family: Tahoma, Geneva, sans-serif; color: #FFFFFF; }";
            ParsedNWNLog += ".logheader { color: #";
            if (_run_settings.ServerNameColor != "")
            {
                ParsedNWNLog += _run_settings.ServerNameColor;
            }
            ParsedNWNLog += " }" +
            ".default { color: #FFFFFF }" +
            ".timestamp { color: #B1A2BD; }" +
            ".actors { color: #8F7FFF; }" +
            ".tells { color: #0F0; }" +
            ".whispers { color: #808080; }" +
            ".emotes { color: #ffaed6; }" +
        "</style>" +
    "</head>";

            string ServerNameTitle = "Server";
            if (_run_settings.ServerName != "")
                ServerNameTitle = _run_settings.ServerName;

            string logTitle = "<h4>[<span class='logheader'>" + ServerNameTitle + " Log</span>] "
            + "<span class='actors'>Date/Time</span>: " + _dateTime.ToString("MM/dd/yyyy hh:m")
            + "</h4>";
            string preLog = "<html>" + ParsedNWNLog + "<body class='logbody'><span class='default'>" + logTitle;
            string postLog = "</span></body></html>";

            // combat text removal
            if (_run_settings.CombatText == true)
            {
                // todo; make into a component
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\*{1}hit\*{1}.*\s\:\s\(\d{1,}\s\+\s\d{1,}\s\=\s\d{1,}.*\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)damages\s.*\:\s{1}\d{1,}\s{1}\({1}\d{1,}\s{1}.*\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\*{1}parried\*{1}.*\({1}\d{1,}.*\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}[a-zA-Z]*\:{1}\s{1}Damage\s{1}[a-zA-Z]*\s{1}absorbs\s{1}.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\*{1}target concealed\:{1}.*\:{1}\s{1}\({1}\d{1,}.*\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\*{1}critical hit\*\s{1}\:{1}\s{1}\({1}\d{1,}.*\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\*{1}resisted\*\s{1}\:{1}\s{1}\({1}\d{1,}.*\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Immune\s{1}to\s{1}Critical\s{1}Hits\.{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\*{1}miss\*{1}.*\s\:\s\(\d{1,}\s{1}.*\d{1,}\s\=\s\d{1,}\)\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\*{1}success\*{1}\s{1}\:{1}\s{1}\(\d{1,}.*\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\*{1}failure\*{1}.*\s\:\s{1}\({1}.*\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\:\s{1}Initiative\s{1}Roll\s{1}\:\s\d{1,}\s\:\s\(\d{1,}\s\+\s{1}\d{1,}\s{1}\={1}\s{1}\d{1,}\){1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\:{1}\s{1}Damage Immunity\s{1}absorbs.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\:{1}\s{1}Immune to Sneak Attacks\.{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\:{1}\s{1}Immune to Negative Levels\.{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\:{1}\s{1}Spell Level Absorption absorbs\s{1}\d{1,}.*\:{1}\s{1}\d{1,}.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}[a-zA-Z]*cast.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}[a-zA-Z]*uses.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}[a-zA-Z]*enables.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)[a-zA-Z]*\s{1}attempts\s{1}to\s{1}.*\:\s{1}.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)[a-zA-Z]*\:{1}\s{1}Healed\s{1}\d{1,}\s{1}hit.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)[a-zA-Z]*\:{1}\sImmune to [a-zA-Z]*.*\.{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}Dispel\s{1}Magic\s{1}\:{1}\s{1}[a-zA-z]*.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}Experience Points Gained\:{1}\s{1,}\d{1,}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)There are signs of recent fighting here...\*{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Stale temporary properties detected, cleaning item\.{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}\[Check for loot\:{1}\s{1}\d{1,}.*\]{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}You.{1}ve reached your maximum level.\s{1}.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}Devastating Critical Hit!\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1,}Done resting\.{1}.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1,}You triggered a Trap!{1}.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}You cannot target a creature you cannot see or do not have a line of sight to\.{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}Weapon equipped as a one-handed weapon.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}You cannot rest so soon after exerting yourself.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}Equipping this armor has disabled your monk abilities.\r\n", "");
                //NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\s{1}No resting is allowed in this area.\r\n", "");
            }

            // combat text removal
            if (_run_settings.EventText == true)
            {
                // todo; make into a component
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*).{1}Event.{1} .*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Minimum Tumble AC Bonus:\s?\+{1}[0-9]*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @"Minimum Tumble AC Bonus:\s?\+{1}[0-9]*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)No Monk\/Shield AC Bonus:\s?\+{1}[0-9]*.*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)You are light sensitive!\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)has left as a player..\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)has joined as a player..\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)has left as a game master..\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)has joined as a game master..\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)You are now in a Party PVP area.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)You are now in a No PVP area.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Resting.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Cancelled Rest.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)You used a key.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Equipped item swapped out.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)You are portalling, you can't do anything right now.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Unknown Speaker: You are being moved to your last location, please wait...\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)You are entering a new area!\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)This container is persistent.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)This container is full.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)You are too busy to barter now.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Player not found.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)You cannot carry any more items, your inventory is full.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)This is a trash, its contents may be purged at anytime.\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Armor\/Shield Applies: Skill .*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)\-{1}\s{1}Your character has been saved\.{1}\s{1}\-{1}\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)New Value: [0-9]*\r\n", "");
                NWNLog = Regex.Replace(NWNLog, @".+?(?=.*)Quick bar\s{1}.*loaded in.*\r\n", "");
            }

            // core format replacements
            NWNLog = Regex.Replace(NWNLog, @"\[CHAT WINDOW TEXT\]", "");
            NWNLog = Regex.Replace(NWNLog, @"\[{1}[A-z]{3}\s[A-z]{3}\s[0-9]{2}\s", "<span class='timestamp'>[");
            NWNLog = Regex.Replace(NWNLog, @"\:{1}[0-9]*]{1}", "]</span>");
            // actors
            NWNLog = Regex.Replace(NWNLog, @"\]<\/span>((...).*: )", "]</span><span class='actors'>$1</span>");
            // tells
            NWNLog = Regex.Replace(NWNLog, @":\s?<\/span>\s?(\[Tell])(.*.*)", "</span><span class='tells'> $1:$2</span><br />");
            // whispers 
            NWNLog = Regex.Replace(NWNLog, @":\s?<\/span>\s?(\[Whisper])(.*.*)", "</span><span class='whispers'> $1:$2</span><br />");
            // emotes 
            NWNLog = Regex.Replace(NWNLog, @"(\*.*\*)", "<span class='emotes'>$1</span>");
            // html formatting
            NWNLog = Regex.Replace(NWNLog, @"\r\n", "<br />") + postLog;

            ParsedNWNLog = preLog + NWNLog + postLog;

            return ParsedNWNLog;
        }
    }
}
