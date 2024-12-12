using DataAccess.DTOs;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface ISensorRepository
    {
        Sensor Add(Sensor sensor);
        void Delete(int id);
        Sensor? Get(int sensorId);
        IEnumerable<SensorDto> GetAll();
        IEnumerable<SensorData> GetSensorData(int id);
        IEnumerable<object> GetSensorDataGroupedByHour(int id);
        Sensor Update(Sensor sensor);
    }
}