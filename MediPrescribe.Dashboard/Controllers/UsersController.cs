using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;
using MediPrescribe.Dashboard.Models;

namespace MediPrescribe.Dashboard.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly IUserApiService _userService;

    public UsersController(IUserApiService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        var models = users.Select(MapToViewModel).ToList();
        return View(models);
    }

    public async Task<IActionResult> Details(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return View(MapToViewModel(user));
    }

    [HttpGet]
    public IActionResult Create()
    {
        PopulateRoles();
        return View(new UserViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateRoles(model.Role);
            return View(model);
        }

        var dto = new CreateUserDto
        {
            Username = model.Username.Trim(),
            FullName = model.FullName.Trim(),
            Email = model.Email,
            Phone = model.Phone,
            Password = model.Password ?? string.Empty,
            Role = model.Role,
            Specialization = model.Specialization,
            PharmacyName = model.PharmacyName,
            LicenseNumber = model.LicenseNumber,
            Address = model.Address
        };

        var success = await _userService.CreateAsync(dto);
        if (success)
        {
            TempData["SuccessMessage"] = "تم إنشاء المستخدم بنجاح";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "تعذر إنشاء المستخدم. تحقق من صحة البيانات.");
        PopulateRoles(model.Role);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        var model = MapToViewModel(user);
        PopulateRoles(model.Role);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel model)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            PopulateRoles(model.Role);
            return View(model);
        }

        var updated = await _userService.UpdateAsync(id, new UpdateUserDto
        {
            FullName = model.FullName.Trim(),
            Email = model.Email,
            Phone = model.Phone,
            Password = string.IsNullOrWhiteSpace(model.Password) ? null : model.Password
        });

        if (!updated)
        {
            ModelState.AddModelError(string.Empty, "تعذر تحديث المستخدم.");
            PopulateRoles(model.Role);
            return View(model);
        }

        var user = await _userService.GetByIdAsync(id);
        if (user != null && user.Role != model.Role)
        {
            await _userService.ChangeRoleAsync(id, model.Role);
        }

        TempData["SuccessMessage"] = "تم تحديث المستخدم بنجاح";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        var success = await _userService.ChangeStatusAsync(id, !user.IsActive);
        if (success)
        {
            TempData["SuccessMessage"] = user.IsActive
                ? "تم تعطيل المستخدم"
                : "تم تفعيل المستخدم";
        }
        else
        {
            TempData["ErrorMessage"] = "تعذر تغيير حالة المستخدم";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _userService.DeleteAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "تم تعطيل المستخدم";
        }
        else
        {
            TempData["ErrorMessage"] = "تعذر حذف المستخدم";
        }
        return RedirectToAction(nameof(Index));
    }

    private void PopulateRoles(UserRole selected = UserRole.Patient)
    {
        var roles = new List<SelectListItem>
        {
            new SelectListItem { Value = UserRole.Admin.ToString(), Text = "مدير" },
            new SelectListItem { Value = UserRole.Doctor.ToString(), Text = "طبيب" },
            new SelectListItem { Value = UserRole.Patient.ToString(), Text = "مريض" },
            new SelectListItem { Value = UserRole.Pharmacist.ToString(), Text = "صيدلي" }
        };

        ViewBag.Roles = new SelectList(roles, "Value", "Text", selected.ToString());
    }

    private static UserViewModel MapToViewModel(UserDto user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName ?? user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}
