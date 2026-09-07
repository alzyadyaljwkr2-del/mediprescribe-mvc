using MediPrescribe.Domain.Enums;

namespace MediPrescribe.Application.DTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string MedicationDetails { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public PrescriptionStatus Status { get; set; }
    }
}