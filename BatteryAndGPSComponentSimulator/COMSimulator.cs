
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
                // Anvönda Haversine för att få sträcka: https://en.wikipedia.org/wiki/Haversine_formula
                double R = 6371e3;
                double lat1, lon1, distance, speed; 
                
                while (batteryLoad > 0)
                {
                    lat1 = longitude;
                    lon1 = longitude;
                    // Simulate GPS data and battery load
                    latitude += rand.NextDouble();
                    longitude += rand.NextDouble();
                    batteryLoad -= consumeDistance;

                    double deltaPhi = (latitude - lat1) * Math.PI / 180;
                    double deltaLambda = (longitude - lon1) * Math.PI / 180;
                    double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                               Math.Cos(lat1 * Math.PI / 180) * Math.Cos(latitude * Math.PI / 180) *
                               Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
                    distance = R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                    speed = distance / 2;

                    // Format data as a string
                    string data = $"GPS: Lat:{latitude:F6}, Lon:{longitude:F6}, Battery:{batteryLoad}%, Speed:{speed:F2} m/s, Distance:{distance:F2} m";

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


