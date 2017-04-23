using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;
using System.Diagnostics;
using Windows.Devices.Enumeration;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TemperatureI2C
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private I2cDevice arduino;
        private const int slaveAdd = 0x40;
        private byte[] buffer = new byte[4];
        private DispatcherTimer timer = new DispatcherTimer();
        public MainPage()
        {
            this.InitializeComponent();
            InitI2C();  //Initializing the I2C bus and assigning arduino as an I2cDevice
            timer.Interval = TimeSpan.FromMilliseconds(500);  //Get data every 500 ms
            timer.Tick += Timer_Tick;
            timer.Start();  //start of timer
        }

        private void Timer_Tick(object sender, object e)
        {
            byte[] response = new byte[2];
            try
            {
                arduino.Read(response);   //read data from I2C device and store it in response
            }
            catch (Exception p)
            {

                Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog(p.Message);
            }
            double tempC = response[0]*4 / 9.31;  //conversion of analog data to temperature
            Debug.WriteLine(tempC.ToString());    //print the temperature to debug window
        }

        private async void InitI2C()
        {
            var i2c = new I2cConnectionSettings(slaveAdd);
            i2c.BusSpeed = I2cBusSpeed.FastMode;
            string aqs = I2cDevice.GetDeviceSelector("I2C1");
            var dis = await DeviceInformation.FindAllAsync(aqs);
            arduino = await I2cDevice.FromIdAsync(dis[0].Id, i2c);
        }
    }
}
