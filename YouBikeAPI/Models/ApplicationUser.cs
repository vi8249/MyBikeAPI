using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace YouBikeAPI.Models
{
    public class ApplicationUser : IdentityUser, IDate
    {
        public Money Money { get; set; }
        public Bike Bike { get; set; }
        public ICollection<HistoryRouteItem> HistoryRouteItem { get; set; } = new List<HistoryRouteItem>();
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
