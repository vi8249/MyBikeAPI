using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
    public interface IBikeStationRepository
    {
        Task<IEnumerable<BikeStationDto>> GetBikeStations();
        Task<BikeStation> GetBikeStationById(Guid? id);
        Task<BikeStationDto> GetBikeStationByIdDto(Guid? id);
        Task<BikeStation> GetBikeStationByName(string stationName);
        BikeStation CreateBikeStation(BikeStationForCreationDto station);
        Task UpdateBikeStation(BikeStationForManipulationDto station);
        Task DeleteBikeStation(Guid id);

        Task<bool> BikeStationExists(Guid id);
        Task<bool> SaveAllAsync();
        Task<bool> ValidateModel(ControllerBase controllerBase, Guid? id);
    }
}
