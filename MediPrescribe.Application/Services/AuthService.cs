using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Application.Interfaces.Services;

namespace MediPrescribe.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await FindUserAsync(request);

            if (user == null || !user.IsActive || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            var token = _jwtProvider.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        private async Task<Domain.User?> FindUserAsync(LoginRequestDto request)
        {
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                return await _userRepository.GetByEmailAsync(request.Email);
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                return await _userRepository.GetByUsernameAsync(request.Username);
            }

            return null;
        }
    }
}
