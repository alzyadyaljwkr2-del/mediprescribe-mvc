using MediPrescribe.Application.DTOs;

namespace MediPrescribe.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto?> GetByUserIdAsync(int userId);
        Task<PatientDto> CreateAsync(PatientDto dto);
        Task<bool> UpdateAsync(int id, PatientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
