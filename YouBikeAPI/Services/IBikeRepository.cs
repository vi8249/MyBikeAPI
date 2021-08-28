using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouBikeAPI.Dtos;
using YouBikeAPI.Helper;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
    public interface IBikeRepository
    {
        Task<PaginationList<Bike, BikeDto>> GetBikes(int pageNum, int pageSize, string query);

        Task<Bike> GetBike(int id);

        Task<BikeDto> GetBikeForUser(int id);

        Task<Bike> CreateBike(BikeForManipulationDto bike);

        Task UpdateBike(int id, BikeForManipulationDto bike);

        Task<(string, bool)> DeleteBike(int id);

        Task TransferBike(int id, Guid stationId);

        Task<bool> RentBike(int id, string userId);

        Task<(bool, string)> ReturnBike(int id, Guid destinationId, string userId);

        Task<int> GetBikesCount();

        Task<int> BikeIncreasedInLastMonth();

        Task<bool> BikeExists(int id);

        Task<bool> SaveAllAsync();
        Task<IEnumerable<PriceDto>> GetPricesAsync();
    }
}
