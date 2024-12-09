namespace DataAccess.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public double TargetTemperature { get; set; }
        public List<SensorDto> Sensors { get; set; } = new List<SensorDto>();
    }
}