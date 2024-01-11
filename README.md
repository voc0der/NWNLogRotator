# NWNLogRotator
Use this NWN Log Rotator to colorize, clean, and automatically save Neverwinter Nights chat logs into beautiful HTML. Enhanced Edition and 1.69 compatible.
##### <a href="http://htmlpreview.github.io/?https://github.com/noblesigma/NWNLogRotator-Node.js/blob/master/output/NWNLogExample.html">Click here</a> for an example of a parsed log. 
##### <a href="https://www.youtube.com/watch?v=b2jie6fbSOA">Click here</a> to see a usage video.

# Download
Latest version can be found <a href="https://github.com/noblesigma/NWNLogRotator/releases/latest">here</a>.

# Screenshots
![Screenshot of NWNLogRotator main would be here](https://raw.githubusercontent.com/noblesigma/NWNLogRotator/master/Assets/Images/screenshot_nwnlr1.png)
![Screenshot of NWNLogRotator export would be here](https://raw.githubusercontent.com/noblesigma/NWNLogRotator/master/Assets/Images/screenshot_nwnlr2.png)
![Screenshot of NWNLogRotator launcher would be here](https://raw.githubusercontent.com/noblesigma/NWNLogRotator/master/Assets/Images/screenshot_nwnlr3.png)

# Automation (How it works)
While open, NWNLogRotator will process new logs automatically based on the status of whether Neverwinter Nights is running. Configure the directories to match with your Neverwinter Nights default log storage, and it will process them automatically.

# Configuration Examples
| Configuration  | Example | Description |
| ------------- | ------------- | ------------- |
| Output Directory | C:\Users\\{USER}¹\Documents\Neverwinter Nights\logs\ | New logs stored in default Steam EE log directory |
| Output Directory | C:\Program Files (x86)\Neverwinter Nights\logs\ | New logs stored in default 1.69 log directory |
| Path to Log | C:\Users\\{USER}¹\Documents\Neverwinter Nights\logs\nwClientLog1.txt | Get log from Steam EE default log directory |
| Path to Log | C:\Program Files (x86)\Neverwinter Nights\logs\nwClientLog1.txt | Get log from 1.69 default log directory |
| Custom Emotes | [],^ | [This will be an emote], ^This will be an emote^ |
| Filter Lines | foo,bar | Any line that has the word foo or bar will be removed |

1. Search %HOMEPATH% in Windows File Explorer, and the last directory is your {USER}

# Other Notes
1) When opening for the first time, Windows may warn you that the publisher is not signed or verified, but if you click 'More Info' it will allow you to run anyways.
2) Make sure you save your configuration settings or some options may not do what you expected!
3) To Launch through Steam set the Launcher path to "C:\Program Files (x86)\Steam\steam.exe".
4) Make sure you have logging enabled if you cannot find a log file. Enhanced Edition and 1.69 in how to enable chat logging.
   * For EE, it is located in settings.tml with a few more options.
     * [game.log.chat.all] enabled = true (all client chat window messages in the log)
     * [game.log.chat.emotes] enabled = true (all graphic emotes are logged as their text equivalent)
     * [game.log.chat.text] enabled = false (if true, then the text part of the chat window is logged; if both this and chat.all are set to true, then the text chat will duplicate in the log)
   * For 1.69, it is located in nwnplayer.ini as ClientEntireChatWindowLogging = 1 under [Game Options].
