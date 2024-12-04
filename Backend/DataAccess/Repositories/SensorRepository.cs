using DataAccess.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories
{
    
    public class SensorRepository
    {
        
        private readonly AppDbContext _context;
        public SensorRepository(AppDbContext context)
        {
            _context = context;
        }

        public Sensor Add(Sensor sensor)
        {
            _context.Sensors.Add(sensor);
            _context.SaveChanges();
            return sensor;
        }

        public IEnumerable<Sensor> GetAll()
        {
            return _context.Sensors.ToList();
        }


        public Sensor? Get(int id)
        {
            return _context.Sensors.FirstOrDefault(s => s.Id == id);
        }

        // Updateret: Nu viser den specifikt kun data, hvor temperature, pressure og humidity er sat
        // Det gør den ved at tjekke om værdierne er NaN, og hvis de er, så bliver de ikke vist
        // Dette er for at undgå at vise data, hvor der ikke er nogen værdier eller som bare spilder plads
        // Jeg bruger også AsEnumerable() for at hente data fra databasen, da det er hurtigere end at bruge LINQ (apparently)
        public IEnumerable<SensorData> GetSensorData(int id)
        {
            return _context.SensorData
                .AsEnumerable() 
                .Where(sd => sd.SensorId == id &&
                             (!double.IsNaN(sd.Temperature) ||
                              !double.IsNaN(sd.Pressure) ||
                              !double.IsNaN(sd.Humidity)))
                .Select(sd => new SensorData
                {
                    Id = sd.Id,
                    SensorId = sd.SensorId,
                    Temperature = sd.Temperature,
                    Pressure = sd.Pressure,
                    Humidity = sd.Humidity
                })
                .ToList();
        }

        public void Delete(int id)
        {
            var sensor = _context.Sensors.FirstOrDefault(s => s.Id == id);
            if (sensor != null)
            {
                _context.Sensors.Remove(sensor);
                _context.SaveChanges();
            }
           
        }

        public Sensor Update(Sensor sensor)
        {
            _context.Sensors.Update(sensor);
            _context.SaveChanges();
            return sensor;
        }

    }
    
}
