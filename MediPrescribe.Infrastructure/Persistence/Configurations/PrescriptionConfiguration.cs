using MediPrescribe.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediPrescribe.Infrastructure.Persistence.Configurations
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.ToTable("Prescriptions");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.MedicationDetails).IsRequired();
            builder.Property(p => p.ImageUrl);
            builder.Property(p => p.DoctorId);
            builder.Property(p => p.PatientId);
        }
    }
}