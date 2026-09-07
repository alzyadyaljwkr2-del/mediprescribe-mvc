using System.Net.Http.Json;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Infrastructure.ApiServices;

public sealed class PharmacistApiService : IPharmacistApiService
{
    private readonly HttpClient _httpClient;

    public PharmacistApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PharmacistDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<List<PharmacistDto>>("api/Pharmacists", cancellationToken) ?? new List<PharmacistDto>();
    }

    public async Task<PharmacistDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<PharmacistDto>($"api/Pharmacists/{id}", cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/Pharmacists/{id}", cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
