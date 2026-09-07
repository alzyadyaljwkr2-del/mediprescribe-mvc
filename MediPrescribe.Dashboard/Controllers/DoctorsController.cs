using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediPrescribe.Dashboard.Models;
using MediPrescribe.Dashboard.Application.Interfaces;
using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Controllers;

[Authorize(Roles = "Admin,Doctor")]
public class DoctorsController : Controller
{
    private readonly IDoctorApiService _doctorService;

    public DoctorsController(IDoctorApiService doctorService)
    {
        _doctorService = doctorService;
    }

    public async Task<IActionResult> Index()
    {
        var doctors = await _doctorService.GetAllAsync();
        var models = doctors.Select(d => new DoctorViewModel
        {
            Id = d.Id,
            Name = d.Name,
            Specialization = d.Specialization,
            Email = d.Email
        }).ToList();
        return View(models);
    }

    public async Task<IActionResult> Details(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        if (doctor == null) return NotFound();
        var model = new DoctorViewModel
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialization = doctor.Specialization,
            Email = doctor.Email
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(DoctorViewModel model)
    {
        if (ModelState.IsValid)
        {
            var dto = new DoctorDto
            {
                Name = model.Name,
                Specialization = model.Specialization,
                Email = model.Email
            };
            var success = await _doctorService.CreateAsync(dto);
            if (success)
            {
                TempData["SuccessMessage"] = "تمت إضافة الطبيب بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات.");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        if (doctor == null) return NotFound();
        var model = new DoctorViewModel
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialization = doctor.Specialization,
            Email = doctor.Email
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, DoctorViewModel model)
    {
        if (ModelState.IsValid)
        {
            var dto = new DoctorDto
            {
                Id = id,
                Name = model.Name,
                Specialization = model.Specialization,
                Email = model.Email
            };
            var success = await _doctorService.UpdateAsync(id, dto);
            if (success)
            {
                TempData["SuccessMessage"] = "تم تعديل الطبيب بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "حدث خطأ أثناء تعديل البيانات.");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _doctorService.DeleteAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "تم حذف الطبيب بنجاح";
        }
        else
        {
            TempData["ErrorMessage"] = "حدث خطأ أثناء الحذف";
        }
        return RedirectToAction(nameof(Index));
    }
}
