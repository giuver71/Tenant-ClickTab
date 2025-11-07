using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClickTab.Core.Migrations.MSSQL
{
    /// <inheritdoc />
    public partial class Update_Categories_Add_Fee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "Categories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "FeeOnPurchasePrice",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFiscal",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Negative",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FeeOnPurchasePrice",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsFiscal",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Negative",
                table: "Categories");
        }
    }
}
