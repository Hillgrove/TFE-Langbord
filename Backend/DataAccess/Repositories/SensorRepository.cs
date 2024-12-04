using DataAccess.Models;


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

        public IEnumerable<SensorData> GetSensorData(int id)
        {
            return _context.SensorData
                .Where(sd => sd.SensorId == id)
                .AsEnumerable()
                .Where(sd => !double.IsNaN(sd.Temperature) ||
                             !double.IsNaN(sd.Pressure) ||
                             !double.IsNaN(sd.Humidity))
                .Select(sd => new SensorData
                {
                    Id = sd.Id,
                    SensorId = sd.SensorId,
                    Temperature = sd.Temperature,
                    Pressure = sd.Pressure,
                    Humidity = sd.Humidity,
                    Timestamp = sd.Timestamp
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
