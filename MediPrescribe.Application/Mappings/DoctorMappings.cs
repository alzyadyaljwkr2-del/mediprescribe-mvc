using MediPrescribe.Application.DTOs;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Mappings
{
    public static class DoctorMappings
    {
        public static DoctorDto ToDoctorDto(Doctor doctor)
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = doctor.Specialization,
                Email = doctor.Email
            };
        }

        public static Doctor ToEntity(DoctorDto dto)
        {
            return new Doctor
            {
                Name = dto.Name,
                Specialization = dto.Specialization,
                Email = dto.Email
            };
        }
    }
}