using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Application.DTOs
{
    public class CreatePrescriptionDto
    {
        [Required(ErrorMessage = "رقم الطبيب مطلوب")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "رقم المريض مطلوب")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "تفاصيل الدواء مطلوبة")]
        public string MedicationDetails { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}