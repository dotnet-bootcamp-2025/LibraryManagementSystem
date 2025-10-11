using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LibraryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    IsBorrowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    Pages = table.Column<int>(type: "INTEGER", nullable: true),
                    IssueNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    Publisher = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReturnDeadLine = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowedItems_LibraryItems_LibraryItemId",
                        column: x => x.LibraryItemId,
                        principalTable: "LibraryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowedItems_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LibraryItems",
                columns: new[] { "Id", "Author", "IsBorrowed", "IssueNumber", "Pages", "Publisher", "Title", "Type" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", false, null, 100, null, "The Great Gatsby", 1 },
                    { 2, "George Orwell", false, null, 328, null, "1984", 1 },
                    { 3, null, false, 7, null, "Time USA LLC", "Time Magazine - July 2023", 2 }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "EndDate", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 10, 1, 26, 26, 902, DateTimeKind.Local).AddTicks(7329), "Alice Johnson", new DateTime(2025, 10, 10, 1, 26, 26, 900, DateTimeKind.Local).AddTicks(9253) },
                    { 2, new DateTime(2026, 1, 10, 1, 26, 26, 902, DateTimeKind.Local).AddTicks(7329), "Bob Smith", new DateTime(2025, 10, 10, 1, 26, 26, 900, DateTimeKind.Local).AddTicks(9253) },
                    { 3, new DateTime(2026, 1, 10, 1, 26, 26, 902, DateTimeKind.Local).AddTicks(7329), "Charlie Brown", new DateTime(2025, 10, 10, 1, 26, 26, 900, DateTimeKind.Local).AddTicks(9253) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowedItems_LibraryItemId",
                table: "BorrowedItems",
                column: "LibraryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowedItems_MemberId",
                table: "BorrowedItems",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowedItems");

            migrationBuilder.DropTable(
                name: "LibraryItems");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
