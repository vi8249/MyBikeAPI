using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YouBikeAPI.Models;
using YouBikeAPI.Dtos.BikeStation;

namespace YouBikeAPI.Dtos
{
    public class BikeForManipulationDto : IValidatableObject
    {
        public int Id { get; set; }
        public int Mileage { get; set; } = 0;
        public bool Rented { get; set; } = false;
        public decimal Revenue { get; set; } = default(decimal);
        [Required]
        public string BikeType { get; set; }
        public PriceDto Price { get; set; }
        public Guid? BikeStationId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            BikeType type;
            if (BikeType == null)
            {
                yield return new ValidationResult("Bike Type 不可為空", new string[1] { validationContext.MemberName });
            }
            else if (!Enum.TryParse<BikeType>(BikeType, out type))
            {
                yield return new ValidationResult("Bike Type 不存在", new string[1] { validationContext.MemberName });
            }
        }
    }
}
