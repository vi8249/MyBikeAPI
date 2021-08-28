using System;
using System.Collections.Generic;
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
using YouBikeAPI.Utilities;

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

        public async Task<PaginationList<Bike, BikeDto>> GetBikes(int pageNum, int pageSize, string query)
        {
            IQueryable<Bike> bikes = _context.Bikes
                .AsQueryable()
                .Include(b => b.BikeStation)
                .OrderBy(b => b.Id);

            if (!string.IsNullOrEmpty(query))
            {
                var properyList = new Dictionary<string, Object>();
                var propertyNames = new List<string> { "Id", "Mileage", "Revenue", "BikeStation.StationName" };

                query = query.Substring(0, 1).ToUpper() + query.Substring(1);
                if (Enum.IsDefined(typeof(BikeType), query)) propertyNames.Add("BikeType");
                if ("閒置中".Contains(query) || "租借中".Contains(query)) propertyNames.Add("Rented");

                foreach (var property in propertyNames)
                {
                    if (property.Split(".")[0] == "BikeStation")
                        properyList.Add(property, typeof(BikeStation).GetProperty("StationName").PropertyType);
                    else if (property.Split(".")[0] == "BikeType")
                        properyList.Add(property, typeof(System.Enum));
                    else
                        properyList.Add(property, typeof(Bike).GetProperty(property.Split(".")[0]).PropertyType);
                }

                query = "閒置中".Contains(query) ? "false" : query;
                query = "租借中".Contains(query) ? "true" : query;

                bikes = bikes.CustomSearch(query, properyList);
            }

            return await PaginationList<Bike, BikeDto>.CreateAsync(pageNum, pageSize, bikes, _mapper);
        }

        public async Task<Bike> GetBike(int id)
        {
            Bike bike = await _context.Bikes
                .Include(b => b.Price)
                .SingleOrDefaultAsync(b => b.Id == id);
            return bike;
        }

        public async Task<BikeDto> GetBikeForUser(int id)
        {
            Bike bike = await _context.Bikes
                .Include(b => b.BikeStation)
                .Include(b => b.Price)
                .Include(b => b.User)
                .SingleOrDefaultAsync(b => b.Id == id);
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

        public async Task<(string, bool)> DeleteBike(int id)
        {
            Bike bike = await GetBike(id);
            if (!bike.Rented)
                _context.Bikes.Remove(bike);
            else
            {
                return ("租借中車輛不得刪除", false);
            }
            if (bike.BikeStationId.HasValue)
            {
                (await _bikeStationRepository.GetBikeStationById(bike.BikeStationId)).BikesInsideParkingLot--;
            }
            return ("刪除成功", true);
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
                return false;

            Bike bike = await GetBike(id);
            if (bike.Rented)
                return false;

            bike.Rented = true;
            bike.UserId = userId;
            currentUser.Bike = bike;
            BikeStation station = await _bikeStationRepository.GetBikeStationById(bike.BikeStationId);

            var route = new HistoryRoute
            {
                Source = station.Id,
                SourceName = station.StationName,
                ApplicationUserId = userId,
                BikeId = bike.Id,
                CurrentRoute = true,
                CreationDate = DateTime.UtcNow.ToLocalTime(),
                ReturnTime = null
            };

            await _paidmentService.CreateHistoryRoute(route);
            if (!await SaveAllAsync()) return false;

            currentUser.HistoryRouteItem.Add(new HistoryRouteItem
            {
                ApplicationUserId = currentUser.Id,
                HistoryRouteId = route.Id
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
            historyRoute.ReturnTime = DateTime.UtcNow.ToLocalTime();
            BikeStation stationFrom = await _bikeStationRepository.GetBikeStationById(historyRoute.Source);
            BikeStation stationTo = await _bikeStationRepository.GetBikeStationById(destinationId);
            if (stationFrom == null || stationTo == null)
            {
                return (false, "查無此車站");
            }
            historyRoute.DestinationName = stationTo.StationName;
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

        public async Task<int> GetBikesCount()
        {
            return await _context.Bikes.CountAsync();
        }

        public async Task<int> BikeIncreasedInLastMonth()
        {
            return await _context.Bikes
                .DateBetween(-2, -1)
                .CountAsync();
        }

        public async Task<IEnumerable<PriceDto>> GetPricesAsync()
        {
            var prices = await _context.Prices.ToListAsync();
            return _mapper.Map<IEnumerable<PriceDto>>(prices);
        }
    }
}
