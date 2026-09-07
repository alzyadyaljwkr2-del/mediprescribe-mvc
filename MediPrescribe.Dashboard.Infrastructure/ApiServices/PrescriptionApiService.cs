using System.Net.Http.Json;
using MediPrescribe.Dashboard.Application.DTOs;
using MediPrescribe.Dashboard.Application.Interfaces;

namespace MediPrescribe.Dashboard.Infrastructure.ApiServices;

public sealed class PrescriptionApiService : IPrescriptionApiService
{
    private readonly HttpClient _httpClient;
    private readonly IDoctorApiService _doctorApiService;
    private readonly IPatientApiService _patientApiService;

    public PrescriptionApiService(
        HttpClient httpClient,
        IDoctorApiService doctorApiService,
        IPatientApiService patientApiService)
    {
        _httpClient = httpClient;
        _doctorApiService = doctorApiService;
        _patientApiService = patientApiService;
    }

    public async Task<List<PrescriptionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var prescriptions = await _httpClient.GetFromJsonAsync<List<PrescriptionDto>>("api/Prescriptions", cancellationToken) ?? new List<PrescriptionDto>();

        foreach (var p in prescriptions)
        {
            p.Doctor = await _doctorApiService.GetByIdAsync(p.DoctorId, cancellationToken);
            p.Patient = await _patientApiService.GetByIdAsync(p.PatientId, cancellationToken);
        }

        return prescriptions;
    }

    public async Task<PrescriptionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var prescription = await _httpClient.GetFromJsonAsync<PrescriptionDto>($"api/Prescriptions/{id}", cancellationToken);
        if (prescription is not null)
        {
            prescription.Doctor = await _doctorApiService.GetByIdAsync(prescription.DoctorId, cancellationToken);
            prescription.Patient = await _patientApiService.GetByIdAsync(prescription.PatientId, cancellationToken);
        }
        return prescription;
    }

    public async Task<bool> CreateAsync(PrescriptionDto model, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Prescriptions", model, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, PrescriptionDto model, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Prescriptions/{id}", model, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/Prescriptions/{id}", cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
