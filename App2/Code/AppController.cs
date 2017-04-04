using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
            _mainTask = new Task(BME280Test, _cancelationSource.Token);
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
