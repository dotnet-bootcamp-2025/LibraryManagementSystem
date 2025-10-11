using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "LibraryItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "BorrowedItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

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
            migrationBuilder.DropColumn(
                name: "Active",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "BorrowedItems");
        }
    }
}
