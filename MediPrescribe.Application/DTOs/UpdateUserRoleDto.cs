using System.ComponentModel.DataAnnotations;
using MediPrescribe.Domain.Enums;

namespace MediPrescribe.Application.DTOs
{
    public class UpdateUserRoleDto
    {
        [Required(ErrorMessage = "الدور مطلوب")]
        public UserRole Role { get; set; }
    }
}
