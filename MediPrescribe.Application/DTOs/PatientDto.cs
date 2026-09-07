using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Application.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المريض مطلوب")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string Phone { get; set; } = string.Empty;

        public string? Address { get; set; }
    }
}