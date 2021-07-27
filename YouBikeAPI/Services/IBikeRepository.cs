using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
    public interface IBikeRepository
    {
        Task<IEnumerable<BikeDto>> GetBikes();
        Task<Bike> GetBike(int id);
        Task<BikeDto> GetBikeForUser(int id);
        Task<Bike> CreateBike(BikeForManipulationDto bike);
        Task UpdateBike(int id, BikeForManipulationDto bike);
        Task DeleteBike(int id);
        Task TransferBike(int id, Guid stationId);
        
        Task<bool> BikeExists(int id);
        Task<bool> SaveAllAsync();
    }
}
