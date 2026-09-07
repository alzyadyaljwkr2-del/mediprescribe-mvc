using MediPrescribe.Domain;

namespace MediPrescribe.Application.Interfaces.Services
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
