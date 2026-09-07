using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Application.Interfaces;

public interface IDashboardApiService
{
    Task<DashboardSummary?> GetSummaryAsync(CancellationToken cancellationToken = default);
}
