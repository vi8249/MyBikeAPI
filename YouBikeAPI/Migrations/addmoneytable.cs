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
	[Migration("20210802064556_add money table")]
	public class addmoneytable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<decimal>("Cost", "HistoryRoutes", "decimal(18,4)", null, null, rowVersion: false, null, nullable: false, 0m);
			migrationBuilder.AddColumn<Guid>("MoneyId", "AspNetUsers", "uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			migrationBuilder.CreateTable("Money", (ColumnsBuilder table) => new
			{
				Id = table.Column<Guid>("uniqueidentifier"),
				Value = table.Column<decimal>("decimal(18,4)")
			}, null, table =>
			{
				table.PrimaryKey("PK_Money", x => x.Id);
			});
			migrationBuilder.UpdateData("AspNetRoles", "Id", "308660dc-ae51-480f-824d-7dca6714c3e2", "ConcurrencyStamp", "f73f43a7-8a7e-4aad-8744-3258302931b3");
			migrationBuilder.UpdateData("AspNetUsers", "Id", "90184155-dee0-40c9-bb1e-b5ed07afc04e", new string[3] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" }, new object[3] { "61d1907d-4176-45f8-854f-791b8c02e8e7", "AQAAAAEAACcQAAAAEEj6SCMnp3KIsd5cjwZ3B15XJKAfCDIuj3+jCHihdn7xIsIBUXsTbCsvQb2EQEwsXg==", "25525449-b590-4cef-b9a9-5ed274d5cc6c" });
			migrationBuilder.CreateIndex("IX_AspNetUsers_MoneyId", "AspNetUsers", "MoneyId");
			migrationBuilder.AddForeignKey("FK_AspNetUsers_Money_MoneyId", "AspNetUsers", "MoneyId", "Money", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey("FK_AspNetUsers_Money_MoneyId", "AspNetUsers");
			migrationBuilder.DropTable("Money");
			migrationBuilder.DropIndex("IX_AspNetUsers_MoneyId", "AspNetUsers");
			migrationBuilder.DropColumn("Cost", "HistoryRoutes");
			migrationBuilder.DropColumn("MoneyId", "AspNetUsers");
			migrationBuilder.UpdateData("AspNetRoles", "Id", "308660dc-ae51-480f-824d-7dca6714c3e2", "ConcurrencyStamp", "33133bce-8e20-4cb7-aad6-4eeba4c5b3b1");
			migrationBuilder.UpdateData("AspNetUsers", "Id", "90184155-dee0-40c9-bb1e-b5ed07afc04e", new string[3] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" }, new object[3] { "b52a0cbf-b328-45c2-b019-11ad2e8dddaa", "AQAAAAEAACcQAAAAENLNfVQQakDo3WKE0tp/hUDYNgcOziVe9q7h8WEXBkXWA5gPVRjUfzmNIEpxCIfMhA==", "5e95556f-b253-416b-8c44-b89acc7cdfe1" });
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
					ConcurrencyStamp = "f73f43a7-8a7e-4aad-8744-3258302931b3",
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
				b.Property<Guid?>("MoneyId").HasColumnType("uniqueidentifier");
				b.Property<string>("NormalizedEmail").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.Property<string>("NormalizedUserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.Property<string>("PasswordHash").HasColumnType("nvarchar(max)");
				b.Property<string>("PhoneNumber").HasColumnType("nvarchar(max)");
				b.Property<bool>("PhoneNumberConfirmed").HasColumnType("bit");
				b.Property<string>("SecurityStamp").HasColumnType("nvarchar(max)");
				b.Property<bool>("TwoFactorEnabled").HasColumnType("bit");
				b.Property<string>("UserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
				b.HasKey("Id");
				b.HasIndex("MoneyId");
				b.HasIndex("NormalizedEmail").HasDatabaseName("EmailIndex");
				b.HasIndex("NormalizedUserName").IsUnique().HasDatabaseName("UserNameIndex")
					.HasFilter("[NormalizedUserName] IS NOT NULL");
				b.ToTable("AspNetUsers");
				b.HasData(new
				{
					Id = "90184155-dee0-40c9-bb1e-b5ed07afc04e",
					AccessFailedCount = 0,
					ConcurrencyStamp = "61d1907d-4176-45f8-854f-791b8c02e8e7",
					Email = "admin@xx.com",
					EmailConfirmed = true,
					LockoutEnabled = false,
					NormalizedEmail = "ADMIN@XX.COM",
					NormalizedUserName = "ADMIN@XX.COM",
					PasswordHash = "AQAAAAEAACcQAAAAEEj6SCMnp3KIsd5cjwZ3B15XJKAfCDIuj3+jCHihdn7xIsIBUXsTbCsvQb2EQEwsXg==",
					PhoneNumber = "123456789",
					PhoneNumberConfirmed = false,
					SecurityStamp = "25525449-b590-4cef-b9a9-5ed274d5cc6c",
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
				b.Property<decimal>("Cost").HasColumnType("decimal(18,4)");
				b.Property<bool>("CurrentRoute").HasColumnType("bit");
				b.Property<Guid>("Destination").HasColumnType("uniqueidentifier");
				b.Property<DateTime>("ReturnTime").HasColumnType("datetime2");
				b.Property<Guid>("Source").HasColumnType("uniqueidentifier");
				b.HasKey("Id");
				b.HasIndex("ApplicationUserId");
				b.HasIndex("BikeId");
				b.ToTable("HistoryRoutes");
			});
			modelBuilder.Entity("YouBikeAPI.Models.Money", delegate(EntityTypeBuilder b)
			{
				b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
				b.Property<decimal>("Value").HasColumnType("decimal(18,4)");
				b.HasKey("Id");
				b.ToTable("Money");
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
			modelBuilder.Entity("YouBikeAPI.Models.ApplicationUser", delegate(EntityTypeBuilder b)
			{
				b.HasOne("YouBikeAPI.Models.Money", "Money").WithMany().HasForeignKey("MoneyId");
				b.Navigation("Money");
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
