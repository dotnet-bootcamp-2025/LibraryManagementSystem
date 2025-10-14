using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedActiveColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "Active",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Active",
                value: false);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "Active",
                value: false);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "Active",
                value: false);
        }
    }
}
