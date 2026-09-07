using MediPrescribe.Application.DTOs;

namespace MediPrescribe.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> GetSummaryAsync();
    }
}
