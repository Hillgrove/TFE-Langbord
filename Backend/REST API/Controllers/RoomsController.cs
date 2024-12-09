using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomRepository _repository;

        public RoomsController(RoomRepository repository)
        {
            _repository = repository;
        }

        // POST: api/<RoomsController>
        [HttpPost]
        public ActionResult<Room> Post([FromBody] Room room)
        {
            var addedRoom = _repository.Add(room);
            return Ok(addedRoom);
        }

        // POST api/<RoomsController>/5/sensor
        [HttpPost("{roomId}/sensor")]
        public ActionResult<IEnumerable<Sensor>> AddSensorToRoom(int roomId, [FromBody] Sensor sensor)
        {
            var room = _repository.Get(roomId);
            if (room == null)
            {
                return NotFound($"Room with id {roomId} not found.");
            }

            room.Sensors.Add(sensor);
            _repository.Update(room);
            return Ok(room.Sensors);
        }

        // GET: api/<RoomsController>
        [HttpGet]
        public ActionResult<IEnumerable<Room>> Get()
        {
            var rooms = _repository.GetAll();
            return Ok(rooms);
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public ActionResult<Room> Get(int id)
        {
            var room = _repository.Get(id);
            if (room == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            return Ok(room);
        }

        // GET api/<RoomsController>/recent/5/7
        [HttpGet("recent/{roomId}/{days}")]
        public ActionResult<IEnumerable<SensorData>> GetRecentSensorDataForRoom(int roomId, int days)
        {
            var data = _repository.GetRecentSensorDataForRoom(roomId, days);
            return Ok(data);
        }

        // PUT api/<RoomsController>/5
        [HttpPut("{id}")]
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

            var existingRoom = _repository.Get(id);
            if(existingRoom == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            _repository.Update(room);

            return Ok(room);
        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var room = _repository.Get(id);
            if (room == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            _repository.Delete(id);
            return NoContent();
        }
    }
}
