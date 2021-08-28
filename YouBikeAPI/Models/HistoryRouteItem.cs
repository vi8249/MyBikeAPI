using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouBikeAPI.Models
{
    public class HistoryRouteItem
    {
        public Guid Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public Guid HistoryRouteId { get; set; }
    }
}