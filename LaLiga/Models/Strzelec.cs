using System.ComponentModel.DataAnnotations;

namespace LaLiga.Models
{
    public class Strzelec
    {
        public int id_druzyny { get; set; }
        [Display(Name = "Numer")]
        public int numer { get; set; }

        public Zawodnik? zawodnik { get; set; }
        public int id_meczu { get; set; }
        public Mecz? mecz { get; set; }
        [Display(Name = "Gole")]
        public int? gole { get; set; }
        [Display(Name = "Asysty")]
        public int? asysty { get; set; }
    }
}