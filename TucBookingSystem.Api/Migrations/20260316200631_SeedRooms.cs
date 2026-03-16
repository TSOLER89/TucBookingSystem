using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TucBookingSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "CreatedAt", "Description", "IsActive", "Location", "Name" },
                values: new object[,]
                {
                    { 1, 8, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Mötesrum Emmalund", true, "Linköping", "Emmalund" },
                    { 2, 12, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Mötesrum Rosenkälla", true, "Linköping", "Rosenkälla" },
                    { 3, 4, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Mötesrum Roxen", true, "Linköping", "Roxen" },
                    { 4, 8, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Mötesrum Berg", true, "Linköping", "Berg" },
                    { 5, 6, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Mötesrum Ådala", true, "Linköping", "Ådala" },
                    { 6, 6, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Mötesrum Stångån", true, "Linköping", "Stångån" },
                    { 7, 4, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Mötesrum Tinnerö", true, "Linköping", "Tinnerö" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
