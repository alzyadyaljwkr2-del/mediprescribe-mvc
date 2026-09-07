using System.ComponentModel.DataAnnotations;

namespace MediPrescribe.Dashboard.Application.DTOs;

public class LoginRequestDto
{
    public string? Username { get; set; }

    public string? Email { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;
}