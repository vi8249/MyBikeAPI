using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Models;

namespace YouBikeAPI.Dtos
{
    public class BikeStationForManipulationDto
    {
        [Required]
        public Guid Id { get; set; }
        public string StationName { get; set; }
        public int TotalParkingSpace { get; set; }
        public int BikesInsideParkingLot { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        //public ICollection<Bike> AvailableBikes { get; set; }
    }
}
