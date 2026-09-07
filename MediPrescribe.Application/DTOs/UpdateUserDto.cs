using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Application.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }
    }
}
