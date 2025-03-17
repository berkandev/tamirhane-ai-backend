using System.Security.Cryptography;
using DecoderApi.Data;
using DecoderApi.DTOs;
using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DecoderApi.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ApplicationDbContext _context;
        
        public LicenseService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<LicenseDto> AssignLicenseAsync(CreateLicenseDto model)
        {
            // Kullanıcı var mı kontrol et
            var user = await _context.Users.FindAsync(model.UserId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
                
            // Lisans anahtarı benzersiz mi kontrol et
            if (await _context.Licenses.AnyAsync(l => l.LicenseKey == model.LicenseKey))
                throw new Exception("Bu lisans anahtarı zaten kullanılıyor.");
                
            // Geçerlilik süresi kontrolü
            if (model.ExpiryDate <= DateTime.UtcNow)
                throw new Exception("Lisans son kullanma tarihi geçerli bir tarih olmalıdır.");
                
            // Yeni lisans oluştur
            var license = new License
            {
                UserId = model.UserId,
                LicenseKey = model.LicenseKey,
                ExpiryDate = model.ExpiryDate,
                IssueDate = DateTime.UtcNow,
                Notes = model.Notes,
                LicenseType = model.LicenseType,
                IsActive = true
            };
            
            _context.Licenses.Add(license);
            await _context.SaveChangesAsync();
            
            // Kullanıcı bilgilerini al
            return await GetLicenseWithUserDetailsAsync(license.Id);
        }
        
        public async Task<IEnumerable<LicenseDto>> GetUserLicensesAsync(int userId)
        {
            // Kullanıcı var mı kontrol et
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
                
            // Kullanıcının lisanslarını al
            var licenses = await _context.Licenses
                .Where(l => l.UserId == userId)
                .Include(l => l.User)
                .OrderByDescending(l => l.IssueDate)
                .ToListAsync();
                
            return licenses.Select(MapToLicenseDto);
        }
        
        public async Task<LicenseDto> DeactivateLicenseAsync(int id, DeactivateLicenseDto model)
        {
            var license = await _context.Licenses.FindAsync(id);
            if (license == null)
                throw new Exception("Lisans bulunamadı.");
                
            // Lisansı devre dışı bırak
            license.IsActive = false;
            
            // Deaktif etme sebebini notlara ekle
            if (!string.IsNullOrEmpty(model.DeactivationReason))
            {
                license.Notes = string.IsNullOrEmpty(license.Notes)
                    ? $"Devre dışı bırakılma sebebi: {model.DeactivationReason}"
                    : $"{license.Notes} | Devre dışı bırakılma sebebi: {model.DeactivationReason}";
            }
            
            await _context.SaveChangesAsync();
            
            return await GetLicenseWithUserDetailsAsync(id);
        }
        
        public async Task<bool> DeleteLicenseAsync(int id)
        {
            var license = await _context.Licenses.FindAsync(id);
            if (license == null)
                return false;
                
            _context.Licenses.Remove(license);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<LicenseDto> GetLicenseByIdAsync(int id)
        {
            return await GetLicenseWithUserDetailsAsync(id);
        }
        
        public async Task<IEnumerable<LicenseDto>> GetAllLicensesAsync()
        {
            var licenses = await _context.Licenses
                .Include(l => l.User)
                .OrderByDescending(l => l.IssueDate)
                .ToListAsync();
                
            return licenses.Select(MapToLicenseDto);
        }
        
        public async Task<string> GenerateLicenseKeyAsync()
        {
            string licenseKey;
            bool exists;
            
            do
            {
                // XXXX-XXXX-XXXX-XXXX formatında benzersiz bir lisans anahtarı oluştur
                licenseKey = GenerateRandomLicenseKey();
                exists = await _context.Licenses.AnyAsync(l => l.LicenseKey == licenseKey);
            } while (exists);
            
            return licenseKey;
        }
        
        // Yardımcı metotlar
        private async Task<LicenseDto> GetLicenseWithUserDetailsAsync(int licenseId)
        {
            var license = await _context.Licenses
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == licenseId);
                
            if (license == null)
                throw new Exception("Lisans bulunamadı.");
                
            return MapToLicenseDto(license);
        }
        
        private LicenseDto MapToLicenseDto(License license)
        {
            return new LicenseDto
            {
                Id = license.Id,
                LicenseKey = license.LicenseKey,
                UserId = license.UserId,
                Username = license.User?.Username ?? "Bilinmeyen Kullanıcı",
                IssueDate = license.IssueDate,
                ExpiryDate = license.ExpiryDate,
                IsActive = license.IsActive,
                Notes = license.Notes,
                LicenseType = license.LicenseType
            };
        }
        
        private string GenerateRandomLicenseKey()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Karışıklığı önlemek için 0, 1, O, I hariç
            const int segmentLength = 4;
            const int segments = 4;
            
            using var rng = RandomNumberGenerator.Create();
            var result = new char[segments * segmentLength + (segments - 1)]; // Tire işaretleri için ekstra alan
            
            for (int i = 0; i < segments; i++)
            {
                var buffer = new byte[segmentLength];
                rng.GetBytes(buffer);
                
                for (int j = 0; j < segmentLength; j++)
                {
                    var index = buffer[j] % chars.Length;
                    result[i * (segmentLength + 1) + j] = chars[index];
                }
                
                // Segmentler arasına - işareti ekle
                if (i < segments - 1)
                    result[i * (segmentLength + 1) + segmentLength] = '-';
            }
            
            return new string(result);
        }
    }
} 