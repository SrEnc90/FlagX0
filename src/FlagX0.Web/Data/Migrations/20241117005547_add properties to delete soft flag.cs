using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagX0.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class addpropertiestodeletesoftflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimeUtc",
                table: "Flags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Flags",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTimeUtc",
                table: "Flags");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Flags");
        }
    }
}
