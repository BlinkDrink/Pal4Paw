using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogCarePlatform.Data.Migrations
{
    public partial class MakeRatingDeletableModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Rating",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Rating",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_IsDeleted",
                table: "Rating",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rating_IsDeleted",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Rating");
        }
    }
}
