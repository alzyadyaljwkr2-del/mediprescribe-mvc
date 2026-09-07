using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Application.DTOs
{
    public class LoginRequestDto
    {
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
