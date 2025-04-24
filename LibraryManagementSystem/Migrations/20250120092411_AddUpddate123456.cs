using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUpddate123456 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "BookRentals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "BookRentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "BookRentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "BookRentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "BookRentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "BookRentals",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "BookRentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BookRentals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "BookRentals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "BookRentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "BookRentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BookRentals",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalCopies",
                table: "BookRentals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "TotalCopies",
                table: "BookRentals");
        }
    }
}
