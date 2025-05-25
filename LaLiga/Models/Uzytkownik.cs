using System.ComponentModel.DataAnnotations;

namespace LaLiga.Models
{
    public class Uzytkownik
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string email { get; set; }
        [Display(Name = "Hasło")]
        public string haslo { get; set; }
        [Display(Name = "Wiek")]
        public int wiek { get; set; }
        [Display(Name = "Imię")]
        public string imie { get; set; }
        [Display(Name = "Nazwisko")]
        public string nazwisko { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data dołączenia")]
        public DateTime data_dołączenia { get; set; }

    }
}