using System.ComponentModel.DataAnnotations;

namespace YouBikeAPI.Models
{
	public class Price
	{
		[Key]
		public BikeType BikeType { get; set; }

		public int Cost { get; set; }

		public int Discount { get; set; }
	}
}
