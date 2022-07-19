using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodlessBoard.Migrations
{
    public partial class removedchats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GameId",
                table: "Messages",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Games_GameId",
                table: "Messages",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Games_GameId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_GameId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_GameId",
                table: "Chats",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
