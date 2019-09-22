using NWNLogRotator.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public DateTime CurrentDateTime()
        {
            return DateTime.Now;
        }

        public string ParseNWNLog( string NWNLog, Settings _run_settings )
        {
            string ParsedNWNLog = "<head>" +
                "<style>" +
                    ".logbody { background-color: #000000; font-family: Tahoma, Geneva, sans-serif; color: #FFFFFF; }" +
                    ".logheader { color: #03FFFF; }" +
                    ".default { color: #FFFFFF }" +
                    ".timestamp { color: #B1A2BD; }" +
                    ".actors { color: #8F7FFF; }" +
                    ".tells { color: #0F0; }" +
                    ".whispers { color: #808080; }" +
                    ".emotes { color: #ffaed6; }" +
                "</style>" +
            "</head>";

            string logTitle = "<h4>[<span class='logheader'>" + _run_settings.ServerName + "Log</span>]"
            + "<span class='actors'>Date/Time</span>: " + CurrentDateTime().ToString("MM/dd/yyyy hh:m")
            + "</h4>";
            string preLog = "<html>" + ParsedNWNLog + "<body class='logbody'><span class='default'>" + logTitle;
            string postLog = "</span></body></html>";

            // combat text removal
            if (_run_settings.CombatText == true)
            {
                NWNLog = Regex.Replace(NWNLog, @"(/.+? (?=.*)\*{ 1}hit\*{ 1}.*\s\:\s\(\d{ 1,}\s\+\s\d{ 1,}\s\=\s\d{ 1,}.*\){ 1}\r\n/g)", "", RegexOptions.IgnoreCase);
            }

            // core format replacements
            NWNLog = Regex.Replace(NWNLog, @"\[CHAT WINDOW TEXT\]", "", RegexOptions.IgnoreCase);
            //.replace(/\[{ 1}[A-z]{3}\s[A - z]{3}\s[0 - 9]{2}\s/g, '<span class="timestamp">[')
            //.replace(/:[0-9]*]{1}/g, ']</span>')
            // actors
            //.replace(/\]<\/span>((...).*: )/g, ']</span><span class="actors">$1</span>')
            // tells
            //.replace(/:\s?<\/span>\s?(\[Tell])(.*.*)/g, '</span><span class="tells"> $1:$2</span>')
            // whispers 
            //.replace(/:\s?<\/span>\s?(\[Whisper])(.*.*)/g, '</span><span class="whispers"> $1:$2</span>')
            // emotes 
            //.replace(/(\*.*\*)/g, '<span class="emotes">$1</span>')
            // html formatting
            NWNLog = Regex.Replace(NWNLog, @"\r\n", "<br />", RegexOptions.IgnoreCase) + postLog;

            ParsedNWNLog = preLog + NWNLog + postLog;

            return ParsedNWNLog;
        }
    }
}
