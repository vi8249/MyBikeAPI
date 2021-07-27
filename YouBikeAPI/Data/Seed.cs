using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YouBikeAPI.Models;

namespace YouBikeAPI.Data
{
    public class Seed
    {
        public static async Task SeedStations(AppDbContext context)
        {
            if (await context.BikeStations.AnyAsync()) return;

            var stationData = await System.IO.File.ReadAllTextAsync("Data/TaichungYoubike.json");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var stations = JsonSerializer.Deserialize<List<JsonToBikeStation>>(stationData, options);
            if (stations == null) return;

            BikeType[] stype = { BikeType.Electric, BikeType.Hybrid, BikeType.Road, BikeType.Lady };
            int[] cost = { 40, 35, 30, 25 };
            int[] discount = { 35, 30, 25, 20 };

            for (int i = 0; i < cost.Length; i++)
            {
                Price price = new()
                {
                    BikeType = stype[i],
                    Cost = cost[i],
                    Discount = discount[i]
                };
                await context.Prices.AddAsync(price);
            }

            foreach (var station in stations)
            {
                var CurrentBikeCount = int.Parse(station.Sbi);
                List<Bike> bikes = new List<Bike>();
                var rand = new Random();

                for (int i = 0; i < CurrentBikeCount; i++)
                {
                    var type = rand.Next(4);
                    var targetBike = stype[type];

                    bikes.Add(new Bike
                    {
                        BikeType = targetBike,
                        Price = await context.Prices.FindAsync(targetBike)
                    });
                }

                BikeStation bikeStation = new()
                {
                    Id = Guid.NewGuid(),
                    StationName = station.Sna,
                    TotalParkingSpace = int.Parse(station.Tot),
                    BikesInsideParkingLot = int.Parse(station.Sbi),
                    Latitude = double.Parse(station.Lat),
                    Longitude = double.Parse(station.Lng),
                    UpdateTime = DateTime.UtcNow,

                    AvailableBikes = bikes
                };
                await context.BikeStations.AddAsync(bikeStation);
            }

            await context.SaveChangesAsync();
        }
    }
}
