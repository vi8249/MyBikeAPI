using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;

namespace YouBikeAPI.Profiles
{
    public class BikeStationProfile : Profile
    {
        public BikeStationProfile()
        {
            CreateMap<BikeStation, BikeStationDto>()
                .ForMember(
                    dest => dest.AvalidableParkingSpace,
                    opt => opt.MapFrom(src => src.TotalParkingSpace - src.BikesInsideParkingLot)
                );

            CreateMap<BikeStationForCreationDto, BikeStation>();
            CreateMap<BikeStationForManipulationDto, BikeStation>();
        }
    }
}
