using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediPrescribe.Dashboard.Models;
using MediPrescribe.Dashboard.Application.Interfaces;
using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Controllers;

[Authorize(Roles = "Admin,Doctor,Pharmacist,Patient")]
public class PrescriptionsController : Controller
{
    private readonly IPrescriptionApiService _prescriptionService;
    private readonly IDoctorApiService _doctorService;
    private readonly IPatientApiService _patientService;

    public PrescriptionsController(
        IPrescriptionApiService prescriptionService,
        IDoctorApiService doctorService,
        IPatientApiService patientService)
    {
        _prescriptionService = prescriptionService;
        _doctorService = doctorService;
        _patientService = patientService;
    }

    public async Task<IActionResult> Index()
    {
        var prescriptions = await _prescriptionService.GetAllAsync();
        var models = prescriptions.Select(p => new PrescriptionViewModel
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
        }).ToList();
        return View(models);
    }

    public async Task<IActionResult> Details(int id)
    {
        var prescription = await _prescriptionService.GetByIdAsync(id);
        if (prescription == null) return NotFound();
        var model = new PrescriptionViewModel
        {
            Id = prescription.Id,
            DoctorId = prescription.DoctorId,
            PatientId = prescription.PatientId,
            MedicationDetails = prescription.MedicationDetails,
            ImageUrl = prescription.ImageUrl,
            Doctor = prescription.Doctor is not null ? new DoctorViewModel
            {
                Id = prescription.Doctor.Id,
                Name = prescription.Doctor.Name,
                Specialization = prescription.Doctor.Specialization,
                Email = prescription.Doctor.Email
            } : null,
            Patient = prescription.Patient is not null ? new PatientViewModel
            {
                Id = prescription.Patient.Id,
                Name = prescription.Patient.Name,
                Phone = prescription.Patient.Phone,
                Address = prescription.Patient.Address
            } : null
        };
        return View(model);
    }

    private async Task PopulateDropdowns()
    {
        var doctors = await _doctorService.GetAllAsync();
        var patients = await _patientService.GetAllAsync();
        ViewBag.Doctors = new SelectList(doctors.Select(d => new { d.Id, d.Name }), "Id", "Name");
        ViewBag.Patients = new SelectList(patients.Select(p => new { p.Id, p.Name }), "Id", "Name");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PrescriptionViewModel model, IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            if (ImageFile is { Length: > 0 })
            {
                model.ImageUrl = await SaveImageAsync(ImageFile);
            }

            var dto = new PrescriptionDto
            {
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                MedicationDetails = model.MedicationDetails,
                ImageUrl = model.ImageUrl
            };
            var success = await _prescriptionService.CreateAsync(dto);
            if (success)
            {
                TempData["SuccessMessage"] = "تمت إضافة الوصفة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات.");
        }
        await PopulateDropdowns();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var prescription = await _prescriptionService.GetByIdAsync(id);
        if (prescription == null) return NotFound();
        var model = new PrescriptionViewModel
        {
            Id = prescription.Id,
            DoctorId = prescription.DoctorId,
            PatientId = prescription.PatientId,
            MedicationDetails = prescription.MedicationDetails,
            ImageUrl = prescription.ImageUrl
        };
        await PopulateDropdowns();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PrescriptionViewModel model, IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            string? newImageUrl = null;
            if (ImageFile is { Length: > 0 })
            {
                newImageUrl = await SaveImageAsync(ImageFile);
            }

            var dto = new PrescriptionDto
            {
                Id = id,
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                MedicationDetails = model.MedicationDetails,
                ImageUrl = newImageUrl ?? model.ImageUrl
            };
            var success = await _prescriptionService.UpdateAsync(id, dto);
            if (success)
            {
                if (newImageUrl is not null)
                {
                    var previousImageUrl = model.ImageUrl;
                    if (!string.IsNullOrEmpty(previousImageUrl) &&
                        !string.Equals(previousImageUrl.Trim(), newImageUrl.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", previousImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }
                }

                TempData["SuccessMessage"] = "تم تعديل الوصفة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "حدث خطأ أثناء تعديل البيانات.");
        }
        await PopulateDropdowns();
        return View(model);
    }

    private async Task<string> SaveImageAsync(IFormFile file)
    {
        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "prescriptions");
        if (!Directory.Exists(uploadsDir))
            Directory.CreateDirectory(uploadsDir);

        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/prescriptions/{fileName}";
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _prescriptionService.DeleteAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "تم حذف الوصفة بنجاح";
        }
        else
        {
            TempData["ErrorMessage"] = "حدث خطأ أثناء الحذف";
        }
        return RedirectToAction(nameof(Index));
    }
}
