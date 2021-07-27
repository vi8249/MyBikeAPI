using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
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
        
        public async Task<IEnumerable<BikeStationDto>> GetBikeStations()
        {
            var bikeStations = await _context.BikeStations.ToListAsync();
            return _mapper.Map<IEnumerable<BikeStationDto>>(bikeStations);
        }

        public async Task<BikeStation> GetBikeStationById(Guid? id)
        {
            var bikeStation = await _context.BikeStations
                .Include(bs => bs.AvailableBikes).ThenInclude(b => b.Price)
                .SingleOrDefaultAsync(bs => bs.Id == id);

            return bikeStation;
        }
        public async Task<BikeStationDto> GetBikeStationByIdDto(Guid? id)
        {
            var bikeStation = await _context.BikeStations
                .Include(bs => bs.AvailableBikes).ThenInclude(b => b.Price)
                .SingleOrDefaultAsync(bs => bs.Id == id);

            return _mapper.Map<BikeStationDto>(bikeStation);
        }

        public async Task<BikeStation> GetBikeStationByName(string stationName)
        {
            var bikeStation = await _context.BikeStations
                .Include(bs => bs.AvailableBikes).ThenInclude(b => b.Price)
                .SingleOrDefaultAsync(bs => bs.StationName == stationName);

            return bikeStation;
        }

        public async Task<bool> ValidateModel(ControllerBase controllerBase, Guid? id)
        {
            var station = await GetBikeStationById(id);
            return !controllerBase.TryValidateModel(station);
        }

        public BikeStation CreateBikeStation(BikeStationForCreationDto station)
        {
            BikeStation stationModel = _mapper.Map<BikeStation>(station);
            _context.BikeStations.Add(stationModel);
            return stationModel;
        }

        public async Task UpdateBikeStation(BikeStationForManipulationDto station)
        {
            var targetStation = await GetBikeStationById(station.Id);
            _mapper.Map(station, targetStation);
        }

        public async Task DeleteBikeStation(Guid id)
        {
            var bikeStation = await _context.BikeStations.FindAsync(id);
            _context.BikeStations.Remove(bikeStation);
        }

        public async Task<bool> BikeStationExists(Guid id)
        {
            return await _context.BikeStations.AnyAsync(bs => bs.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
