using MediPrescribe.Application.DTOs;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Mappings
{
    public static class PrescriptionMappings
    {
        public static PrescriptionDto ToPrescriptionDto(Prescription prescription)
        {
            return new PrescriptionDto
            {
                Id = prescription.Id,
                DoctorId = prescription.DoctorId,
                PatientId = prescription.PatientId,
                MedicationDetails = prescription.MedicationDetails,
                ImageUrl = prescription.ImageUrl
            };
        }

        public static Prescription ToEntity(CreatePrescriptionDto dto)
        {
            return new Prescription
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                MedicationDetails = dto.MedicationDetails,
                ImageUrl = dto.ImageUrl
            };
        }

        public static void ApplyTo(UpdatePrescriptionDto dto, Prescription prescription)
        {
            prescription.DoctorId = dto.DoctorId;
            prescription.PatientId = dto.PatientId;
            prescription.MedicationDetails = dto.MedicationDetails;
            prescription.ImageUrl = dto.ImageUrl;
        }
    }
}