using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUploadedBookInBookMOdel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
      name: "UploadedByUserId",
      table: "Books",
      type: "int",
      nullable: true,
      defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UploadedByUserId",
                table: "Books",
                column: "UploadedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UploadedByUserId",
                table: "Books",
                column: "UploadedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UploadedByUserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UploadedByUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UploadedByUserId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UserId",
                table: "Books",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
