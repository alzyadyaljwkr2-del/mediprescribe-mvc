using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Application.Mappings;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Services
{
    public class PharmacistService : IPharmacistService
    {
        private readonly IPharmacistRepository _repository;
        private readonly IUserRepository _userRepository;

        public PharmacistService(IPharmacistRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<PharmacistDto>> GetAllAsync()
        {
            var pharmacists = await _repository.GetAllAsync();
            return pharmacists.Select(PharmacistMappings.ToPharmacistDto);
        }

        public async Task<PharmacistDto?> GetByIdAsync(int id)
        {
            var pharmacist = await _repository.GetByIdAsync(id);
            return pharmacist == null ? null : PharmacistMappings.ToPharmacistDto(pharmacist);
        }

        public async Task<PharmacistDto> CreateAsync(CreatePharmacistDto dto, int userId)
        {
            if (await _repository.GetByUserIdAsync(userId) != null)
            {
                throw new InvalidOperationException("هذا المستخدم لديه ملف صيدلي بالفعل.");
            }

            var pharmacist = new Pharmacist
            {
                UserId = userId,
                PharmacyName = dto.PharmacyName,
                LicenseNumber = dto.LicenseNumber,
                Phone = dto.Phone,
                Address = dto.Address
            };

            var created = await _repository.AddAsync(pharmacist);
            return PharmacistMappings.ToPharmacistDto(created);
        }

        public async Task<bool> UpdateAsync(int id, CreatePharmacistDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.PharmacyName = dto.PharmacyName;
            existing.LicenseNumber = dto.LicenseNumber;
            existing.Phone = dto.Phone;
            existing.Address = dto.Address;

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
