using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediPrescribe.Dashboard.Models;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _dashboardService.GetDashboardDataAsync();

        var model = new DashboardViewModel
        {
            TotalUsers = result.TotalUsers,
            TotalAdmins = result.TotalAdmins,
            TotalDoctors = result.TotalDoctors,
            TotalPatients = result.TotalPatients,
            TotalPharmacists = result.TotalPharmacists,
            TotalPrescriptions = result.TotalPrescriptions,
            RecentPrescriptions = result.RecentPrescriptions.Select(p => new PrescriptionViewModel
            {
                Id = p.Id,
                DoctorId = p.DoctorId,
                PatientId = p.PatientId,
                MedicationDetails = p.MedicationDetails,
                ImageUrl = p.ImageUrl,
                Doctor = p.Doctor is not null ? new DoctorViewModel
                {
                    Id = p.Doctor.Id,
                    Name = p.Doctor.Name,
                    Specialization = p.Doctor.Specialization,
                    Email = p.Doctor.Email
                } : null,
                Patient = p.Patient is not null ? new PatientViewModel
                {
                    Id = p.Patient.Id,
                    Name = p.Patient.Name,
                    Phone = p.Patient.Phone,
                    Address = p.Patient.Address
                } : null
            }).ToList()
        };

        return View(model);
    }
}
