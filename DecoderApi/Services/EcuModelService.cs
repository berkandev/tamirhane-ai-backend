using DecoderApi.Data;
using DecoderApi.DTOs;
using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DecoderApi.Services
{
    public class EcuModelService : IEcuModelService
    {
        private readonly ApplicationDbContext _context;

        public EcuModelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EcuModel> CreateEcuModelAsync(CreateEcuModelDto model)
        {
            var ecuModel = new EcuModel
            {
                Manufacturer = model.Manufacturer,
                ModelName = model.ModelName,
                HardwareVersion = model.HardwareVersion,
                SoftwareVersion = model.SoftwareVersion,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.EcuModels.Add(ecuModel);
            await _context.SaveChangesAsync();
            return ecuModel;
        }

        public async Task<List<EcuModel>> GetAllEcuModelsAsync()
        {
            return await _context.EcuModels.ToListAsync();
        }

        public async Task<EcuModel> GetEcuModelByIdAsync(int id)
        {
            var ecuModel = await _context.EcuModels.FindAsync(id);
            if (ecuModel == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({id}) sahip ECU modeli bulunamadı.");
            }
            return ecuModel;
        }

        public async Task<bool> DeleteEcuModelAsync(int id)
        {
            var ecuModel = await _context.EcuModels.FindAsync(id);
            if (ecuModel == null)
            {
                return false;
            }

            // İlişkili TuneCatalog kayıtlarını kontrol et
            var hasTuneCatalogs = await _context.TuneCatalogs.AnyAsync(tc => tc.EcuModelId == id);
            if (hasTuneCatalogs)
            {
                throw new InvalidOperationException("Bu ECU modeline ait yazılım güncellemeleri bulunduğu için silinemez.");
            }

            _context.EcuModels.Remove(ecuModel);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 