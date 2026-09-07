using MediPrescribe.Application.DTOs;

namespace MediPrescribe.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorDto> CreateAsync(DoctorDto dto);
        Task<bool> UpdateAsync(int id, DoctorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
