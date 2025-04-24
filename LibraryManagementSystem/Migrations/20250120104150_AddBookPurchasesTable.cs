using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBookPurchasesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "BookPurchases",
                columns: table => new
                {
                    BookPurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TotalCopies = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookPurchases", x => x.BookPurchaseId);
                    table.ForeignKey(
                        name: "FK_BookPurchases_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookPurchases_BookId",
                table: "BookPurchases",
                column: "BookId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRentals_Books_BookId",
                table: "BookRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRentals_Users_UserId",
                table: "BookRentals");

            migrationBuilder.DropTable(
                name: "BookPurchases");

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
    }
}
