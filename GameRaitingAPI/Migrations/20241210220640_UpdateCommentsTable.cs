using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRaitingAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_Users_UsuarioId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_UsuarioId",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "comments");

            migrationBuilder.CreateIndex(
                name: "IX_comments_UserId",
                table: "comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Users_UserId",
                table: "comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_Users_UserId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_UserId",
                table: "comments");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_comments_UsuarioId",
                table: "comments",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Users_UsuarioId",
                table: "comments",
                column: "UsuarioId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
