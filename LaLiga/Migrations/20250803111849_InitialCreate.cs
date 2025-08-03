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
                    stadion = table.Column<string>(type: "TEXT", nullable: false),
                    punkty = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    gole = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
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
                    data_dolaczenia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    rola = table.Column<string>(type: "TEXT", nullable: true)
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
                    termin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    sedzia = table.Column<string>(type: "TEXT", nullable: false)
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
                    imie = table.Column<string>(type: "TEXT", nullable: true),
                    nazwisko = table.Column<string>(type: "TEXT", nullable: true),
                    pozycja = table.Column<string>(type: "TEXT", nullable: true),
                    wiek = table.Column<int>(type: "INTEGER", nullable: false),
                    kraj_pochodzenia = table.Column<string>(type: "TEXT", nullable: true),
                    injured = table.Column<bool>(type: "INTEGER", nullable: false),
                    APIid = table.Column<int>(type: "INTEGER", nullable: false)
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
                    gole_gospodarzy = table.Column<int>(type: "INTEGER", nullable: true),
                    gole_gosci = table.Column<int>(type: "INTEGER", nullable: true),
                    strzaly_gospodarzy = table.Column<int>(type: "INTEGER", nullable: true),
                    strzaly_gosci = table.Column<int>(type: "INTEGER", nullable: true),
                    posiadanie_pilki_gospodarzy = table.Column<int>(type: "INTEGER", nullable: true),
                    posiadanie_pilki_gosci = table.Column<int>(type: "INTEGER", nullable: true)
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
                    asysty = table.Column<int>(type: "INTEGER", nullable: true),
                    samoboje = table.Column<int>(type: "INTEGER", nullable: true),
                    APIid = table.Column<int>(type: "INTEGER", nullable: false)
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
            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS new_guests_goals AFTER INSERT ON Statystyki
                                    BEGIN
                                        UPDATE Druzyna SET gole = gole + NEW.gole_gosci WHERE id_druzyny = 
                                        (SELECT m.id_gosci from Mecz m WHERE m.id_meczu = NEW.id_meczu)
                                        AND NOT EXISTS (SELECT 1 FROM Statystyki s WHERE s.id_meczu = NEW.id_meczu AND NEW.gole_gosci IS NULL);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS new_hosts_goals AFTER INSERT ON Statystyki
                                    BEGIN
                                        UPDATE Druzyna SET gole = gole + NEW.gole_gospodarzy WHERE id_druzyny = 
                                        (SELECT m.id_gospodarzy from Mecz m WHERE m.id_meczu = NEW.id_meczu)
                                        AND NOT EXISTS (SELECT 1 FROM Statystyki s WHERE s.id_meczu = NEW.id_meczu AND NEW.gole_gospodarzy IS NULL);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS new_guests_points AFTER INSERT ON Statystyki
                                    BEGIN 
                                        UPDATE Druzyna SET punkty = punkty + 
                                            CASE
                                                WHEN NEW.gole_gosci IS NULL OR NEW.gole_gospodarzy IS NULL THEN 0
                                                WHEN NEW.gole_gosci > NEW.gole_gospodarzy THEN 3
                                                WHEN NEW.gole_gosci = NEW.gole_gospodarzy THEN 1
                                                ELSE 0
                                            END
                                        WHERE id_druzyny = (SELECT m.id_gosci from Mecz m WHERE m.id_meczu = NEW.id_meczu);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS new_hosts_points AFTER INSERT ON Statystyki
                                    BEGIN 
                                        UPDATE Druzyna SET punkty = punkty + 
                                            CASE 
                                                WHEN NEW.gole_gosci IS NULL OR NEW.gole_gospodarzy IS NULL THEN 0
                                                WHEN NEW.gole_gosci < NEW.gole_gospodarzy THEN 3
                                                WHEN NEW.gole_gosci = NEW.gole_gospodarzy THEN 1
                                                ELSE 0
                                            END
                                        WHERE id_druzyny = (SELECT m.id_gospodarzy from Mecz m WHERE m.id_meczu = NEW.id_meczu);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS updated_guests_goals AFTER UPDATE ON Statystyki
                                    BEGIN
                                        UPDATE Druzyna SET gole = gole + NEW.gole_gosci - OLD.gole_gosci WHERE id_druzyny = 
                                        (SELECT m.id_gosci from Mecz m WHERE m.id_meczu = NEW.id_meczu) 
                                        AND NEW.gole_gosci IS NOT NULL AND OLD.gole_gosci IS NOT NULL;

                                        UPDATE Druzyna SET gole = gole - OLD.gole_gosci WHERE id_druzyny = 
                                        (SELECT m.id_gosci from Mecz m WHERE m.id_meczu = NEW.id_meczu) 
                                        AND NEW.gole_gosci IS NULL AND OLD.gole_gosci IS NOT NULL;

                                        UPDATE Druzyna SET gole = gole + NEW.gole_gosci WHERE id_druzyny = 
                                        (SELECT m.id_gosci from Mecz m WHERE m.id_meczu = NEW.id_meczu) 
                                        AND OLD.gole_gosci IS NULL AND NEW.gole_gosci IS NOT NULL;
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS updated_hosts_goals AFTER UPDATE ON Statystyki
                                    BEGIN
                                        UPDATE Druzyna SET gole = gole + NEW.gole_gospodarzy - OLD.gole_gospodarzy WHERE id_druzyny = 
                                        (SELECT m.id_gospodarzy from Mecz m WHERE m.id_meczu = NEW.id_meczu)
                                        AND NEW.gole_gospodarzy IS NOT NULL AND OLD.gole_gospodarzy IS NOT NULL;

                                        UPDATE Druzyna SET gole = gole - OLD.gole_gospodarzy WHERE id_druzyny = 
                                        (SELECT m.id_gospodarzy from Mecz m WHERE m.id_meczu = NEW.id_meczu)
                                        AND NEW.gole_gospodarzy IS NULL AND OLD.gole_gospodarzy IS NOT NULL;
                                        
                                        UPDATE Druzyna SET gole = gole + NEW.gole_gospodarzy WHERE id_druzyny = 
                                        (SELECT m.id_gospodarzy from Mecz m WHERE m.id_meczu = NEW.id_meczu)
                                        AND OLD.gole_gospodarzy IS NULL AND NEW.gole_gospodarzy IS NOT NULL;
                                    END;");


            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS updated_guests_points AFTER UPDATE ON Statystyki
                                    BEGIN 
                                        UPDATE Druzyna SET punkty = punkty + 
                                            CASE 
                                                WHEN (NEW.gole_gosci IS NULL OR NEW.gole_gospodarzy IS NULL) AND OLD.gole_gosci > OLD.gole_gospodarzy THEN -3
                                                WHEN (NEW.gole_gosci IS NULL OR NEW.gole_gospodarzy IS NULL) AND OLD.gole_gosci = OLD.gole_gospodarzy THEN -1
                                                WHEN NEW.gole_gosci > NEW.gole_gospodarzy AND (OLD.gole_gosci IS NULL OR OLD.gole_gospodarzy IS NULL) THEN 3
                                                WHEN NEW.gole_gosci = NEW.gole_gospodarzy AND (OLD.gole_gosci IS NULL OR OLD.gole_gospodarzy IS NULL) THEN 1
                                                WHEN NEW.gole_gosci > NEW.gole_gospodarzy AND OLD.gole_gosci < OLD.gole_gospodarzy THEN 3
                                                WHEN NEW.gole_gosci > NEW.gole_gospodarzy AND OLD.gole_gosci = OLD.gole_gospodarzy THEN 2
                                                WHEN NEW.gole_gosci = NEW.gole_gospodarzy AND OLD.gole_gosci < OLD.gole_gospodarzy THEN 1
                                                WHEN NEW.gole_gosci = NEW.gole_gospodarzy AND OLD.gole_gosci > OLD.gole_gospodarzy THEN -2
                                                WHEN NEW.gole_gosci < NEW.gole_gospodarzy AND OLD.gole_gosci > OLD.gole_gospodarzy THEN -3
                                                WHEN NEW.gole_gosci < NEW.gole_gospodarzy AND OLD.gole_gosci = OLD.gole_gospodarzy THEN -1
                                                ELSE 0
                                            END
                                        WHERE id_druzyny = (SELECT m.id_gosci from Mecz m WHERE m.id_meczu = NEW.id_meczu);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS updated_hosts_points AFTER UPDATE ON Statystyki
                                    BEGIN 
                                        UPDATE Druzyna SET punkty = punkty + 
                                            CASE
                                                WHEN (NEW.gole_gosci IS NULL OR NEW.gole_gospodarzy IS NULL) AND OLD.gole_gosci < OLD.gole_gospodarzy THEN -3
                                                WHEN (NEW.gole_gosci IS NULL OR NEW.gole_gospodarzy IS NULL) AND OLD.gole_gosci = OLD.gole_gospodarzy THEN -1
                                                WHEN NEW.gole_gosci < NEW.gole_gospodarzy AND (OLD.gole_gosci IS NULL OR OLD.gole_gospodarzy IS NULL) THEN 3
                                                WHEN NEW.gole_gosci = NEW.gole_gospodarzy AND (OLD.gole_gosci IS NULL OR OLD.gole_gospodarzy IS NULL) THEN 1
                                                WHEN NEW.gole_gosci > NEW.gole_gospodarzy AND OLD.gole_gosci < OLD.gole_gospodarzy THEN -3
                                                WHEN NEW.gole_gosci > NEW.gole_gospodarzy AND OLD.gole_gosci = OLD.gole_gospodarzy THEN -1
                                                WHEN NEW.gole_gosci = NEW.gole_gospodarzy AND OLD.gole_gosci < OLD.gole_gospodarzy THEN -2
                                                WHEN NEW.gole_gosci = NEW.gole_gospodarzy AND OLD.gole_gosci > OLD.gole_gospodarzy THEN 1
                                                WHEN NEW.gole_gosci < NEW.gole_gospodarzy AND OLD.gole_gosci > OLD.gole_gospodarzy THEN 3
                                                WHEN NEW.gole_gosci < NEW.gole_gospodarzy AND OLD.gole_gosci = OLD.gole_gospodarzy THEN 2
                                                ELSE 0
                                            END
                                        WHERE id_druzyny = (SELECT m.id_gospodarzy from Mecz m WHERE m.id_meczu = NEW.id_meczu);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS deleted_guests_goals AFTER DELETE ON Statystyki
                                    BEGIN 
                                        UPDATE Druzyna SET gole = gole - OLD.gole_gosci WHERE id_druzyny = 
                                        (SELECT m.id_gosci from Mecz m WHERE m.id_meczu = OLD.id_meczu);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS deleted_hosts_goals AFTER DELETE ON Statystyki
                                    BEGIN 
                                        UPDATE Druzyna SET gole = gole - OLD.gole_gospodarzy WHERE id_druzyny = 
                                        (SELECT m.id_gospodarzy from Mecz m WHERE m.id_meczu = OLD.id_meczu);
                                    END;");


            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS deleted_guests_points AFTER DELETE ON Statystyki
                                    BEGIN 
                                        UPDATE Druzyna SET punkty = punkty + 
                                            CASE 
                                                WHEN OLD.gole_gosci > OLD.gole_gospodarzy THEN -3
                                                WHEN OLD.gole_gosci = OLD.gole_gospodarzy THEN -1
                                                ELSE 0
                                            END
                                        WHERE id_druzyny = (SELECT m.id_gosci from Mecz m WHERE m.id_meczu = OLD.id_meczu);
                                    END;");


            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS deleted_hosts_points AFTER DELETE ON Statystyki
                                    BEGIN 
                                        UPDATE Druzyna SET punkty = punkty + 
                                            CASE 
                                                WHEN OLD.gole_gosci < OLD.gole_gospodarzy THEN -3
                                                WHEN OLD.gole_gosci = OLD.gole_gospodarzy THEN -1
                                                ELSE 0
                                            END
                                        WHERE id_druzyny = (SELECT m.id_gospodarzy from Mecz m WHERE m.id_meczu = OLD.id_meczu);
                                    END;");


            migrationBuilder.Sql(@"CREATE TRIGGER deleted_match_goals BEFORE DELETE ON Mecz
                                    BEGIN
                                        UPDATE Druzyna SET gole = IFNULL(gole, 0) - IFNULL((SELECT gole_gosci FROM Statystyki WHERE id_meczu = OLD.id_meczu), 0)
                                        WHERE id_druzyny = OLD.id_gosci;

                                        UPDATE Druzyna SET gole = IFNULL(gole, 0) - IFNULL((SELECT gole_gospodarzy FROM Statystyki WHERE id_meczu = OLD.id_meczu), 0)
                                        WHERE id_druzyny = OLD.id_gospodarzy;
                                    END;");


            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS deleted_match_points BEFORE DELETE ON Mecz
                                    BEGIN 
                                        UPDATE Druzyna SET punkty = punkty + 
                                            CASE 
                                                WHEN (SELECT gole_gosci FROM Statystyki s WHERE s.id_meczu = OLD.id_meczu) < (SELECT gole_gospodarzy FROM Statystyki s WHERE s.id_meczu = OLD.id_meczu) THEN -3
                                                WHEN (SELECT gole_gosci FROM Statystyki s WHERE s.id_meczu = OLD.id_meczu) = (SELECT gole_gospodarzy FROM Statystyki s WHERE s.id_meczu = OLD.id_meczu) THEN -1
                                                ELSE 0
                                            END
                                        WHERE id_druzyny = OLD.id_gospodarzy;

                                        UPDATE Druzyna SET punkty = punkty + 
                                            CASE 
                                                WHEN (SELECT gole_gosci FROM Statystyki s WHERE s.id_meczu = OLD.id_meczu) > (SELECT gole_gospodarzy FROM Statystyki s WHERE s.id_meczu = OLD.id_meczu) THEN -3
                                                WHEN (SELECT gole_gosci FROM Statystyki s WHERE s.id_meczu = OLD.id_meczu) = (SELECT gole_gospodarzy FROM Statystyki s WHERE s.id_meczu = OLD.id_meczu) THEN -1
                                                ELSE 0
                                            END
                                        WHERE id_druzyny = OLD.id_gosci;
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS insert_goals AFTER INSERT ON Strzelec
                                    BEGIN
                                        -- Wstaw Statystyki, jeśli nie istnieją
                                        INSERT OR IGNORE INTO Statystyki(id_meczu, gole_gosci, gole_gospodarzy) VALUES(NEW.id_meczu, 0, 0);
                                        UPDATE Statystyki SET gole_gospodarzy = 0, gole_gosci = 0 WHERE id_meczu = NEW.id_meczu AND
                                        EXISTS (SELECT 1 FROM Statystyki s WHERE s.id_meczu = NEW.id_meczu AND (s.gole_gospodarzy IS NULL OR s.gole_gosci IS NULL));

                                        UPDATE Statystyki SET gole_gosci = gole_gosci + NEW.gole WHERE NEW.id_meczu = id_meczu 
                                        AND NEW.id_druzyny = (SELECT id_gosci FROM Mecz WHERE id_meczu = NEW.id_meczu);
                                        UPDATE Statystyki SET gole_gosci = gole_gosci + NEW.samoboje WHERE NEW.id_meczu = id_meczu 
                                        AND NEW.id_druzyny = (SELECT id_gospodarzy FROM Mecz WHERE id_meczu = NEW.id_meczu);

                                        UPDATE Statystyki SET gole_gospodarzy = gole_gospodarzy + NEW.gole WHERE NEW.id_meczu = id_meczu
                                        AND NEW.id_druzyny = (SELECT id_gospodarzy FROM Mecz WHERE id_meczu = NEW.id_meczu);
                                        UPDATE Statystyki SET gole_gospodarzy = gole_gospodarzy + NEW.samoboje WHERE NEW.id_meczu = id_meczu
                                        AND NEW.id_druzyny = (SELECT id_gosci FROM Mecz WHERE id_meczu = NEW.id_meczu);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS update_goals AFTER UPDATE ON Strzelec
                                    BEGIN 
                                        UPDATE Statystyki SET gole_gosci = gole_gosci + NEW.gole - OLD.gole WHERE NEW.id_meczu = id_meczu 
                                        AND NEW.id_druzyny = (SELECT id_gosci FROM Mecz WHERE id_meczu = NEW.id_meczu);
                                        UPDATE Statystyki SET gole_gosci = gole_gosci + NEW.gole - OLD.samoboje WHERE NEW.id_meczu = id_meczu 
                                        AND NEW.id_druzyny = (SELECT id_gospodarzy FROM Mecz WHERE id_meczu = NEW.id_meczu);

                                        UPDATE Statystyki SET gole_gospodarzy = gole_gospodarzy + NEW.gole - OLD.gole WHERE NEW.id_meczu = id_meczu
                                        AND NEW.id_druzyny = (SELECT id_gospodarzy FROM Mecz WHERE id_meczu = NEW.id_meczu);
                                        UPDATE Statystyki SET gole_gospodarzy = gole_gospodarzy + NEW.gole - OLD.samoboje WHERE NEW.id_meczu = id_meczu
                                        AND NEW.id_druzyny = (SELECT id_gosci FROM Mecz WHERE id_meczu = NEW.id_meczu);
                                    END;");

            migrationBuilder.Sql(@"CREATE TRIGGER IF NOT EXISTS delete_goals AFTER DELETE ON Strzelec
                                    BEGIN 
                                        UPDATE Statystyki SET gole_gosci = gole_gosci - OLD.gole WHERE OLD.id_meczu = id_meczu 
                                        AND OLD.id_druzyny = (SELECT id_gosci FROM Mecz WHERE id_meczu = OLD.id_meczu);
                                        UPDATE Statystyki SET gole_gosci = gole_gosci - OLD.samoboje WHERE OLD.id_meczu = id_meczu 
                                        AND OLD.id_druzyny = (SELECT id_gospodarzy FROM Mecz WHERE id_meczu = OLD.id_meczu);

                                        UPDATE Statystyki SET gole_gospodarzy = gole_gospodarzy - OLD.gole WHERE OLD.id_meczu = id_meczu
                                        AND OLD.id_druzyny = (SELECT id_gospodarzy FROM Mecz WHERE id_meczu = OLD.id_meczu);
                                        UPDATE Statystyki SET gole_gospodarzy = gole_gospodarzy - OLD.samoboje WHERE OLD.id_meczu = id_meczu
                                        AND OLD.id_druzyny = (SELECT id_gosci FROM Mecz WHERE id_meczu = OLD.id_meczu);

                                        -- Jeśli nie ma już strzelców w tym meczu — ustaw gole na NULL
                                        UPDATE Statystyki
                                        SET gole_gosci = NULL, gole_gospodarzy = NULL
                                        WHERE id_meczu = OLD.id_meczu
                                        AND NOT EXISTS (SELECT 1 FROM Strzelec WHERE id_meczu = OLD.id_meczu);
                                    END;");
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

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS new_guests_goals;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS new_hosts_goals;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS new_guests_points;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS new_hosts_points;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS updated_guests_goals;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS updated_hosts_goals;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS updated_guests_pionts;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS updated_hosts_points;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS deleted_guests_goals;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS deleted_hosts_goals;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS deleted_guests_pionts;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS deleted_hosts_points;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS deleted_match_pionts;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS deleted_match_points;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS insert_goals;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_goals;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS delete_goals;");

        }
    }
}
