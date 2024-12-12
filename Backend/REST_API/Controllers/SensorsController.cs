using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using DataAccess.DTOs;
using DataAccess.Interfaces;


namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorRepository _repository;

        public SensorsController(ISensorRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<SensorsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SensorDto>> Get()
        {
            var sensors = _repository.GetAll();
            return Ok(sensors);
        }

        // GET api/<SensorsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Sensor> Get(int id)
        {
            var sensor = _repository.Get(id);
            if (sensor == null)
            {
                return NotFound($"Sensor with id {id} not found.");
            }
            return Ok(sensor);
        }

        // GET api/<SensorsController>/5/data
        [HttpGet("{id}/data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SensorData>> GetSensorData(int id)
        {
            var sensor = _repository.Get(id);
            if (sensor == null)
            {
                return NotFound($"No data found for sensor with id {id}.");
            }
            return Ok(_repository.GetSensorData(id));
        }

        // GET api/sensors/{id}/grouped-by-hour
        [HttpGet("{id}/grouped-by-hour")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetSensorDataGroupedByHour(int id)
        {
            var data = _repository.GetSensorDataGroupedByHour(id);
            if (data == null)
            {
                return NotFound($"No sensor data found for sensor with id {id}.");
            }

            return Ok(data);
        }

        // DELETE api/<SensorsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            var sensor = _repository.Get(id);
            if (sensor == null)
            {
                return NotFound($"Sensor with id {id} not found.");
            }
            _repository.Delete(id);
            return Ok($"Sensor with id {id} deleted.");
        }

        // PUT api/<SensorsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Put(int id, [FromBody] Sensor? sensor)
        {
            if (sensor == null)
            {
                return BadRequest("Sensor data is missing.");
            }

            if (id != sensor.Id)
            {
                return BadRequest("Sensor ID mismatch.");
            }

            var existingSensor = _repository.Get(sensor.Id);
            if (existingSensor == null)
            {
                return NotFound($"Sensor with id {sensor.Id} not found.");
            }

            _repository.Update(sensor);
            return Ok(sensor);
        }
    }
}
