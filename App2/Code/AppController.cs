using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace App2.Code
{
    public class AppController
    {
        private static AppController Controller;
        private static GPIOController GpioController;


        Task _mainTask;
        CancellationTokenSource _cancelationSource = null;


        private AppController()
        {
            _cancelationSource = new CancellationTokenSource();
            _cancelationSource.Token.Register(() => { Debug.WriteLine("Action cancelled."); });
            _mainTask = new Task(TSL2671Test, _cancelationSource.Token);
        }

        public static void Initialize()
        {
            try
            {
                Controller = new AppController();
                GpioController = new GPIOController();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void Start()
        {
            Controller._mainTask.Start();
        }

        public static void Cancel(int delay)
        {

            Controller._cancelationSource.CancelAfter(delay);

        }

        private async void TSL2671Test()
        {
            //Initialize i2c tsl2561 device 
            var settings = new I2cConnectionSettings(TSL2561Sensor.TSL2561_ADDR);
            settings.BusSpeed = I2cBusSpeed.FastMode;
            settings.SharingMode = I2cSharingMode.Shared;

            string aqs = I2cDevice.GetDeviceSelector("I2C1");  /* Find the selector string for the I2C bus controller                   */
            var dis = await DeviceInformation.FindAllAsync(aqs);            /* Find the I2C bus controller device with our selector string           */

            var I2CDev = await I2cDevice.FromIdAsync(dis[0].Id, settings);

            TSL2561Sensor TSL2561 = new TSL2561Sensor(ref I2CDev);

            TSL2561.PowerUp();

            Debug.WriteLine("TSL2561 ID: " + TSL2561.GetId());

            Boolean Gain = false;
            uint MS = (uint)TSL2561.SetTiming(false, 2);
            double CurrentLux = 0;

            for (var i = 0; i < 100; i++)
            {
                uint[] Data = TSL2561.GetData();

                Debug.WriteLine("Data1: " + Data[0] + ", Data2: " + Data[1]);

                CurrentLux = TSL2561.GetLux(Gain, MS, Data[0], Data[1]);

                String strLux = String.Format("{0:0.00}", CurrentLux);
                String strInfo = "Luminosity: " + strLux + " lux";

                Debug.WriteLine(strInfo);


                await Task.Delay(1000);
            }



        }

        private async void BME280Test()
        {
            try
            {
                BME280Sensor BME280;
                BME280 = new BME280Sensor();
                //Initialize the sensor
                await BME280.Initialize();
                float temp = 0;
                float pressure = 0;
                float altitude = 0;
                float humidity = 0;
                const float seaLevelPressure = 1022.00f;

                for (int i = 0; i < 10; i++)
                {
                    temp = await BME280.ReadTemperature();
                    pressure = await BME280.ReadPreasure();

                    humidity = await BME280.ReadHumidity();


                    altitude = await BME280.ReadAltitude(seaLevelPressure);
                    //Write the values to your debug console
                    Debug.WriteLine("Temperature: " + temp.ToString() + " deg C");
                    Debug.WriteLine("Humidity: " + humidity.ToString() + " %");
                    Debug.WriteLine("Pressure: " + pressure.ToString() + " Pa");
                    Debug.WriteLine("Altitude: " + altitude.ToString() + " m");
                    Debug.WriteLine("");
                    await Task.Delay(1000);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void LEDTest()
        {
            try
            {
                Random rand = new Random();
                Debug.WriteLine("Starting blinking action.");
                while (true)
                {
                    GpioController.ToggleLed();
                    await Task.Delay(TimeSpan.FromMilliseconds(rand.Next(500, 5000)));

                    if (Controller._cancelationSource.Token.IsCancellationRequested)
                    {
                        GpioController.TurnOff();
                        Controller._cancelationSource.Cancel();
                        break;
                    }
                }

                Debug.WriteLine("Finishing blinking action");

                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}
