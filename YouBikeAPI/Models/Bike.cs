using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouBikeAPI.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public int Mileage { get; set; } = new Random().Next(100, 250);
        public bool Rented { get; set; } = false;
        public decimal Revenue { get; set; } = new Random().Next(1000, 5000) / 10 * 10 + new Random().Next(0,2) * 5;

        [ForeignKey("Price")]
        public BikeType BikeType { get; set; }
        public Price Price { get; set; }

        [ForeignKey("BikeStation")]
        public Guid? BikeStationId { get; set; }
        //public BikeStation BikeStation { get; set; }

    }
}
