using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapaAdmin.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingAddressPhoneSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
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

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Billing",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "NameSeller",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Billing");
        }
    }
}
