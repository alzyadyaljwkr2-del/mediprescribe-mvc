using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MediPrescribe.Infrastructure.Persistence.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetSummaryAsync()
        {
            return new DashboardDto
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalAdmins = await _context.Users.CountAsync(u => u.Role == UserRole.Admin),
                TotalDoctors = await _context.Doctors.CountAsync(),
                TotalPatients = await _context.Patients.CountAsync(),
                TotalPharmacists = await _context.Pharmacists.CountAsync(),
                TotalPrescriptions = await _context.Prescriptions.CountAsync(),
                NewPrescriptions = await _context.Prescriptions.CountAsync(p => p.Status == PrescriptionStatus.Pending),
                DispensedPrescriptions = await _context.Prescriptions.CountAsync(p => p.Status == PrescriptionStatus.Dispensed)
            };
        }
    }
}
