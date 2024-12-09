namespace DataAccess.DTOs
{
    public class SensorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public List<SensorDataDto> SensorData { get; set; } = new List<SensorDataDto>();
    }
}
