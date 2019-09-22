/*  
    *  AUTHOR: Ravenmyst
    *  DATE: 9/21/2019
    *  LICENSE: MIT
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NWNLogRotator.classes;
using NWNLogRotator.Components;

namespace NWNLogRotator
{
    public partial class MainWindow : Window
    {
        Settings _settings;
        public MainWindow()
        {
            InitializeComponent();

            SetupApplication();
        }

        private void SetupApplication()
        {
            IterateNWN_Watcher( false );

            LoadSettings_Handler();
        }

        private Settings Settings_Get()
        {
            FileHandler instance = new FileHandler();
            _settings = instance.InitSettingsIni();
            return _settings;
        }

        private async void IterateNWN_Watcher(bool PreviousStatus)
        {
            var Status = NWNProcessStatus_Get();
            if(Status == true)
            {
                NWNStatusTextBlock.Text = "nwmain is active!";
                NWNStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                await Task.Delay(10000);
                IterateNWN_Watcher(true);
            }
            else
            {
                if( PreviousStatus == true )
                {
                    NWNLog_Save();
                }
                NWNStatusTextBlock.Text = "nwmain not found!";
                NWNStatusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                await Task.Delay(10000);
                IterateNWN_Watcher(false);
            }
            // eventually obtain the Path automatically if it's checked (scrape files in known locs, find dates, if null do default)
        }

        private bool NWNProcessStatus_Get()
        {
            Process[] processlist = Process.GetProcesses();

            foreach (Process theProcess in processlist)
            {
                if( theProcess.ProcessName.IndexOf("nwmain") != -1 )
                {
                    return true;
                }      
            }
            return false;
        }

        private void NWNLog_Save()
        {
            UpdateResultsPane(1);
        }

        private void Settings_Save()
        {
            string OutputDirectory = OutputDirectoryTextBox.Text;
            string PathToLog = PathToLogTextBox.Text;
            int MinimumRowsToInteger = int.Parse(MinimumRowsCountTextBlock.Text.Substring(1));
            string ServerName = "";
            string ServerNameColor = "";
            bool? EventText = EventTextCheckBox.IsChecked;
            bool? CombatText = CombatTextCheckBox.IsChecked;
            string UseTheme = _settings.UseTheme;
            // no implementation yet
            bool Tray = false;

            _settings = new Settings(         OutputDirectory,
                                              PathToLog,
                                              MinimumRowsToInteger,
                                              ServerName,
                                              ServerNameColor,
                                              EventText,
                                              CombatText,
                                              UseTheme,
                                              Tray
                                            );

            FileHandler instance = new FileHandler();
            instance.SaveSettingsIni( _settings );

            UpdateResultsPane(2);
        }

        private async void UpdateResultsPane( int result )
        {
            switch (result)
            {
                case 1:
                    EventStatusTextBlock.Text = "Log Saved Successfully!";
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    await Task.Delay(2000);
                    EventStatusTextBlock.Text = "";
                    break;
                case 2:
                    EventStatusTextBlock.Text = "Settings Saved Successfully!";
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    await Task.Delay(2000);
                    EventStatusTextBlock.Text = "";
                    break;
                case 3:
                    EventStatusTextBlock.Text = "Settings Loaded Successfully!";
                    EventStatusTextBlock.Foreground = new SolidColorBrush(Colors.LawnGreen);
                    await Task.Delay(2000);
                    EventStatusTextBlock.Text = "";
                    break;
            }
        }

        private void LoadSettings_Handler()
        {
            Settings_Get();

            OutputDirectoryTextBox.Text = _settings.OutputDirectory;
            PathToLogTextBox.Text = _settings.PathToLog;
            MinimumRowsCountSlider.Value = _settings.MinimumRowsCount;
            if (_settings.ServerName != "")
            {
                ServerNameCheckBox.IsChecked = true;
                // left to implement: ServerNameTextBox.Text = _settings.ServerName;
            }
            if (_settings.ServerNameColor != "")
            {
                ServerNameColorCheckBox.IsChecked = true;
                // left to implement: ServerNameColorTextBox.Text = _settings.ServerNameColor;
            }
            EventTextCheckBox.IsChecked = _settings.EventText;
            CombatTextCheckBox.IsChecked = _settings.CombatText;

            UpdateResultsPane(3);
        }

        private void InvertColorScheme(object sender, MouseButtonEventArgs e)
        {
            // white => black 
            if(Grid.Background == Brushes.White || Grid.Background == null )
            {
                // purple => black => purple 
                LinearGradientBrush myBrush = new LinearGradientBrush();
                myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.0));
                myBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.5));
                myBrush.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
                Grid.Background = myBrush;

                OutputDirectoryTextBox.Background = Brushes.Black;
                PathToLogTextBox.Background = Brushes.Black;
                OutputDirectoryTextBox.Foreground = new SolidColorBrush(Colors.White);
                PathToLogTextBox.Foreground = new SolidColorBrush(Colors.White);
                SettingsTextBlock.Foreground = new SolidColorBrush(Colors.White);
                OutputDirectoryLabel.Foreground = new SolidColorBrush(Colors.White);
                PathToLogLabel.Foreground = new SolidColorBrush(Colors.White);
                FlagGroupBox.Foreground = new SolidColorBrush(Colors.White);
                ServerNameCheckBox.Foreground = new SolidColorBrush(Colors.White);
                ServerNameColorCheckBox.Foreground = new SolidColorBrush(Colors.White);
                EventTextCheckBox.Foreground = new SolidColorBrush(Colors.White);
                CombatTextCheckBox.Foreground = new SolidColorBrush(Colors.White);
                MinimumRowsLabel.Foreground = new SolidColorBrush(Colors.White);
                MinimumRowsCountTextBlock.Foreground = new SolidColorBrush(Colors.White);

                _settings.UseTheme = "dark";
            }
            // black => white
            else
            {
                // white background
                Grid.Background = Brushes.White;

                OutputDirectoryTextBox.Background = Brushes.White;
                PathToLogTextBox.Background = Brushes.White;
                OutputDirectoryTextBox.Foreground = new SolidColorBrush(Colors.Black);
                PathToLogTextBox.Foreground = new SolidColorBrush(Colors.Black);
                SettingsTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                OutputDirectoryLabel.Foreground = new SolidColorBrush(Colors.Black);
                PathToLogLabel.Foreground = new SolidColorBrush(Colors.Black);
                FlagGroupBox.Foreground = new SolidColorBrush(Colors.Black);
                ServerNameCheckBox.Foreground = new SolidColorBrush(Colors.Black);
                ServerNameColorCheckBox.Foreground = new SolidColorBrush(Colors.Black);
                EventTextCheckBox.Foreground = new SolidColorBrush(Colors.Black);
                CombatTextCheckBox.Foreground = new SolidColorBrush(Colors.Black);
                MinimumRowsLabel.Foreground = new SolidColorBrush(Colors.Black);
                MinimumRowsCountTextBlock.Foreground = new SolidColorBrush(Colors.Black);

                _settings.UseTheme = "light";
            }

        }

        private void SettingsTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
        }

        private void SettingsTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void MinimumRowsCountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(MinimumRowsCountTextBlock != null)
                MinimumRowsCountTextBlock.Text = e.NewValue.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings_Save();
        }
    }
}
