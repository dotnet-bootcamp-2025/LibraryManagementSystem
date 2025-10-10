using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "LibraryItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BorrowedByMemberId",
                table: "LibraryItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Active", "BorrowedByMemberId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Active", "BorrowedByMemberId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Active", "BorrowedByMemberId" },
                values: new object[] { false, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "BorrowedByMemberId",
                table: "LibraryItems");
        }
    }
}
