using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Dashboard.Models;

public class PatientViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم المريض مطلوب")]
    [Display(Name = "اسم المريض")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الجوال مطلوب")]
    [Display(Name = "رقم الجوال")]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "العنوان")]
    public string? Address { get; set; }
}
