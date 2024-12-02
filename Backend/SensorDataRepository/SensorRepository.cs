using Microsoft.EntityFrameworkCore;
using SensorDataRepository.Models;

namespace SensorDataRepository
{
    public class SensorRepository
    {
        private EnviromentSecrets _enviromentSecrets = new EnviromentSecrets();
        private SensorDbContext _context;

        public SensorRepository()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SensorDbContext>();
            optionsBuilder.UseSqlServer(_enviromentSecrets.getDbConnectionString());

            _context = new SensorDbContext(optionsBuilder.Options);
        }

        public void TruncateDb()
        {
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE SensorData");
        }

        public SensorData Add(SensorData sensorData)
        {
            _context.SensorData.Add(sensorData);
            _context.SaveChanges();

            return sensorData;
        }

        public IEnumerable<SensorData> GetAll()
        {
            return _context.SensorData.ToList();
        }
    }
}
