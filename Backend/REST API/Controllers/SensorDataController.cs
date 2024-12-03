using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly SensorDataRepository _repository;

        public SensorDataController(SensorDataRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<SensorDataController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SensorData>> Get()
        {
            IEnumerable<SensorData> data = _repository.GetAll();
            return Ok(data);
        }

        // GET api/<SensorDataController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SensorData?> Get(int id)
        {
            var sensorData = _repository.Get(id);
            if (sensorData == null)
            {
                return NotFound($"Sensordata with id {id} not found.");
            }

            return Ok(sensorData);
        }

        // DELETE api/sensordata/older-than/{days}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("older-than/{days}")]
        public ActionResult DeleteOldSensorData(int days)
        {
            if(days < 0)
    {
                return BadRequest("The number of days must be a positive integer.");
            }

            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            var deletedCount = _repository.DeleteOlderThan(cutoffDate);

            if (deletedCount > 0)
            {
                return Ok($"{deletedCount} data entires deleted."); 
            }
            else
            {
                return NoContent();
            }
        }
    }
}
