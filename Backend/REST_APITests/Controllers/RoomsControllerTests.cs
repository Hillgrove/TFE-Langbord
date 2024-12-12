using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace REST_API.Controllers.Tests
{
    [TestClass()]
    public class RoomsControllerTests
    {
        private Mock<IRoomRepository> _mockRoomRepository;
        private Mock<ISensorRepository> _mockSensorRepository;
        private RoomsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRoomRepository = new Mock<IRoomRepository>();
            _mockSensorRepository = new Mock<ISensorRepository>();
            _controller = new RoomsController(_mockRoomRepository.Object, _mockSensorRepository.Object);
        }

        [TestMethod()]
        public void AddSensorToRoom_RoomNotFound_ReturnsNotFound()
        {
            // Arrange
            int roomId = 1;
            int sensorId = 1;
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns((Room)null);

            // Act
            var result = _controller.AddSensorToRoom(roomId, sensorId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void AddSensorToRoom_SensorNotFound_ReturnsNotFound()
        {
            // Arrange
            int roomId = 1;
            int sensorId = 1;
            var room = new Room { Id = roomId };
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns(room);
            _mockSensorRepository.Setup(repo => repo.Get(sensorId)).Returns((Sensor)null);

            // Act
            var result = _controller.AddSensorToRoom(roomId, sensorId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void AddSensorToRoom_SensorAlreadyInRoom_ReturnsOk()
        {
            // Arrange
            int roomId = 1;
            int sensorId = 1;
            var room = new Room { Id = roomId };
            var sensor = new Sensor { Id = sensorId, RoomId = roomId };
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns(room);
            _mockSensorRepository.Setup(repo => repo.Get(sensorId)).Returns(sensor);

            // Act
            var result = _controller.AddSensorToRoom(roomId, sensorId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void AddSensorToRoom_ValidRequest_ReturnsOk()
        {
            // Arrange
            int roomId = 1;
            int sensorId = 1;
            var room = new Room { Id = roomId, Sensors = new List<Sensor>() };
            var sensor = new Sensor { Id = sensorId, RoomId = null };
            var updatedRoom = new Room { Id = roomId, Sensors = new List<Sensor> { sensor } };
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns(room);
            _mockSensorRepository.Setup(repo => repo.Get(sensorId)).Returns(sensor);
            _mockRoomRepository.Setup(repo => repo.AddSensorToRoom(roomId, sensor)).Returns(updatedRoom);

            // Act
            var result = _controller.AddSensorToRoom(roomId, sensorId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void AddSensorToRoom_AddSensorFails_ReturnsInternalServerError()
        {
            // Arrange
            int roomId = 1;
            int sensorId = 1;
            var room = new Room { Id = roomId, Sensors = new List<Sensor>() };
            var sensor = new Sensor { Id = sensorId, RoomId = null };
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns(room);
            _mockSensorRepository.Setup(repo => repo.Get(sensorId)).Returns(sensor);
            _mockRoomRepository.Setup(repo => repo.AddSensorToRoom(roomId, sensor)).Returns((Room)null);

            // Act
            var result = _controller.AddSensorToRoom(roomId, sensorId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [TestMethod()]
        public void Post_ValidRoom_ReturnsOk()
        {
            // Arrange
            var room = new Room { Id = 1, Name = "Room1" };
            _mockRoomRepository.Setup(repo => repo.Add(room)).Returns(room);

            // Act
            var result = _controller.Post(room);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void Get_ReturnsAllRooms()
        {
            // Arrange
            var rooms = new List<Room>
            {
                new Room { Id = 1, Name = "Room1" },
                new Room { Id = 2, Name = "Room2" }
            };
            _mockRoomRepository.Setup(repo => repo.GetAll()).Returns(rooms);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void Get_RoomNotFound_ReturnsNotFound()
        {
            // Arrange
            int roomId = 1;
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns((Room)null);

            // Act
            var result = _controller.Get(roomId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void Get_ValidRoom_ReturnsOk()
        {
            // Arrange
            int roomId = 1;
            var room = new Room { Id = roomId, Name = "Room1" };
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns(room);

            // Act
            var result = _controller.Get(roomId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void GetRecentSensorDataForRoomGroupedByHour_ReturnsData()
        {
            // Arrange
            int roomId = 1;
            var sensorData = new List<SensorData>
            {
                new SensorData { Id = 1, SensorId = 1, Temperature = 25.0 },
                new SensorData { Id = 2, SensorId = 1, Temperature = 26.0 }
            };
            _mockRoomRepository.Setup(repo => repo.GetRecentSensorDataForRoomGroupedByHour(roomId, null)).Returns(sensorData);

            // Act
            var result = _controller.GetRecentSensorDataForRoomGroupedByHour(roomId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void Put_RoomDataMissing_ReturnsBadRequest()
        {
            // Arrange
            int roomId = 1;

            // Act
            var result = _controller.Put(roomId, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod()]
        public void Put_RoomIdMismatch_ReturnsBadRequest()
        {
            // Arrange
            int roomId = 1;
            var room = new Room { Id = 2, Name = "Room1" };

            // Act
            var result = _controller.Put(roomId, room);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod()]
        public void Put_RoomNotFound_ReturnsNotFound()
        {
            // Arrange
            int roomId = 1;
            var room = new Room { Id = roomId, Name = "Room1" };
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns((Room)null);

            // Act
            var result = _controller.Put(roomId, room);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void Put_ValidRoom_ReturnsOk()
        {
            // Arrange
            int roomId = 1;
            var room = new Room { Id = roomId, Name = "Room1" };
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns(room);
            _mockRoomRepository.Setup(repo => repo.Update(room)).Returns(room);

            // Act
            var result = _controller.Put(roomId, room);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void Delete_RoomNotFound_ReturnsNotFound()
        {
            // Arrange
            int roomId = 1;
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns((Room)null);

            // Act
            var result = _controller.Delete(roomId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void Delete_ValidRoom_ReturnsNoContent()
        {
            // Arrange
            int roomId = 1;
            var room = new Room { Id = roomId, Name = "Room1" };
            _mockRoomRepository.Setup(repo => repo.Get(roomId)).Returns(room);

            // Act
            var result = _controller.Delete(roomId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}