using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using YouBikeAPI.Data;
using YouBikeAPI.Extensions;
using YouBikeAPI.Models;
using YouBikeAPI.Services;
using YouBikeAPI.SignalR;

namespace YouBikeAPI
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentitySevices(configuration);
            services.AddValidationServices();
            services.AddHttpClient();
            services.AddDbContext<AppDbContext>(delegate (DbContextOptionsBuilder options)
            {
                options.UseSqlServer(configuration["DbContext:ConnectionString"]);
            });
            services.AddRepositories();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddCors();
            services.AddSignalR();
            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(delegate (SwaggerUIOptions c)
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "YouBike API v1");
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseCors(policy => policy.AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("x-pagination")
                .WithOrigins("https://localhost:4200"));
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
            {
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");
                endpoints.MapSwagger();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
