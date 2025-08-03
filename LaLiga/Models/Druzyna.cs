using System.ComponentModel.DataAnnotations;

namespace LaLiga.Models
{
    public class Druzyna
    {
        [Key]
        public int id_druzyny { get; set; }
        [Display(Name = "Nazwa druzyny")]
        public string nazwa_druzyny { get; set; }
        [Display(Name = "Nazwa stadionu")]
        public string stadion { get; set; }
        [Display(Name = "Punkty")]
        public int punkty { get; set; }
        [Display(Name = "Liczba goli")]
        public int gole { get; set; }
        public ICollection<Mecz>? meczeUSiebie { get; set; }
        public ICollection<Mecz>? meczeNaWyjezdzie { get; set; }
        public ICollection<Zawodnik>? zawodnicy { get; set; }

        public Druzyna(string nazwa_druzyny, string stadion)
        {
            this.nazwa_druzyny = nazwa_druzyny;
            this.stadion = stadion;
        }

        public Druzyna(int id_druzyny, string nazwa_druzyny, string stadion)
        {
            this.id_druzyny = id_druzyny;
            this.nazwa_druzyny = nazwa_druzyny;
            this.stadion = stadion;
        }

        public Druzyna() { }
    }
}
