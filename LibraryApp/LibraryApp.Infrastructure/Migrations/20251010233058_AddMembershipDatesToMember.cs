using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipDatesToMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
              name: "StartDate",
              table: "Members",
              type: "TEXT",
              nullable: false,
              defaultValue: new DateTime(2025, 1, 1)); 

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 1));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

                migrationBuilder.Sql(@"
    UPDATE Members
    SET StartDate = CURRENT_TIMESTAMP,
        EndDate = DATE(CURRENT_TIMESTAMP, '+1 year')
    WHERE StartDate IS NULL OR EndDate IS NULL;
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 4, 10, 17, 28, 30, 547, DateTimeKind.Local).AddTicks(6366), new DateTime(2025, 10, 10, 17, 28, 30, 547, DateTimeKind.Local).AddTicks(6166) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 10, 10, 17, 28, 30, 547, DateTimeKind.Local).AddTicks(6544), new DateTime(2025, 10, 10, 17, 28, 30, 547, DateTimeKind.Local).AddTicks(6543) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 10, 17, 28, 30, 547, DateTimeKind.Local).AddTicks(6547), new DateTime(2025, 10, 10, 17, 28, 30, 547, DateTimeKind.Local).AddTicks(6547) });
        }
    }
}
