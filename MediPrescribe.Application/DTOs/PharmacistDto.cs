namespace MediPrescribe.Application.DTOs
{
    public class PharmacistDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PharmacyName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
