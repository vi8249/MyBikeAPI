using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Models;

namespace YouBikeAPI.Dtos
{
    public class BikeForManipulationDto : IValidatableObject
    {   
        public int Mileage { get; set; } = 0;
        public bool Rented { get; set; } = false;
        public decimal Revenue { get; set; } = 0;
        [Required]
        public string BikeType { get; set; }        
        public PriceDto Price { get; set; }
        public Guid? BikeStationId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BikeType == null)
            {
                yield return new ValidationResult("Bike Type 不可為空", new[] { validationContext.MemberName });
            }
            else
            {
                if (!Enum.TryParse<BikeType>(BikeType, out BikeType type))
                {
                    yield return new ValidationResult("Bike Type 不存在", new[] { validationContext.MemberName });
                }
            }
        }
    }
}
