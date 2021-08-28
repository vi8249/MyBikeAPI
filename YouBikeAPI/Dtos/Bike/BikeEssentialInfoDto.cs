namespace YouBikeAPI.Dtos.Bike
{
    public class BikeEssentialInfoDto
    {
        public int Id { get; set; }
        public int Mileage { get; set; }
        public bool Rented { get; set; }
        public string BikeType { get; set; }

    }
}