using DataAccess.DTOs;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace REST_API.Controllers.Tests
{
    [TestClass()]
    public class SensorsControllerTests
    {
        private Mock<ISensorRepository> _mockRepository;
        private SensorsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<ISensorRepository>();
            _controller = new SensorsController(_mockRepository.Object);
        }

        [TestMethod()]
        public void Get_ReturnsAllSensors()
        {
            // Arrange
            var sensors = new List<SensorDto>
            {
                new SensorDto { Id = 1, Name = "Sensor1", SerialNumber = "SN1" },
                new SensorDto { Id = 2, Name = "Sensor2", SerialNumber = "SN2" }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(sensors);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedSensors = okResult.Value as IEnumerable<SensorDto>;
            Assert.AreEqual(2, returnedSensors.Count());
        }

        [TestMethod()]
        public void Get_WithValidId_ReturnsSensor()
        {
            // Arrange
            var sensor = new Sensor { Id = 1, Name = "Sensor1", SerialNumber = "SN1" };
            _mockRepository.Setup(repo => repo.Get(1)).Returns(sensor);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedSensor = okResult.Value as Sensor;
            Assert.AreEqual(1, returnedSensor.Id);
        }

        [TestMethod()]
        public void Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(1)).Returns((Sensor)null);

            // Act
            var result = _controller.Get(1);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Sensor with id 1 not found.", notFoundResult.Value);
        }

        [TestMethod()]
        public void GetSensorData_WithValidId_ReturnsSensorData()
        {
            // Arrange
            var sensorData = new List<SensorData>
            {
                new SensorData { Id = 1, SensorId = 1, Temperature = 25, Humidity = 50, Pressure = 1000 },
                new SensorData { Id = 2, SensorId = 1, Temperature = 26, Humidity = 51, Pressure = 1001 }
            };
            _mockRepository.Setup(repo => repo.Get(1)).Returns(new Sensor { Id = 1 });
            _mockRepository.Setup(repo => repo.GetSensorData(1)).Returns(sensorData);

            // Act
            var result = _controller.GetSensorData(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedSensorData = okResult.Value as IEnumerable<SensorData>;
            Assert.AreEqual(2, returnedSensorData.Count());
        }

        [TestMethod()]
        public void GetSensorData_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(1)).Returns((Sensor)null);

            // Act
            var result = _controller.GetSensorData(1);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("No data found for sensor with id 1.", notFoundResult.Value);
        }

        [TestMethod()]
        public void Delete_WithValidId_DeletesSensor()
        {
            // Arrange
            var sensor = new Sensor { Id = 1, Name = "Sensor1", SerialNumber = "SN1" };
            _mockRepository.Setup(repo => repo.Get(1)).Returns(sensor);
            _mockRepository.Setup(repo => repo.Delete(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Sensor with id 1 deleted.", okResult.Value);
        }

        [TestMethod()]
        public void Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(1)).Returns((Sensor)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Sensor with id 1 not found.", notFoundResult.Value);
        }

        [TestMethod()]
        public void Put_WithValidSensor_UpdatesSensor()
        {
            // Arrange
            var sensor = new Sensor { Id = 1, Name = "UpdatedSensor", SerialNumber = "SN1" };
            _mockRepository.Setup(repo => repo.Get(1)).Returns(sensor);
            _mockRepository.Setup(repo => repo.Update(sensor)).Returns(sensor);

            // Act
            var result = _controller.Put(1, sensor);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var updatedSensor = okResult.Value as Sensor;
            Assert.AreEqual("UpdatedSensor", updatedSensor.Name);
        }

        [TestMethod()]
        public void Put_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var sensor = new Sensor { Id = 1, Name = "UpdatedSensor", SerialNumber = "SN1" };
            _mockRepository.Setup(repo => repo.Get(1)).Returns((Sensor)null);

            // Act
            var result = _controller.Put(1, sensor);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Sensor with id 1 not found.", notFoundResult.Value);
        }

        [TestMethod()]
        public void Put_WithMismatchedId_ReturnsBadRequest()
        {
            // Arrange
            var sensor = new Sensor { Id = 2, Name = "UpdatedSensor", SerialNumber = "SN1" };

            // Act
            var result = _controller.Put(1, sensor);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Sensor ID mismatch.", badRequestResult.Value);
        }

        [TestMethod()]
        public void Put_WithNullSensor_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Put(1, null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Sensor data is missing.", badRequestResult.Value);
        }
    }
}