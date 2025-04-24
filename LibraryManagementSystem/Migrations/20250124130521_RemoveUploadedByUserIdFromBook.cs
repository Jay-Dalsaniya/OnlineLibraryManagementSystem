using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    public partial class RemoveUploadedByUserIdFromBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key for the UploadedByUserId column
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UploadedByUserId", // Ensure the name matches the actual foreign key name
                table: "Books");

            // Drop the index for the UploadedByUserId column
            migrationBuilder.DropIndex(
                name: "IX_Books_UploadedByUserId", // Ensure the name matches the actual index name
                table: "Books");

            // Drop the UploadedByUserId column
            migrationBuilder.DropColumn(
                name: "UploadedByUserId", // Ensure the name matches the actual column name
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate the UploadedByUserId column if rolling back the migration
            migrationBuilder.AddColumn<int>(
                name: "UploadedByUserId", // Ensure the name matches the original column name
                table: "Books",
                type: "int",
                nullable: false, // Specify if it should be nullable or not
                defaultValue: 0); // Default value if required

            // Recreate the index for the UploadedByUserId column
            migrationBuilder.CreateIndex(
                name: "IX_Books_UploadedByUserId",
                table: "Books",
                column: "UploadedByUserId");

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UploadedByUserId",
                table: "Books",
                column: "UploadedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction); // Or use ReferentialAction.NoAction if appropriate
        }
    }
}
