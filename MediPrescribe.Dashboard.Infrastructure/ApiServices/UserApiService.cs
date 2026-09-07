using System.Net.Http.Json;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Infrastructure.ApiServices;

public sealed class UserApiService : IUserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/Users", cancellationToken) ?? new List<UserDto>();
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<UserDto>($"api/Users/{id}", cancellationToken);
    }

    public async Task<bool> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Users", dto, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Users/{id}", dto, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ChangeRoleAsync(int id, UserRole role, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Users/{id}/role", new UpdateUserRoleDto { Role = role }, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ChangeStatusAsync(int id, bool isActive, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Users/{id}/status", new UpdateUserStatusDto { IsActive = isActive }, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/Users/{id}", cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
