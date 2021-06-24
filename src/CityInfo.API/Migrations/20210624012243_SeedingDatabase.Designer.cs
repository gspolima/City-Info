﻿// <auto-generated />
using CityInfo.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CityInfo.API.Migrations
{
    [DbContext(typeof(CityInfoContext))]
    [Migration("20210624012243_SeedingDatabase")]
    partial class SeedingDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CityInfo.API.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new { Id = 1, Description = "A fairly well-known rock n roll band was born there.", Name = "Liverpool" },
                        new { Id = 2, Description = "The city with a gigantic tower.", Name = "Paris" },
                        new { Id = 3, Description = "The city with a cathedral that was never really finished.", Name = "Antwerp" },
                        new { Id = 4, Description = "A little glimpse of Britain in Spaniard lands.", Name = "Gibraltar" }
                    );
                });

            modelBuilder.Entity("CityInfo.API.Entities.PointOfInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PointsOfInterest");

                    b.HasData(
                        new { Id = 1, CityId = 1, Description = "The world’s largest permanent exhibition purely devoted to telling the story of The Beatles’ rise to fame.", Name = "The Beatles Story" },
                        new { Id = 2, CityId = 1, Description = "A magnificent 200-acre Park that looks like a natural landscape rather than a man-made park.", Name = "Sefton Park" },
                        new { Id = 3, CityId = 2, Description = "Landmark art museum with vast collection.", Name = "Louvre Museum" },
                        new { Id = 4, CityId = 2, Description = "Triumphal arch and national monument.", Name = "Arc de Triomphe" },
                        new { Id = 5, CityId = 3, Description = "The Grote Markt with its town hall and numerous guild houses is the heart of the old town.", Name = "Grand Place" },
                        new { Id = 6, CityId = 3, Description = "Belgium's largest Gothic church.", Name = "Cathedral of Our Lady" },
                        new { Id = 7, CityId = 4, Description = "This famous limestone promontory features a nature reserve, a network of tunnels & sea views.", Name = "Rock of Gibraltar" }
                    );
                });

            modelBuilder.Entity("CityInfo.API.Entities.PointOfInterest", b =>
                {
                    b.HasOne("CityInfo.API.Entities.City", "City")
                        .WithMany("PointsOfInterest")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
