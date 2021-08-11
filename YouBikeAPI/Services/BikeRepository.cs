using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Helper;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
    public class BikeRepository : IBikeRepository
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        private readonly IBikeStationRepository _bikeStationRepository;

        private readonly IPaidmentService _paidmentService;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public BikeRepository(AppDbContext context, IMapper mapper, IBikeStationRepository bikeStationRepository, IPaidmentService paidmentService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _bikeStationRepository = bikeStationRepository;
            _paidmentService = paidmentService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<PaginationList<Bike, BikeDto>> GetBikes(int pageNum, int pageSize)
        {
            IQueryable<Bike> bikes = _context.Bikes.AsQueryable().OrderBy(b => b.Id);
            return await PaginationList<Bike, BikeDto>.CreateAsync(pageNum, pageSize, bikes, _mapper);
        }

        public async Task<Bike> GetBike(int id)
        {
            return await _context.Bikes.Include((Bike b) => b.Price).SingleOrDefaultAsync((Bike b) => b.Id == id);
        }

        public async Task<BikeDto> GetBikeForUser(int id)
        {
            Bike bike = await _context.Bikes.Include((Bike b) => b.Price).Include((Bike b) => b.User).FirstOrDefaultAsync((Bike b) => b.Id == id);
            return _mapper.Map<BikeDto>(bike);
        }

        public async Task<Bike> CreateBike(BikeForManipulationDto bike)
        {
            Bike bikeModel = _mapper.Map<Bike>(bike);
            if (!bike.BikeStationId.HasValue)
            {
                _context.Bikes.Add(bikeModel);
            }
            else
            {
                BikeStation station = await _bikeStationRepository.GetBikeStationById(bike.BikeStationId);
                station.AvailableBikes.Add(bikeModel);
                station.BikesInsideParkingLot++;
            }
            Bike bike2 = bikeModel;
            bike2.Price = await _context.Prices.FindAsync(bikeModel.BikeType);
            return bikeModel;
        }

        public async Task UpdateBike(int id, BikeForManipulationDto bike)
        {
            Bike targetBike = await GetBike(id);
            _mapper.Map(bike, targetBike);
        }

        public async Task DeleteBike(int id)
        {
            Bike bike = await GetBike(id);
            _context.Bikes.Remove(bike);
            if (bike.BikeStationId.HasValue)
            {
                (await _bikeStationRepository.GetBikeStationById(bike.BikeStationId)).BikesInsideParkingLot--;
            }
        }

        public async Task<bool> BikeExists(int id)
        {
            return await _context.Bikes.AnyAsync((Bike e) => e.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task TransferBike(int id, Guid stationId)
        {
            Bike bike = await GetBike(id);
            if (bike.BikeStationId.HasValue)
            {
                BikeStation currentStation = await _bikeStationRepository.GetBikeStationById(bike.BikeStationId);
                currentStation.AvailableBikes.Remove(bike);
                currentStation.BikesInsideParkingLot--;
            }
            BikeStation targetStation = await _bikeStationRepository.GetBikeStationById(stationId);
            targetStation.AvailableBikes.Add(bike);
            targetStation.BikesInsideParkingLot++;
        }

        public async Task<bool> RentBike(int id, string userId)
        {
            ApplicationUser currentUser = await _context.Users.SingleOrDefaultAsync((ApplicationUser u) => u.Id == userId);
            if (currentUser == null || currentUser.Bike != null)
            {
                return false;
            }
            Bike bike = await GetBike(id);
            if (bike.Rented)
            {
                return false;
            }
            bike.Rented = true;
            bike.UserId = userId;
            currentUser.Bike = bike;
            BikeStation station = await _bikeStationRepository.GetBikeStationById(bike.BikeStationId);
            currentUser.HistoryRoutes.Add(new HistoryRoute
            {
                Source = station.Id,
                ApplicationUserId = userId,
                BikeId = bike.Id,
                CurrentRoute = true,
                BorrowTime = DateTime.UtcNow
            });
            station.BikesInsideParkingLot--;
            station.AvailableBikes.Remove(bike);
            return true;
        }

        public async Task<(bool, string)> ReturnBike(int id, Guid destinationId, string userId)
        {
            Bike bike = await GetBike(id);
            if (!bike.Rented)
            {
                return (false, "該單車已歸還");
            }
            if (bike.UserId != userId)
            {
                return (false, "歸還人與租借人不同");
            }
            HistoryRoute historyRoute = await _context.HistoryRoutes.SingleOrDefaultAsync((HistoryRoute h) => h.ApplicationUserId == userId && h.CurrentRoute);
            historyRoute.Destination = destinationId;
            historyRoute.ReturnTime = DateTime.UtcNow;
            BikeStation stationFrom = await _bikeStationRepository.GetBikeStationById(historyRoute.Source);
            BikeStation stationTo = await _bikeStationRepository.GetBikeStationById(destinationId);
            if (stationFrom == null || stationTo == null)
            {
                return (false, "查無此車站");
            }
            string lan = $"{stationFrom.Latitude},{stationFrom.Longitude}";
            string lon = $"{stationTo.Latitude},{stationTo.Longitude}";
            (bool, string) paymentResult = await _paidmentService.PayBill(userId, lan, lon);
            if (!paymentResult.Item1)
            {
                return (false, paymentResult.Item2);
            }
            bike.Rented = false;
            bike.UserId = null;
            historyRoute.CurrentRoute = false;
            if (stationFrom.Id != destinationId)
            {
                stationFrom.BikesInsideParkingLot--;
            }
            stationTo.BikesInsideParkingLot++;
            stationTo.AvailableBikes.Add(bike);
            return (true, "成功歸還");
        }
    }
}
