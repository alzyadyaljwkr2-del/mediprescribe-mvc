using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Application.Interfaces.Repositories;

namespace MediPrescribe.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardDto> GetSummaryAsync()
        {
            return await _dashboardRepository.GetSummaryAsync();
        }
    }
}
