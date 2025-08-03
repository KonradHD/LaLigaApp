using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LaLiga.Models;

namespace LaLiga.Models
{
    public class Statystyki
    {
        [Key, ForeignKey("mecz")]
        public int id_meczu { get; set; }
        public Mecz? mecz { get; set; }
        [Display(Name = "Gole gospodarzy")]
        public int? gole_gospodarzy { get; set; }
        [Display(Name = "Gole gości")]
        public int? gole_gosci { get; set; }
        [Display(Name = "Strzały gospodarzy")]
        public int? strzaly_gospodarzy { get; set; }
        [Display(Name = "Strzały gości")]
        public int? strzaly_gosci { get; set; }
        [Display(Name = "Posiadanie piłki gospodarzy")]
        public int? posiadanie_pilki_gospodarzy { get; set; }
        [Display(Name = "Posiadanie piłki gości")]
        public int? posiadanie_pilki_gosci { get; set; }

        public Statystyki(int id_meczu)
        {
            this.id_meczu = id_meczu;
        }

        public void SetHomeShoots(int strzaly_gospodarzy)
        {
            this.strzaly_gospodarzy = strzaly_gospodarzy;
        }

        public void SetAwayShoots(int strzaly_gosci)
        {
            this.strzaly_gosci = strzaly_gosci;
        }

        public void SetHomeBallPossession(int posiadanie_pilki_gospodarzy)
        {
            this.posiadanie_pilki_gospodarzy = posiadanie_pilki_gospodarzy;
        }

        public void SetAwayBallPossession(int posiadanie_pilki_gosci)
        {
            this.posiadanie_pilki_gosci = posiadanie_pilki_gosci;
        }
    }

}