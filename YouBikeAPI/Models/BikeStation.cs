using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeAPI.Models
{
    public class BikeStation : IValidatableObject
    {
        public Guid Id { get; set; }
        public string StationName { get; set; }
        public int TotalParkingSpace { get; set; }
        public int BikesInsideParkingLot { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<Bike> AvailableBikes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BikesInsideParkingLot >= TotalParkingSpace)
                yield return new ValidationResult("已無停車空間", new[] { "BikesInsideParkingLot" });
        }
    }
}
