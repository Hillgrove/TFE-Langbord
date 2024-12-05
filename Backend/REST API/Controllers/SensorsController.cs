using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly SensorRepository _repository;

        public SensorsController(SensorRepository repository)
        {
            _repository = repository;
        }


        // GET: api/<SensorsController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult <IEnumerable<Sensor>> Get()
        {
            IEnumerable<Sensor> sensors = _repository.GetAll();
            return Ok(sensors);
        }

        // GET api/<SensorsController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Sensor> Get(int id)
        {
            var sensor = _repository.Get(id);
            if(sensor == null)
            {
                return NotFound($"Sensor with id {id} not found.");
            }
            return Ok(sensor);
        }


        // GET api/<SensorsController>/5/data
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/data")]
        public ActionResult<IEnumerable<SensorData>> GetSensorData(int id)
        {
            var sensor = _repository.Get(id);
            if (sensor == null)
            {
                return NotFound($"No data found for sensor with id {id}.");
            }
            return Ok(_repository.GetSensorData(id));
        }


        // DELETE api/<SensorsController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
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

        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Sensor? sensor)
        {
            if (sensor == null)
            {
                return BadRequest("Sensor data is null.");
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
