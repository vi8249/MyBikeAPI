using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YouBikeAPI.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BikeStations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalParkingSpace = table.Column<int>(type: "int", nullable: false),
                    BikesInsideParkingLot = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BikeStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    BikeType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.BikeType);
                });

            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Rented = table.Column<bool>(type: "bit", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BikeType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BikeStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bikes_BikeStations_BikeStationId",
                        column: x => x.BikeStationId,
                        principalTable: "BikeStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bikes_Prices_BikeType",
                        column: x => x.BikeType,
                        principalTable: "Prices",
                        principalColumn: "BikeType",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_BikeStationId",
                table: "Bikes",
                column: "BikeStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_BikeType",
                table: "Bikes",
                column: "BikeType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bikes");

            migrationBuilder.DropTable(
                name: "BikeStations");

            migrationBuilder.DropTable(
                name: "Prices");
        }
    }
}
