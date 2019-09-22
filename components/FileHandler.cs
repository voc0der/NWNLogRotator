using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NWNLogRotator.Components
{
    public partial class FileHandler : Component
    {
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
            string theWorkingDirectory = "";

            //get the current working directory
            theWorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return theWorkingDirectory;
        }

        public bool SettingsExist_Get()
        {
            string[] files =
                Directory.GetFiles( CurrentProgramDirectory_Get(), "*NWNLogRotator.ini", SearchOption.TopDirectoryOnly );

            if(files.Length == 1)
            {
                return true;
            }
            return false;
        }

        public void CreateSettingsIni()
        {
            // File.WriteAllText("C:\" , string[]);
            MessageBox.Show(SettingsExist_Get().ToString());
        }
    }
}
