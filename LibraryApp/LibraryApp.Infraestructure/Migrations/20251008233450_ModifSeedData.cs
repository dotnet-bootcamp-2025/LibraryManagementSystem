using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "BorrowedDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "BorrowedDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "BorrowedDate",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "BorrowedDate",
                value: new DateTime(2025, 10, 8, 17, 32, 35, 315, DateTimeKind.Local).AddTicks(840));

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "BorrowedDate",
                value: new DateTime(2025, 10, 8, 17, 32, 35, 316, DateTimeKind.Local).AddTicks(1993));

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "BorrowedDate",
                value: new DateTime(2025, 10, 8, 17, 32, 35, 316, DateTimeKind.Local).AddTicks(2370));
        }
    }
}
