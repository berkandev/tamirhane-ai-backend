using DecoderApi.Data;
using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DecoderApi.Services
{
    public class ApiLogService : IApiLogService
    {
        private readonly ApplicationDbContext _context;

        public ApiLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiLog> CreateLogAsync(ApiLog log)
        {
            _context.ApiLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<List<ApiLog>> GetLastLogsAsync(int count = 50)
        {
            return await _context.ApiLogs
                .OrderByDescending(l => l.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<ApiLog>> GetLogsByUserIdAsync(int userId)
        {
            // Kullanıcı var mı kontrol et
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Belirtilen ID'ye ({userId}) sahip kullanıcı bulunamadı.");
            }

            return await _context.ApiLogs
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ApiLog>> GetErrorLogsAsync()
        {
            return await _context.ApiLogs
                .Where(l => l.StatusCode >= 500 || l.StatusCode == 0) // 0 for uncaught exceptions
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> CleanupOldLogsAsync(int daysToKeep = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);
            
            var oldLogs = await _context.ApiLogs
                .Where(l => l.CreatedAt < cutoffDate)
                .ToListAsync();

            if (oldLogs.Any())
            {
                _context.ApiLogs.RemoveRange(oldLogs);
                await _context.SaveChangesAsync();
                return true;
            }
            
            return false;
        }
    }
} 