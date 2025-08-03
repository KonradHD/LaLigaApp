using System.ComponentModel.DataAnnotations;

namespace LaLiga.Models
{
    public class Zawodnik
    {
        [Key]
        public int id_druzyny { get; set; }
        public Druzyna? druzyna { get; set; }
        [Display(Name = "Numer")]
        public int numer { get; set; }
        [Display(Name = "Imię")]
        public string? imie { get; set; }
        [Display(Name = "Nazwisko")]
        [DisplayFormat(NullDisplayText = "Brak")]
        public string? nazwisko { get; set; }
        [Display(Name = "Pozycja")]
        [DisplayFormat(NullDisplayText = "Brak")]
        public string? pozycja { get; set; }
        [Display(Name = "Wiek")]
        public int wiek { get; set; }
        [Display(Name = "Kraj pochodzenia")]
        [DisplayFormat(NullDisplayText = "Nieznane")]
        public string? kraj_pochodzenia { get; set; }
        [Display(Name = "Kontuzjowany")]
        public bool injured { get; set; }
        [Display(Name = "APIid")]
        public int APIid { get; set; }
        public ICollection<Strzelec>? strzelcy { get; set; }


        public Zawodnik(int id_druzyny, int numer, string imie, string? nazwisko, string pozycja, int wiek, string kraj_pochodzenia, bool injured, int APIid)
        {
            this.id_druzyny = id_druzyny;
            this.numer = numer;
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.pozycja = pozycja;
            this.wiek = wiek;
            this.kraj_pochodzenia = kraj_pochodzenia;
            this.injured = injured;
            this.APIid = APIid;
        }

        public Zawodnik(int id_druzyny, int numer, string imie, string pozycja, int wiek, int APIid)
        {
            this.id_druzyny = id_druzyny;
            this.numer = numer;
            this.imie = imie;
            this.pozycja = pozycja;
            this.wiek = wiek;
            this.APIid = APIid;

            injured = false;
        }

        public Zawodnik(int id_druzyny, int numer, int APIid)
        {
            this.id_druzyny = id_druzyny;
            this.numer = numer;
            this.APIid = APIid;
        }

        public Zawodnik SetAge(int wiek)
        {
            this.wiek = wiek;
            return this;
        }

        public Zawodnik SetNationality(string kraj_pochodzenia)
        {
            this.kraj_pochodzenia = kraj_pochodzenia;
            return this;
        }

        public Zawodnik SetInjured(bool injured)
        {
            this.injured = injured;
            return this;
        }

        public Zawodnik SetFirstName(string imie)
        {
            this.imie = imie;
            return this;
        }

        public Zawodnik SetLastName(string nazwisko)
        {
            this.nazwisko = nazwisko;
            return this;
        }
    }
}