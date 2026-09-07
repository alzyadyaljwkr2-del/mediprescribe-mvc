using MediPrescribe.Application.DTOs;

namespace MediPrescribe.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
