using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Application.Interfaces;

public interface IAuthApiService
{
    Task<LoginResult> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}