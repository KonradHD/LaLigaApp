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
        public int gole_gospodarzy { get; set; }
        [Display(Name = "Gole gości")]
        public int gole_gosci { get; set; }
        [Display(Name = "Strzały gospodarzy")]
        public int strzaly_gospodarzy { get; set; }
        [Display(Name = "Strzały gości")]
        public int strzaly_gosci { get; set; }
    }
}