using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowDateAndActiveColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBorrowed",
                table: "LibraryItems",
                newName: "isBorrowed");

            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowed",
                table: "LibraryItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "BorrowedItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

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
                column: "IsBorrowed",
                value: false);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsBorrowed",
                value: false);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsBorrowed",
                value: false);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsBorrowed",
                value: false);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsBorrowed", "Type" },
                values: new object[] { false, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBorrowed",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "BorrowedItems");

            migrationBuilder.DropColumn(
                name: "BorrowedDate",
                table: "BorrowedItems");

            migrationBuilder.RenameColumn(
                name: "isBorrowed",
                table: "LibraryItems",
                newName: "IsBorrowed");

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "Type",
                value: 1);
        }
    }
}
