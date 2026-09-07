using System.ComponentModel.DataAnnotations;
using MediPrescribe.Domain.Enums;

namespace MediPrescribe.Application.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = string.Empty;

        public string? Phone { get; set; }

        [Required(ErrorMessage = "الدور مطلوب")]
        public UserRole Role { get; set; }

        public string? Specialization { get; set; }

        public string? PharmacyName { get; set; }

        public string? LicenseNumber { get; set; }

        public string? Address { get; set; }
    }
}
