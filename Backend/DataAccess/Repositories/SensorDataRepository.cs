using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SensorDataRepository
    {
        private readonly AppDbContext _context;

        public SensorDataRepository(AppDbContext context)
        {
            _context = context;
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