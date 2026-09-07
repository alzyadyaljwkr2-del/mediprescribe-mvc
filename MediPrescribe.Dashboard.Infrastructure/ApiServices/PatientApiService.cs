using System.Net.Http.Json;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Infrastructure.ApiServices;

public sealed class PatientApiService : IPatientApiService
{
    private readonly HttpClient _httpClient;

    public PatientApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<List<PatientDto>>("api/Patients", cancellationToken) ?? new List<PatientDto>();
    }

    public async Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<PatientDto>($"api/Patients/{id}", cancellationToken);
    }

    public async Task<bool> CreateAsync(PatientDto model, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Patients", model, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, PatientDto model, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Patients/{id}", model, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/Patients/{id}", cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
