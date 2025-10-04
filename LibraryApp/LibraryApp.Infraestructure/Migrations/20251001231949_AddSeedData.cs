using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LibraryItems",
                columns: new[] { "Id", "IsBorrowed", "IssueNumber", "Pages", "Publisher", "Title", "Type", "author" },
                values: new object[,]
                {
                    { 1, false, null, 180, null, "The Great Gatsby", 1, "F. Scott Fitzgerald" },
                    { 2, false, null, 328, null, "1984", 1, "George Orwell" },
                    { 3, false, 7, null, "Time USA LLC", "Time Magazine - July 2023", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Alice Johnson" },
                    { 2, "Bob Smith" },
                    { 3, "Charlie Brown" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LibraryItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
