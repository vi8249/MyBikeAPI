using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
