using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagX0.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatepropertynametoisDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Flags",
                newName: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Flags",
                newName: "IsDelete");
        }
    }
}
