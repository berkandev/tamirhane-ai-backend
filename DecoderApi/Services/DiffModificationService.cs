using DecoderApi.Data;
using DecoderApi.DTOs;
using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DecoderApi.Services
{
    public class DiffModificationService : IDiffModificationService
    {
        private readonly ApplicationDbContext _context;

        public DiffModificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiffModification> CreateDiffModificationAsync(CreateDiffModificationDto model)
        {
            // TuneCatalog'un var olup olmadığını kontrol et
            var tuneCatalog = await _context.TuneCatalogs.FindAsync(model.TuneCatalogId);
            if (tuneCatalog == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({model.TuneCatalogId}) sahip yazılım güncellemesi bulunamadı.");
            }

            // JSON formatının geçerli olduğunu kontrol et
            try
            {
                JsonDocument.Parse(model.OriginalDataJson);
                JsonDocument.Parse(model.ModifiedDataJson);
            }
            catch (JsonException)
            {
                throw new InvalidOperationException("Geçersiz JSON formatı. Lütfen geçerli bir JSON formatı sağlayın.");
            }

            var diffModification = new DiffModification
            {
                TuneCatalogId = model.TuneCatalogId,
                Name = model.Name,
                Description = model.Description,
                OffsetAddress = model.OffsetAddress,
                OriginalDataJson = model.OriginalDataJson,
                ModifiedDataJson = model.ModifiedDataJson,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.DiffModifications.Add(diffModification);
            await _context.SaveChangesAsync();
            return diffModification;
        }

        public async Task<List<DiffModification>> GetDiffModificationsByTuneCatalogAsync(int tuneCatalogId)
        {
            // TuneCatalog'un var olup olmadığını kontrol et
            var tuneCatalog = await _context.TuneCatalogs.FindAsync(tuneCatalogId);
            if (tuneCatalog == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({tuneCatalogId}) sahip yazılım güncellemesi bulunamadı.");
            }

            return await _context.DiffModifications
                .Where(dm => dm.TuneCatalogId == tuneCatalogId)
                .ToListAsync();
        }

        public async Task<DiffModification> GetDiffModificationByIdAsync(int id)
        {
            var diffModification = await _context.DiffModifications.FindAsync(id);
            if (diffModification == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({id}) sahip HEX modifikasyonu bulunamadı.");
            }
            return diffModification;
        }

        public async Task<bool> DeleteDiffModificationAsync(int id)
        {
            var diffModification = await _context.DiffModifications.FindAsync(id);
            if (diffModification == null)
            {
                return false;
            }

            _context.DiffModifications.Remove(diffModification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 