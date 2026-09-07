using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Application.Interfaces;

public interface IPharmacistApiService
{
    Task<List<PharmacistDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PharmacistDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
