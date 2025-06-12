using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapaAdmin.Migrations
{
    /// <inheritdoc />
    public partial class SixteethMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NoFactura",
                table: "Billing",
                type: "nvarchar(max)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,2)",
                oldPrecision: 16,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NoFactura",
                table: "Billing",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldPrecision: 16,
                oldScale: 2);
        }
    }
}
