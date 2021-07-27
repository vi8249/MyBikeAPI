using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeAPI.Dtos
{
    public class BikeStationForCreationDto
    {
        public string StationName { get; set; }
        public int TotalParkingSpace { get; set; }
        public int BikesInsideParkingLot { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        //public DateTime UpdateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        //public ICollection<Bike> AvailableBikes { get; set; }
    }
}
