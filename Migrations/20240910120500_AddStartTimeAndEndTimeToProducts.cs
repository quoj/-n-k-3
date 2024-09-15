using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product_Creation.Migrations
{
    /// <inheritdoc />
    public partial class AddStartTimeAndEndTimeToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_categories_category_id",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Products_categories_category_id",
                table: "Products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_categories_category_id",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_categories_category_id",
                table: "Products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
