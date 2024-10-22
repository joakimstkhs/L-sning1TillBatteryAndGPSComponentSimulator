using System;
using System.IO.Ports;
using System.Threading;

namespace BatteryControlUnit
{
    public class BCUSimulator
    {
        public static void Main(string[] args)
        {
                // Set up the serial port
                SerialPort serialPort = new SerialPort
                {
                    PortName = "COM2", // Change COM port as necessary
                    BaudRate = 9600,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Handshake = Handshake.None
                };

                try
                {
                    serialPort.Open();
                    Console.WriteLine(@"BCU Simulator is sending data on COM2...");

                    Random rand = new Random();
                    int batteryLoad = rand.Next(10, 101); // Battery percentage 10 - 100
                    int consumeDistance = 1;

                while (batteryLoad > 0)
                    {
                        // Simulate battery load
                        batteryLoad -= consumeDistance;

                        // Format data as a string
                        string data = $"Battery load:{batteryLoad}%";

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
