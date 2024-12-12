using DataAccess.DTOs;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;


namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ISensorRepository _sensorRepository;

        public RoomsController(IRoomRepository roomRepository, ISensorRepository sensorRepository)
        {
            _roomRepository = roomRepository;
            _sensorRepository = sensorRepository;
        }

        // POST: api/<RoomsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Room> Post([FromBody] Room room)
        {
            var addedRoom = _roomRepository.Add(room);
            return Ok(addedRoom);
        }

        // POST api/<RoomsController>/5/addsensor/10
        [HttpPost("{id}/addsensor/{sensorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RoomDto> AddSensorToRoom(int id, int sensorId)
        {
            var room = _roomRepository.Get(id);
            if (room == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            var sensor = _sensorRepository.Get(sensorId);
            if (sensor == null)
            {
                return NotFound($"Sensor with id {sensorId} not found.");
            }

            if (sensor.RoomId == id)
            {
                return Ok($"Sensor with id {sensorId} already added.");
            }

            var updatedRoom = _roomRepository.AddSensorToRoom(id, sensor);
            if (updatedRoom == null)
            {
                return StatusCode(500, "An error occurred while adding the sensor to the room.");
            }

            var roomDto = new RoomDto
            {
                Id = updatedRoom.Id,
                Name = updatedRoom.Name,
                CreatedDate = updatedRoom.CreatedDate,
                TargetTemperature = updatedRoom.TargetTemperature,
                Sensors = updatedRoom.Sensors.Select(s => new SensorDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    SerialNumber = s.SerialNumber
                }).ToList()
            };

            return Ok(roomDto);
        }

        // GET: api/<RoomsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<RoomDto>> Get()
        {
            var rooms = _roomRepository.GetAll();

            var roomDtos = rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                CreatedDate = r.CreatedDate,
                TargetTemperature = r.TargetTemperature,
                Sensors = r.Sensors.Select(s => new SensorDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    SerialNumber = s.SerialNumber,
                    SensorData = s.SensorData.Select(sd => new SensorDataDto
                    {
                        Id = sd.Id,
                        SensorId = sd.SensorId,
                        Temperature = sd.Temperature,
                        Humidity = sd.Humidity,
                        Pressure = sd.Pressure,
                        Timestamp = sd.Timestamp
                    }).OrderByDescending(sd => sd.Id)
                    .ToList()
                }).ToList()
            }).ToList();

            return Ok(roomDtos);
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Room> Get(int id)
        {
            var room = _roomRepository.Get(id);
            if (room == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            var roomDto = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                CreatedDate = room.CreatedDate,
                TargetTemperature = room.TargetTemperature,
                Sensors = room.Sensors.Select(s => new SensorDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    SerialNumber = s.SerialNumber
                }).ToList()
            };

            return Ok(roomDto);
        }

        // GET api/<RoomsController>/5/data/recent
        [HttpGet("{id}/data/recent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SensorData>> GetRecentSensorDataForRoomGroupedByHour(int id, [FromQuery] int? days = null)
        {
            var data = _roomRepository.GetRecentSensorDataForRoomGroupedByHour(id, days);

            return Ok(data);
        }

        // PUT api/<RoomsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Put(int id, [FromBody] Room? room)
        {
            if (room == null)
            {
                return BadRequest("Room data is missing.");
            }

            if (id != room.Id)
            {
                return BadRequest("Room id mismatch.");
            }

            var existingRoom = _roomRepository.Get(id);
            if (existingRoom == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            _roomRepository.Update(room);
            return Ok(room);
        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var room = _roomRepository.Get(id);
            if (room == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            _roomRepository.Delete(id);
            return NoContent();
        }
    }
}
