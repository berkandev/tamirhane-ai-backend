using DecoderApi.Data;
using DecoderApi.DTOs;
using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DecoderApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle> CreateVehicleAsync(CreateVehicleDto model)
        {
            var vehicle = new Vehicle
            {
                Make = model.Make,
                Model = model.Model,
                Year = model.Year,
                Generation = model.Generation,
                Engine = model.Engine,
                Notes = model.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({id}) sahip araç bulunamadı.");
            }
            return vehicle;
        }

        public async Task<Vehicle> UpdateVehicleAsync(int id, UpdateVehicleDto model)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({id}) sahip araç bulunamadı.");
            }

            // Sadece boş olmayan alanları güncelle
            if (!string.IsNullOrEmpty(model.Make))
                vehicle.Make = model.Make;

            if (!string.IsNullOrEmpty(model.Model))
                vehicle.Model = model.Model;

            if (!string.IsNullOrEmpty(model.Year))
                vehicle.Year = model.Year;

            if (model.Generation != null)
                vehicle.Generation = model.Generation;

            if (model.Engine != null)
                vehicle.Engine = model.Engine;

            if (model.Notes != null)
                vehicle.Notes = model.Notes;

            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return false;
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 