using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapaAdmin.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBillings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billing_Products_ProductId",
                table: "Billing");

            migrationBuilder.DropIndex(
                name: "IX_Billing_ProductId",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Billing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Billing",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Billing_ProductId",
                table: "Billing",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Billing_Products_ProductId",
                table: "Billing",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
