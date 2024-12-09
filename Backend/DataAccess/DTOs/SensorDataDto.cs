namespace DataAccess.DTOs
{
    public class SensorDataDto
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

