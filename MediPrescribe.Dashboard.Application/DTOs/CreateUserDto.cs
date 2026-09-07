using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Dashboard.Application.DTOs;

public class CreateUserDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }

    [Required]
    public UserRole Role { get; set; }

    public string? Specialization { get; set; }

    public string? PharmacyName { get; set; }

    public string? LicenseNumber { get; set; }

    public string? Address { get; set; }
}
