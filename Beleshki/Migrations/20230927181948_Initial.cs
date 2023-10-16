using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beleshki.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fakultet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FakultetIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniverzitetIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fakultet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predmet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PredmetIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Krediti = table.Column<int>(type: "int", nullable: true),
                    Institut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudiskaGodina = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beleshka",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeleshkaIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumKreiranje = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DownloadUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PredmetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beleshka", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beleshka_Predmet_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredmetFakultet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    FakultetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredmetFakultet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredmetFakultet_Fakultet_FakultetId",
                        column: x => x.FakultetId,
                        principalTable: "Fakultet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PredmetFakultet_Predmet_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Komentar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentIme = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    komentar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Ocenka = table.Column<int>(type: "int", nullable: true),
                    BeleshkaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komentar_Beleshka_BeleshkaId",
                        column: x => x.BeleshkaId,
                        principalTable: "Beleshka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentBeleshki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentIme = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BeleshkaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBeleshki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentBeleshki_Beleshka_BeleshkaId",
                        column: x => x.BeleshkaId,
                        principalTable: "Beleshka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beleshka_PredmetId",
                table: "Beleshka",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_BeleshkaId",
                table: "Komentar",
                column: "BeleshkaId");

            migrationBuilder.CreateIndex(
                name: "IX_PredmetFakultet_FakultetId",
                table: "PredmetFakultet",
                column: "FakultetId");

            migrationBuilder.CreateIndex(
                name: "IX_PredmetFakultet_PredmetId",
                table: "PredmetFakultet",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBeleshki_BeleshkaId",
                table: "StudentBeleshki",
                column: "BeleshkaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Komentar");

            migrationBuilder.DropTable(
                name: "PredmetFakultet");

            migrationBuilder.DropTable(
                name: "StudentBeleshki");

            migrationBuilder.DropTable(
                name: "Fakultet");

            migrationBuilder.DropTable(
                name: "Beleshka");

            migrationBuilder.DropTable(
                name: "Predmet");
        }
    }
}
