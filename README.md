# NWNLogRotator
Transforms and parses any Neverwinter Nights chat log into beautiful HTML. Written in C# .NET. NWNLogRotator is both Enhanced Edition and 1.69 compatible.

# Configuration Examples
Important: Open Windows Explorer and enter %HOMEPATH%, and the last directory there is your _USER_.
  
| Configuration  | Example | Description |
| ------------- | ------------- | ------------- |
| Output Directory | C:\Users\\_USER_\Documents\Neverwinter Nights\logs\ | New logs stored in default Steam EE log directory |
| Output Directory | C:\Program Files (x86)\Neverwinter Nights\logs\ | New logs stored in default 1.69 log directory |
| Path to Log | C:\Users\\_USER_\Documents\Neverwinter Nights\logs\nwClientLog1.txt | Get log from Steam EE default log directory |
| Path to Log | C:\Program Files (x86)\Neverwinter Nights\logs\nwClientLog1.txt | Get log from 1.69 default log directory |
| Server Name | Test | Server logs will be written to OutputDirectory\Test |
| Server Name Color | FFCC00 | The server name in the log appears yellow |
| Server Name Color | FF0000 | The server name in the log appears red |

# Automation (How it works)
While open, NWNLogRotator will try to process new logs automatically based on the status of whether Neverwinter Nights is running. If your directories are configured to match with your Neverwinter Nights default log storage, it will process them automatically.

# Notes
1) This application may warn you that the publisher is not signed or verified, but if you click 'More Info' it will allow you to run anyways.
2) Make sure you have logging enabled if you cannot find a log file. The setting to enable logging is located in nwnplayer.ini as ClientEntireChatWindowLogging = 1 under [Game Options].
3) The parsing will not be accurate if obtained from a third-party server that alters the standard NWN logging.
4) Changing the theme is available by clicking the "NWNLogRotator" header.
5) Please report any bugs or post suggestions. 

# Releases
Download Latest <a href="https://github.com/ravenmyst/NWNLogRotator/releases">Here</a>.

# Example Parsed Log
Right Click <a href="https://raw.githubusercontent.com/ravenmyst/NWN-Log-Rotator/master/output/NWNLogExample.html">This Text</a> then select "Save Link As" and it should save it as an HTML file, viewable in any browser.

# NWNLogRotator Screenshot
![Screenshot of NWNLogRotator would be here](https://raw.githubusercontent.com/ravenmyst/NWNLogRotator/master/Assets/Images/screenshot_nwnlr1.png)

# In Development
1) Optimizing the parser and cleaning and compartementalization. (Credits to BasicHuman)