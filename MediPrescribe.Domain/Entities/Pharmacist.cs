using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Domain
{
    public class Pharmacist
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "رقم المستخدم مطلوب")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "اسم الصيدلية مطلوب")]
        public string PharmacyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الترخيص مطلوب")]
        public string LicenseNumber { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public User? User { get; set; }
    }
}
