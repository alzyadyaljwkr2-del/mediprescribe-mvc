using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardApiService _dashboardApiService;
    private readonly IPrescriptionApiService _prescriptionApiService;

    public DashboardService(
        IDashboardApiService dashboardApiService,
        IPrescriptionApiService prescriptionApiService)
    {
        _dashboardApiService = dashboardApiService;
        _prescriptionApiService = prescriptionApiService;
    }

    public async Task<DashboardResult> GetDashboardDataAsync(CancellationToken cancellationToken = default)
    {
        var summary = await _dashboardApiService.GetSummaryAsync(cancellationToken) ?? new DashboardSummary();
        var prescriptions = await _prescriptionApiService.GetAllAsync(cancellationToken);

        return new DashboardResult
        {
            TotalUsers = summary.TotalUsers,
            TotalAdmins = summary.TotalAdmins,
            TotalDoctors = summary.TotalDoctors,
            TotalPatients = summary.TotalPatients,
            TotalPharmacists = summary.TotalPharmacists,
            TotalPrescriptions = summary.TotalPrescriptions,
            RecentPrescriptions = prescriptions
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToList()
        };
    }
}
