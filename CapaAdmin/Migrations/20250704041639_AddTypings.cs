using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapaAdmin.Migrations
{
    /// <inheritdoc />
    public partial class AddTypings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Checks",
                table: "Billing",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "TypingId",
                table: "Billing",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Typing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", precision: 16, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", precision: 16, scale: 2, nullable: false),
                    ITB = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    NoFactura = table.Column<string>(type: "nvarchar(max)", precision: 16, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Typing", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Billing_TypingId",
                table: "Billing",
                column: "TypingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Billing_Typing_TypingId",
                table: "Billing",
                column: "TypingId",
                principalTable: "Typing",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billing_Typing_TypingId",
                table: "Billing");

            migrationBuilder.DropTable(
                name: "Typing");

            migrationBuilder.DropIndex(
                name: "IX_Billing_TypingId",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "TypingId",
                table: "Billing");

            migrationBuilder.AlterColumn<decimal>(
                name: "Checks",
                table: "Billing",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,2)",
                oldPrecision: 16,
                oldScale: 2);
        }
    }
}
