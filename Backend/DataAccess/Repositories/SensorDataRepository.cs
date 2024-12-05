﻿using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class SensorDataRepository
    {
        private readonly AppDbContext _context;

        public SensorDataRepository(AppDbContext context)
        {
            _context = context;
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

        public IEnumerable<SensorData> GetRecentSensorData(int days)
        {
            var cutoffData = DateTime.UtcNow.AddDays(-days);
            return _context.SensorData.Where(s => s.Timestamp >= cutoffData).ToList();
        }

        public SensorData? Get(int id)
        {
            return _context.SensorData.FirstOrDefault(s => s.Id == id);
        }

        public int DeleteOlderThan(DateTime cutoffDate)
        {
            var sensorDataToDelete = _context.SensorData.Where(s => s.Timestamp < cutoffDate).ToList();

            if (sensorDataToDelete.Any())
            {
                _context.SensorData.RemoveRange(sensorDataToDelete);
                return _context.SaveChanges();
            }
            return 0; 
        }
    }
}