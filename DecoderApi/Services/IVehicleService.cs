using DecoderApi.DTOs;
using DecoderApi.Models;

namespace DecoderApi.Services
{
    public interface IVehicleService
    {
        Task<Vehicle> CreateVehicleAsync(CreateVehicleDto model);
        Task<List<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(int id);
        Task<Vehicle> UpdateVehicleAsync(int id, UpdateVehicleDto model);
        Task<bool> DeleteVehicleAsync(int id);
    }
} 