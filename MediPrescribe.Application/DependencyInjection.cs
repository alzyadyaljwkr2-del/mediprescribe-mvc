using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MediPrescribe.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPharmacistService, PharmacistService>();
            services.AddScoped<IDashboardService, DashboardService>();

            return services;
        }
    }
}
