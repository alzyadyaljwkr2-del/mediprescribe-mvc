using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Application.Mappings;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctors = await _repository.GetAllAsync();
            return doctors.Select(d => DoctorMappings.ToDoctorDto(d));
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var d = await _repository.GetByIdAsync(id);
            return d == null ? null : DoctorMappings.ToDoctorDto(d);
        }

        public async Task<DoctorDto> CreateAsync(DoctorDto dto)
        {
            var doctor = new Doctor
            {
                Name = dto.Name,
                Specialization = dto.Specialization,
                Email = dto.Email
            };

            var created = await _repository.AddAsync(doctor);
            return DoctorMappings.ToDoctorDto(created);
        }

        public async Task<bool> UpdateAsync(int id, DoctorDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.Specialization = dto.Specialization;
            existing.Email = dto.Email;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(existing);
            return true;
        }
    }
}