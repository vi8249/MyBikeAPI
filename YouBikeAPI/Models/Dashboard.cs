namespace YouBikeAPI.Models
{
    public class Dashboard
    {
        public int UserIncreasedInLastMonth { get; set; }
        public int UserIncreasedInThisMonth { get; set; }
        public int BikeLendInThisMonth { get; set; }
        public int BikeLendInLastMonth { get; set; }
        public decimal RevenueInThisMonth { get; set; }
        public decimal RevenueInLastMonth { get; set; }
        public int TotalStationsAmount { get; set; }
        public int StationIncreasedInThisMonth { get; set; }
        public int StationIncreasedInLastMonth { get; set; }
    }
}