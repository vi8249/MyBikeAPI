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
using YouBikeAPI.Models;
using YouBikeAPI.Services;

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
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication("Bearer").AddJwtBearer(delegate (JwtBearerOptions options)
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
            services.AddHttpClient();
            services.AddDbContext<AppDbContext>(delegate (DbContextOptionsBuilder options)
            {
                options.UseSqlServer(configuration["DbContext:ConnectionString"]);
            });
            services.AddTransient<IBikeStationRepository, BikeStationRepository>();
            services.AddTransient<IBikeRepository, BikeRepository>();
            services.AddTransient<IPaidmentService, PaidmentService>();
            services.AddTransient<IDashboardInfo, DashboardInfo>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddCors();
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
                endpoints.MapSwagger();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
