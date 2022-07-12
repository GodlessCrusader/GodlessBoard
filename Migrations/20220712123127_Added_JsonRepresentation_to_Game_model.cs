using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodlessBoard.Migrations
{
    public partial class Added_JsonRepresentation_to_Game_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JsonRepresentation",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonRepresentation",
                table: "Games");
        }
    }
}
