using MediPrescribe.Application.Interfaces.Infrastructure;
using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Infrastructure.Authentication;
using MediPrescribe.Infrastructure.FileStorage;
using MediPrescribe.Infrastructure.Persistence;
using MediPrescribe.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediPrescribe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<IPharmacistRepository, PharmacistRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IFileStorageService, FileStorageService>();

            return services;
        }
    }
}
