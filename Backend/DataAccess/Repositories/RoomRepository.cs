using DataAccess.Models;


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
            return _context.Rooms.ToList();
        }


        public IEnumerable<SensorData> GetRecentSensorDataForRoom(int roomId, int days)
        {
            // find all sensor data for the specific room and within the last x days
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return _context.SensorData
                           .Where(sd => sd.Sensor.RoomId == roomId && sd.Timestamp >= cutoffDate)
                           .ToList();
        }

        public Room? Get(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.Id == id);
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
    }
}
