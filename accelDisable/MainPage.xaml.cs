using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using CSharp___DllImport;
using System.Windows.Threading;
namespace accelDisable
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool hasChanged = false;

        //Delay timer
        DispatcherTimer timer;

        // Constructor
        public MainPage()
        {         
            InitializeComponent();


            statusLocked.Visibility = System.Windows.Visibility.Collapsed;
            statusUnlocked.Visibility = System.Windows.Visibility.Collapsed;
            accelstatus.Visibility = System.Windows.Visibility.Collapsed;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 25);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            if (Accelerometer.IsSupported)
            {
                Accelerometer sens = new Accelerometer();
                sens.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(sens_CurrentValueChanged);
                sens.Start();
                
            }
            else
            {   //It's disabled
                accelstatus.IsChecked = true;
                statusLocked.Visibility = System.Windows.Visibility.Visible;
                statusUnlocked.Visibility = System.Windows.Visibility.Collapsed;
            }
               
        }


        void wph_updateAvailableEvent(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("An update is available.\nVisit WindowsPhoneHacker.com for the latest version.", "Update Available", MessageBoxButton.OK);
            }
            );
        }

        void timer_Tick(object sender, EventArgs e)
        {
            accelstatus.IsChecked = true;
            accelstatus.Visibility = System.Windows.Visibility.Visible;
            statusLocked.Visibility = System.Windows.Visibility.Visible;
            statusUnlocked.Visibility = System.Windows.Visibility.Collapsed;
            timer.Stop();
        }

        void sens_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            if (!hasChanged)
            {
                this.Dispatcher.BeginInvoke(() =>
                    {
                        accelstatus.IsChecked = false;
                        accelstatus.Visibility = System.Windows.Visibility.Visible;
                        statusLocked.Visibility = System.Windows.Visibility.Collapsed;
                        statusUnlocked.Visibility = System.Windows.Visibility.Visible;
                        timer.Stop();
                    });
                hasChanged = true;
                
            }
            
        }


        private void accelstatus_Unchecked(object sender, RoutedEventArgs e)
        {
            DllImportCaller.lib.StringIntIntCall("coredll", "SetDevicePower", "ACC1:", 1, (int)Phone.Network.WiFi.PowerState.D0);
            statusLocked.Visibility = System.Windows.Visibility.Collapsed;
            statusUnlocked.Visibility = System.Windows.Visibility.Visible;
            updateTile();
            Tile.setLastSetting(!accelstatus.IsChecked.Value);
        }

        private void accelstatus_Checked(object sender, RoutedEventArgs e)
        {

            DllImportCaller.lib.StringIntIntCall("coredll", "SetDevicePower", "ACC1:", 1, (int)Phone.Network.WiFi.PowerState.D4);
            statusLocked.Visibility = System.Windows.Visibility.Visible;
            statusUnlocked.Visibility = System.Windows.Visibility.Collapsed;
            updateTile();
            Tile.setLastSetting(!accelstatus.IsChecked.Value);
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            addTile();
            updateTile();
        }
        void addTile()
        {
            try
            {
                Microsoft.Phone.Shell.StandardTileData data = new Microsoft.Phone.Shell.StandardTileData
                {
                    Title = "Orientation Lock"
                };
                
                Microsoft.Phone.Shell.ShellTile.Create(new Uri("/Toggle.xaml", UriKind.Relative), data);
            }
            catch
            {
            }
        }
        void updateTile()
        {
            Tile.updateTile(accelstatus.IsChecked.Value);
        }
               
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            WindowsPhoneHacker.wph.bacon("orientationlock3");
            WindowsPhoneHacker.wph.updateAvailableEvent += new EventHandler(wph_updateAvailableEvent);
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("OrientationLock v3\n\nDeveloped by Jaxbot @ WindowsPhoneHacker.com\n\nPowered by fiinix's DllImport Project", "About OrientationLock", MessageBoxButton.OK);
        }

    }
}