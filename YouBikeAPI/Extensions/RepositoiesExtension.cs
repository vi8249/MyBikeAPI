using Microsoft.Extensions.DependencyInjection;
using YouBikeAPI.Services;

namespace YouBikeAPI.Extensions
{
    public static class RepositoiesExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBikeStationRepository, BikeStationRepository>();
            services.AddTransient<IBikeRepository, BikeRepository>();
            services.AddTransient<IPaidmentService, PaidmentService>();
            services.AddTransient<IDashboardInfo, DashboardInfo>();
        }
    }
}