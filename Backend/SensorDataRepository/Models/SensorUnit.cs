namespace SensorDataRepository.Models
{
    public class SensorUnit
    {
        //[Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        //[InverseProperty("SensorUnit")]
        private List<SensorData> SensorData { get; set; } = new List<SensorData>();
    }
}
