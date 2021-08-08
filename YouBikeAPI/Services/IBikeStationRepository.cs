using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YouBikeAPI.Dtos;
using YouBikeAPI.Helper;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
	public interface IBikeStationRepository
	{
		Task<PaginationList<BikeStation, BikeStationDto>> GetBikeStations(int pageNum, int pageSize);

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
