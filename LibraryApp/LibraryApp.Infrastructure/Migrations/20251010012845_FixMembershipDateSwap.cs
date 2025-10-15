using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMembershipDateSwap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add temporary column
            migrationBuilder.AddColumn<string>(
                name: "TempDate",
                table: "Members",
                type: "TEXT",
                nullable: true);

            // Copy MembershipStartDate to TempDate
            migrationBuilder.Sql("UPDATE Members SET TempDate = MembershipStartDate;");

            // Copy MembershipEndDate to MembershipStartDate
            migrationBuilder.Sql("UPDATE Members SET MembershipStartDate = MembershipEndDate;");

            // Copy TempDate to MembershipEndDate
            migrationBuilder.Sql("UPDATE Members SET MembershipEndDate = TempDate;");

            // Drop temporary column
            migrationBuilder.DropColumn(
                name: "TempDate",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Swap back if rolling back this migration
            migrationBuilder.AddColumn<string>(
                name: "TempDate",
                table: "Members",
                type: "TEXT",
                nullable: true);

            migrationBuilder.Sql("UPDATE Members SET TempDate = MembershipStartDate;");
            migrationBuilder.Sql("UPDATE Members SET MembershipStartDate = MembershipEndDate;");
            migrationBuilder.Sql("UPDATE Members SET MembershipEndDate = TempDate;");

            migrationBuilder.DropColumn(
                name: "TempDate",
                table: "Members");
        }
    }
}