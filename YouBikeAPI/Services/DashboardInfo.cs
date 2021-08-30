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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBikeRepository _bikeRepository;
        private readonly IBikeStationRepository _stationRepository;

        public DashboardInfo(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IBikeRepository bikeRepository,
            IBikeStationRepository stationRepository)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _bikeRepository = bikeRepository;
            _stationRepository = stationRepository;
        }
        public async Task<Dashboard> GetDashboardInfo()
        {
            var admin = await _roleManager.FindByNameAsync("Admin");

            return new Dashboard
            {
                UserIncreasedInThisMonth = await GetUserCreatedInPeriodOfTime(admin, -1, 0).CountAsync(),
                UserIncreasedInLastMonth = await GetUserCreatedInPeriodOfTime(admin, -2, -1).CountAsync(),
                BikeLendInThisMonth = await GetBikesLendInPeriodOfTime(-1, 0).CountAsync(),
                BikeLendInLastMonth = await GetBikesLendInPeriodOfTime(-2, -1).CountAsync(),
                RevenueInThisMonth = await GetBikesLendInPeriodOfTime(-1, 0).Select(h => h.Cost).SumAsync(),
                RevenueInLastMonth = await GetBikesLendInPeriodOfTime(-2, -1).Select(h => h.Cost).SumAsync(),
                TotalStationsAmount = await _stationRepository.GetBikeStationsCount(),
                StationIncreasedInThisMonth = await GetStationCreatedInPeriodOfTime(-1, 0).CountAsync(),
                StationIncreasedInLastMonth = await GetStationCreatedInPeriodOfTime(-2, -1).CountAsync()
            };

        }

        private IQueryable<HistoryRoute> GetBikesLendInPeriodOfTime(int start, int end)
        {
            return _context.HistoryRoutes.DateBetween(start, end);
        }

        private IQueryable<BikeStation> GetStationCreatedInPeriodOfTime(int start, int end)
        {
            return _context.BikeStations.DateBetween(start, end);
        }

        private IQueryable<ApplicationUser> GetUserCreatedInPeriodOfTime(IdentityRole admin, int start, int end)
        {
            return _userManager.Users.Where(u => u.Id != admin.Id).DateBetween(start, end);
        }
    }
}