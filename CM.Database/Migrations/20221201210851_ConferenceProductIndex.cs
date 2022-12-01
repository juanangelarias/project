using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CM.Database.Migrations
{
    public partial class ConferenceProductIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ConferenceId",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ConferenceId_Name",
                table: "Product",
                columns: new[] { "ConferenceId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ConferenceId_Name",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ConferenceId",
                table: "Product",
                column: "ConferenceId");
        }
    }
}
