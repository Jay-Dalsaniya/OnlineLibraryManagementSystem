using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRentls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BookPurchases_UserId",
                table: "BookPurchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPurchases_Users_UserId",
                table: "BookPurchases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPurchases_Users_UserId",
                table: "BookPurchases");

            migrationBuilder.DropIndex(
                name: "IX_BookPurchases_UserId",
                table: "BookPurchases");
        }
    }
}
