using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Application.Interfaces;

public interface IPatientApiService
{
    Task<List<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(PatientDto model, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, PatientDto model, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
