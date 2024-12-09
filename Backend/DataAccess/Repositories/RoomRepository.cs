using DataAccess.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories
{
    public class RoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }


        public Room Add(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            return room;
        }

        public IEnumerable<Room> GetAll()
        {
            return _context.Rooms
                .Include(r => r.Sensors)
                .ThenInclude(s => s.SensorData)
                .ToList();
        }

        // TODO: return list grouped by hour
        public IEnumerable<SensorData> GetRecentSensorDataForRoomGroupedByHour(int roomId, int? days)
        {
            int daysValue = days ?? 14;
            var cutoffDate = DateTime.UtcNow.AddDays(-daysValue);

            var groupedData = _context.SensorData
                .Where(sd => sd.Sensor.RoomId == roomId && sd.Timestamp >= cutoffDate)
                .GroupBy(sd => new
                {
                    sd.Timestamp.Year,
                    sd.Timestamp.Month,
                    sd.Timestamp.Day,
                    sd.Timestamp.Hour
                })
                .Select(g => new SensorData
                {
                    Timestamp = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0),
                    Temperature = Math.Round(g.Average(sd => sd.Temperature),2),
                    Humidity = Math.Round(g.Average(sd => sd.Humidity),2),
                    Pressure = Math.Round(g.Average(sd => sd.Pressure),2)
                })
                .ToList();

            return groupedData;
        }

        public Room? Get(int id)
        {
            //return _context.Rooms.FirstOrDefault(r => r.Id == id);
            var room = _context.Rooms.Include(r => r.Sensors).FirstOrDefault(r => r.Id == id);
            return room;
        }

        public Room? Update(Room room)
        {
            var existingRoom = _context.Rooms.Local.FirstOrDefault(r => r.Id == room.Id);

            if (existingRoom == null)
            {
                return null;
            }

            existingRoom.Name = room.Name;
            _context.SaveChanges();
            return existingRoom;
        }

        public void Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);

            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }

        //public IEnumerable<Sensor>? AddSensorToRoom(int roomId, Sensor sensor)
        public Room? AddSensorToRoom(int roomId, Sensor sensor)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                return null;
            }

            room.Sensors.Add(sensor);
            _context.SaveChanges();

            var updatedRoom = _context.Rooms.Include(r => r.Sensors).FirstOrDefault(r => r.Id == roomId);
            return updatedRoom;
        }

    }
}
