using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Dashboard.Models;

public class DoctorViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم الطبيب مطلوب")]
    [Display(Name = "اسم الطبيب")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "التخصص مطلوب")]
    [Display(Name = "التخصص")]
    public string Specialization { get; set; } = string.Empty;

    [Display(Name = "البريد الإلكتروني")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string? Email { get; set; }
}
