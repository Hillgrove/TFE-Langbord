using DataAccess.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories.Tests
{
    [TestClass]
    public class SensorDataRepositoryTests
    {
        private SensorDataRepository _repository = null!;
        private AppDbContext _context = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new SensorDataRepository(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod]
        public void Add_ShouldAddSensorData()
        {
            // Arrange
            var sensorData = new SensorData
            {
                SensorId = 1,
                Temperature = 25.0,
                Humidity = 50.0,
                Pressure = 1000.0,
                Timestamp = DateTime.Now
            };

            // Act
            var result = _repository.Add(sensorData);


            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, _context.SensorData.Count());
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllSensorData()
        {
            // Arrange
            var sensorData1 = new SensorData { SensorId = 1, Temperature = 25.0, Humidity = 50.0, Pressure = 1000.0 };
            var sensorData2 = new SensorData { SensorId = 2, Temperature = 26.0, Humidity = 51.0, Pressure = 1001.0 };

            _context.SensorData.AddRange(sensorData1, sensorData2);
            _context.SaveChanges();

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Get_ValidId_ReturnsCorrectSensorData()
        {
            var sensorData = new SensorData { SensorId = 1, Temperature = 25.0, Humidity = 50.0, Pressure = 1000.0, Timestamp = DateTime.Now };
            _context.SensorData.Add(sensorData);
            _context.SaveChanges();

            var result = _repository.Get(sensorData.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(sensorData.Id, result.Id);
        }

        [TestMethod]
        public void Get_InvalidId_ReturnsNull()
        {
            var sensorData = new SensorData { SensorId = 1, Temperature = 25.0, Humidity = 50.0, Pressure = 1000.0, Timestamp = DateTime.Now };
            _context.SensorData.Add(sensorData);
            _context.SaveChanges();

            var result = _repository.Get(sensorData.Id + 1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRecentSensorData_ShouldReturnOnlyRecentSensorData()
        {
            // Arrange
            var recentSensorData = new SensorData
            {
                SensorId = 1,
                Temperature = 25.0,
                Humidity = 50.0,
                Pressure = 1000.0,
                Timestamp = DateTime.UtcNow.AddDays(-1)
            };
            var oldSensorData = new SensorData
            {
                SensorId = 2,
                Temperature = 26.0,
                Humidity = 51.0,
                Pressure = 1001.0,
                Timestamp = DateTime.UtcNow.AddDays(-10)
            };

            _context.SensorData.AddRange(recentSensorData, oldSensorData);
            _context.SaveChanges();

            // Act
            var result = _repository.GetRecentSensorData(7).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(recentSensorData.Id, result.First().Id);
        }

        [TestMethod]
        public void DeleteOlderThan_CutoffDatePassed_DeletesOldSensorData()
        {
            var oldSensorData = new SensorData { SensorId = 1, Temperature = 25.0, Humidity = 50.0, Pressure = 1000.0, Timestamp = DateTime.Now.AddYears(-1) };
            var newSensorData = new SensorData { SensorId = 2, Temperature = 26.0, Humidity = 51.0, Pressure = 1001.0, Timestamp = DateTime.Now };

            _context.SensorData.AddRange(oldSensorData, newSensorData);
            _context.SaveChanges();

            var result = _repository.DeleteOlderThan(DateTime.Now.AddMonths(-6));

            Assert.AreEqual(1, result);
            Assert.AreEqual(1, _context.SensorData.Count());
        }
    }
}
