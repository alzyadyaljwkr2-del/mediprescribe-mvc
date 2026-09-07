using MediPrescribe.Domain;

namespace MediPrescribe.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task<User> CreateWithProfileAsync(User user, Doctor? doctor, Patient? patient, Pharmacist? pharmacist);
        Task UpdateAsync(User user);
        Task UpdateProfileForRoleAsync(User user, Doctor? doctor, Patient? patient, Pharmacist? pharmacist);
        Task<bool> UsernameExistsAsync(string username, int? excludeId = null);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<int> CountAsync();
    }
}
