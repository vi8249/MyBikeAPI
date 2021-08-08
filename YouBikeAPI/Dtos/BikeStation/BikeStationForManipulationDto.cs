using System;
using System.ComponentModel.DataAnnotations;

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
	}
}
