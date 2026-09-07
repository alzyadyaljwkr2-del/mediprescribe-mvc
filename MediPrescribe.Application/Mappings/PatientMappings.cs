using MediPrescribe.Application.DTOs;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Mappings
{
    public static class PatientMappings
    {
        public static PatientDto ToPatientDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Phone = patient.Phone,
                Address = patient.Address
            };
        }

        public static Patient ToEntity(PatientDto dto)
        {
            return new Patient
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Address = dto.Address
            };
        }
    }
}