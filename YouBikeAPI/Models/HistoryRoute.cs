using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouBikeAPI.Models
{
	public class HistoryRoute
	{
		public int Id { get; set; }

		public Guid Source { get; set; }

		public Guid Destination { get; set; }

		[Column(TypeName = "decimal(18,4)")]
		public decimal Cost { get; set; }

		[ForeignKey("ApplicationUser")]
		public string ApplicationUserId { get; set; }

		public ApplicationUser User { get; set; }

		[ForeignKey("Bike")]
		public int BikeId { get; set; }

		public Bike Bike { get; set; }

		public bool CurrentRoute { get; set; }

		public DateTime BorrowTime { get; set; }

		public DateTime ReturnTime { get; set; }
	}
}
