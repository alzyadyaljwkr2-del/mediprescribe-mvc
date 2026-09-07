using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Domain;
using Microsoft.EntityFrameworkCore;

namespace MediPrescribe.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> CreateWithProfileAsync(
            User user,
            Doctor? doctor,
            Patient? patient,
            Pharmacist? pharmacist)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (doctor != null)
            {
                doctor.UserId = user.Id;
                _context.Doctors.Add(doctor);
            }

            if (patient != null)
            {
                patient.UserId = user.Id;
                _context.Patients.Add(patient);
            }

            if (pharmacist != null)
            {
                pharmacist.UserId = user.Id;
                _context.Pharmacists.Add(pharmacist);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileForRoleAsync(
            User user,
            Doctor? doctor,
            Patient? patient,
            Pharmacist? pharmacist)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            if (doctor != null)
            {
                var existingDoctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id);
                if (existingDoctor != null)
                {
                    existingDoctor.Name = doctor.Name;
                    existingDoctor.Specialization = doctor.Specialization;
                    existingDoctor.Email = doctor.Email;
                }
                else
                {
                    doctor.UserId = user.Id;
                    _context.Doctors.Add(doctor);
                }
            }

            if (patient != null)
            {
                var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (existingPatient != null)
                {
                    existingPatient.Name = patient.Name;
                    existingPatient.Phone = patient.Phone;
                    existingPatient.Address = patient.Address;
                }
                else
                {
                    patient.UserId = user.Id;
                    _context.Patients.Add(patient);
                }
            }

            if (pharmacist != null)
            {
                var existingPharmacist = await _context.Pharmacists.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (existingPharmacist != null)
                {
                    existingPharmacist.PharmacyName = pharmacist.PharmacyName;
                    existingPharmacist.LicenseNumber = pharmacist.LicenseNumber;
                    existingPharmacist.Phone = pharmacist.Phone;
                    existingPharmacist.Address = pharmacist.Address;
                }
                else
                {
                    pharmacist.UserId = user.Id;
                    _context.Pharmacists.Add(pharmacist);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username, int? excludeId = null)
        {
            var query = _context.Users.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }
            return await query.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var query = _context.Users.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }
            return await query.AnyAsync(u => u.Email == email);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}
