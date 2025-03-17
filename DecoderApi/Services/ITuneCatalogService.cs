using DecoderApi.DTOs;
using DecoderApi.Models;

namespace DecoderApi.Services
{
    public interface ITuneCatalogService
    {
        Task<TuneCatalog> CreateTuneCatalogAsync(CreateTuneCatalogDto model);
        Task<List<TuneCatalog>> GetTuneCatalogsByEcuModelAsync(int ecuModelId);
        Task<TuneCatalog> GetTuneCatalogByIdAsync(int id);
        Task<bool> DeleteTuneCatalogAsync(int id);
    }
} 