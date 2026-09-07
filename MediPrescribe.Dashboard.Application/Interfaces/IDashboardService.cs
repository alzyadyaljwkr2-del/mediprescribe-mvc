using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardResult> GetDashboardDataAsync(CancellationToken cancellationToken = default);
}
