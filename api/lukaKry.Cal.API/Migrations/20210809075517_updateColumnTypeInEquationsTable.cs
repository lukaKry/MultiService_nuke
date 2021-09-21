using Microsoft.EntityFrameworkCore.Migrations;

namespace lukaKry.Calc.API.Migrations
{
    public partial class updateColumnTypeInEquationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Result",
                table: "Equations",
                type: "decimal(28,14)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Result",
                table: "Equations",
                type: "decimal(10,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28,14)");
        }
    }
}
