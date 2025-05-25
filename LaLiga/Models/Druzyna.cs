using System.ComponentModel.DataAnnotations;

namespace LaLiga.Models
{
    public class Druzyna
    {
        [Key]
        public int id_druzyny { get; set; }
        [Display(Name = "Nazwa druzyny")]
        public string nazwa_druzyny { get; set; }
        [Display(Name = "Punkty")]
        public int? punkty { get; set; }
        [Display(Name = "Liczba goli")]
        public int? gole { get; set; }
        public ICollection<Mecz>? meczeUSiebie { get; set; }
        public ICollection<Mecz>? meczeNaWyjezdzie { get; set; }
        public ICollection<Zawodnik>? zawodnicy { get; set; }
    }
}
