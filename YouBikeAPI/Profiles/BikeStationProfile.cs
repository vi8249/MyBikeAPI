using AutoMapper;
using YouBikeAPI.Dtos;
using YouBikeAPI.Dtos.BikeStation;
using YouBikeAPI.Models;

namespace YouBikeAPI.Profiles
{
    public class BikeStationProfile : Profile
    {
        public BikeStationProfile()
        {
            CreateMap<BikeStation, BikeStationDto>().ForMember((BikeStationDto dest) => dest.AvalidableParkingSpace, delegate (IMemberConfigurationExpression<BikeStation, BikeStationDto, int> opt)
            {
                opt.MapFrom((BikeStation src) => src.TotalParkingSpace - src.BikesInsideParkingLot);
            });
            CreateMap<BikeStationForCreationDto, BikeStation>();
            CreateMap<BikeStationForManipulationDto, BikeStation>();
            CreateMap<BikeStation, BikeStationNameDto>();
        }
    }
}
