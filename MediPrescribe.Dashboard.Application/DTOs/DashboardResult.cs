namespace MediPrescribe.Dashboard.Application.DTOs;

public class DashboardResult
{
    public int TotalUsers { get; set; }
    public int TotalAdmins { get; set; }
    public int TotalDoctors { get; set; }
    public int TotalPatients { get; set; }
    public int TotalPharmacists { get; set; }
    public int TotalPrescriptions { get; set; }
    public List<PrescriptionDto> RecentPrescriptions { get; set; } = new();
}
