using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Application.Interfaces;

public interface IUserApiService
{
    Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken = default);
    Task<bool> ChangeRoleAsync(int id, UserRole role, CancellationToken cancellationToken = default);
    Task<bool> ChangeStatusAsync(int id, bool isActive, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
