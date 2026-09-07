using System.Net.Http.Json;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Infrastructure.ApiServices;

public sealed class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResult> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return LoginResult.InvalidCredentials();
        }

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return LoginResult.InvalidRequest();
        }

        if (!response.IsSuccessStatusCode)
        {
            return LoginResult.ServerError();
        }

        var data = await response.Content.ReadFromJsonAsync<LoginResponseDto>(cancellationToken: cancellationToken);

        if (data == null)
        {
            return LoginResult.ServerError();
        }

        return LoginResult.Ok(data);
    }
}