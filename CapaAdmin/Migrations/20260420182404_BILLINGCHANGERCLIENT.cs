using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapaAdmin.Migrations
{
    /// <inheritdoc />
    public partial class BILLINGCHANGERCLIENT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressSeller",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "NameSeller",
                table: "Billing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressSeller",
                table: "Billing",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameSeller",
                table: "Billing",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
