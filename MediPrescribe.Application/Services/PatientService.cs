using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Application.Mappings;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return patients.Select(p => PatientMappings.ToPatientDto(p));
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            return p == null ? null : PatientMappings.ToPatientDto(p);
        }

        public async Task<PatientDto?> GetByUserIdAsync(int userId)
        {
            var p = await _repository.GetByUserIdAsync(userId);
            return p == null ? null : PatientMappings.ToPatientDto(p);
        }

        public async Task<PatientDto> CreateAsync(PatientDto dto)
        {
            var patient = new Patient
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Address = dto.Address
            };

            var created = await _repository.AddAsync(patient);
            return PatientMappings.ToPatientDto(created);
        }

        public async Task<bool> UpdateAsync(int id, PatientDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.Phone = dto.Phone;
            existing.Address = dto.Address;

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