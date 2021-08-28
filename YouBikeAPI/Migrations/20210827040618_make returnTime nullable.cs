using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YouBikeAPI.Migrations
{
    public partial class makereturnTimenullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnTime",
                table: "HistoryRoutes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "6889d3d0-a970-4321-ae61-1619ca4e278f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8002045d-33e2-43ef-b092-b92d4890a335", new DateTime(2021, 8, 27, 4, 6, 17, 746, DateTimeKind.Utc).AddTicks(4660), "AQAAAAEAACcQAAAAEIEzPCfORjlmXWJBqqdAO5R8WkUvaZWTOaeRI8dpo48fqrb/OJXEAfxkPRABjXAgEQ==", "38f000d4-abd6-451e-aab8-eaf7959b4d6d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnTime",
                table: "HistoryRoutes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
    }
}
