using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBookPurchaseTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRentals_Books_BookId",
                table: "BookRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRentals_Users_UserId",
                table: "BookRentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRentals",
                table: "BookRentals");

            migrationBuilder.RenameTable(
                name: "BookRentals",
                newName: "BookRental");

            migrationBuilder.RenameIndex(
                name: "IX_BookRentals_UserId",
                table: "BookRental",
                newName: "IX_BookRental_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRentals_BookId",
                table: "BookRental",
                newName: "IX_BookRental_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRental",
                table: "BookRental",
                column: "BookRentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRental_Books_BookId",
                table: "BookRental",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRental_Users_UserId",
                table: "BookRental",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRental_Books_BookId",
                table: "BookRental");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRental_Users_UserId",
                table: "BookRental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRental",
                table: "BookRental");

            migrationBuilder.RenameTable(
                name: "BookRental",
                newName: "BookRentals");

            migrationBuilder.RenameIndex(
                name: "IX_BookRental_UserId",
                table: "BookRentals",
                newName: "IX_BookRentals_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRental_BookId",
                table: "BookRentals",
                newName: "IX_BookRentals_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRentals",
                table: "BookRentals",
                column: "BookRentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRentals_Books_BookId",
                table: "BookRentals",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRentals_Users_UserId",
                table: "BookRentals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
