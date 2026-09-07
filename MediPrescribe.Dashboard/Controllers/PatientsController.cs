using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediPrescribe.Dashboard.Models;
using MediPrescribe.Dashboard.Application.Interfaces;
using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Controllers;

[Authorize(Roles = "Admin,Doctor")]
public class PatientsController : Controller
{
    private readonly IPatientApiService _patientService;

    public PatientsController(IPatientApiService patientService)
    {
        _patientService = patientService;
    }

    public async Task<IActionResult> Index()
    {
        var patients = await _patientService.GetAllAsync();
        var models = patients.Select(p => new PatientViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Phone = p.Phone,
            Address = p.Address
        }).ToList();
        return View(models);
    }

    public async Task<IActionResult> Details(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient == null) return NotFound();
        var model = new PatientViewModel
        {
            Id = patient.Id,
            Name = patient.Name,
            Phone = patient.Phone,
            Address = patient.Address
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PatientViewModel model)
    {
        if (ModelState.IsValid)
        {
            var dto = new PatientDto
            {
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address
            };
            var success = await _patientService.CreateAsync(dto);
            if (success)
            {
                TempData["SuccessMessage"] = "تمت إضافة المريض بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات.");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient == null) return NotFound();
        var model = new PatientViewModel
        {
            Id = patient.Id,
            Name = patient.Name,
            Phone = patient.Phone,
            Address = patient.Address
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PatientViewModel model)
    {
        if (ModelState.IsValid)
        {
            var dto = new PatientDto
            {
                Id = id,
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address
            };
            var success = await _patientService.UpdateAsync(id, dto);
            if (success)
            {
                TempData["SuccessMessage"] = "تم تعديل المريض بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "حدث خطأ أثناء تعديل البيانات.");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _patientService.DeleteAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "تم حذف المريض بنجاح";
        }
        else
        {
            TempData["ErrorMessage"] = "حدث خطأ أثناء الحذف";
        }
        return RedirectToAction(nameof(Index));
    }
}
