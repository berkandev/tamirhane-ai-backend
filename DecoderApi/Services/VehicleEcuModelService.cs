using DecoderApi.Data;
using DecoderApi.DTOs;
using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DecoderApi.Services
{
    public class VehicleEcuModelService : IVehicleEcuModelService
    {
        private readonly ApplicationDbContext _context;

        public VehicleEcuModelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleEcuModel> AssignEcuModelToVehicleAsync(AssignEcuModelToVehicleDto model)
        {
            // Araç ve ECU modelinin var olup olmadığını kontrol et
            var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({model.VehicleId}) sahip araç bulunamadı.");
            }

            var ecuModel = await _context.EcuModels.FindAsync(model.EcuModelId);
            if (ecuModel == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({model.EcuModelId}) sahip ECU modeli bulunamadı.");
            }

            // Bu ilişki zaten var mı kontrol et
            var existingRelation = await _context.VehicleEcuModels.FirstOrDefaultAsync(
                ve => ve.VehicleId == model.VehicleId && ve.EcuModelId == model.EcuModelId);

            if (existingRelation != null)
            {
                throw new InvalidOperationException("Bu araç ve ECU modeli arasında zaten bir ilişki mevcut.");
            }

            var vehicleEcuModel = new VehicleEcuModel
            {
                VehicleId = model.VehicleId,
                EcuModelId = model.EcuModelId,
                Notes = model.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.VehicleEcuModels.Add(vehicleEcuModel);
            await _context.SaveChangesAsync();
            return vehicleEcuModel;
        }

        public async Task<List<VehicleEcuModel>> GetVehicleEcuModelsByVehicleIdAsync(int vehicleId)
        {
            // Önce aracın var olup olmadığını kontrol et
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({vehicleId}) sahip araç bulunamadı.");
            }

            return await _context.VehicleEcuModels
                .Include(ve => ve.EcuModel)
                .Where(ve => ve.VehicleId == vehicleId)
                .ToListAsync();
        }
    }
} 