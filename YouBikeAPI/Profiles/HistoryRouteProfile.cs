using AutoMapper;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;

namespace YouBikeAPI.Profiles
{
    public class HistoryRouteProfile : Profile
    {
        public HistoryRouteProfile()
        {
            CreateMap<HistoryRoute, HistoryRouteDto>()
                .ForMember(dest => dest.BorrowTime, src => src.MapFrom(h => h.CreationDate))
                .ForMember(dest => dest.SourceName, src => src.MapFrom(h => (h.SourceStation != null) ? h.SourceStation.StationName : "無"))
                .ForMember(dest => dest.SLat, src => src.MapFrom(h => (h.SourceStation != null) ? h.SourceStation.Latitude : 0))
                .ForMember(dest => dest.SLng, src => src.MapFrom(h => (h.SourceStation != null) ? h.SourceStation.Longitude : 0))
                .ForMember(dest => dest.DestinationName, src => src.MapFrom(h => (h.DestinationStation != null) ? h.DestinationStation.StationName : "無"))
                .ForMember(dest => dest.DLat, src => src.MapFrom(h => (h.DestinationStation != null) ? h.DestinationStation.Latitude : 0))
                .ForMember(dest => dest.DLng, src => src.MapFrom(h => (h.DestinationStation != null) ? h.DestinationStation.Longitude : 0));
        }
    }
}
