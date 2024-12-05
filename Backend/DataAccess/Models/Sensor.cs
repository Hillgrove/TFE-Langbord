using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string SerialNumber { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("Room")]
        public int? RoomId { get; set; }


        // Navigation Property
        public Room? Room { get; set; }
        public ICollection<SensorData> SensorData { get; set; } = new List<SensorData>();
    }
        
}
