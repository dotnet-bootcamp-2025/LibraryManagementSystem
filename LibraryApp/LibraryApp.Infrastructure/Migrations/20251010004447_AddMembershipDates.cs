using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipEndDate",
                table: "Members",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025,10,09)
                );

            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipStartDate",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2026,10,09));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MembershipEndDate", "MembershipStartDate" },
                values: new object[] { new DateTime(2025, 1, 1), new DateTime(2026, 1, 1) }); //Alice is active

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "MembershipEndDate", "MembershipStartDate" },
                values: new object[] { new DateTime(2025, 10, 09), new DateTime(2026, 10, 09) });  //Bob is active

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MembershipEndDate", "MembershipStartDate" },
                values: new object[] { new DateTime(2024, 08, 08), new DateTime(2025, 08, 08) }); //Charlie is EXPIRED
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipEndDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MembershipStartDate",
                table: "Members");
        }
    }
}
