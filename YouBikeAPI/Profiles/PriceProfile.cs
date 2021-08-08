using AutoMapper;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;

namespace YouBikeAPI.Profiles
{
	public class PriceProfile : Profile
	{
		public PriceProfile()
		{
			CreateMap<Price, PriceDto>();
		}
	}
}
