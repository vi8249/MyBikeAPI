using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouBikeAPI.Models
{
    public class Bike : IDate
    {
        public int Id { get; set; }
        public int Mileage { get; set; } = new Random().Next(100, 250);
        public bool Rented { get; set; } = false;
        [Column(TypeName = "decimal(18,4)")]
        public decimal Revenue { get; set; } = new Random().Next(1000, 5000) / 10 * 10 + new Random().Next(0, 2) * 5;
        [ForeignKey("Price")]
        public BikeType BikeType { get; set; }
        public Price Price { get; set; }
        public BikeStation BikeStation { get; set; }
        [ForeignKey("BikeStation")]
        public Guid? BikeStationId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
