using System.ComponentModel.DataAnnotations;
using LaLiga.Models;

namespace LaLiga.Models
{
    public class Mecz
    {
        [Key]
        public int id_meczu { get; set; }
        public int id_gosci { get; set; }
        [Display(Name = "Goście")]
        public Druzyna? goscie { get; set; }
        public int id_gospodarzy { get; set; }
        [Display(Name = "Gospodarze")]
        public Druzyna? gospodarze { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Termin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime termin { get; set; }
        [Display(Name = "Sędzia")]
        public string sedzia { get; set; }
        public Statystyki? stats { get; set; }
        public ICollection<Strzelec>? strzelcy { get; set; }

        public Mecz(int id_meczu, int id_gospodarzy, int id_gosci, DateTime termin, string sedzia)
        {
            this.id_meczu = id_meczu;
            this.id_gosci = id_gosci;
            this.id_gospodarzy = id_gospodarzy;
            this.termin = termin;
            this.sedzia = sedzia;
        }
    }

}