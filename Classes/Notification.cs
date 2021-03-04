/*  
    *  Author:   © noblesigma
    *  Project:  NWNLogRotator
    *  GitHub:   https://github.com/noblesigma/NWNLogRotator
    *  Date:     3/3/2021
    *  License:  MIT
    *  Purpose:  This program is designed used alongside the Neverwinter Nights game, either Enhanced Edition, or 1.69.
    *  This program does not come with a warranty. Any support may be found on the GitHub page.
*/

using System.Reflection;
using System.Windows.Forms;

namespace NWNLogRotator.Classes
{
    class Notification
    {
        private readonly NotifyIcon _notifyIcon;

        public Notification()
        {
            _notifyIcon = new NotifyIcon();
            // Extracts your app's icon and uses it as notify icon
            _notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            // Hides the icon when the notification is closed
            _notifyIcon.BalloonTipClosed += (s, e) => _notifyIcon.Visible = false;
        }

        public void ShowNotification(string theMessage)
        {
            _notifyIcon.Visible = true;
            // Shows a notification with specified message and title
            _notifyIcon.ShowBalloonTip(3000, "NWNLogRotator", theMessage, ToolTipIcon.Info);
        }
    }
}

