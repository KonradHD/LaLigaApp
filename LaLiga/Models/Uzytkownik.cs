using System.ComponentModel.DataAnnotations;

namespace LaLiga.Models
{
    public class Uzytkownik
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Email jest wymagany.")]
        public string email { get; set; }
        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        public string haslo { get; set; }
        [Display(Name = "Wiek")]
        [Required(ErrorMessage = "Wiek jest wymagany.")]
        [Range(1, 120, ErrorMessage = "Wiek musi być w przedziale 1-120.")]
        public int wiek { get; set; }
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string imie { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string nazwisko { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data dołączenia")]
        public DateTime data_dolaczenia { get; set; }
        [Display(Name = "Rola")]
        public string? rola { get; set; }

    }
}