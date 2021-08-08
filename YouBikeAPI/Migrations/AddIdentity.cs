using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using YouBikeAPI.Data;

namespace YouBikeAPI.Migrations
{
	[DbContext(typeof(AppDbContext))]
	[Migration("20210727081446_Add Identity")]
	public class AddIdentity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable("AspNetRoles", delegate(ColumnsBuilder table)
			{
				OperationBuilder<AddColumnOperation> id2 = table.Column<string>("nvarchar(450)");
				int? maxLength2 = 256;
				OperationBuilder<AddColumnOperation> name = table.Column<string>("nvarchar(256)", null, maxLength2, rowVersion: false, null, nullable: true);
				maxLength2 = 256;
				return new
				{
					Id = id2,
					Name = name,
					NormalizedName = table.Column<string>("nvarchar(256)", null, maxLength2, rowVersion: false, null, nullable: true),
					ConcurrencyStamp = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
				};
			}, null, table =>
			{
				table.PrimaryKey("PK_AspNetRoles", x => x.Id);
			});
			migrationBuilder.CreateTable("AspNetUsers", delegate(ColumnsBuilder table)
			{
				OperationBuilder<AddColumnOperation> id = table.Column<string>("nvarchar(450)");
				OperationBuilder<AddColumnOperation> bikeId = table.Column<int>("int", null, null, rowVersion: false, null, nullable: true);
				int? maxLength = 256;
				OperationBuilder<AddColumnOperation> userName = table.Column<string>("nvarchar(256)", null, maxLength, rowVersion: false, null, nullable: true);
				maxLength = 256;
				OperationBuilder<AddColumnOperation> normalizedUserName = table.Column<string>("nvarchar(256)", null, maxLength, rowVersion: false, null, nullable: true);
				maxLength = 256;
				OperationBuilder<AddColumnOperation> email = table.Column<string>("nvarchar(256)", null, maxLength, rowVersion: false, null, nullable: true);
				maxLength = 256;
				return new
				{
					Id = id,
					bikeId = bikeId,
					UserName = userName,
					NormalizedUserName = normalizedUserName,
					Email = email,
					NormalizedEmail = table.Column<string>("nvarchar(256)", null, maxLength, rowVersion: false, null, nullable: true),
					EmailConfirmed = table.Column<bool>("bit"),
					PasswordHash = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
					SecurityStamp = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
					ConcurrencyStamp = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
					PhoneNumber = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
					PhoneNumberConfirmed = table.Column<bool>("bit"),
					TwoFactorEnabled = table.Column<bool>("bit"),
					LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset", null, null, rowVersion: false, null, nullable: true),
					LockoutEnabled = table.Column<bool>("bit"),
					AccessFailedCount = table.Column<int>("int")
				};
			}, null, table =>
			{
				table.PrimaryKey("PK_AspNetUsers", x => x.Id);
				table.ForeignKey("FK_AspNetUsers_Bikes_bikeId", x => x.bikeId, "Bikes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			});
			migrationBuilder.CreateTable("AspNetRoleClaims", (ColumnsBuilder table) => new
			{
				Id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1"),
				RoleId = table.Column<string>("nvarchar(450)"),
				ClaimType = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				ClaimValue = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
			}, null, table =>
			{
				table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
				table.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId", x => x.RoleId, "AspNetRoles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			});
			migrationBuilder.CreateTable("AspNetUserClaims", (ColumnsBuilder table) => new
			{
				Id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1"),
				UserId = table.Column<string>("nvarchar(450)"),
				ClaimType = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				ClaimValue = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
			}, null, table =>
			{
				table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
				table.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			});
			migrationBuilder.CreateTable("AspNetUserLogins", (ColumnsBuilder table) => new
			{
				LoginProvider = table.Column<string>("nvarchar(450)"),
				ProviderKey = table.Column<string>("nvarchar(450)"),
				ProviderDisplayName = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				UserId = table.Column<string>("nvarchar(450)")
			}, null, table =>
			{
				table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
				table.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			});
			migrationBuilder.CreateTable("AspNetUserRoles", (ColumnsBuilder table) => new
			{
				UserId = table.Column<string>("nvarchar(450)"),
				RoleId = table.Column<string>("nvarchar(450)")
			}, null, table =>
			{
				table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
				table.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId", x => x.RoleId, "AspNetRoles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
				table.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			});
			migrationBuilder.CreateTable("AspNetUserTokens", (ColumnsBuilder table) => new
			{
				UserId = table.Column<string>("nvarchar(450)"),
				LoginProvider = table.Column<string>("nvarchar(450)"),
				Name = table.Column<string>("nvarchar(450)"),
				Value = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
			}, null, table =>
			{
				table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
				table.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			});
			migrationBuilder.InsertData("AspNetRoles", new string[4] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" }, new object[4] { "308660dc-ae51-480f-824d-7dca6714c3e2", "c60988ac-3442-4dc2-80e1-0f84df297df1", "Admin", "ADMIN" });
			migrationBuilder.InsertData("AspNetUsers", new string[16]
			{
				"Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash",
				"PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "bikeId"
			}, new object[16]
			{
				"90184155-dee0-40c9-bb1e-b5ed07afc04e", 0, "fd6e4ca8-12ca-4adc-b4f0-eb7f55b073ec", "admin@xx.com", true, false, null, "ADMIN@XX.COM", "ADMIN@XX.COM", "AQAAAAEAACcQAAAAEESMR8hNMCAdgtwO+HHGIUE8H1Ew4LL6b4xA6tRyoyvVitChJzWh4VINLMXcahltPA==",
				"123456789", false, "f1259101-9f1f-4481-a362-6a36e3b3d8b3", false, "admin@xx.com", null
			});
			migrationBuilder.InsertData("AspNetUserRoles", new string[2] { "RoleId", "UserId" }, new object[2] { "308660dc-ae51-480f-824d-7dca6714c3e2", "90184155-dee0-40c9-bb1e-b5ed07afc04e" });
			migrationBuilder.CreateIndex("IX_AspNetRoleClaims_RoleId", "AspNetRoleClaims", "RoleId");
			migrationBuilder.CreateIndex("RoleNameIndex", "AspNetRoles", "NormalizedName", null, unique: true, "[NormalizedName] IS NOT NULL");
			migrationBuilder.CreateIndex("IX_AspNetUserClaims_UserId", "AspNetUserClaims", "UserId");
			migrationBuilder.CreateIndex("IX_AspNetUserLogins_UserId", "AspNetUserLogins", "UserId");
			migrationBuilder.CreateIndex("IX_AspNetUserRoles_RoleId", "AspNetUserRoles", "RoleId");
			migrationBuilder.CreateIndex("EmailIndex", "AspNetUsers", "NormalizedEmail");
			migrationBuilder.CreateIndex("IX_AspNetUsers_bikeId", "AspNetUsers", "bikeId");
			migrationBuilder.CreateIndex("UserNameIndex", "AspNetUsers", "NormalizedUserName", null, unique: true, "[NormalizedUserName] IS NOT NULL");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable("AspNetRoleClaims");
			migrationBuilder.DropTable("AspNetUserClaims");
			migrationBuilder.DropTable("AspNetUserLogins");
			migrationBuilder.DropTable("AspNetUserRoles");
			migrationBuilder.DropTable("AspNetUserTokens");
			migrationBuilder.DropTable("AspNetRoles");
			migrationBuilder.DropTable("AspNetUsers");
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
					ConcurrencyStamp = "c60988ac-3442-4dc2-80e1-0f84df297df1",
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
				b.Property<int?>("bikeId").HasColumnType("int");
				b.HasKey("Id");
				b.HasIndex("NormalizedEmail").HasDatabaseName("EmailIndex");
				b.HasIndex("NormalizedUserName").IsUnique().HasDatabaseName("UserNameIndex")
					.HasFilter("[NormalizedUserName] IS NOT NULL");
				b.HasIndex("bikeId");
				b.ToTable("AspNetUsers");
				b.HasData(new
				{
					Id = "90184155-dee0-40c9-bb1e-b5ed07afc04e",
					AccessFailedCount = 0,
					ConcurrencyStamp = "fd6e4ca8-12ca-4adc-b4f0-eb7f55b073ec",
					Email = "admin@xx.com",
					EmailConfirmed = true,
					LockoutEnabled = false,
					NormalizedEmail = "ADMIN@XX.COM",
					NormalizedUserName = "ADMIN@XX.COM",
					PasswordHash = "AQAAAAEAACcQAAAAEESMR8hNMCAdgtwO+HHGIUE8H1Ew4LL6b4xA6tRyoyvVitChJzWh4VINLMXcahltPA==",
					PhoneNumber = "123456789",
					PhoneNumberConfirmed = false,
					SecurityStamp = "f1259101-9f1f-4481-a362-6a36e3b3d8b3",
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
				b.HasKey("Id");
				b.HasIndex("BikeStationId");
				b.HasIndex("BikeType");
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
			modelBuilder.Entity("YouBikeAPI.Models.ApplicationUser", delegate(EntityTypeBuilder b)
			{
				b.HasOne("YouBikeAPI.Models.Bike", "bike").WithMany().HasForeignKey("bikeId");
				b.Navigation("bike");
			});
			modelBuilder.Entity("YouBikeAPI.Models.Bike", delegate(EntityTypeBuilder b)
			{
				b.HasOne("YouBikeAPI.Models.BikeStation", null).WithMany("AvailableBikes").HasForeignKey("BikeStationId");
				b.HasOne("YouBikeAPI.Models.Price", "Price").WithMany().HasForeignKey("BikeType")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
				b.Navigation("Price");
			});
			modelBuilder.Entity("YouBikeAPI.Models.ApplicationUser", delegate(EntityTypeBuilder b)
			{
				b.Navigation("UserRoles");
			});
			modelBuilder.Entity("YouBikeAPI.Models.BikeStation", delegate(EntityTypeBuilder b)
			{
				b.Navigation("AvailableBikes");
			});
		}
	}
}
