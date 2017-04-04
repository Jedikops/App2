using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Devices.Gpio;

namespace App2.Code
{
    public class GPIOController
    {

        private const int LED_PIN = 5;
        private const int LED_PIN2 = 6;
        private const int LED_PIN3 = 13;

        GpioController GPIO;

        List<GPIOPin> pins;

        public GPIOController()
        {
            GPIO = GpioController.GetDefault();

            if (GPIO == null)
            {
                throw new System.Exception("Unable to connect to GPIO Controller.");
            }
            else
            {
                pins = new List<GPIOPin>();
                pins.Add(new GPIOPin() { PIN = LED_PIN, pin = GPIO.OpenPin(LED_PIN), pinValue = GpioPinValue.High });
                pins.Add(new GPIOPin() { PIN = LED_PIN2, pin = GPIO.OpenPin(LED_PIN2), pinValue = GpioPinValue.Low });
                pins.Add(new GPIOPin() { PIN = LED_PIN3, pin = GPIO.OpenPin(LED_PIN3), pinValue = GpioPinValue.High });

                foreach (var pin in pins)
                {
                    pin.pin.Write(pin.pinValue);
                    pin.pin.SetDriveMode(GpioPinDriveMode.Output);
                }

                Debug.WriteLine("GPIO pin initialized correctly.");

            }
        }

        public void ToggleLed()
        {
            try
            {
                foreach (var pin in pins)
                {
                    if (pin == null)
                    {
                        throw new Exception("Pin not initialized.");
                    }

                    if (pin.pinValue == GpioPinValue.High)
                    {
                        pin.pinValue = GpioPinValue.Low;
                        pin.pin.Write(GpioPinValue.Low);

                    }
                    else
                    {
                        pin.pinValue = GpioPinValue.High;
                        pin.pin.Write(GpioPinValue.High);
                    }
                }
                return;
            }
            catch
            {
                throw;
            }
        }

        internal void TurnOff()
        {
            try
            {
                foreach (var pin in pins)
                {
                    if (pin != null)
                    {
                        pin.pinValue = GpioPinValue.High;
                        pin.pin.Write(GpioPinValue.High);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
