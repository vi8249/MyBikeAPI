using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YouBikeAPI.Migrations
{
    public partial class modifliedhistoryRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationName",
                table: "HistoryRoutes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceName",
                table: "HistoryRoutes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "f315434a-470e-4d71-b9c5-8fb7e9c04b7c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8274719-6189-4737-ac45-3ab48422c103", new DateTime(2021, 8, 23, 8, 53, 42, 317, DateTimeKind.Utc).AddTicks(5590), "AQAAAAEAACcQAAAAEEg3Wdim620RFiE0bpV2wBsxNnwuQfA8HmSgePKJStBdP2xAN0A5iSnHwEbt2bPa8A==", "758e29ae-88ce-4e92-a315-2253a6bcc571" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationName",
                table: "HistoryRoutes");

            migrationBuilder.DropColumn(
                name: "SourceName",
                table: "HistoryRoutes");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "e2e65c6a-b36f-47d3-a12f-7f346fc3f77f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a304330-38d3-4e0e-a367-54c264d893e6", new DateTime(2021, 8, 20, 15, 31, 20, 539, DateTimeKind.Utc).AddTicks(7240), "AQAAAAEAACcQAAAAEIred7eMfda2b7I/mFptAl1WptlaXVafR8NuVGolx8x95KGnJ0XzjQnM/xKEG1ixMQ==", "94b43969-6469-4b62-b2a0-71c63ded007f" });
        }
    }
}
