using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using DataAccess.Models;

namespace REST_API.Controllers
{
    [Route("api/sensor")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly SensorDataRepository? _repository;

        public SensorsController(SensorDataRepository repository)
        {
            _repository = repository;
        }
        
        // GET: api/<SensorsController>
        [HttpGet]
        public IEnumerable<SensorData> Get()
        {
            return _repository.GetAll();
        }

        // GET api/<SensorsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SensorsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] SensorData sensorData)
        {
            _repository.Add(sensorData);
            return Ok(sensorData);
        }

        // PUT api/<SensorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SensorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{id}/data")]
        public IActionResult GetSensorData(int id)
        {
            // Her returneres eksempeldata for sensoren med ID'et
            var sensorData = new
            {
                SensorId = id,
                Timestamp = DateTime.UtcNow,
                Value = 42.0 // F.eks. temperaturværdi
            };
            return Ok(sensorData);
        }
    }
}