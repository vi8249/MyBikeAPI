using System;

namespace YouBikeAPI.Dtos
{
	public class BikeDto
	{
		public int Id { get; set; }

		public int Mileage { get; set; }

		public bool Rented { get; set; }

		public decimal Revenue { get; set; }

		public string BikeType { get; set; }

		public Guid? BikeStationId { get; set; }

		public PriceDto Price { get; set; }
	}
}
