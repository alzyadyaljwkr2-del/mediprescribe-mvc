using System.ComponentModel.DataAnnotations;
using MediPrescribe.Domain.Enums;

namespace MediPrescribe.Domain
{
    public class Prescription
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "رقم الطبيب مطلوب")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "رقم المريض مطلوب")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "تفاصيل الدواء مطلوبة")]
        public string MedicationDetails { get; set; } = string.Empty;

        public string? ImageUrl { get; set; } // لدعم رفع الصور المطلوب في المشروع

        public PrescriptionStatus Status { get; set; } = PrescriptionStatus.Pending;
    }
}
