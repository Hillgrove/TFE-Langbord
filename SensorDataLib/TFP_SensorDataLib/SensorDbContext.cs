using Microsoft.EntityFrameworkCore;

namespace TFP_SensorDataLib
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
		public DbSet<SensorUnit> SensorUnits{ get; set; }
	}
}
