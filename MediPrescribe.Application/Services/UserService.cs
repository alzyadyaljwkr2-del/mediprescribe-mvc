using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Repositories;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Application.Mappings;
using MediPrescribe.Domain;
using MediPrescribe.Domain.Enums;

namespace MediPrescribe.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(UserMappings.ToUserDto);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : UserMappings.ToUserDto(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            if (await _userRepository.UsernameExistsAsync(dto.Username))
            {
                throw new InvalidOperationException("اسم المستخدم موجود مسبقاً.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Email) && await _userRepository.EmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException("البريد الإلكتروني موجود مسبقاً.");
            }

            ValidateProfileForRole(dto.Role, dto);

            var user = new User
            {
                Username = dto.Username,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var doctor = dto.Role == UserRole.Doctor
                ? new Doctor { Name = dto.FullName, Specialization = dto.Specialization!, Email = dto.Email }
                : null;

            var patient = dto.Role == UserRole.Patient
                ? new Patient { Name = dto.FullName, Phone = dto.Phone ?? string.Empty, Address = dto.Address }
                : null;

            var pharmacist = dto.Role == UserRole.Pharmacist
                ? new Pharmacist
                {
                    PharmacyName = dto.PharmacyName!,
                    LicenseNumber = dto.LicenseNumber!,
                    Phone = dto.Phone,
                    Address = dto.Address
                }
                : null;

            var created = await _userRepository.CreateWithProfileAsync(user, doctor, patient, pharmacist);
            return UserMappings.ToUserDto(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Email)
                && await _userRepository.EmailExistsAsync(dto.Email, id))
            {
                throw new InvalidOperationException("البريد الإلكتروني موجود مسبقاً.");
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = _passwordHasher.Hash(dto.Password);
            }

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ChangeRoleAsync(int id, UserRole newRole)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;
            if (user.Role == newRole) return true;

            switch (newRole)
            {
                case UserRole.Doctor:
                    await _userRepository.UpdateProfileForRoleAsync(
                        user,
                        new Doctor { Name = user.FullName ?? user.Username, Specialization = user.Username, Email = user.Email },
                        null,
                        null);
                    break;
                case UserRole.Patient:
                    await _userRepository.UpdateProfileForRoleAsync(
                        user,
                        null,
                        new Patient { Name = user.FullName ?? user.Username, Phone = user.Phone ?? string.Empty, Address = null },
                        null);
                    break;
                case UserRole.Pharmacist:
                    await _userRepository.UpdateProfileForRoleAsync(
                        user,
                        null,
                        null,
                        new Pharmacist
                        {
                            PharmacyName = user.FullName ?? user.Username,
                            LicenseNumber = user.Username,
                            Phone = user.Phone,
                            Address = null
                        });
                    break;
                default:
                    await _userRepository.UpdateAsync(user);
                    break;
            }

            user.Role = newRole;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id, bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsActive = isActive;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        private static void ValidateProfileForRole(UserRole role, CreateUserDto dto)
        {
            if (role == UserRole.Doctor && string.IsNullOrWhiteSpace(dto.Specialization))
            {
                throw new InvalidOperationException("التخصص مطلوب لإنشاء حساب طبيب.");
            }

            if (role == UserRole.Pharmacist &&
                (string.IsNullOrWhiteSpace(dto.PharmacyName) || string.IsNullOrWhiteSpace(dto.LicenseNumber)))
            {
                throw new InvalidOperationException("اسم الصيدلية ورقم الترخيص مطلوبان لإنشاء حساب صيدلي.");
            }

            if (role == UserRole.Patient && string.IsNullOrWhiteSpace(dto.Phone))
            {
                throw new InvalidOperationException("رقم الهاتف مطلوب لإنشاء حساب مريض.");
            }
        }
    }
}
