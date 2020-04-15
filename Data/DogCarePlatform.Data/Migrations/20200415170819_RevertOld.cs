using Microsoft.EntityFrameworkCore.Migrations;

namespace DogCarePlatform.Data.Migrations
{
    public partial class RevertOld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Rating_RatingId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_RatingId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "RatingId1",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "RatingScore",
                table: "Comments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingScore",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Rating",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RatingId1",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RatingId1",
                table: "Comments",
                column: "RatingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Rating_RatingId1",
                table: "Comments",
                column: "RatingId1",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
