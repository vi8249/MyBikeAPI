using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;

namespace YouBikeAPI.Profiles
{
    public class BikeProfile : Profile
    {
        public BikeProfile()
        {
            CreateMap<Bike, BikeDto>()
                .ForMember(
                    dest => dest.BikeType,
                    opt => opt.MapFrom(src => src.BikeType.ToString())
                );
            CreateMap<BikeForManipulationDto, Bike>()
                .ForMember(
                    dest => dest.BikeType,
                    opt => opt.MapFrom(src => (BikeType)Enum.Parse(typeof(BikeType), src.BikeType))
                );
        }
    }
}
