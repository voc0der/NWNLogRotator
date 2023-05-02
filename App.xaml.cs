using NWNLogRotator.Classes;
using System.Windows;

namespace NWNLogRotator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void DisposeNotifyIcon(object sender, ExitEventArgs e)
        {
            NWNLogRotator.MainWindow.ExitEvent();
        }
    }
}