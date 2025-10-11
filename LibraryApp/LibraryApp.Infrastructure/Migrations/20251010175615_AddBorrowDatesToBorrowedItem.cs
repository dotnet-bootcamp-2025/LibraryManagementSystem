using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowDatesToBorrowedItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- Add BorrowDate column ---
            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowDate",
                table: "BorrowedItems",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP"); // works in SQLite

            // --- Add ExpirationDate column ---
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "BorrowedItems",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "DATE(CURRENT_TIMESTAMP, '+3 days')");

            // --- Optional: Patch existing records (SQLite-compatible) ---
            migrationBuilder.Sql(@"
                UPDATE BorrowedItems 
                SET BorrowDate = COALESCE(NULLIF(BorrowDate, ''), CURRENT_TIMESTAMP),
                    ExpirationDate = COALESCE(NULLIF(ExpirationDate, ''), DATE(CURRENT_TIMESTAMP, '+3 days'))
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowDate",
                table: "BorrowedItems");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "BorrowedItems");
        }
    }
}
