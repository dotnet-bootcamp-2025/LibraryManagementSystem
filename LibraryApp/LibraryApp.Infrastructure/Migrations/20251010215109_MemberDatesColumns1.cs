using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MemberDatesColumns1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
