using MediPrescribe.Application.DTOs;

namespace MediPrescribe.Application.Interfaces.Services
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetAllAsync();
        Task<PrescriptionDto?> GetByIdAsync(int id);
        Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto dto);
        Task<bool> UpdateAsync(int id, UpdatePrescriptionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
