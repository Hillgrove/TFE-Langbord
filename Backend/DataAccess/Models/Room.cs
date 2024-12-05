using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Room
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // TODO: make new migration and scaffold to db. Also rename variable
        public double ControlTemperature { get; set; }

        // metode SetFurnaceTemperature(ControlTemperature, CurrentTemperature)
        //{
        //    if (ControlTemperature != CurrentTemperature)
        //    do magic stuff
        //}

        // Navigation Property
        public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
    }

}
