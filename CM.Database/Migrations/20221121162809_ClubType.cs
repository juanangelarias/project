using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CM.Database.Migrations
{
    public partial class ClubType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClubTypeId",
                table: "Club",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClubType",
                columns: table => new
                {
                    ClubTypeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubType", x => x.ClubTypeId);
                    table.ForeignKey(
                        name: "FK_ClubType_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_ClubType_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Club_ClubTypeId",
                table: "Club",
                column: "ClubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubType_CreatedBy",
                table: "ClubType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ClubType_ModifiedBy",
                table: "ClubType",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ClubType_Name",
                table: "ClubType",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Club_ClubType_ClubTypeId",
                table: "Club",
                column: "ClubTypeId",
                principalTable: "ClubType",
                principalColumn: "ClubTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Club_ClubType_ClubTypeId",
                table: "Club");

            migrationBuilder.DropTable(
                name: "ClubType");

            migrationBuilder.DropIndex(
                name: "IX_Club_ClubTypeId",
                table: "Club");

            migrationBuilder.DropColumn(
                name: "ClubTypeId",
                table: "Club");
        }
    }
}
