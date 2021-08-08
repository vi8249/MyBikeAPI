using AutoMapper;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;

namespace YouBikeAPI.Profiles
{
	public class HistoryRouteProfile : Profile
	{
		public HistoryRouteProfile()
		{
			CreateMap<HistoryRoute, HistoryRouteDto>();
		}
	}
}
