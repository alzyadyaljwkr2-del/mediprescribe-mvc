using MediPrescribe.Application.DTOs;

namespace MediPrescribe.Application.Interfaces.Services
{
    public interface IPharmacistService
    {
        Task<IEnumerable<PharmacistDto>> GetAllAsync();
        Task<PharmacistDto?> GetByIdAsync(int id);
        Task<PharmacistDto> CreateAsync(CreatePharmacistDto dto, int userId);
        Task<bool> UpdateAsync(int id, CreatePharmacistDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
