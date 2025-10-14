using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MemberDatesColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 20, 21, 46, 47, 892, DateTimeKind.Utc).AddTicks(6817), new DateTime(2025, 9, 20, 21, 46, 47, 892, DateTimeKind.Utc).AddTicks(6545) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 8, 21, 46, 47, 892, DateTimeKind.Utc).AddTicks(6945), new DateTime(2025, 9, 8, 21, 46, 47, 892, DateTimeKind.Utc).AddTicks(6944) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 20, 21, 46, 47, 892, DateTimeKind.Utc).AddTicks(6946), new DateTime(2025, 9, 20, 21, 46, 47, 892, DateTimeKind.Utc).AddTicks(6946) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Members");
        }
    }
}
