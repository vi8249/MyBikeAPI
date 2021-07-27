using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Models;

namespace YouBikeAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<BikeStation> BikeStations { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Price> Prices { get; set; }

        //public DbSet<ElectricBike> ElectricBikes { get; set; }
        //public DbSet<HybridBike> HybridBikes { get; set; }
        //public DbSet<RoadBike> RoadBikes { get; set; }
        //public DbSet<LadyBike> LadyBikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //TPC
            //builder.Entity<Bike>()
            //    .HasDiscriminator(b => b.BikeType)
            //    .HasValue<ElectricBike>("Electric")
            //    .HasValue<HybridBike>("Hybrid")
            //    .HasValue<RoadBike>("Road")
            //    .HasValue<LadyBike>("Lady");

            //TPT
            //builder.Entity<ElectricBike>().ToTable("ElectricBikes");
            //builder.Entity<HybridBike>().ToTable("HybridBikes");
            //builder.Entity<RoadBike>().ToTable("RoadBikes");
            //builder.Entity<LadyBike>().ToTable("LadyBikes");

            // parse enum type to string for storeage in database, vice versa.
            builder.Entity<Bike>()
                .Property(b => b.BikeType)
                .HasConversion(
                    b => b.ToString(),
                    b => (BikeType)Enum.Parse(typeof(BikeType), b));

            builder.Entity<Price>()
                .Property(b => b.BikeType)
                .HasConversion(
                    b => b.ToString(),
                    b => (BikeType)Enum.Parse(typeof(BikeType), b));

            builder.Entity<Bike>()
                .Property(b => b.Revenue)
                .HasPrecision(18, 2);
        }
    }
}
