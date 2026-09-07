using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Application.Interfaces;

public interface IDoctorApiService
{
    Task<List<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(DoctorDto model, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, DoctorDto model, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
