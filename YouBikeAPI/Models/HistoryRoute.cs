using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouBikeAPI.Models
{
    public class HistoryRoute : IDate
    {
        public Guid Id { get; set; }
        public Guid Source { get; set; }
        public string SourceName { get; set; }
        public Guid Destination { get; set; }
        public string DestinationName { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public int BikeId { get; set; }
        public bool CurrentRoute { get; set; }
        [Column("BorrowTime")]
        public DateTime CreationDate { get; set; }
        public DateTime? ReturnTime { get; set; }
    }
}
