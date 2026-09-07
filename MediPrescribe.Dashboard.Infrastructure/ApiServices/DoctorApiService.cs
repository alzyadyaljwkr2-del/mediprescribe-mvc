using System.Net.Http.Json;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Infrastructure.ApiServices;

public sealed class DoctorApiService : IDoctorApiService
{
    private readonly HttpClient _httpClient;

    public DoctorApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<List<DoctorDto>>("api/Doctors", cancellationToken) ?? new List<DoctorDto>();
    }

    public async Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<DoctorDto>($"api/Doctors/{id}", cancellationToken);
    }

    public async Task<bool> CreateAsync(DoctorDto model, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Doctors", model, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, DoctorDto model, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Doctors/{id}", model, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/Doctors/{id}", cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
