using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Application.DTOs
{
    public class UpdateUserStatusDto
    {
        [Required(ErrorMessage = "الحالة مطلوبة")]
        public bool IsActive { get; set; }
    }
}
