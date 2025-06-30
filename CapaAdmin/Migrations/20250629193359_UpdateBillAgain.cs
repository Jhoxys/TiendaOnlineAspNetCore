using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapaAdmin.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBillAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Billing",
                type: "nvarchar(max)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Billing");
        }
    }
}
