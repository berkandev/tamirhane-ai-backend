using DecoderApi.DTOs;
using DecoderApi.Models;

namespace DecoderApi.Services
{
    public interface IVehicleEcuModelService
    {
        Task<VehicleEcuModel> AssignEcuModelToVehicleAsync(AssignEcuModelToVehicleDto model);
        Task<List<VehicleEcuModel>> GetVehicleEcuModelsByVehicleIdAsync(int vehicleId);
    }
} 