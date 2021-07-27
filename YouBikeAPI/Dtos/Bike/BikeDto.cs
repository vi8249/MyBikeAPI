using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Models;

namespace YouBikeAPI.Dtos
{
    public class BikeDto
    {
        public int Id { get; set; }
        public int Mileage { get; set; }
        public bool Rented { get; set; }
        public decimal Revenue { get; set; }
        public string BikeType { get; set; }
        public PriceDto Price { get; set; }
    }
}
