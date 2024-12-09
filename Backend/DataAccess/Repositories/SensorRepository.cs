using DataAccess.DTOs;
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

        public IEnumerable<SensorDto> GetAll()
        {
            return _context.Sensors
                           .Select(s => new SensorDto
                           {
                               Id = s.Id,
                               Name = s.Name,
                               SerialNumber = s.SerialNumber

                           })
                           .ToList();
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
            var existingSensor = _context.Sensors.Local.FirstOrDefault(s => s.Id == sensor.Id);
            if (existingSensor != null)
            {
                _context.Entry(existingSensor).State = EntityState.Detached;
            }

            _context.Sensors.Update(sensor);
            _context.SaveChanges();
            return sensor;
        }

        public IEnumerable<object> GetSensorDataGroupedByHour(int id)
        {
            return _context.SensorData
                .Where(sd => sd.SensorId == id)
                .AsEnumerable()
                .Where(sd => !double.IsNaN(sd.Temperature) ||
                             !double.IsNaN(sd.Pressure) ||
                             !double.IsNaN(sd.Humidity))
                .GroupBy(sd => new
                {
                    sd.Timestamp.Year,
                    sd.Timestamp.Month,
                    sd.Timestamp.Day,
                    sd.Timestamp.Hour
                })
                .Select(group => new
                {
                    SensorId = id,
                    Temperature = Math.Round(group.Average(sd => sd.Temperature), 2),
                    Pressure = Math.Round(group.Average(sd => sd.Pressure), 2),
                    Humidity = Math.Round(group.Average(sd => sd.Humidity), 2),
                    Timestamp = new DateTime(group.Key.Year, group.Key.Month, group.Key.Day, group.Key.Hour, 0, 0)
                })
                .ToList();
        }
    }
}