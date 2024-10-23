
using System.IO.Ports;
using System.Text.Json;
using Microsoft.Extensions.Configuration;




namespace BatteryAndGPSComponentSimulator
{
    public class COMSimulator
    {
        public static void Main()
        {
            var appsettings = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            // Set up the serial port
            SerialPort serialPort = new SerialPort
            {
                PortName = appsettings["PortName"],
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None
            };

            try
            {
                serialPort.Open();
                Console.WriteLine("COM Simulator is sending data...");

                Random rand = new Random();
                int batteryLoad = rand.Next(10, 101); // Battery percentage 10 - 100
                double latitude = 59.8;
                double longitude = 17.5;
                int consumeDistance = 1;
                while (batteryLoad > 0)
                {
                    // Simulate GPS data and battery load
                    latitude += rand.NextDouble();
                    longitude += rand.NextDouble();
                    batteryLoad -= consumeDistance; 

                    // Format data as a string
                    string data = $"GPS: Lat:{latitude:F6}, Lon:{longitude:F6}, Battery:{batteryLoad}%";

                    // Send data via the serial port
                    serialPort.WriteLine(data);

                    Console.WriteLine($"Sent: {data}");

                    // Wait for 2 seconds before sending the next data packet
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (serialPort.IsOpen)
                    serialPort.Close();
            }

        }
    }
}


