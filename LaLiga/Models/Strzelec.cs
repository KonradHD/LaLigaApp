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
        [Display(Name = "Samobóje")]
        public int? samoboje { get; set; }
        public int APIid { get; set; }

        public Strzelec(int id_druzyny, int id_meczu, int APIid)
        {
            this.id_druzyny = id_druzyny;
            this.id_meczu = id_meczu;
            this.APIid = APIid;
            gole = 0;
            asysty = 0;
            samoboje = 0;
        }

        public void SetNumber(int numer)
        {
            this.numer = numer;
        }

        public void AddGoal()
        {
            gole++;
        }

        public void AddAssist()
        {
            asysty++;
        }

        public void AddOwnGoal()
        {
            samoboje++;
        }
    }
}