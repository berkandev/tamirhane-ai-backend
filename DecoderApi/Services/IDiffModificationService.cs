using DecoderApi.DTOs;
using DecoderApi.Models;

namespace DecoderApi.Services
{
    public interface IDiffModificationService
    {
        Task<DiffModification> CreateDiffModificationAsync(CreateDiffModificationDto model);
        Task<List<DiffModification>> GetDiffModificationsByTuneCatalogAsync(int tuneCatalogId);
        Task<DiffModification> GetDiffModificationByIdAsync(int id);
        Task<bool> DeleteDiffModificationAsync(int id);
    }
} 