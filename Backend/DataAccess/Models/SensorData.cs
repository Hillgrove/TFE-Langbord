using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class SensorData
    {
        public int Id { get; set; }

        [ForeignKey("Sensor")]
        public int SensorId { get; set; }

        [Range(-50, 100)]
        public double Temperature { get; set; }

        [Range(0, 100)]
        public double Humidity { get; set; }

        [Range(900, 1100)]
        public double Pressure { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public Sensor Sensor { get; set; } = null!;
    }
}
