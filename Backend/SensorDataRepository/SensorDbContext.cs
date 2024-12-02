using Microsoft.EntityFrameworkCore;
using SensorDataRepository.Models;

namespace SensorDataRepository
{
    public class SensorDbContext : DbContext
    {
        public SensorDbContext(DbContextOptions<SensorDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorData>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }

        public DbSet<SensorData> SensorData { get; set; }
        public DbSet<SensorUnit> SensorUnits { get; set; }
    }
}
