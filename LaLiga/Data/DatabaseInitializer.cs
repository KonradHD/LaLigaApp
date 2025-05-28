using System.Collections;
using LaLiga.Models;
using LaLiga.Service;
using Microsoft.EntityFrameworkCore;

namespace LaLiga.Data
{
    public class DatabaseInitializer
    {
        public static void Initialize(LaLigaContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Druzyna.Any())
            {
                List<Druzyna> druzyny = new List<Druzyna>
                {
                    new Druzyna { nazwa_druzyny = "Alavés" },
                    new Druzyna { nazwa_druzyny = "Athletic Club" },
                    new Druzyna { nazwa_druzyny = "Atlético Madrid" },
                    new Druzyna { nazwa_druzyny = "Barcelona" },
                    new Druzyna { nazwa_druzyny = "Cádiz" },
                    new Druzyna { nazwa_druzyny = "Celta Vigo" },
                    new Druzyna { nazwa_druzyny = "Getafe" },
                    new Druzyna { nazwa_druzyny = "Girona" },
                    new Druzyna { nazwa_druzyny = "Granada" },
                    new Druzyna { nazwa_druzyny = "Las Palmas" },
                    new Druzyna { nazwa_druzyny = "Mallorca" },
                    new Druzyna { nazwa_druzyny = "Osasuna" },
                    new Druzyna { nazwa_druzyny = "Rayo Vallecano" },
                    new Druzyna { nazwa_druzyny = "Real Betis" },
                    new Druzyna { nazwa_druzyny = "Real Madrid" },
                    new Druzyna { nazwa_druzyny = "Real Sociedad" },
                    new Druzyna { nazwa_druzyny = "Sevilla" },
                    new Druzyna { nazwa_druzyny = "Valencia" },
                    new Druzyna { nazwa_druzyny = "Villarreal" },
                    new Druzyna { nazwa_druzyny = "Almería" }
                };

                context.Druzyna.AddRange(druzyny);
                context.SaveChangesAsync();
            }

            if (!context.Zawodnik.Any())
            {
                List<Zawodnik> zawodnicy = new List<Zawodnik>
                {
                    new Zawodnik{id_druzyny = 1, imie = "Antonio", nazwisko = "Sivera", wiek = 28, numer = 15, pozycja ="bramkarz", wartosc_rynkowa = 800.00M},
                    new Zawodnik{id_druzyny = 1, imie = "Rubén", nazwisko = "Duarte", wiek = 28, numer = 3, pozycja ="obrońca", wartosc_rynkowa = 1200.00M},
                    new Zawodnik{id_druzyny = 1, imie = "Jon", nazwisko = "Guridi", wiek = 29, numer = 18, pozycja ="pomocnik", wartosc_rynkowa = 900.00M},
                    new Zawodnik{id_druzyny = 1, imie = "Luis", nazwisko = "Rioja", wiek = 30, numer = 11, pozycja ="skrzydłowy", wartosc_rynkowa = 950.00M},
                    new Zawodnik{id_druzyny = 1, imie = "Samu", nazwisko = "Omorodion", wiek = 21, numer = 9, pozycja ="napastnik", wartosc_rynkowa = 3000.00M},

                    new Zawodnik{id_druzyny = 2, imie = "Unai", nazwisko = "Simón", wiek = 27, numer = 1, pozycja ="bramkarz", wartosc_rynkowa = 2500.00M},
                    new Zawodnik{id_druzyny = 2, imie = "Iñaki", nazwisko = "Williams", wiek = 30, numer = 9, pozycja ="napastnik", wartosc_rynkowa = 2200.00M},
                    new Zawodnik{id_druzyny = 2, imie = "Nico", nazwisko = "Williams", wiek = 21, numer = 11, pozycja ="skrzydłowy", wartosc_rynkowa = 4500.00M},
                    new Zawodnik{id_druzyny = 2, imie = "Oihan", nazwisko = "Sancet", wiek = 24, numer = 8, pozycja ="pomocnik", wartosc_rynkowa = 3000.00M},
                    new Zawodnik{id_druzyny = 2, imie = "Yeray", nazwisko = "Álvarez", wiek = 29, numer = 5, pozycja ="obrońca", wartosc_rynkowa = 1600.00M},

                    new Zawodnik{id_druzyny = 3, imie = "Jan", nazwisko = "Oblak", wiek = 31, numer = 13, pozycja ="bramkarz", wartosc_rynkowa = 1800.00M},
                    new Zawodnik{id_druzyny = 3, imie = "José María", nazwisko = "Giménez", wiek = 29, numer = 2, pozycja ="obrońca", wartosc_rynkowa = 2000.00M},
                    new Zawodnik{id_druzyny = 3, imie = "Koke", nazwisko = "Resurrección", wiek = 32, numer = 6, pozycja ="pomocnik", wartosc_rynkowa = 1000.00M},
                    new Zawodnik{id_druzyny = 3, imie = "Antoine", nazwisko = "Griezmann", wiek = 34, numer = 7, pozycja ="napastnik", wartosc_rynkowa = 5000.00M},
                    new Zawodnik{id_druzyny = 3, imie = "Álvaro", nazwisko = "Morata", wiek = 31, numer = 19, pozycja ="napastnik", wartosc_rynkowa = 2500.00M},

                    new Zawodnik{id_druzyny = 4, imie = "Marc-André", nazwisko = "ter Stegen", wiek = 33, numer = 1, pozycja ="bramkarz", wartosc_rynkowa = 4000.00M},
                    new Zawodnik{id_druzyny = 4, imie = "Ronald", nazwisko = "Araújo", wiek = 26, numer = 4, pozycja ="obrońca", wartosc_rynkowa = 4500.00M},
                    new Zawodnik{id_druzyny = 4, imie = "Frenkie", nazwisko = "de Jong", wiek = 28, numer = 21, pozycja ="pomocnik", wartosc_rynkowa = 5000.00M},
                    new Zawodnik{id_druzyny = 4, imie = "Pedri", nazwisko = "González", wiek = 22, numer = 8, pozycja ="pomocnik", wartosc_rynkowa = 6000.00M},
                    new Zawodnik{id_druzyny = 4, imie = "Robert", nazwisko = "Lewandowski", wiek = 36, numer = 9, pozycja ="napastnik", wartosc_rynkowa = 3500.00M},

                    new Zawodnik{id_druzyny = 5, imie = "Jeremías", nazwisko = "Ledesma", wiek = 31, numer = 1, pozycja ="bramkarz", wartosc_rynkowa = 1500.00M},
                    new Zawodnik{id_druzyny = 5, imie = "Fali", nazwisko = "Jiménez", wiek = 30, numer = 3, pozycja ="obrońca", wartosc_rynkowa = 900.00M},
                    new Zawodnik{id_druzyny = 5, imie = "Rubén", nazwisko = "Alcaraz", wiek = 33, numer = 4, pozycja ="pomocnik", wartosc_rynkowa = 700.00M},
                    new Zawodnik{id_druzyny = 5, imie = "Álex", nazwisko = "Fernández", wiek = 31, numer = 8, pozycja ="pomocnik", wartosc_rynkowa = 750.00M},
                    new Zawodnik{id_druzyny = 5, imie = "Roger", nazwisko = "Martí", wiek = 32, numer = 9, pozycja ="napastnik", wartosc_rynkowa = 1100.00M},
                    // Celta Vigo (id_druzyny = 6)
                    new Zawodnik { id_druzyny = 6, imie = "Iago", nazwisko = "Aspas", wiek = 36, numer = 10, pozycja = "napastnik", wartosc_rynkowa = 250.00M },
                    new Zawodnik { id_druzyny = 6, imie = "Fran", nazwisko = "Beltrán", wiek = 25, numer = 8, pozycja = "pomocnik", wartosc_rynkowa = 160.00M },
                    new Zawodnik { id_druzyny = 6, imie = "Unai", nazwisko = "Núñez", wiek = 27, numer = 4, pozycja = "obrońca", wartosc_rynkowa = 180.00M },
                    new Zawodnik { id_druzyny = 6, imie = "Jørgen", nazwisko = "Strand Larsen", wiek = 24, numer = 18, pozycja = "napastnik", wartosc_rynkowa = 210.00M },
                    new Zawodnik { id_druzyny = 6, imie = "Iván", nazwisko = "Villar", wiek = 27, numer = 13, pozycja = "bramkarz", wartosc_rynkowa = 100.00M },

                    // Getafe (id_druzyny = 7)
                    new Zawodnik { id_druzyny = 7, imie = "Borja", nazwisko = "Mayoral", wiek = 27, numer = 19, pozycja = "napastnik", wartosc_rynkowa = 220.00M },
                    new Zawodnik { id_druzyny = 7, imie = "Mason", nazwisko = "Greenwood", wiek = 22, numer = 12, pozycja = "napastnik", wartosc_rynkowa = 300.00M },
                    new Zawodnik { id_druzyny = 7, imie = "Damián", nazwisko = "Suárez", wiek = 36, numer = 22, pozycja = "obrońca", wartosc_rynkowa = 50.00M },
                    new Zawodnik { id_druzyny = 7, imie = "David", nazwisko = "Soria", wiek = 31, numer = 13, pozycja = "bramkarz", wartosc_rynkowa = 100.00M },
                    new Zawodnik { id_druzyny = 7, imie = "Nemanja", nazwisko = "Maksimović", wiek = 29, numer = 20, pozycja = "pomocnik", wartosc_rynkowa = 150.00M },

                    // Girona (id_druzyny = 8)
                    new Zawodnik { id_druzyny = 8, imie = "Artem", nazwisko = "Dovbyk", wiek = 26, numer = 9, pozycja = "napastnik", wartosc_rynkowa = 280.00M },
                    new Zawodnik { id_druzyny = 8, imie = "Aleix", nazwisko = "García", wiek = 27, numer = 14, pozycja = "pomocnik", wartosc_rynkowa = 240.00M },
                    new Zawodnik { id_druzyny = 8, imie = "Paulo", nazwisko = "Gazzaniga", wiek = 32, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 90.00M },
                    new Zawodnik { id_druzyny = 8, imie = "Miguel", nazwisko = "Gutiérrez", wiek = 23, numer = 3, pozycja = "obrońca", wartosc_rynkowa = 200.00M },
                    new Zawodnik { id_druzyny = 8, imie = "Iván", nazwisko = "Martín", wiek = 25, numer = 23, pozycja = "pomocnik", wartosc_rynkowa = 120.00M },

                    // Granada (id_druzyny = 9)
                    new Zawodnik { id_druzyny = 9, imie = "Myrto", nazwisko = "Uzuni", wiek = 29, numer = 11, pozycja = "napastnik", wartosc_rynkowa = 150.00M },
                    new Zawodnik { id_druzyny = 9, imie = "Lucas", nazwisko = "Boyé", wiek = 28, numer = 7, pozycja = "napastnik", wartosc_rynkowa = 140.00M },
                    new Zawodnik { id_druzyny = 9, imie = "Ricard", nazwisko = "Sánchez", wiek = 24, numer = 12, pozycja = "obrońca", wartosc_rynkowa = 130.00M },
                    new Zawodnik { id_druzyny = 9, imie = "Raúl", nazwisko = "Fernández", wiek = 36, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 60.00M },
                    new Zawodnik { id_druzyny = 9, imie = "Gonzalo", nazwisko = "Villar", wiek = 26, numer = 24, pozycja = "pomocnik", wartosc_rynkowa = 110.00M },

                    // Las Palmas (id_druzyny = 10)
                    new Zawodnik { id_druzyny = 10, imie = "Kirian", nazwisko = "Rodríguez", wiek = 28, numer = 20, pozycja = "pomocnik", wartosc_rynkowa = 130.00M },
                    new Zawodnik { id_druzyny = 10, imie = "Álvaro", nazwisko = "Valles", wiek = 27, numer = 13, pozycja = "bramkarz", wartosc_rynkowa = 100.00M },
                    new Zawodnik { id_druzyny = 10, imie = "Jonathan", nazwisko = "Viera", wiek = 34, numer = 21, pozycja = "pomocnik", wartosc_rynkowa = 80.00M },
                    new Zawodnik { id_druzyny = 10, imie = "Mika", nazwisko = "Mármol", wiek = 22, numer = 4, pozycja = "obrońca", wartosc_rynkowa = 140.00M },
                    new Zawodnik { id_druzyny = 10, imie = "Munir", nazwisko = "El Haddadi", wiek = 29, numer = 17, pozycja = "napastnik", wartosc_rynkowa = 160.00M },
                    // Mallorca (id_druzyny = 11)
                    new Zawodnik { id_druzyny = 11, imie = "Vedat", nazwisko = "Muriqi", wiek = 30, numer = 7, pozycja = "napastnik", wartosc_rynkowa = 150.00M },
                    new Zawodnik { id_druzyny = 11, imie = "Predrag", nazwisko = "Rajković", wiek = 28, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 120.00M },
                    new Zawodnik { id_druzyny = 11, imie = "Antonio", nazwisko = "Sánchez", wiek = 27, numer = 10, pozycja = "pomocnik", wartosc_rynkowa = 90.00M },
                    new Zawodnik { id_druzyny = 11, imie = "Giovanni", nazwisko = "González", wiek = 29, numer = 20, pozycja = "obrońca", wartosc_rynkowa = 80.00M },
                    new Zawodnik { id_druzyny = 11, imie = "Dani", nazwisko = "Rodríguez", wiek = 36, numer = 14, pozycja = "pomocnik", wartosc_rynkowa = 70.00M },

                    // Osasuna (id_druzyny = 12)
                    new Zawodnik { id_druzyny = 12, imie = "Chimy", nazwisko = "Ávila", wiek = 30, numer = 9, pozycja = "napastnik", wartosc_rynkowa = 170.00M },
                    new Zawodnik { id_druzyny = 12, imie = "Aimar", nazwisko = "Oroz", wiek = 22, numer = 10, pozycja = "pomocnik", wartosc_rynkowa = 180.00M },
                    new Zawodnik { id_druzyny = 12, imie = "David", nazwisko = "García", wiek = 30, numer = 5, pozycja = "obrońca", wartosc_rynkowa = 160.00M },
                    new Zawodnik { id_druzyny = 12, imie = "Sergio", nazwisko = "Herrera", wiek = 31, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 100.00M },
                    new Zawodnik { id_druzyny = 12, imie = "Rubén", nazwisko = "García", wiek = 30, numer = 14, pozycja = "pomocnik", wartosc_rynkowa = 90.00M },

                    // Rayo Vallecano (id_druzyny = 13)
                    new Zawodnik { id_druzyny = 13, imie = "Isi", nazwisko = "Palazón", wiek = 29, numer = 7, pozycja = "pomocnik", wartosc_rynkowa = 140.00M },
                    new Zawodnik { id_druzyny = 13, imie = "Stole", nazwisko = "Dimitrievski", wiek = 30, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 110.00M },
                    new Zawodnik { id_druzyny = 13, imie = "Óscar", nazwisko = "Valentín", wiek = 30, numer = 23, pozycja = "pomocnik", wartosc_rynkowa = 90.00M },
                    new Zawodnik { id_druzyny = 13, imie = "Álvaro", nazwisko = "García", wiek = 31, numer = 18, pozycja = "skrzydłowy", wartosc_rynkowa = 130.00M },
                    new Zawodnik { id_druzyny = 13, imie = "Abdul", nazwisko = "Mumin", wiek = 26, numer = 16, pozycja = "obrońca", wartosc_rynkowa = 100.00M },

                    // Real Betis (id_druzyny = 14)
                    new Zawodnik { id_druzyny = 14, imie = "Isco", nazwisko = "Alarcón", wiek = 32, numer = 22, pozycja = "pomocnik", wartosc_rynkowa = 200.00M },
                    new Zawodnik { id_druzyny = 14, imie = "Nabil", nazwisko = "Fekir", wiek = 31, numer = 8, pozycja = "pomocnik", wartosc_rynkowa = 220.00M },
                    new Zawodnik { id_druzyny = 14, imie = "Guido", nazwisko = "Rodríguez", wiek = 30, numer = 5, pozycja = "pomocnik", wartosc_rynkowa = 160.00M },
                    new Zawodnik { id_druzyny = 14, imie = "Claudio", nazwisko = "Bravo", wiek = 41, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 20.00M },
                    new Zawodnik { id_druzyny = 14, imie = "Ayoze", nazwisko = "Pérez", wiek = 31, numer = 10, pozycja = "napastnik", wartosc_rynkowa = 180.00M },

                    // Real Madrid (id_druzyny = 15)
                    new Zawodnik { id_druzyny = 15, imie = "Jude", nazwisko = "Bellingham", wiek = 21, numer = 5, pozycja = "pomocnik", wartosc_rynkowa = 350.00M },
                    new Zawodnik { id_druzyny = 15, imie = "Vinícius", nazwisko = "Júnior", wiek = 24, numer = 7, pozycja = "napastnik", wartosc_rynkowa = 400.00M },
                    new Zawodnik { id_druzyny = 15, imie = "Thibaut", nazwisko = "Courtois", wiek = 32, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 180.00M },
                    new Zawodnik { id_druzyny = 15, imie = "Luka", nazwisko = "Modrić", wiek = 39, numer = 10, pozycja = "pomocnik", wartosc_rynkowa = 60.00M },
                    new Zawodnik { id_druzyny = 15, imie = "Antonio", nazwisko = "Rüdiger", wiek = 32, numer = 22, pozycja = "obrońca", wartosc_rynkowa = 150.00M },
                    // Real Sociedad (id_druzyny = 16)
                    new Zawodnik { id_druzyny = 16, imie = "Mikel", nazwisko = "Oyarzabal", wiek = 27, numer = 10, pozycja = "skrzydłowy", wartosc_rynkowa = 250.00M },
                    new Zawodnik { id_druzyny = 16, imie = "Takefusa", nazwisko = "Kubo", wiek = 23, numer = 14, pozycja = "skrzydłowy", wartosc_rynkowa = 220.00M },
                    new Zawodnik { id_druzyny = 16, imie = "Martin", nazwisko = "Zubimendi", wiek = 26, numer = 4, pozycja = "pomocnik", wartosc_rynkowa = 180.00M },
                    new Zawodnik { id_druzyny = 16, imie = "Álex", nazwisko = "Remiro", wiek = 29, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 160.00M },
                    new Zawodnik { id_druzyny = 16, imie = "Robin", nazwisko = "Le Normand", wiek = 28, numer = 24, pozycja = "obrońca", wartosc_rynkowa = 170.00M },

                    // Sevilla (id_druzyny = 17)
                    new Zawodnik { id_druzyny = 17, imie = "Ivan", nazwisko = "Rakitić", wiek = 36, numer = 10, pozycja = "pomocnik", wartosc_rynkowa = 50.00M },
                    new Zawodnik { id_druzyny = 17, imie = "Youssef", nazwisko = "En-Nesyri", wiek = 27, numer = 15, pozycja = "napastnik", wartosc_rynkowa = 140.00M },
                    new Zawodnik { id_druzyny = 17, imie = "Jesús", nazwisko = "Navas", wiek = 39, numer = 16, pozycja = "obrońca", wartosc_rynkowa = 30.00M },
                    new Zawodnik { id_druzyny = 17, imie = "Lucas", nazwisko = "Ocampos", wiek = 30, numer = 5, pozycja = "skrzydłowy", wartosc_rynkowa = 90.00M },
                    new Zawodnik { id_druzyny = 17, imie = "Marko", nazwisko = "Dmitrović", wiek = 33, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 70.00M },

                    // Valencia (id_druzyny = 18)
                    new Zawodnik { id_druzyny = 18, imie = "José", nazwisko = "Gayà", wiek = 29, numer = 14, pozycja = "obrońca", wartosc_rynkowa = 150.00M },
                    new Zawodnik { id_druzyny = 18, imie = "Javi", nazwisko = "Guerra", wiek = 21, numer = 8, pozycja = "pomocnik", wartosc_rynkowa = 180.00M },
                    new Zawodnik { id_druzyny = 18, imie = "Giorgi", nazwisko = "Mamardashvili", wiek = 24, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 170.00M },
                    new Zawodnik { id_druzyny = 18, imie = "Pepelu", nazwisko = "", wiek = 26, numer = 18, pozycja = "pomocnik", wartosc_rynkowa = 120.00M },
                    new Zawodnik { id_druzyny = 18, imie = "Diego", nazwisko = "López", wiek = 22, numer = 16, pozycja = "skrzydłowy", wartosc_rynkowa = 100.00M },

                    // Villarreal (id_druzyny = 19)
                    new Zawodnik { id_druzyny = 19, imie = "Gerard", nazwisko = "Moreno", wiek = 32, numer = 7, pozycja = "napastnik", wartosc_rynkowa = 180.00M },
                    new Zawodnik { id_druzyny = 19, imie = "Álex", nazwisko = "Baena", wiek = 23, numer = 16, pozycja = "pomocnik", wartosc_rynkowa = 160.00M },
                    new Zawodnik { id_druzyny = 19, imie = "Pau", nazwisko = "Torres", wiek = 27, numer = 4, pozycja = "obrońca", wartosc_rynkowa = 190.00M },
                    new Zawodnik { id_druzyny = 19, imie = "Filip", nazwisko = "Jörgensen", wiek = 23, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 120.00M },
                    new Zawodnik { id_druzyny = 19, imie = "Etienne", nazwisko = "Capoue", wiek = 35, numer = 6, pozycja = "pomocnik", wartosc_rynkowa = 60.00M },

                    // Almería (id_druzyny = 20)
                    new Zawodnik { id_druzyny = 20, imie = "Largie", nazwisko = "Ramazani", wiek = 23, numer = 7, pozycja = "skrzydłowy", wartosc_rynkowa = 110.00M },
                    new Zawodnik { id_druzyny = 20, imie = "Luis", nazwisko = "Suárez", wiek = 27, numer = 9, pozycja = "napastnik", wartosc_rynkowa = 100.00M },
                    new Zawodnik { id_druzyny = 20, imie = "Lucas", nazwisko = "Robertone", wiek = 27, numer = 5, pozycja = "pomocnik", wartosc_rynkowa = 90.00M },
                    new Zawodnik { id_druzyny = 20, imie = "César", nazwisko = "De la Hoz", wiek = 32, numer = 4, pozycja = "pomocnik", wartosc_rynkowa = 60.00M },
                    new Zawodnik { id_druzyny = 20, imie = "Maximiliano", nazwisko = "Silva", wiek = 31, numer = 1, pozycja = "bramkarz", wartosc_rynkowa = 80.00M },
                };

                context.Zawodnik.AddRange(zawodnicy);
                context.SaveChangesAsync();
            }

            if (!context.Uzytkownik.Any())
            {
                Uzytkownik admin = new Uzytkownik
                {
                    email = "admin@gmail.com",
                    haslo = HashHelper.HashMD5("admin"),
                    imie = "Konrad",
                    nazwisko = "Ćwięka",
                    wiek = 21,
                    data_dolaczenia = DateTime.Now,
                    rola = "admin"
                };
                context.Uzytkownik.Add(admin);
                context.SaveChangesAsync();
            }

            if (!context.Mecz.Any())
            {
                List<Mecz> mecze = new List<Mecz>
                {
                    // Kolejka 1 (data: 2024-08-16)
                    new Mecz { id_gospodarzy = 1, id_gosci = 2, termin = new DateTime(2024, 8, 16) },
                    new Mecz { id_gospodarzy = 3, id_gosci = 4, termin = new DateTime(2024, 8, 16) },
                    new Mecz { id_gospodarzy = 5, id_gosci = 6, termin = new DateTime(2024, 8, 17) },
                    new Mecz { id_gospodarzy = 7, id_gosci = 8, termin = new DateTime(2024, 8, 17) },
                    new Mecz { id_gospodarzy = 9, id_gosci = 10, termin = new DateTime(2024, 8, 18) },
                    new Mecz { id_gospodarzy = 11, id_gosci = 12, termin = new DateTime(2024, 8, 18) },
                    new Mecz { id_gospodarzy = 13, id_gosci = 14, termin = new DateTime(2024, 8, 18) },
                    new Mecz { id_gospodarzy = 15, id_gosci = 16, termin = new DateTime(2024, 8, 19) },
                    new Mecz { id_gospodarzy = 17, id_gosci = 18, termin = new DateTime(2024, 8, 19) },
                    new Mecz { id_gospodarzy = 19, id_gosci = 20, termin = new DateTime(2024, 8, 19) },

                    // Kolejka 2 (data: 2024-08-23)
                    new Mecz { id_gospodarzy = 2, id_gosci = 3, termin = new DateTime(2024, 8, 23) },
                    new Mecz { id_gospodarzy = 4, id_gosci = 5, termin = new DateTime(2024, 8, 23) },
                    new Mecz { id_gospodarzy = 6, id_gosci = 7, termin = new DateTime(2024, 8, 24) },
                    new Mecz { id_gospodarzy = 8, id_gosci = 9, termin = new DateTime(2024, 8, 24) },
                    new Mecz { id_gospodarzy = 10, id_gosci = 11, termin = new DateTime(2024, 8, 25) },
                    new Mecz { id_gospodarzy = 12, id_gosci = 13, termin = new DateTime(2024, 8, 25) },
                    new Mecz { id_gospodarzy = 14, id_gosci = 15, termin = new DateTime(2024, 8, 25) },
                    new Mecz { id_gospodarzy = 16, id_gosci = 17, termin = new DateTime(2024, 8, 26) },
                    new Mecz { id_gospodarzy = 18, id_gosci = 19, termin = new DateTime(2024, 8, 26) },
                    new Mecz { id_gospodarzy = 20, id_gosci = 1, termin = new DateTime(2024, 8, 26) },

                    // Kolejka 3 (data: 2024-08-30)
                    new Mecz { id_gospodarzy = 3, id_gosci = 1, termin = new DateTime(2024, 8, 30) },
                    new Mecz { id_gospodarzy = 5, id_gosci = 2, termin = new DateTime(2024, 8, 30) },
                    new Mecz { id_gospodarzy = 7, id_gosci = 4, termin = new DateTime(2024, 8, 31) },
                    new Mecz { id_gospodarzy = 9, id_gosci = 6, termin = new DateTime(2024, 8, 31) },
                    new Mecz { id_gospodarzy = 11, id_gosci = 8, termin = new DateTime(2024, 9, 1) },
                    new Mecz { id_gospodarzy = 13, id_gosci = 10, termin = new DateTime(2024, 9, 1) },
                    new Mecz { id_gospodarzy = 15, id_gosci = 12, termin = new DateTime(2024, 9, 1) },
                    new Mecz { id_gospodarzy = 17, id_gosci = 14, termin = new DateTime(2024, 9, 2) },
                    new Mecz { id_gospodarzy = 19, id_gosci = 16, termin = new DateTime(2024, 9, 2) },
                    new Mecz { id_gospodarzy = 18, id_gosci = 20, termin = new DateTime(2024, 9, 2) },
                };

                context.Mecz.AddRange(mecze);
                context.SaveChangesAsync();
            }

            if (!context.Statystyki.Any())
            {
                var statystykiList = new List<Statystyki>
                {
                    new Statystyki { id_meczu = 1, gole_gospodarzy = 2, gole_gosci = 1, strzaly_gospodarzy = 12, strzaly_gosci = 6 },
                    new Statystyki { id_meczu = 2, gole_gospodarzy = 0, gole_gosci = 0, strzaly_gospodarzy = 7, strzaly_gosci = 5 },
                    new Statystyki { id_meczu = 3, gole_gospodarzy = 3, gole_gosci = 2, strzaly_gospodarzy = 15, strzaly_gosci = 11 },
                    new Statystyki { id_meczu = 4, gole_gospodarzy = 1, gole_gosci = 1, strzaly_gospodarzy = 9, strzaly_gosci = 8 },
                    new Statystyki { id_meczu = 5, gole_gospodarzy = 0, gole_gosci = 2, strzaly_gospodarzy = 4, strzaly_gosci = 13 },
                    new Statystyki { id_meczu = 6, gole_gospodarzy = 4, gole_gosci = 0, strzaly_gospodarzy = 18, strzaly_gosci = 3 },
                    new Statystyki { id_meczu = 7, gole_gospodarzy = 2, gole_gosci = 2, strzaly_gospodarzy = 10, strzaly_gosci = 10 },
                    new Statystyki { id_meczu = 8, gole_gospodarzy = 1, gole_gosci = 3, strzaly_gospodarzy = 6, strzaly_gosci = 14 },
                    new Statystyki { id_meczu = 9, gole_gospodarzy = 0, gole_gosci = 1, strzaly_gospodarzy = 5, strzaly_gosci = 8 },
                    new Statystyki { id_meczu = 10, gole_gospodarzy = 3, gole_gosci = 1, strzaly_gospodarzy = 16, strzaly_gosci = 9 },

                    new Statystyki { id_meczu = 11, gole_gospodarzy = 2, gole_gosci = 0, strzaly_gospodarzy = 11, strzaly_gosci = 4 },
                    new Statystyki { id_meczu = 12, gole_gospodarzy = 0, gole_gosci = 2, strzaly_gospodarzy = 3, strzaly_gosci = 12 },
                    new Statystyki { id_meczu = 13, gole_gospodarzy = 1, gole_gosci = 2, strzaly_gospodarzy = 7, strzaly_gosci = 11 },
                    new Statystyki { id_meczu = 14, gole_gospodarzy = 1, gole_gosci = 0, strzaly_gospodarzy = 8, strzaly_gosci = 5 },
                    new Statystyki { id_meczu = 15, gole_gospodarzy = 2, gole_gosci = 3, strzaly_gospodarzy = 10, strzaly_gosci = 15 },
                    new Statystyki { id_meczu = 16, gole_gospodarzy = 4, gole_gosci = 2, strzaly_gospodarzy = 17, strzaly_gosci = 13 },
                    new Statystyki { id_meczu = 17, gole_gospodarzy = 0, gole_gosci = 0, strzaly_gospodarzy = 6, strzaly_gosci = 6 },
                    new Statystyki { id_meczu = 18, gole_gospodarzy = 2, gole_gosci = 1, strzaly_gospodarzy = 12, strzaly_gosci = 9 },
                    new Statystyki { id_meczu = 19, gole_gospodarzy = 1, gole_gosci = 1, strzaly_gospodarzy = 8, strzaly_gosci = 8 },
                    new Statystyki { id_meczu = 20, gole_gospodarzy = 3, gole_gosci = 0, strzaly_gospodarzy = 14, strzaly_gosci = 4 },

                    new Statystyki { id_meczu = 21, gole_gospodarzy = 1, gole_gosci = 2, strzaly_gospodarzy = 9, strzaly_gosci = 13 },
                    new Statystyki { id_meczu = 22, gole_gospodarzy = 2, gole_gosci = 2, strzaly_gospodarzy = 13, strzaly_gosci = 13 },
                    new Statystyki { id_meczu = 23, gole_gospodarzy = 0, gole_gosci = 1, strzaly_gospodarzy = 4, strzaly_gosci = 7 },
                    new Statystyki { id_meczu = 24, gole_gospodarzy = 1, gole_gosci = 3, strzaly_gospodarzy = 10, strzaly_gosci = 16 },
                    new Statystyki { id_meczu = 25, gole_gospodarzy = 2, gole_gosci = 0, strzaly_gospodarzy = 11, strzaly_gosci = 5 },
                    new Statystyki { id_meczu = 26, gole_gospodarzy = 3, gole_gosci = 2, strzaly_gospodarzy = 15, strzaly_gosci = 10 },
                    new Statystyki { id_meczu = 27, gole_gospodarzy = 0, gole_gosci = 4, strzaly_gospodarzy = 5, strzaly_gosci = 17 },
                    new Statystyki { id_meczu = 28, gole_gospodarzy = 1, gole_gosci = 1, strzaly_gospodarzy = 9, strzaly_gosci = 9 },
                    new Statystyki { id_meczu = 29, gole_gospodarzy = 2, gole_gosci = 1, strzaly_gospodarzy = 13, strzaly_gosci = 6 },
                    new Statystyki { id_meczu = 30, gole_gospodarzy = 3, gole_gosci = 3, strzaly_gospodarzy = 18, strzaly_gosci = 18 },
                };

                context.Statystyki.AddRange(statystykiList);
                context.SaveChangesAsync();
            }

            if (!context.Strzelec.Any())
            {
                List<Strzelec> strzelcy = new List<Strzelec>
                {
                    // Mecz 1: Alavés vs Athletic Club
                    new Strzelec { id_meczu = 1, id_druzyny = 1, numer = 15, gole = 1, asysty = 0 },
                    new Strzelec { id_meczu = 1, id_druzyny = 2, numer = 9, gole = 2, asysty = 1 },

                    // Mecz 2: Atlético Madrid vs Barcelona
                    new Strzelec { id_meczu = 2, id_druzyny = 4, numer = 10, gole = 1, asysty = 0 },
                    new Strzelec { id_meczu = 2, id_druzyny = 3, numer = 19, gole = 1, asysty = 1 },

                    // Mecz 3: Cádiz vs Celta Vigo
                    new Strzelec { id_meczu = 3, id_druzyny = 5, numer = 7, gole = 0, asysty = 1 },
                    new Strzelec { id_meczu = 3, id_druzyny = 6, numer = 11, gole = 2, asysty = 0 },

                    // Mecz 4: Getafe vs Girona
                    new Strzelec { id_meczu = 4, id_druzyny = 7, numer = 20, gole = 1, asysty = 1 },
                    new Strzelec { id_meczu = 4, id_druzyny = 8, numer = 10, gole = 0, asysty = 2 },

                    // Mecz 5: Granada vs Las Palmas
                    new Strzelec { id_meczu = 5, id_druzyny = 10, numer = 17, gole = 1, asysty = 0 },
                    new Strzelec { id_meczu = 5, id_druzyny = 9, numer = 8, gole = 2, asysty = 0 },

                    // Mecz 6: Mallorca vs Osasuna
                    new Strzelec { id_meczu = 6, id_druzyny = 11, numer = 21, gole = 1, asysty = 1 },
                    new Strzelec { id_meczu = 6, id_druzyny = 12, numer = 3, gole = 0, asysty = 1 },

                    // Mecz 7: Rayo Vallecano vs Real Betis
                    new Strzelec { id_meczu = 7, id_druzyny = 14, numer = 9, gole = 2, asysty = 0 },

                    // Mecz 8: Real Madrid vs Real Sociedad
                    new Strzelec { id_meczu = 8, id_druzyny = 15, numer = 7, gole = 1, asysty = 2 },
                    new Strzelec { id_meczu = 8, id_druzyny = 16, numer = 19, gole = 0, asysty = 1 },

                    // Mecz 9: Sevilla vs Valencia
                    new Strzelec { id_meczu = 9, id_druzyny = 17, numer = 6, gole = 1, asysty = 0 },
                    new Strzelec { id_meczu = 9, id_druzyny = 18, numer = 10, gole = 1, asysty = 1 },

                    // Mecz 10: Villarreal vs Almería
                    new Strzelec { id_meczu = 10, id_druzyny = 19, numer = 14, gole = 3, asysty = 1 },
                    new Strzelec { id_meczu = 10, id_druzyny = 20, numer = 22, gole = 1, asysty = 0 },
                };

                //context.Strzelec.AddRange(strzelcy);
                //context.SaveChangesAsync();
            }
        }
    }
}