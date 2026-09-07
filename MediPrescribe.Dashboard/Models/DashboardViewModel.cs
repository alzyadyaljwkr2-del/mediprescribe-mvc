namespace MediPrescribe.Dashboard.Models;

public class DashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalAdmins { get; set; }
    public int TotalDoctors { get; set; }
    public int TotalPatients { get; set; }
    public int TotalPharmacists { get; set; }
    public int TotalPrescriptions { get; set; }

    public List<PrescriptionViewModel> RecentPrescriptions { get; set; } = new();
}
