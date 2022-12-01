using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CM.Database.Migrations
{
    public partial class ConferenceProgramIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Program_ConferenceId",
                table: "Program");

            migrationBuilder.CreateIndex(
                name: "IX_Program_ConferenceId_DateTime",
                table: "Program",
                columns: new[] { "ConferenceId", "DateTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Program_ConferenceId_DateTime",
                table: "Program");

            migrationBuilder.CreateIndex(
                name: "IX_Program_ConferenceId",
                table: "Program",
                column: "ConferenceId");
        }
    }
}
