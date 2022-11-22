using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CM.Database.Migrations
{
    public partial class Conference_Currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                    table.ForeignKey(
                        name: "FK_Currency_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Currency_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Conference",
                columns: table => new
                {
                    ConferenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HostClubId = table.Column<long>(type: "bigint", nullable: false),
                    PrimaryCurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    SecondaryCurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conference", x => x.ConferenceId);
                    table.ForeignKey(
                        name: "FK_Conference_Club_HostClubId",
                        column: x => x.HostClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conference_Currency_PrimaryCurrencyId",
                        column: x => x.PrimaryCurrencyId,
                        principalTable: "Currency",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conference_Currency_SecondaryCurrencyId",
                        column: x => x.SecondaryCurrencyId,
                        principalTable: "Currency",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conference_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Conference_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conference_CreatedBy",
                table: "Conference",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Conference_HostClubId",
                table: "Conference",
                column: "HostClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Conference_ModifiedBy",
                table: "Conference",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Conference_PrimaryCurrencyId",
                table: "Conference",
                column: "PrimaryCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Conference_SecondaryCurrencyId",
                table: "Conference",
                column: "SecondaryCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_CreatedBy",
                table: "Currency",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_ModifiedBy",
                table: "Currency",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_Name",
                table: "Currency",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conference");

            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
