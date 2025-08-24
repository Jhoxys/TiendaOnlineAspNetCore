using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapaAdmin.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvetory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingId",
                table: "Inventory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_BillingId",
                table: "Inventory",
                column: "BillingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Billing_BillingId",
                table: "Inventory",
                column: "BillingId",
                principalTable: "Billing",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Billing_BillingId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_BillingId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "BillingId",
                table: "Inventory");
        }
    }
}
