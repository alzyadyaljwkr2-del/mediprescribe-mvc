using MediPrescribe.Domain;

namespace MediPrescribe.Application.Interfaces.Repositories
{
    public interface IPharmacistRepository
    {
        Task<IEnumerable<Pharmacist>> GetAllAsync();
        Task<Pharmacist?> GetByIdAsync(int id);
        Task<Pharmacist?> GetByUserIdAsync(int userId);
        Task<Pharmacist> AddAsync(Pharmacist pharmacist);
        Task UpdateAsync(Pharmacist pharmacist);
        Task DeleteAsync(Pharmacist pharmacist);
    }
}
