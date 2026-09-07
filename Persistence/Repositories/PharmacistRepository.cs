using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Domain;
using Microsoft.EntityFrameworkCore;

namespace MediPrescribe.Infrastructure.Persistence.Repositories
{
    public class PharmacistRepository : IPharmacistRepository
    {
        private readonly AppDbContext _context;

        public PharmacistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pharmacist>> GetAllAsync()
        {
            return await _context.Pharmacists.ToListAsync();
        }

        public async Task<Pharmacist?> GetByIdAsync(int id)
        {
            return await _context.Pharmacists.FindAsync(id);
        }

        public async Task<Pharmacist?> GetByUserIdAsync(int userId)
        {
            return await _context.Pharmacists.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Pharmacist> AddAsync(Pharmacist pharmacist)
        {
            _context.Pharmacists.Add(pharmacist);
            await _context.SaveChangesAsync();
            return pharmacist;
        }

        public async Task UpdateAsync(Pharmacist pharmacist)
        {
            _context.Pharmacists.Update(pharmacist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Pharmacist pharmacist)
        {
            _context.Pharmacists.Remove(pharmacist);
            await _context.SaveChangesAsync();
        }
    }
}
