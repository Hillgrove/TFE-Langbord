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
        public IEnumerable<SensorData> GetRecentSensorDataForRoom(int roomId, int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return _context.SensorData
                           .Where(sd => sd.Sensor.RoomId == roomId && sd.Timestamp >= cutoffDate)
                           .ToList();
        }

        public Room? Get(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.Id == id);
        }

        public Room Update(Room room)
        {
            var existingRoom = _context.Rooms.Local.FirstOrDefault(r => r.Id == room.Id);

            // TODO: remove this?
            //if (existingRoom != null)
            //{
            //    _context.Entry(existingRoom).State = EntityState.Detached;
            //}

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

        public IEnumerable<Sensor>? AddSensorToRoom(int roomId, Sensor sensor)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);

            if (room != null)
            {
                room.Sensors.Add(sensor);
                _context.SaveChanges();
            }

            return room?.Sensors;
        }
    }
}
