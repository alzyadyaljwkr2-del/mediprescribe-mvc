using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Application.DTOs
{
    public class CreatePharmacistDto
    {
        [Required(ErrorMessage = "اسم الصيدلية مطلوب")]
        public string PharmacyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الترخيص مطلوب")]
        public string LicenseNumber { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}
