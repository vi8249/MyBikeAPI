using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YouBikeAPI.Data;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
    public class PaidmentService : IPaidmentService
    {
        private readonly AppDbContext _context;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public PaidmentService(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private async Task<(double, TimeSpan)> DistanceMatrixAPI(string lan, string lon)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?origins={0}&destinations={1}&key=AIzaSyBCwOiIPr6ihSZlzB1PfEDv9RNg_Uw1iGY";
            HttpResponseMessage response = await httpClient.GetAsync(string.Format(url, lan, lon, _configuration["GoogleAPIKey:APIKey"]));
            long distance = 0L;
            long duration = 0L;
            if (response.IsSuccessStatusCode)
            {
                DistanceMatrix distanceMatrix = JsonSerializer.Deserialize<DistanceMatrix>(await response.Content.ReadAsStringAsync());
                if (distanceMatrix.status == "OK" && distanceMatrix.rows[0].elements[0].status == "OK")
                {
                    distance = distanceMatrix.rows[0].elements[0].distance.value;
                    duration = distanceMatrix.rows[0].elements[0].duration.value;
                }
            }
            return (Math.Round((double)distance / 1000.0, 0), TimeSpan.FromSeconds(duration));
        }

        public async Task<(bool, string)> PayBill(string userId, string lan, string lon)
        {
            ApplicationUser user = await _context.Users.Include((ApplicationUser u) => u.Money).SingleOrDefaultAsync((ApplicationUser u) => u.Id == userId);
            if (user == null)
            {
                return (false, "使用者不存在");
            }

            HistoryRoute currHistoryRoute = await _context.HistoryRoutes.SingleOrDefaultAsync((HistoryRoute h) => h.ApplicationUserId == userId && h.CurrentRoute == true);
            TimeSpan userTimeSpan = -currHistoryRoute.CreationDate.Subtract(currHistoryRoute.ReturnTime ?? DateTime.UtcNow.ToLocalTime());

            (double, TimeSpan) apiResult = await DistanceMatrixAPI(lan, lon);

            if (userTimeSpan.TotalMinutes >= apiResult.Item2.TotalMinutes)
            {
                if (user.Money.Value < (decimal)user.Bike.Price.Cost)
                {
                    return (false, "餘額不足");
                }
                var value = (decimal)(user.Bike.Price.Cost * (Math.Ceiling(userTimeSpan.TotalMinutes / 30)));
                user.Money.Value -= value;
                user.Bike.Revenue += value;
                currHistoryRoute.Cost = value;
            }
            else
            {
                if (user.Money.Value < (decimal)user.Bike.Price.Discount)
                {
                    return (false, "餘額不足");
                }
                user.Money.Value -= user.Bike.Price.Discount;
                user.Bike.Revenue += (decimal)user.Bike.Price.Discount;
                currHistoryRoute.Cost = user.Bike.Price.Discount;
            }
            user.Bike.Mileage += (int)apiResult.Item1;
            return (true, "付款成功");
        }

        public async Task CreateHistoryRoute(HistoryRoute historyRoute)
        {
            await _context.HistoryRoutes.AddAsync(historyRoute);
        }
    }
}
