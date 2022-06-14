using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodlessBoard.Migrations
{
    public partial class ChangedUserMediaWeightTypeToUlong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MaxMediaWeight",
                table: "Users",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentMediaWeight",
                table: "Users",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Weight",
                table: "Media",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxMediaWeight",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentMediaWeight",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Media",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
