using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YouBikeAPI.Data;

namespace YouBikeAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			IHost host = CreateHostBuilder(args).Build();
			using IServiceScope scope = host.Services.CreateScope();
			IServiceProvider services = scope.ServiceProvider;
			try
			{
				AppDbContext context = services.GetRequiredService<AppDbContext>();
				await context.Database.MigrateAsync();
				await Seed.SeedStations(context);
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
				logger.LogError(ex, "An error occurred during migration");
			}
			await host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(delegate(IWebHostBuilder webBuilder)
			{
				webBuilder.UseStartup<Startup>();
			});
		}
	}
}
