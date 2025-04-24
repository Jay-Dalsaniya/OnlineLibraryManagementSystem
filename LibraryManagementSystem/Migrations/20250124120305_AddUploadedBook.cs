using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUploadedBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the UploadedByUserId column
            migrationBuilder.AddColumn<int>(
                name: "UploadedByUserId",
                table: "Books",
                type: "int",
                nullable: true);  // Make sure it's nullable if the relationship is optional

            // Create an index for the UploadedByUserId column
            migrationBuilder.CreateIndex(
                name: "IX_Books_UploadedByUserId",
                table: "Books",
                column: "UploadedByUserId");

            // Add the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UploadedByUserId",
                table: "Books",
                column: "UploadedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);  // Avoid cascade delete conflict
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UploadedByUserId",
                table: "Books");

            // Drop the index on UploadedByUserId
            migrationBuilder.DropIndex(
                name: "IX_Books_UploadedByUserId",
                table: "Books");

            // Drop the UploadedByUserId column
            migrationBuilder.DropColumn(
                name: "UploadedByUserId",
                table: "Books");
        }
    }
}
