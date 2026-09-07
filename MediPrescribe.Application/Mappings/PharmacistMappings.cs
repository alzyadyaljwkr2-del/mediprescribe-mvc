using MediPrescribe.Application.DTOs;
using MediPrescribe.Domain;

namespace MediPrescribe.Application.Mappings
{
    public static class PharmacistMappings
    {
        public static PharmacistDto ToPharmacistDto(Pharmacist pharmacist)
        {
            return new PharmacistDto
            {
                Id = pharmacist.Id,
                UserId = pharmacist.UserId,
                PharmacyName = pharmacist.PharmacyName,
                LicenseNumber = pharmacist.LicenseNumber,
                Phone = pharmacist.Phone,
                Address = pharmacist.Address
            };
        }
    }
}
