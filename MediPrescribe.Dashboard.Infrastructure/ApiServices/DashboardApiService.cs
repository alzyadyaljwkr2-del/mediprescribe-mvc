using System.Net.Http.Json;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Infrastructure.ApiServices;

public sealed class DashboardApiService : IDashboardApiService
{
    private readonly HttpClient _httpClient;

    public DashboardApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DashboardSummary?> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<DashboardSummary>("api/Dashboard/summary", cancellationToken);
    }
}
