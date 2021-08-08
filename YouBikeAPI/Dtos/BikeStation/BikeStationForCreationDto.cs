using System;

namespace YouBikeAPI.Dtos
{
	public class BikeStationForCreationDto
	{
		public string StationName { get; set; }

		public int TotalParkingSpace { get; set; }

		public int BikesInsideParkingLot { get; set; }

		public DateTime CreateTime { get; set; } = DateTime.UtcNow;


		public double Latitude { get; set; }

		public double Longitude { get; set; }
	}
}
