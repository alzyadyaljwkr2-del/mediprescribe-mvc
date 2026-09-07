using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Application.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطبيب مطلوب")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "التخصص مطلوب")]
        public string Specialization { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string? Email { get; set; }
    }
}