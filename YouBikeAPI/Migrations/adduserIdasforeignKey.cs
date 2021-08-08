using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using YouBikeAPI.Data;

namespace YouBikeAPI.Migrations
{
	[DbContext(typeof(AppDbContext))]
	[Migration("20210729080631_add userId as foreignKey")]
	public class adduserIdasforeignKey : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey("FK_AspNetUsers_Bikes_bikeId", "AspNetUsers");
			migrationBuilder.DropIndex("IX_AspNetUsers_bikeId", "AspNetUsers");
			migrationBuilder.DropColumn("bikeId", "AspNetUsers");
			migrationBuilder.AddColumn<string>("UserId", "Bikes", "nvarchar(450)", null, null, rowVersion: false, null, nullable: true);
			migrationBuilder.UpdateData("AspNetRoles", "Id", "308660dc-ae51-480f-824d-7dca6714c3e2", "ConcurrencyStamp", "3517609d-a193-433f-9efc-1abdcc4ea01e");
			migrationBuilder.UpdateData("AspNetUsers", "Id", "90184155-dee0-40c9-bb1e-b5ed07afc04e", new string[3] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" }, new object[3] { "6c81ef4d-f838-44f9-8b9c-e9f9e14089e1", "AQAAAAEAACcQAAAAEBTnaFLWuGHHC3FPhrrfSGnigpyj8/LRUQ2gKVaUNIGFPGYtq3pfEPAefbtu2niy6Q==", "16e323d8-05c9-47d3-9806-f476f18ef846" });
			migrationBuilder.CreateIndex("IX_Bikes_UserId", "Bikes", "UserId", null, unique: true, "[UserId] IS NOT NULL");
			migrationBuilder.AddForeignKey("FK_Bikes_AspNetUsers_UserId", "Bikes", "UserId", "AspNetUsers", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey("FK_Bikes_AspNetUsers_UserId", "Bikes");
			migrationBuilder.DropIndex("IX_Bikes_UserId", "Bikes");
			migrationBuilder.DropColumn("UserId", "Bikes");
			migrationBuilder.AddColumn<int>("bikeId", "AspNetUsers", "int", null, null, rowVersion: false, null, nullable: true);
			migrationBuilder.UpdateData("AspNetRoles", "Id", "308660dc-ae51-480f-824d-7dca6714c3e2", "ConcurrencyStamp", "c60988ac-3442-4dc2-80e1-0f84df297df1");
			migrationBuilder.UpdateData("AspNetUsers", "Id", "90184155-dee0-40c9-bb1e-b5ed07afc04e", new string[3] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" }, new object[3] { "fd6e4ca8-12ca-4adc-b4f0-eb7f55b073ec", "AQAAAAEAACcQAAAAEESMR8hNMCAdgtwO+HHGIUE8H1Ew4LL6b4xA6tRyoyvVitChJzWh4VINLMXcahltPA==", "f1259101-9f1f-4481-a362-6a36e3b3d8b3" });
			migrationBuilder.CreateIndex("IX_AspNetUsers_bikeId", "AspNetUsers", "bikeId");
			migrationBuilder.AddForeignKey("FK_AspNetUsers_Bikes_bikeId", "AspNetUsers", "bikeId", "Bikes", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Restrict);
		}

		protected override void BuildTargetModel(ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("ProductVersion", "5.0.6").HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("Id").HasColumnType("nvarchar(450)");
				b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
				b.Property<string>("Name").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.Property<string>("NormalizedName").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.HasKey("Id");
				b.HasIndex("NormalizedName").IsUnique().HasDatabaseName("RoleNameIndex")
					.HasFilter("[NormalizedName] IS NOT NULL");
				b.ToTable("AspNetRoles");
				b.HasData(new
				{
					Id = "308660dc-ae51-480f-824d-7dca6714c3e2",
					ConcurrencyStamp = "3517609d-a193-433f-9efc-1abdcc4ea01e",
					Name = "Admin",
					NormalizedName = "ADMIN"
				});
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
					.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
				b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
				b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
				b.Property<string>("RoleId").IsRequired().HasColumnType("nvarchar(450)");
				b.HasKey("Id");
				b.HasIndex("RoleId");
				b.ToTable("AspNetRoleClaims");
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
					.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
				b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
				b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
				b.Property<string>("UserId").IsRequired().HasColumnType("nvarchar(450)");
				b.HasKey("Id");
				b.HasIndex("UserId");
				b.ToTable("AspNetUserClaims");
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("LoginProvider").HasColumnType("nvarchar(450)");
				b.Property<string>("ProviderKey").HasColumnType("nvarchar(450)");
				b.Property<string>("ProviderDisplayName").HasColumnType("nvarchar(max)");
				b.Property<string>("UserId").IsRequired().HasColumnType("nvarchar(450)");
				b.HasKey("LoginProvider", "ProviderKey");
				b.HasIndex("UserId");
				b.ToTable("AspNetUserLogins");
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("UserId").HasColumnType("nvarchar(450)");
				b.Property<string>("RoleId").HasColumnType("nvarchar(450)");
				b.HasKey("UserId", "RoleId");
				b.HasIndex("RoleId");
				b.ToTable("AspNetUserRoles");
				b.HasData(new
				{
					UserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e",
					RoleId = "308660dc-ae51-480f-824d-7dca6714c3e2"
				});
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("UserId").HasColumnType("nvarchar(450)");
				b.Property<string>("LoginProvider").HasColumnType("nvarchar(450)");
				b.Property<string>("Name").HasColumnType("nvarchar(450)");
				b.Property<string>("Value").HasColumnType("nvarchar(max)");
				b.HasKey("UserId", "LoginProvider", "Name");
				b.ToTable("AspNetUserTokens");
			});
			modelBuilder.Entity("YouBikeAPI.Models.ApplicationUser", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("Id").HasColumnType("nvarchar(450)");
				b.Property<int>("AccessFailedCount").HasColumnType("int");
				b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
				b.Property<string>("Email").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.Property<bool>("EmailConfirmed").HasColumnType("bit");
				b.Property<bool>("LockoutEnabled").HasColumnType("bit");
				b.Property<DateTimeOffset?>("LockoutEnd").HasColumnType("datetimeoffset");
				b.Property<string>("NormalizedEmail").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.Property<string>("NormalizedUserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.Property<string>("PasswordHash").HasColumnType("nvarchar(max)");
				b.Property<string>("PhoneNumber").HasColumnType("nvarchar(max)");
				b.Property<bool>("PhoneNumberConfirmed").HasColumnType("bit");
				b.Property<string>("SecurityStamp").HasColumnType("nvarchar(max)");
				b.Property<bool>("TwoFactorEnabled").HasColumnType("bit");
				b.Property<string>("UserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.HasKey("Id");
				b.HasIndex("NormalizedEmail").HasDatabaseName("EmailIndex");
				b.HasIndex("NormalizedUserName").IsUnique().HasDatabaseName("UserNameIndex")
					.HasFilter("[NormalizedUserName] IS NOT NULL");
				b.ToTable("AspNetUsers");
				b.HasData(new
				{
					Id = "90184155-dee0-40c9-bb1e-b5ed07afc04e",
					AccessFailedCount = 0,
					ConcurrencyStamp = "6c81ef4d-f838-44f9-8b9c-e9f9e14089e1",
					Email = "admin@xx.com",
					EmailConfirmed = true,
					LockoutEnabled = false,
					NormalizedEmail = "ADMIN@XX.COM",
					NormalizedUserName = "ADMIN@XX.COM",
					PasswordHash = "AQAAAAEAACcQAAAAEBTnaFLWuGHHC3FPhrrfSGnigpyj8/LRUQ2gKVaUNIGFPGYtq3pfEPAefbtu2niy6Q==",
					PhoneNumber = "123456789",
					PhoneNumberConfirmed = false,
					SecurityStamp = "16e323d8-05c9-47d3-9806-f476f18ef846",
					TwoFactorEnabled = false,
					UserName = "admin@xx.com"
				});
			});
			modelBuilder.Entity("YouBikeAPI.Models.Bike", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
					.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
				b.Property<Guid?>("BikeStationId").HasColumnType("uniqueidentifier");
				b.Property<string>("BikeType").IsRequired().HasColumnType("nvarchar(450)");
				b.Property<int>("Mileage").HasColumnType("int");
				b.Property<bool>("Rented").HasColumnType("bit");
				b.Property<decimal>("Revenue").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
				b.Property<string>("UserId").HasColumnType("nvarchar(450)");
				b.HasKey("Id");
				b.HasIndex("BikeStationId");
				b.HasIndex("BikeType");
				b.HasIndex("UserId").IsUnique().HasFilter("[UserId] IS NOT NULL");
				b.ToTable("Bikes");
			});
			modelBuilder.Entity("YouBikeAPI.Models.BikeStation", delegate(EntityTypeBuilder b)
			{
				b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
				b.Property<int>("BikesInsideParkingLot").HasColumnType("int");
				b.Property<DateTime>("CreateTime").HasColumnType("datetime2");
				b.Property<double>("Latitude").HasColumnType("float");
				b.Property<double>("Longitude").HasColumnType("float");
				b.Property<string>("StationName").HasColumnType("nvarchar(max)");
				b.Property<int>("TotalParkingSpace").HasColumnType("int");
				b.Property<DateTime>("UpdateTime").HasColumnType("datetime2");
				b.HasKey("Id");
				b.ToTable("BikeStations");
			});
			modelBuilder.Entity("YouBikeAPI.Models.Price", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("BikeType").HasColumnType("nvarchar(450)");
				b.Property<int>("Cost").HasColumnType("int");
				b.Property<int>("Discount").HasColumnType("int");
				b.HasKey("BikeType");
				b.ToTable("Prices");
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null).WithMany().HasForeignKey("RoleId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("YouBikeAPI.Models.ApplicationUser", null).WithMany().HasForeignKey("UserId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("YouBikeAPI.Models.ApplicationUser", null).WithMany().HasForeignKey("UserId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null).WithMany().HasForeignKey("RoleId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
				b.HasOne("YouBikeAPI.Models.ApplicationUser", null).WithMany("UserRoles").HasForeignKey("UserId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("YouBikeAPI.Models.ApplicationUser", null).WithMany().HasForeignKey("UserId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
			});
			modelBuilder.Entity("YouBikeAPI.Models.Bike", delegate(EntityTypeBuilder b)
			{
				b.HasOne("YouBikeAPI.Models.BikeStation", null).WithMany("AvailableBikes").HasForeignKey("BikeStationId");
				b.HasOne("YouBikeAPI.Models.Price", "Price").WithMany().HasForeignKey("BikeType")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
				b.HasOne("YouBikeAPI.Models.ApplicationUser", "User").WithOne("bike").HasForeignKey("YouBikeAPI.Models.Bike", "UserId");
				b.Navigation("Price");
				b.Navigation("User");
			});
			modelBuilder.Entity("YouBikeAPI.Models.ApplicationUser", delegate(EntityTypeBuilder b)
			{
				b.Navigation("bike");
				b.Navigation("UserRoles");
			});
			modelBuilder.Entity("YouBikeAPI.Models.BikeStation", delegate(EntityTypeBuilder b)
			{
				b.Navigation("AvailableBikes");
			});
		}
	}
}
