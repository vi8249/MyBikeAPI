using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Helper;
using YouBikeAPI.Models;
using YouBikeAPI.Utilities;

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

        public async Task<PaginationList<BikeStation, BikeStationDto>> GetBikeStations(int pageNum, int pageSize, string query)
        {
            IQueryable<BikeStation> stations = _context.BikeStations
                .AsQueryable()
                .OrderBy(s => s.Id);

            if (!string.IsNullOrEmpty(query))
            {
                var properyList = new Dictionary<string, Object>();
                var propertyNames = new List<string> { "StationName", "Latitude", "Longitude", "CreationDate" };

                foreach (var property in propertyNames)
                {
                    properyList.Add(property, typeof(BikeStation).GetProperty(property.Split(".")[0]).PropertyType);
                }

                stations = stations.CustomSearch(query, properyList);
            }

            return await PaginationList<BikeStation, BikeStationDto>.CreateAsync(pageNum, pageSize, stations, _mapper);
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
            //_mapper.Map(station, targetStation);
            foreach (var property in typeof(BikeStationForManipulationDto).GetProperties())
            {
                var value = typeof(BikeStationForManipulationDto).GetProperty(property.Name)?.GetValue(station);
                if (value != null)
                {
                    typeof(BikeStation)?.GetProperty(property.Name)?.SetValue(targetStation, value);
                }
            }
        }

        public async Task DeleteBikeStation(Guid id)
        {
            BikeStation bikeStation = await _context.BikeStations
                .Include(s => s.AvailableBikes)
                .SingleOrDefaultAsync(s => s.Id == id);

            var hisList = await _context.HistoryRoutes
                .Where(h => h.SourceStationId == id || h.DestinationStationId == id)
                .ToListAsync();
            foreach (var h in hisList)
            {
                if (id == h.SourceStationId)
                    h.SourceStationId = null;
                if (id == h.DestinationStationId)
                    h.DestinationStationId = null;
            }

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

        public async Task<int> GetBikeStationsCount()
        {
            return await _context.BikeStations.CountAsync();
        }

        public async Task<int> BikeStationIncreasedInLastMonth()
        {
            return await _context.Bikes
                .DateBetween(-2, -1)
                .CountAsync();
        }
    }
}
