using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace YouBikeAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Money Money { get; set; }
        public Bike Bike { get; set; }
        public ICollection<HistoryRoute> HistoryRoutes { get; set; } = new List<HistoryRoute>();
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
    }
}
