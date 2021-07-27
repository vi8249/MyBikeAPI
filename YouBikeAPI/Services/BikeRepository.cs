using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Extensions;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
    public class BikeRepository : IBikeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBikeStationRepository _bikeStationRepository;

        public BikeRepository(
            AppDbContext context, 
            IMapper mapper, 
            IBikeStationRepository bikeStationRepository)
        {
            _context = context;
            _mapper = mapper;
            _bikeStationRepository = bikeStationRepository;
        }

        public async Task<IEnumerable<BikeDto>> GetBikes()
        {
            var bikes = await _context.Bikes.ToListAsync();
            return _mapper.Map<IEnumerable<BikeDto>>(bikes);
        }

        public async Task<Bike> GetBike(int id)
        {
            var bike = await _context.Bikes.Include(b => b.Price).SingleOrDefaultAsync(b => b.Id == id);

            return bike;
        }

        public async Task<BikeDto> GetBikeForUser(int id)
        {
            var bike = await _context.Bikes.Include(b => b.Price).FirstOrDefaultAsync(b => b.Id == id);

            return _mapper.Map<BikeDto>(bike);
        }

        public async Task<Bike> CreateBike(BikeForManipulationDto bike)
        {

            Bike bikeModel = _mapper.Map<Bike>(bike);

            if (bike.BikeStationId == null)
                _context.Bikes.Add(bikeModel);
            else
            {
                var station = await _bikeStationRepository.GetBikeStationById(bike.BikeStationId);
                station.AvailableBikes.Add(bikeModel);
                station.BikesInsideParkingLot++;
            }

            //從Price Table取得對應cost、discount
            bikeModel.Price = await _context.Prices.FindAsync(bikeModel.BikeType);

            return bikeModel;
        }

        public async Task UpdateBike(int id, BikeForManipulationDto bike)
        {
            var targetBike = await GetBike(id);
            _mapper.Map(bike, targetBike);
        }

        public async Task DeleteBike(int id)
        {
            var bike = await GetBike(id);

            _context.Bikes.Remove(bike);

            if (bike.BikeStationId != null)
            {
                var currentStation = await _bikeStationRepository.GetBikeStationById(bike.BikeStationId);
                currentStation.BikesInsideParkingLot--;
            }
        }        
        
        public async Task<bool> BikeExists(int id)
        {
            return await _context.Bikes.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task TransferBike(int id, Guid stationId)
        {
            var bike = await GetBike(id);

            if (bike.BikeStationId != null)
            {
                var currentStation = await _bikeStationRepository.GetBikeStationById(bike.BikeStationId);
                currentStation.AvailableBikes.Remove(bike);
                currentStation.BikesInsideParkingLot--;
            }
                
            var targetStation = await _bikeStationRepository.GetBikeStationById(stationId);
            //targetStation.AvailableBikes.Add(bike);
            targetStation.BikesInsideParkingLot++;
        }


    }
}
