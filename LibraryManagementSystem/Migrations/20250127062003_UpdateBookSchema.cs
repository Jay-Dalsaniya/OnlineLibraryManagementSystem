using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Books_Users_UploadedByUserId",
            //    table: "Books");

            //migrationBuilder.DropIndex(
            //    name: "IX_Books_UploadedByUserId",
            //    table: "Books");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UploadedByUserId",
                table: "Books",
                type: "int",
                nullable: false);


            migrationBuilder.AddForeignKey(
         name: "FK_Books_Users_UploadedByUserId",
         table: "Books",
         column: "UploadedByUserId",
         principalTable: "Users",
         principalColumn: "Id",
         onDelete: ReferentialAction.Cascade);
        }
    }
    }

