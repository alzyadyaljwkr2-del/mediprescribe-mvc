using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Application.Mappings;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public PrescriptionService(
            IPrescriptionRepository repository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository)
        {
            _repository = repository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllAsync()
        {
            var prescriptions = await _repository.GetAllAsync();
            return prescriptions.Select(PrescriptionMappings.ToPrescriptionDto);
        }

        public async Task<PrescriptionDto?> GetByIdAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            return p == null ? null : PrescriptionMappings.ToPrescriptionDto(p);
        }

        public async Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);

            if (doctor == null || patient == null)
            {
                throw new ArgumentException("الطبيب أو المريض غير موجود.");
            }

            var prescription = new Prescription
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                MedicationDetails = dto.MedicationDetails,
                ImageUrl = dto.ImageUrl
            };

            var created = await _repository.AddAsync(prescription);
            return PrescriptionMappings.ToPrescriptionDto(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdatePrescriptionDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);

            if (doctor == null || patient == null)
            {
                throw new ArgumentException("الطبيب أو المريض غير موجود.");
            }

            existing.DoctorId = dto.DoctorId;
            existing.PatientId = dto.PatientId;
            existing.MedicationDetails = dto.MedicationDetails;
            existing.ImageUrl = dto.ImageUrl;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(existing);
            return true;
        }
    }
}