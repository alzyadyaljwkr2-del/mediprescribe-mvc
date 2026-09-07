using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Domain
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطبيب مطلوب")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "التخصص مطلوب")]
        public string Specialization { get; set; } = string.Empty;

        public string? Email { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}
