using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passwordmanager.Model
{
    public class UserInfo 
    {
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        [NotMapped]
        public string? Token { get; set; }


    }
}
