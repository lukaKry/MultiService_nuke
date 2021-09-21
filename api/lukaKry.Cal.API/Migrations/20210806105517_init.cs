using Microsoft.EntityFrameworkCore.Migrations;

namespace lukaKry.Calc.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbols = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<decimal>(type: "decimal(10,10)", nullable: false),
                    Equation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equations");
        }
    }
}
