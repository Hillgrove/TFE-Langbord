using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IRoomRepository
    {
        Room Add(Room room);
        Room? AddSensorToRoom(int roomId, Sensor sensor);
        void Delete(int id);
        Room? Get(int id);
        IEnumerable<Room> GetAll();
        IEnumerable<SensorData> GetRecentSensorDataForRoomGroupedByHour(int roomId, int? days);
        Room? Update(Room room);
    }
}