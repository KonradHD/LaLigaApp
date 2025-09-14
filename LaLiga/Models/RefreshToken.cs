using System.ComponentModel.DataAnnotations;

namespace LaLiga.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Token { get; set; } = string.Empty;
        
        [Required]
        public DateTime ExpiryDate { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public Uzytkownik User { get; set; } = null!;
        
        public bool IsRevoked { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

