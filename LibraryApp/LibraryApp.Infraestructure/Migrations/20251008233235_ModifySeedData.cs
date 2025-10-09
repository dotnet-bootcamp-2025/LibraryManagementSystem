using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Pages",
                table: "LibraryItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowedDate",
                table: "LibraryItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowedDate",
                table: "BorrowedItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                columns: new[] { "BorrowedDate", "Pages" },
                values: new object[] { new DateTime(2025, 10, 8, 17, 32, 35, 316, DateTimeKind.Local).AddTicks(2370), 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowedDate",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "BorrowedDate",
                table: "BorrowedItems");

            migrationBuilder.AlterColumn<int>(
                name: "Pages",
                table: "LibraryItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "Pages",
                value: null);
        }
    }
}
