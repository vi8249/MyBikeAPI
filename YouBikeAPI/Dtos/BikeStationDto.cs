using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeAPI.Dtos
{
    public class BikeStationDto
    {
        public Guid Id { get; set; }
        public string StationName { get; set; }
        //public int TotalParkingSpace { get; set; }
        //public int CurrentParkingSpace { get; set; }
        public int AvalidableParkingSpace { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<BikeDto> AvailableBikes { get; set; }
    }
}
