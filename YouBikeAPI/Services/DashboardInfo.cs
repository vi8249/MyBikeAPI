using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YouBikeAPI.Data;
using YouBikeAPI.Models;
using YouBikeAPI.Utilities;

namespace YouBikeAPI.Services
{
    public class DashboardInfo : IDashboardInfo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBikeRepository _bikeRepository;
        private readonly IBikeStationRepository _stationRepository;

        public DashboardInfo(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IBikeRepository bikeRepository,
            IBikeStationRepository stationRepository)
        {
            _context = context;
            _userManager = userManager;
            _bikeRepository = bikeRepository;
            _stationRepository = stationRepository;
        }
        public async Task<Dashboard> GetDashboardInfo()
        {
            var users = await _userManager.Users.ToListAsync();

            var userCreatedInLastMonthList = new List<ApplicationUser> { };
            var userCreatedInThisMonthList = new List<ApplicationUser> { };

            foreach (var u in users)
            {
                if (!await _userManager.IsInRoleAsync(u, "Admin"))
                {
                    if (u.CreationDate.IsBetween(-2, -1))
                        userCreatedInLastMonthList.Add(u);

                    if (u.CreationDate.IsBetween(-1, 0))
                        userCreatedInThisMonthList.Add(u);
                }
            }

            var userCreatedInLastMonth = userCreatedInLastMonthList.Count();
            var userCreatedInThisMonth = userCreatedInThisMonthList.Count();

            return new Dashboard
            {
                UserIncreasedInThisMonth = userCreatedInThisMonth,
                UserIncreasedInLastMonth = userCreatedInLastMonth,
                BikeLendInThisMonth = await GetBikesLendInPeriodOfTime(-1, 0).CountAsync(),
                BikeLendInLastMonth = await GetBikesLendInPeriodOfTime(-2, -1).CountAsync(),
                RevenueInThisMonth = await GetBikesLendInPeriodOfTime(-1, 0).Select(h => h.Cost).SumAsync(),
                RevenueInLastMonth = await GetBikesLendInPeriodOfTime(-2, -1).Select(h => h.Cost).SumAsync(),
                TotalStationsAmount = await _stationRepository.GetBikeStationsCount(),
                StationIncreasedInThisMonth = await GetStationCreatedInPeriodOfTime(-1, 0).CountAsync(),
                StationIncreasedInLastMonth = await GetStationCreatedInPeriodOfTime(-2, -1).CountAsync()
            };

        }

        public IQueryable<HistoryRoute> GetBikesLendInPeriodOfTime(int start, int end)
        {
            return _context.HistoryRoutes.DateBetween(start, end);
        }

        public IQueryable<BikeStation> GetStationCreatedInPeriodOfTime(int start, int end)
        {
            return _context.BikeStations.DateBetween(start, end);
        }

    }
}