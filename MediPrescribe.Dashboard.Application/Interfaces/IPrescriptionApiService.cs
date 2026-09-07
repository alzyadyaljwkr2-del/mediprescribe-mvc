using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Application.Interfaces;

public interface IPrescriptionApiService
{
    Task<List<PrescriptionDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PrescriptionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(PrescriptionDto model, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, PrescriptionDto model, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
