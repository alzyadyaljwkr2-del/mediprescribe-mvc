using System.ComponentModel.DataAnnotations;
using MediPrescribe.Domain.Enums;

namespace MediPrescribe.Domain
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string? Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
