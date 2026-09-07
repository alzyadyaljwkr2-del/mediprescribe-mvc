using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediPrescribe.Dashboard.Application.Interfaces;
using MediPrescribe.Dashboard.Models;

namespace MediPrescribe.Dashboard.Controllers;

[Authorize(Roles = "Admin")]
public class PharmacistsController : Controller
{
    private readonly IPharmacistApiService _pharmacistService;

    public PharmacistsController(IPharmacistApiService pharmacistService)
    {
        _pharmacistService = pharmacistService;
    }

    public async Task<IActionResult> Index()
    {
        var pharmacists = await _pharmacistService.GetAllAsync();
        var models = pharmacists.Select(p => new PharmacistViewModel
        {
            Id = p.Id,
            UserId = p.UserId,
            PharmacyName = p.PharmacyName,
            LicenseNumber = p.LicenseNumber,
            Phone = p.Phone,
            Address = p.Address
        }).ToList();
        return View(models);
    }

    public async Task<IActionResult> Details(int id)
    {
        var pharmacist = await _pharmacistService.GetByIdAsync(id);
        if (pharmacist == null) return NotFound();
        var model = new PharmacistViewModel
        {
            Id = pharmacist.Id,
            UserId = pharmacist.UserId,
            PharmacyName = pharmacist.PharmacyName,
            LicenseNumber = pharmacist.LicenseNumber,
            Phone = pharmacist.Phone,
            Address = pharmacist.Address
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _pharmacistService.DeleteAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "تم حذف الصيدلي بنجاح";
        }
        else
        {
            TempData["ErrorMessage"] = "تعذر حذف الصيدلي";
        }
        return RedirectToAction(nameof(Index));
    }
}
