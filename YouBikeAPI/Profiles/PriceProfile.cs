using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
