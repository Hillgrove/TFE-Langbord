using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }   


        public DbSet<Room> Rooms { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var secrets = new EnvironmentSecrets();
                optionsBuilder.UseSqlServer(
                    secrets.getDbConnectionString(),
                    b => b.MigrationsAssembly("DataAccess"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Room has many Sensors
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Sensors)
                .WithOne(s => s.Room)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.SetNull);

            // Sensor has many SensorData
            modelBuilder.Entity<Sensor>()
                .HasMany(s => s.SensorData)
                .WithOne(sd => sd.Sensor)
                .HasForeignKey(sd => sd.SensorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
