using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouBikeAPI.Models
{
	public class Money
	{
		public Guid Id { get; set; }

		[Column(TypeName = "decimal(18,4)")]
		public decimal Value { get; set; }
	}
}
