using Microsoft.EntityFrameworkCore.Migrations;

namespace DogCarePlatform.Data.Migrations
{
    public partial class AddDogsDescriptionColumnToOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DogsDescription",
                table: "Owners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DogsDescription",
                table: "Owners");
        }
    }
}
