using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Laliga.Models
{
    public class PlayerOverallStats
    {
        [Key]
        [Display(Name = "ID Zawodnika")]
        public int id_zawodnika { get; set; }
        [Display(Name = "Imię i Nazwisko")]
        public string zawodnik { get; set; }
        [Display(Name = "Pozycja")]
        public string pozycja { get; set; }
        [Display(Name = "Druzyna")]
        public string nazwa_druzyny { get; set; }
        [Display(Name = "Liga")]
        public string liga { get; set; }
        [Display(Name = "Wiek")]
        public int wiek { get; set; }
        [Display(Name = "Liczba występów")]
        public int MP { get; set; }
        [Display(Name = "Mecze w pierwszym składzie")]
        public int Starts { get; set; }
        [Display(Name = "Minuty")]
        public int Min { get; set; }
        [Display(Name = "Gole")]
        public int Gls { get; set; }
        [Display(Name = "Asysty")]
        public int Ast { get; set; }
        [Display(Name = "Żółte kartki")]
        public int CrdY { get; set; }
        [Display(Name = "Czerwone kartki")]
        public int CrdR { get; set; }
        [Display(Name = "Expected Goals")]
        public double xG { get; set; }
        [Display(Name = "Expected Assists")]
        public double xAG { get; set; }
        [Display(Name = "Progressive Carries")]
        public int PrgC { get; set; }
        [Display(Name = "Progressive Passes")]
        public int PrgP { get; set; }
        [Display(Name = "Strzały")]
        public int Sh { get; set; }
        [Display(Name = "Strzały na bramkę")]
        public int SoT { get; set; }
        [Display(Name = "Pass Completion %")]
        public double CmpP { get; set; }
        [Display(Name = "Kluczowe podania")]
        public int KP { get; set; }
        [Display(Name = "Passes into Penalty Area")]
        public int PPA { get; set; }
        [Display(Name = "Total Tackles")]
        public int Tkl { get; set; }
        [Display(Name = "Tackles Won Percentage")]
        public double? TklW { get; set; }
        [Display(Name = "Straty")]
        public int Lost { get; set; }
        [Display(Name = "Interceptions")]
        public int Int { get; set; }
        [Display(Name = "Clearances")]
        public int Clr { get; set; }
        [Display(Name = "Error leading to Goal")]
        public int Err { get; set; }
        [Display(Name = "Touches")]
        public int Touches { get; set; }
        [Display(Name = "Carries")]
        public int Carries { get; set; }
        [Display(Name = "Recoveries")]
        public int Recov { get; set; }
        [Display(Name = "Won percentage")]
        public double WonP { get; set; }
        [Display(Name = "Gole stracone na 90 minut")]
        public double? GA90 { get; set; }
        [Display(Name = "Saves")]
        public int? Saves { get; set; }
        [Display(Name = "Saves percentage")]
        public double? SaveP { get; set; }
        [Display(Name = "Clean Sheets")]
        public int? CS { get; set; }
        [Display(Name = "Penalties Saved")]
        public int? PKsv { get; set; }
        [Display(Name = "Wartość rynkowa (mln €)")]
        public double? Wartoscrynkowa { get; set; }
    }
}