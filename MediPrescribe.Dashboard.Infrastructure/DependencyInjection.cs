using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediPrescribe.Dashboard.Application.Interfaces;
using MediPrescribe.Dashboard.Application.Services;
using MediPrescribe.Dashboard.Infrastructure.ApiServices;
using MediPrescribe.Dashboard.Infrastructure.Authentication;
using MediPrescribe.Dashboard.Infrastructure.Configuration;

namespace MediPrescribe.Dashboard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDashboardInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));

        var apiSettings = new ApiSettings();
        configuration.GetSection("ApiSettings").Bind(apiSettings);
        var baseAddress = new Uri(apiSettings.BaseUrl);

        services.AddHttpContextAccessor();
        services.AddTransient<JwtAuthorizationHandler>();

        services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();

        services.AddHttpClient<IDoctorApiService, DoctorApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();

        services.AddHttpClient<IPatientApiService, PatientApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();

        services.AddHttpClient<IPrescriptionApiService, PrescriptionApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();

        services.AddHttpClient<IUserApiService, UserApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();

        services.AddHttpClient<IPharmacistApiService, PharmacistApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();

        services.AddHttpClient<IDashboardApiService, DashboardApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();

        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
