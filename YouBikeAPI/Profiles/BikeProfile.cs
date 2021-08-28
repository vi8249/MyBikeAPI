using System;
using AutoMapper;
using YouBikeAPI.Dtos;
using YouBikeAPI.Dtos.Bike;
using YouBikeAPI.Models;

namespace YouBikeAPI.Profiles
{
    public class BikeProfile : Profile
    {
        public BikeProfile()
        {
            CreateMap<Bike, BikeDto>().ForMember((BikeDto dest) => dest.BikeType, delegate (IMemberConfigurationExpression<Bike, BikeDto, string> opt)
            {
                opt.MapFrom((Bike src) => src.BikeType.ToString());
            });
            CreateMap<BikeForManipulationDto, Bike>().ForMember((Bike dest) => dest.BikeType, delegate (IMemberConfigurationExpression<BikeForManipulationDto, Bike, BikeType> opt)
            {
                opt.MapFrom((BikeForManipulationDto src) => (BikeType)Enum.Parse(typeof(BikeType), src.BikeType));
            });
            CreateMap<Bike, BikeEssentialInfoDto>().ForMember((BikeEssentialInfoDto dest) => dest.BikeType, delegate (IMemberConfigurationExpression<Bike, BikeEssentialInfoDto, string> opt)
            {
                opt.MapFrom((Bike src) => src.BikeType.ToString());
            });
            CreateMap<Price, PriceDto>().ForMember(dest => dest.BikeType, src => src.MapFrom(p => p.BikeType.ToString()));
        }
    }
}
