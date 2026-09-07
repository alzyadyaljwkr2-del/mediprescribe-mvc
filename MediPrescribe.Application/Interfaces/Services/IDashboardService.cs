using MediPrescribe.Application.DTOs;

namespace MediPrescribe.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetSummaryAsync();
    }
}
