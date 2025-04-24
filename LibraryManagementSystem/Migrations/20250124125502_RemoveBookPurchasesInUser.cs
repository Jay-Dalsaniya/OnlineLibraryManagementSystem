using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    public partial class RemoveBookPurchasesInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT 1 
            FROM sys.foreign_keys 
            WHERE name = 'FK_Users_BookPurchases_BookPurchaseId'
        )
        BEGIN
            ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_BookPurchases_BookPurchaseId];
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // If you want to restore the `BookPurchases` field during rollback, you should recreate it
            migrationBuilder.AddForeignKey(
          name: "FK_Users_BookPurchases_BookPurchaseId",
          table: "Users",
          column: "BookPurchaseId",
          principalTable: "BookPurchases",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
        }
    }
}
