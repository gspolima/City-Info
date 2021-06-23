using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Contexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<City>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<City>()
                .Property(c => c.Description)
                .HasMaxLength(200);

            modelBuilder.Entity<PointOfInterest>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PointOfInterest>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<PointOfInterest>()
                .Property(p => p.Description)
                .HasMaxLength(200);
        }
    }
}
