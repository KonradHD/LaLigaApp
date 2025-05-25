using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaLiga.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Druzyna",
                columns: table => new
                {
                    id_druzyny = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nazwa_druzyny = table.Column<string>(type: "TEXT", nullable: false),
                    punkty = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    gole = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Druzyna", x => x.id_druzyny);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownik",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    haslo = table.Column<string>(type: "TEXT", nullable: false),
                    wiek = table.Column<int>(type: "INTEGER", nullable: false),
                    imie = table.Column<string>(type: "TEXT", nullable: false),
                    nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    data_dołączenia = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownik", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Mecz",
                columns: table => new
                {
                    id_meczu = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_gosci = table.Column<int>(type: "INTEGER", nullable: false),
                    id_gospodarzy = table.Column<int>(type: "INTEGER", nullable: false),
                    termin = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mecz", x => x.id_meczu);
                    table.ForeignKey(
                        name: "FK_Mecz_Druzyna_id_gosci",
                        column: x => x.id_gosci,
                        principalTable: "Druzyna",
                        principalColumn: "id_druzyny",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mecz_Druzyna_id_gospodarzy",
                        column: x => x.id_gospodarzy,
                        principalTable: "Druzyna",
                        principalColumn: "id_druzyny",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zawodnik",
                columns: table => new
                {
                    id_druzyny = table.Column<int>(type: "INTEGER", nullable: false),
                    numer = table.Column<int>(type: "INTEGER", nullable: false),
                    imie = table.Column<string>(type: "TEXT", nullable: false),
                    nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    pozycja = table.Column<string>(type: "TEXT", nullable: true),
                    wiek = table.Column<int>(type: "INTEGER", nullable: false),
                    wartosc_rynkowa = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zawodnik", x => new { x.id_druzyny, x.numer });
                    table.ForeignKey(
                        name: "FK_Zawodnik_Druzyna_id_druzyny",
                        column: x => x.id_druzyny,
                        principalTable: "Druzyna",
                        principalColumn: "id_druzyny",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statystyki",
                columns: table => new
                {
                    id_meczu = table.Column<int>(type: "INTEGER", nullable: false),
                    gole_gospodarzy = table.Column<int>(type: "INTEGER", nullable: false),
                    gole_gosci = table.Column<int>(type: "INTEGER", nullable: false),
                    strzaly_gospodarzy = table.Column<int>(type: "INTEGER", nullable: false),
                    strzaly_gosci = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statystyki", x => x.id_meczu);
                    table.ForeignKey(
                        name: "FK_Statystyki_Mecz_id_meczu",
                        column: x => x.id_meczu,
                        principalTable: "Mecz",
                        principalColumn: "id_meczu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Strzelec",
                columns: table => new
                {
                    id_druzyny = table.Column<int>(type: "INTEGER", nullable: false),
                    numer = table.Column<int>(type: "INTEGER", nullable: false),
                    id_meczu = table.Column<int>(type: "INTEGER", nullable: false),
                    gole = table.Column<int>(type: "INTEGER", nullable: true),
                    asysty = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strzelec", x => new { x.id_druzyny, x.numer, x.id_meczu });
                    table.ForeignKey(
                        name: "FK_Strzelec_Mecz_id_meczu",
                        column: x => x.id_meczu,
                        principalTable: "Mecz",
                        principalColumn: "id_meczu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Strzelec_Zawodnik_id_druzyny_numer",
                        columns: x => new { x.id_druzyny, x.numer },
                        principalTable: "Zawodnik",
                        principalColumns: new[] { "id_druzyny", "numer" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mecz_id_gosci",
                table: "Mecz",
                column: "id_gosci");

            migrationBuilder.CreateIndex(
                name: "IX_Mecz_id_gospodarzy",
                table: "Mecz",
                column: "id_gospodarzy");

            migrationBuilder.CreateIndex(
                name: "IX_Strzelec_id_meczu",
                table: "Strzelec",
                column: "id_meczu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statystyki");

            migrationBuilder.DropTable(
                name: "Strzelec");

            migrationBuilder.DropTable(
                name: "Uzytkownik");

            migrationBuilder.DropTable(
                name: "Mecz");

            migrationBuilder.DropTable(
                name: "Zawodnik");

            migrationBuilder.DropTable(
                name: "Druzyna");
        }
    }
}
