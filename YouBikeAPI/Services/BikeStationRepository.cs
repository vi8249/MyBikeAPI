using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Helper;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
	public class BikeStationRepository : IBikeStationRepository
	{
		private readonly AppDbContext _context;

		private readonly IMapper _mapper;

		public BikeStationRepository(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<PaginationList<BikeStation, BikeStationDto>> GetBikeStations(int pageNum, int pageSize)
		{
			IQueryable<BikeStation> bikeStations = _context.BikeStations.AsQueryable();
			return await PaginationList<BikeStation, BikeStationDto>.CreateAsync(pageNum, pageSize, bikeStations, _mapper);
		}

		public async Task<BikeStation> GetBikeStationById(Guid? id)
		{
			return await _context.BikeStations.Include((BikeStation bs) => bs.AvailableBikes).ThenInclude((Bike b) => b.Price).SingleOrDefaultAsync((BikeStation bs) => bs.Id == id);
		}

		public async Task<BikeStationDto> GetBikeStationByIdDto(Guid? id)
		{
			BikeStation bikeStation = await _context.BikeStations.Include((BikeStation bs) => bs.AvailableBikes).ThenInclude((Bike b) => b.Price).SingleOrDefaultAsync((BikeStation bs) => bs.Id == id);
			return _mapper.Map<BikeStationDto>(bikeStation);
		}

		public async Task<BikeStation> GetBikeStationByName(string stationName)
		{
			return await _context.BikeStations.Include((BikeStation bs) => bs.AvailableBikes).ThenInclude((Bike b) => b.Price).SingleOrDefaultAsync((BikeStation bs) => bs.StationName == stationName);
		}

		public async Task<bool> ValidateModel(ControllerBase controllerBase, Guid? id)
		{
			return !controllerBase.TryValidateModel(await GetBikeStationById(id));
		}

		public BikeStation CreateBikeStation(BikeStationForCreationDto station)
		{
			BikeStation stationModel = _mapper.Map<BikeStation>(station);
			_context.BikeStations.Add(stationModel);
			return stationModel;
		}

		public async Task UpdateBikeStation(BikeStationForManipulationDto station)
		{
			BikeStation targetStation = await GetBikeStationById(station.Id);
			_mapper.Map(station, targetStation);
		}

		public async Task DeleteBikeStation(Guid id)
		{
			BikeStation bikeStation = await _context.BikeStations.FindAsync(id);
			_context.BikeStations.Remove(bikeStation);
		}

		public async Task<bool> BikeStationExists(Guid id)
		{
			return await _context.BikeStations.AnyAsync((BikeStation bs) => bs.Id == id);
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
