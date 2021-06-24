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

            modelBuilder.Entity<City>()
                .HasData(
                    new City()
                    {
                        Id = 1,
                        Name = "Liverpool",
                        Description = "A fairly well-known rock n roll band was born there."
                    },
                    new City()
                    {
                        Id = 2,
                        Name = "Paris",
                        Description = "The city with a gigantic tower."
                    },
                    new City()
                    {
                        Id = 3,
                        Name = "Antwerp",
                        Description = "The city with a cathedral that was never really finished."
                    },
                    new City()
                    {
                        Id = 4,
                        Name = "Gibraltar",
                        Description = "A little glimpse of Britain in Spaniard lands."
                    }
                );

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                    new PointOfInterest()
                    {
                        Id = 1,
                        Name = "The Beatles Story",
                        Description = "The world’s largest permanent exhibition purely devoted to telling the story of The Beatles’ rise to fame.",
                        CityId = 1
                    },
                    new PointOfInterest()
                    {
                        Id = 2,
                        Name = "Sefton Park",
                        Description = "A magnificent 200-acre Park that looks like a natural landscape rather than a man-made park.",
                        CityId = 1
                    },
                    new PointOfInterest()
                    {
                        Id = 3,
                        Name = "Louvre Museum",
                        Description = "Landmark art museum with vast collection.",
                        CityId = 2
                    },
                    new PointOfInterest()
                    {
                        Id = 4,
                        Name = "Arc de Triomphe",
                        Description = "Triumphal arch and national monument.",
                        CityId = 2
                    },
                    new PointOfInterest()
                    {
                        Id = 5,
                        Name = "Grand Place",
                        Description = "The Grote Markt with its town hall and numerous guild houses is the heart of the old town.",
                        CityId = 3
                    },
                    new PointOfInterest()
                    {
                        Id = 6,
                        Name = "Cathedral of Our Lady",
                        Description = "Belgium's largest Gothic church.",
                        CityId = 3
                    },
                    new PointOfInterest()
                    {
                        Id = 7,
                        Name = "Rock of Gibraltar",
                        Description = "This famous limestone promontory features a nature reserve, a network of tunnels & sea views.",
                        CityId = 4
                    }
                );
        }
    }
}
