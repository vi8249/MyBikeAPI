using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using YouBikeAPI.Data;
using YouBikeAPI.Models;

namespace YouBikeAPI.Extensions
{
    public static class IdentityExtensions
    {
        public static void AddIdentitySevices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                                .AddRoles<IdentityRole>()
                                .AddRoleManager<RoleManager<IdentityRole>>()
                                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer(delegate (JwtBearerOptions options)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(configuration["Authentication:SecretKey"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Authentication:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Authentication:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(bytes)
                };
            });
        }
    }
}