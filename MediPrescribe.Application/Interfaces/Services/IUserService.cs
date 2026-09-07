using MediPrescribe.Application.DTOs;
using MediPrescribe.Domain.Enums;

namespace MediPrescribe.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<bool> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangeRoleAsync(int id, UserRole role);
        Task<bool> ChangeStatusAsync(int id, bool isActive);
    }
}
