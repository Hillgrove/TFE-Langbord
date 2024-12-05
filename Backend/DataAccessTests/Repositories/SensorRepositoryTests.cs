using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Tests
{
    [TestClass()]
    public class SensorRepositoryTests
    {
        private SensorRepository _repository = null!;
        private AppDbContext _context = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new SensorRepository(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod()]
        public void Add_ShouldAddSensor()
        {
            var sensor = new Sensor { Name = "Test Sensor" };
            var result = _repository.Add(sensor);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test Sensor", result.Name);
            Assert.AreEqual(1, _context.Sensors.Count());
        }

        [TestMethod()]
        public void GetAll_ShouldReturnAllSensors()
        {
            _context.Sensors.Add(new Sensor { Name = "Sensor 1" });
            _context.Sensors.Add(new Sensor { Name = "Sensor 2" });
            _context.SaveChanges();

            var result = _repository.GetAll();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod()]
        public void Get_ValidId_ShouldReturnCorrectSensor()
        {
            var sensor = new Sensor { Name = "Test Sensor" };
            _context.Sensors.Add(sensor);
            _context.SaveChanges();

            var result = _repository.Get(sensor.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test Sensor", result.Name);
        }

        [TestMethod()]
        public void GetSensorData_ValidSensorId_ShouldReturnAssociatedSensorData()
        {
            var sensor = new Sensor { Name = "Test Sensor" };
            _context.Sensors.Add(sensor);
            _context.SensorData.Add(new SensorData { SensorId = sensor.Id, Temperature = 25.0 });
            _context.SensorData.Add(new SensorData { SensorId = sensor.Id, Pressure = 1013.0 });
            _context.SaveChanges();

            var result = _repository.GetSensorData(sensor.Id);

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod()]
        public void Delete_ShouldRemoveSensorById()
        {
            var sensor = new Sensor { Name = "Test Sensor" };
            _context.Sensors.Add(sensor);
            _context.SaveChanges();

            _repository.Delete(sensor.Id);

            Assert.AreEqual(0, _context.Sensors.Count());
        }

        [TestMethod()]
        public void Update_ShouldModifyExistingSensor()
        {
            var sensor = new Sensor { Name = "Test Sensor" };
            _context.Sensors.Add(sensor);
            _context.SaveChanges();

            sensor.Name = "Updated Sensor";
            var result = _repository.Update(sensor);

            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Sensor", result.Name);
        }
    }
}
