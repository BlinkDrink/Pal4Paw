using Microsoft.EntityFrameworkCore.Migrations;

namespace DogCarePlatform.Data.Migrations
{
    public partial class AddSentByColumnRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SentBy",
                table: "Rating",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentBy",
                table: "Rating");
        }
    }
}
