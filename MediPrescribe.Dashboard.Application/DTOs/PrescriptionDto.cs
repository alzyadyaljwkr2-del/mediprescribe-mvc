namespace MediPrescribe.Dashboard.Application.DTOs;

public class PrescriptionDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string MedicationDetails { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DoctorDto? Doctor { get; set; }
    public PatientDto? Patient { get; set; }
}
