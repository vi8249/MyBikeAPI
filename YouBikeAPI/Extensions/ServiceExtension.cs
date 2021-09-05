using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using YouBikeAPI.Data;
using YouBikeAPI.Models;
using YouBikeAPI.Services;

namespace YouBikeAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void AddIdentitySevices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                                .AddRoles<IdentityRole>()
                                .AddRoleManager<RoleManager<IdentityRole>>()
                                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void AddValidationServices(this IServiceCollection services)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(delegate (ApiBehaviorOptions setupAction)
            {
                setupAction.InvalidModelStateResponseFactory = delegate (ActionContext context)
                {
                    ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Title = "資料驗證失敗",
                        Status = 422,
                        Detail = "請看下方詳細說明",
                        Instance = (string)context.HttpContext.Request.Path
                    };
                    validationProblemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(validationProblemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBikeStationRepository, BikeStationRepository>();
            services.AddTransient<IBikeRepository, BikeRepository>();
            services.AddTransient<IPaidmentService, PaidmentService>();
            services.AddTransient<IDashboardInfo, DashboardInfo>();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(delegate (SwaggerGenOptions c)
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "YouBike API",
                    Version = "v1"
                });
                OpenApiSecurityScheme openApiSecurityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", openApiSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    openApiSecurityScheme,
                    new string[1] { "Bearer" }
                } });
            });
        }

    }
}