using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Dashboard.Models;

public class PharmacistViewModel
{
    public int Id { get; set; }

    [Display(Name = "معرف المستخدم")]
    public int UserId { get; set; }

    [Display(Name = "اسم الصيدلية")]
    public string PharmacyName { get; set; } = string.Empty;

    [Display(Name = "رقم الترخيص")]
    public string LicenseNumber { get; set; } = string.Empty;

    [Display(Name = "الهاتف")]
    public string? Phone { get; set; }

    [Display(Name = "العنوان")]
    public string? Address { get; set; }
}
