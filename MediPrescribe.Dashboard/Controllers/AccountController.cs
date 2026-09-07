using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;
using MediPrescribe.Dashboard.Models;

namespace MediPrescribe.Dashboard.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IAuthApiService _authApiService;

    public AccountController(IAuthApiService authApiService)
    {
        _authApiService = authApiService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        var input = model.Username.Trim();
        var loginResult = await _authApiService.LoginAsync(new LoginRequestDto
        {
            Username = input,
            Email = input,
            Password = model.Password
        });

        if (loginResult.Status != LoginResultStatus.Success || loginResult.Data is null)
        {
            ModelState.AddModelError(string.Empty, loginResult.Status switch
            {
                LoginResultStatus.InvalidRequest => "بيانات تسجيل الدخول غير صحيحة",
                LoginResultStatus.ServerError => "حدث خطأ في الخادم، حاول مرة أخرى لاحقًا",
                _ => "اسم المستخدم أو كلمة المرور غير صحيحة"
            });
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        var response = loginResult.Data;

        var roleName = response.Role switch
        {
            1 => "Doctor",
            2 => "Patient",
            3 => "Pharmacist",
            _ => "Admin"
        };

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, response.UserId.ToString()),
            new Claim(ClaimTypes.Name, string.IsNullOrEmpty(response.FullName) ? response.Username : response.FullName!),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("access_token", response.Token),
            new Claim("user_id", response.UserId.ToString()),
            new Claim("username", response.Username),
            new Claim("email", response.Email ?? string.Empty)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // عمر الكعكة يطابق عمر الـ JWT (7 أيام) حتى لا يُرسَل Token منتهي الصلاحية.
        var properties = new AuthenticationProperties
        {
            IsPersistent = true,
            AllowRefresh = false,
            ExpiresUtc = DateTime.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            properties);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", GetHomeController(roleName));
    }

    private static string GetHomeController(string roleName)
    {
        return roleName switch
        {
            "Doctor" => "Doctors",
            "Pharmacist" => "Pharmacists",
            "Patient" => "Prescriptions",
            _ => "Dashboard"
        };
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}