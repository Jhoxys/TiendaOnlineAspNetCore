using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapaAdmin.Migrations
{
    /// <inheritdoc />
    public partial class CodepUpdat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CodeProduct",
                table: "Products",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeProduct",
                table: "Products");
        }
    }
}
