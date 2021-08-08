using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouBikeAPI.Models;

namespace YouBikeAPI.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<BikeStation> BikeStations { get; set; }

		public DbSet<Bike> Bikes { get; set; }

		public DbSet<Price> Prices { get; set; }

		public DbSet<HistoryRoute> HistoryRoutes { get; set; }

		public DbSet<Money> Money { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base((DbContextOptions)options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<Bike>().Property((Bike b) => b.BikeType).HasConversion((BikeType b) => b.ToString(), (string b) => (BikeType)Enum.Parse(typeof(BikeType), b));
			builder.Entity<Price>().Property((Price b) => b.BikeType).HasConversion((BikeType b) => b.ToString(), (string b) => (BikeType)Enum.Parse(typeof(BikeType), b));
			builder.Entity<Bike>().Property((Bike b) => b.Revenue);
			builder.Entity(delegate(EntityTypeBuilder<ApplicationUser> u)
			{
				u.HasMany((ApplicationUser x) => x.UserRoles).WithOne().HasForeignKey((IdentityUserRole<string> ur) => ur.UserId)
					.IsRequired();
			});
			string adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2";
			builder.Entity<IdentityRole>().HasData(new IdentityRole
			{
				Id = adminRoleId,
				Name = "Admin",
				NormalizedName = "Admin".ToUpper()
			});
			string adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
			ApplicationUser adminUser = new ApplicationUser
			{
				Id = adminUserId,
				UserName = "admin@xx.com",
				NormalizedUserName = "admin@xx.com".ToUpper(),
				Email = "admin@xx.com",
				NormalizedEmail = "admin@xx.com".ToUpper(),
				TwoFactorEnabled = false,
				EmailConfirmed = true,
				PhoneNumber = "123456789",
				PhoneNumberConfirmed = false
			};
			PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
			adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "PaSSwoRD123$");
			builder.Entity<ApplicationUser>().HasData(adminUser);
			builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
			{
				RoleId = adminRoleId,
				UserId = adminUserId
			});
		}
	}
}
