using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using SensorDataRepository;
using SensorDataRepository.Models;

Console.WriteLine("Receiving Sensor Data (UDP)");

int port = 65000;
UdpClient socket = new UdpClient();
socket.Client.Bind(new IPEndPoint(IPAddress.Any, port));
SensorRepository _repo = new SensorRepository();

while (true)
{
	IPEndPoint from = null;

	byte[] data = socket.Receive(ref from);
	string dataString = Encoding.UTF8.GetString(data);

	// TODO: We need some sort of validation - if the data is not correctly formatted as JSON our script fails.
	SensorData sensorData = JsonSerializer.Deserialize<SensorData>(dataString);
	_repo.Add(sensorData);

	Console.WriteLine("Data Received " + from.Address);
}