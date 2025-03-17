using DecoderApi.Models;

namespace DecoderApi.Services
{
    public interface IApiLogService
    {
        Task<ApiLog> CreateLogAsync(ApiLog log);
        Task<List<ApiLog>> GetLastLogsAsync(int count = 50);
        Task<List<ApiLog>> GetLogsByUserIdAsync(int userId);
        Task<List<ApiLog>> GetErrorLogsAsync();
        Task<bool> CleanupOldLogsAsync(int daysToKeep = 30);
    }
} 