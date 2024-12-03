namespace UDP_Server
{
    public class IncomingSensorData
    {
        public string SerialNumber { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }

}
