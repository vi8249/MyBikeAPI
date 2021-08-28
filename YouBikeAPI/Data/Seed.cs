using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YouBikeAPI.Models;

namespace YouBikeAPI.Data
{
    public class Seed
    {
        public static async Task SeedStations(AppDbContext context)
        {
            if (await context.BikeStations.AnyAsync())
            {
                return;
            }
            string stationData = await File.ReadAllTextAsync("Data/TaichungYoubike.json");
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            List<JsonToBikeStation> stations = JsonSerializer.Deserialize<List<JsonToBikeStation>>(stationData, options);
            if (stations == null)
            {
                return;
            }
            BikeType[] stype = new BikeType[4]
            {
                BikeType.Electric,
                BikeType.Hybrid,
                BikeType.Road,
                BikeType.Lady
            };
            int[] cost = new int[4] { 40, 35, 30, 25 };
            int[] discount = new int[4] { 35, 30, 25, 20 };
            for (int j = 0; j < cost.Length; j++)
            {
                Price price = new Price
                {
                    BikeType = stype[j],
                    Cost = cost[j],
                    Discount = discount[j]
                };
                await context.Prices.AddAsync(price);
            }
            foreach (JsonToBikeStation station in stations)
            {
                int CurrentBikeCount = int.Parse(station.Sbi);
                List<Bike> bikes = new List<Bike>();
                Random rand = new Random();
                for (int i = 0; i < CurrentBikeCount; i++)
                {
                    int type = rand.Next(4);
                    BikeType targetBike = stype[type];
                    List<Bike> list = bikes;
                    Bike bike = new Bike
                    {
                        BikeType = targetBike
                    };
                    Bike bike2 = bike;
                    bike2.Price = await context.Prices.FindAsync(targetBike);
                    list.Add(bike);
                }
                BikeStation bikeStation = new BikeStation
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

            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 1; i <= 10; i++)
            {
                var months = rnd.Next(3);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"{i}@xx.com",
                    NormalizedUserName = $"{i}@xx.com".ToUpper(),
                    Email = $"{i}@xx.com",
                    NormalizedEmail = $"{i}@xx.com".ToUpper(),
                    TwoFactorEnabled = false,
                    EmailConfirmed = true,
                    PhoneNumber = "123456789",
                    PhoneNumberConfirmed = false,
                    CreationDate = DateTime.UtcNow.AddMonths(-months),
                    Money = new Money
                    {
                        Value = 500
                    }
                };
                PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, "PassWord12!");
                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }
}
