using DataAccess;
using DataAccess.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;


using var dbContext = new AppDbContext();

Console.WriteLine("Receiving Sensor Data (UDP)");

int port = 65000;
UdpClient socket = new UdpClient();
socket.Client.Bind(new IPEndPoint(IPAddress.Any, port));

while (true)
{
    IPEndPoint from = null;

    try
    {
        // Receive and decode UDP data
        byte[] data = socket.Receive(ref from);
        string dataString = Encoding.UTF8.GetString(data);

        var incomingData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(dataString);

        if (incomingData == null || !incomingData.ContainsKey("SerialNumber"))
        {
            Console.WriteLine("Missing SerialNumber. Skipping...");
            continue;
        }

        string serialNumber = incomingData["SerialNumber"].ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(serialNumber))
        {
            Console.WriteLine("Invalid SerialNumber. Skipping...");
            continue;
        }

        // Extract sensor readings
        double temperature = incomingData["Temperature"].GetDouble();
        double humidity = incomingData["Humidity"].GetDouble();
        double pressure = incomingData["Pressure"].GetDouble();

        // Check if the Sensor exists in the database and if not, create it
        var sensor = dbContext.Sensors.FirstOrDefault(s => s.SerialNumber == serialNumber);

        if (sensor == null)
        {
            sensor = new Sensor
            {
                Name = $"Sensor {serialNumber}",
                SerialNumber = serialNumber,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.Sensors.Add(sensor);
            dbContext.SaveChanges();
        }

        // Add SensorData to the database
        var sensorData = new SensorData
        {
            SensorId = sensor.Id,
            Temperature = temperature,
            Humidity = humidity,
            Pressure = pressure,
            Timestamp = DateTime.UtcNow
        };

        dbContext.SensorData.Add(sensorData);
        dbContext.SaveChanges();

        Console.WriteLine($"Data saved for Sensor {sensor.SerialNumber} from {from.Address}");
    }

    catch (JsonException ex)
    {
        Console.WriteLine($"Failed to parse incoming JSON: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}