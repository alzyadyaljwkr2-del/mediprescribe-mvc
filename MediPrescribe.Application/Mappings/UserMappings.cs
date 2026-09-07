using MediPrescribe.Application.DTOs;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Mappings
{
    public static class UserMappings
    {
        public static UserDto ToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
