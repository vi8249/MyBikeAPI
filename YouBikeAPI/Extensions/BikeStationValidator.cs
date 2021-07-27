using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Models;

namespace YouBikeAPI.Extensions
{
    public class BikeStationValidator : AbstractValidator<BikeStation>
    {
        public BikeStationValidator()
		{
			RuleFor(bs => bs.Id).NotNull();
			RuleFor(bs => bs.BikesInsideParkingLot).LessThanOrEqualTo(bs => bs.TotalParkingSpace).WithMessage("停車場已無駐車空間");
		}
	}
}
