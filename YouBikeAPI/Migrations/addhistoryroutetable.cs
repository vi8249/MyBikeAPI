using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using YouBikeAPI.Data;

namespace YouBikeAPI.Migrations
{
	[DbContext(typeof(AppDbContext))]
	[Migration("20210801093302_add history route table")]
	public class addhistoryroutetable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable("HistoryRoutes", (ColumnsBuilder table) => new
			{
				Id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1"),
				Source = table.Column<Guid>("uniqueidentifier"),
				Destination = table.Column<Guid>("uniqueidentifier"),
				ApplicationUserId = table.Column<string>("nvarchar(450)", null, null, rowVersion: false, null, nullable: true),
				BikeId = table.Column<int>("int"),
				CurrentRoute = table.Column<bool>("bit"),
				BorrowTime = table.Column<DateTime>("datetime2"),
				ReturnTime = table.Column<DateTime>("datetime2")
			}, null, table =>
			{
				table.PrimaryKey("PK_HistoryRoutes", x => x.Id);
				table.ForeignKey("FK_HistoryRoutes_AspNetUsers_ApplicationUserId", x => x.ApplicationUserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
				table.ForeignKey("FK_HistoryRoutes_Bikes_BikeId", x => x.BikeId, "Bikes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			});
			migrationBuilder.UpdateData("AspNetRoles", "Id", "308660dc-ae51-480f-824d-7dca6714c3e2", "ConcurrencyStamp", "33133bce-8e20-4cb7-aad6-4eeba4c5b3b1");
			migrationBuilder.UpdateData("AspNetUsers", "Id", "90184155-dee0-40c9-bb1e-b5ed07afc04e", new string[3] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" }, new object[3] { "b52a0cbf-b328-45c2-b019-11ad2e8dddaa", "AQAAAAEAACcQAAAAENLNfVQQakDo3WKE0tp/hUDYNgcOziVe9q7h8WEXBkXWA5gPVRjUfzmNIEpxCIfMhA==", "5e95556f-b253-416b-8c44-b89acc7cdfe1" });
			migrationBuilder.CreateIndex("IX_HistoryRoutes_ApplicationUserId", "HistoryRoutes", "ApplicationUserId");
			migrationBuilder.CreateIndex("IX_HistoryRoutes_BikeId", "HistoryRoutes", "BikeId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable("HistoryRoutes");
			migrationBuilder.UpdateData("AspNetRoles", "Id", "308660dc-ae51-480f-824d-7dca6714c3e2", "ConcurrencyStamp", "3517609d-a193-433f-9efc-1abdcc4ea01e");
			migrationBuilder.UpdateData("AspNetUsers", "Id", "90184155-dee0-40c9-bb1e-b5ed07afc04e", new string[3] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" }, new object[3] { "6c81ef4d-f838-44f9-8b9c-e9f9e14089e1", "AQAAAAEAACcQAAAAEBTnaFLWuGHHC3FPhrrfSGnigpyj8/LRUQ2gKVaUNIGFPGYtq3pfEPAefbtu2niy6Q==", "16e323d8-05c9-47d3-9806-f476f18ef846" });
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
					ConcurrencyStamp = "33133bce-8e20-4cb7-aad6-4eeba4c5b3b1",
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
					ConcurrencyStamp = "b52a0cbf-b328-45c2-b019-11ad2e8dddaa",
					Email = "admin@xx.com",
					EmailConfirmed = true,
					LockoutEnabled = false,
					NormalizedEmail = "ADMIN@XX.COM",
					NormalizedUserName = "ADMIN@XX.COM",
					PasswordHash = "AQAAAAEAACcQAAAAENLNfVQQakDo3WKE0tp/hUDYNgcOziVe9q7h8WEXBkXWA5gPVRjUfzmNIEpxCIfMhA==",
					PhoneNumber = "123456789",
					PhoneNumberConfirmed = false,
					SecurityStamp = "5e95556f-b253-416b-8c44-b89acc7cdfe1",
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
			modelBuilder.Entity("YouBikeAPI.Models.HistoryRoute", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
					.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
				b.Property<string>("ApplicationUserId").HasColumnType("nvarchar(450)");
				b.Property<int>("BikeId").HasColumnType("int");
				b.Property<DateTime>("BorrowTime").HasColumnType("datetime2");
				b.Property<bool>("CurrentRoute").HasColumnType("bit");
				b.Property<Guid>("Destination").HasColumnType("uniqueidentifier");
				b.Property<DateTime>("ReturnTime").HasColumnType("datetime2");
				b.Property<Guid>("Source").HasColumnType("uniqueidentifier");
				b.HasKey("Id");
				b.HasIndex("ApplicationUserId");
				b.HasIndex("BikeId");
				b.ToTable("HistoryRoutes");
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
				b.HasOne("YouBikeAPI.Models.ApplicationUser", "User").WithOne("Bike").HasForeignKey("YouBikeAPI.Models.Bike", "UserId");
				b.Navigation("Price");
				b.Navigation("User");
			});
			modelBuilder.Entity("YouBikeAPI.Models.HistoryRoute", delegate(EntityTypeBuilder b)
			{
				b.HasOne("YouBikeAPI.Models.ApplicationUser", "User").WithMany("HistroyRoutes").HasForeignKey("ApplicationUserId");
				b.HasOne("YouBikeAPI.Models.Bike", "Bike").WithMany().HasForeignKey("BikeId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
				b.Navigation("Bike");
				b.Navigation("User");
			});
			modelBuilder.Entity("YouBikeAPI.Models.ApplicationUser", delegate(EntityTypeBuilder b)
			{
				b.Navigation("Bike");
				b.Navigation("HistroyRoutes");
				b.Navigation("UserRoles");
			});
			modelBuilder.Entity("YouBikeAPI.Models.BikeStation", delegate(EntityTypeBuilder b)
			{
				b.Navigation("AvailableBikes");
			});
		}
	}
}
