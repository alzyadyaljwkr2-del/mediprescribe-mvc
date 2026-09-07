using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Domain;
using MediPrescribe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediPrescribe.Infrastructure.Persistence.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;

        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            return await _context.Prescriptions.ToListAsync();
        }

        public async Task<Prescription?> GetByIdAsync(int id)
        {
            return await _context.Prescriptions.FindAsync(id);
        }

        public async Task<Prescription> AddAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task UpdateAsync(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Prescription prescription)
        {
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
        }
    }
}