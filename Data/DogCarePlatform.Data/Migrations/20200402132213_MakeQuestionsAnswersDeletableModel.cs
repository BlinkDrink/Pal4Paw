using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogCarePlatform.Data.Migrations
{
    public partial class MakeQuestionsAnswersDeletableModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "QuestionsAnswers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuestionsAnswers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsAnswers_IsDeleted",
                table: "QuestionsAnswers",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuestionsAnswers_IsDeleted",
                table: "QuestionsAnswers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "QuestionsAnswers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuestionsAnswers");
        }
    }
}
