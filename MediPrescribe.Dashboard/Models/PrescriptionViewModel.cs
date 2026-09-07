using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Dashboard.Models;

public class PrescriptionViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "الطبيب مطلوب")]
    [Display(Name = "الطبيب")]
    public int DoctorId { get; set; }

    [Required(ErrorMessage = "المريض مطلوب")]
    [Display(Name = "المريض")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "تفاصيل الأدوية مطلوبة")]
    [Display(Name = "الأدوية")]
    public string MedicationDetails { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }
    
    // For display purposes in views
    public DoctorViewModel? Doctor { get; set; }
    public PatientViewModel? Patient { get; set; }
}
