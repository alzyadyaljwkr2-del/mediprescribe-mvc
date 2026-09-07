using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Dashboard.Application.DTOs;

public class UpdateUserDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }
}
