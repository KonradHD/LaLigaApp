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
        public string imie { get; set; }
        [Display(Name = "Nazwisko")]
        public string nazwisko { get; set; }
        [Display(Name = "pozycja")]
        [DisplayFormat(NullDisplayText = "Brak")]
        public string? pozycja { get; set; }
        [Display(Name = "Wiek")]
        public int wiek { get; set; }
        [Display(Name = "Wartość rynkowa")]
        public decimal wartosc_rynkowa { get; set; }
        public ICollection<Strzelec>? strzelcy { get; set; }
    }
}