using System.ComponentModel.DataAnnotations;
using MediPrescribe.Dashboard.Application.DTOs;

namespace MediPrescribe.Dashboard.Models;

public class UserViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [Display(Name = "اسم المستخدم")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [Display(Name = "الاسم الكامل")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "البريد الإلكتروني")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
    public string? Email { get; set; }

    [Display(Name = "الهاتف")]
    public string? Phone { get; set; }

    [Display(Name = "الدور")]
    public UserRole Role { get; set; }

    [Display(Name = "الحالة")]
    public bool IsActive { get; set; }

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
    [Display(Name = "كلمة المرور")]
    public string? Password { get; set; }

    [Display(Name = "التخصص")]
    public string? Specialization { get; set; }

    [Display(Name = "اسم الصيدلية")]
    public string? PharmacyName { get; set; }

    [Display(Name = "رقم الترخيص")]
    public string? LicenseNumber { get; set; }

    [Display(Name = "العنوان")]
    public string? Address { get; set; }
}
