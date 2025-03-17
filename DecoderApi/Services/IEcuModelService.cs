using DecoderApi.DTOs;
using DecoderApi.Models;

namespace DecoderApi.Services
{
    public interface IEcuModelService
    {
        Task<EcuModel> CreateEcuModelAsync(CreateEcuModelDto model);
        Task<List<EcuModel>> GetAllEcuModelsAsync();
        Task<EcuModel> GetEcuModelByIdAsync(int id);
        Task<bool> DeleteEcuModelAsync(int id);
    }
} 