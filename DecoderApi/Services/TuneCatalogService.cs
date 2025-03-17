using DecoderApi.Data;
using DecoderApi.DTOs;
using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace DecoderApi.Services
{
    public class TuneCatalogService : ITuneCatalogService
    {
        private readonly ApplicationDbContext _context;

        public TuneCatalogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TuneCatalog> CreateTuneCatalogAsync(CreateTuneCatalogDto model)
        {
            // ECU modelinin var olup olmadığını kontrol et
            var ecuModel = await _context.EcuModels.FindAsync(model.EcuModelId);
            if (ecuModel == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({model.EcuModelId}) sahip ECU modeli bulunamadı.");
            }

            // Dosyayı belleğe yükle
            using var memoryStream = new MemoryStream();
            await model.File.CopyToAsync(memoryStream);

            var tuneCatalog = new TuneCatalog
            {
                Name = model.Name,
                EcuModelId = model.EcuModelId,
                Version = model.Version,
                Description = model.Description,
                FileName = model.File.FileName,
                FileData = memoryStream.ToArray(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.TuneCatalogs.Add(tuneCatalog);
            await _context.SaveChangesAsync();
            return tuneCatalog;
        }

        public async Task<List<TuneCatalog>> GetTuneCatalogsByEcuModelAsync(int ecuModelId)
        {
            // ECU modelinin var olup olmadığını kontrol et
            var ecuModel = await _context.EcuModels.FindAsync(ecuModelId);
            if (ecuModel == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({ecuModelId}) sahip ECU modeli bulunamadı.");
            }

            return await _context.TuneCatalogs
                .Where(tc => tc.EcuModelId == ecuModelId)
                .ToListAsync();
        }

        public async Task<TuneCatalog> GetTuneCatalogByIdAsync(int id)
        {
            var tuneCatalog = await _context.TuneCatalogs.FindAsync(id);
            if (tuneCatalog == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({id}) sahip yazılım güncellemesi bulunamadı.");
            }
            return tuneCatalog;
        }

        public async Task<bool> DeleteTuneCatalogAsync(int id)
        {
            var tuneCatalog = await _context.TuneCatalogs.FindAsync(id);
            if (tuneCatalog == null)
            {
                return false;
            }

            // İlişkili DiffModification kayıtlarını kontrol et
            var hasDiffModifications = await _context.DiffModifications.AnyAsync(dm => dm.TuneCatalogId == id);
            if (hasDiffModifications)
            {
                throw new InvalidOperationException("Bu yazılım güncellemesine ait HEX modifikasyonları bulunduğu için silinemez.");
            }

            _context.TuneCatalogs.Remove(tuneCatalog);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 