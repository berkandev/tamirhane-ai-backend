using DecoderApi.DTOs;

namespace DecoderApi.Services
{
    public interface ILicenseService
    {
        Task<LicenseDto> AssignLicenseAsync(CreateLicenseDto model);
        Task<IEnumerable<LicenseDto>> GetUserLicensesAsync(int userId);
        Task<LicenseDto> DeactivateLicenseAsync(int id, DeactivateLicenseDto model);
        Task<bool> DeleteLicenseAsync(int id);
        Task<LicenseDto> GetLicenseByIdAsync(int id);
        Task<IEnumerable<LicenseDto>> GetAllLicensesAsync();
        Task<string> GenerateLicenseKeyAsync();
    }
} 