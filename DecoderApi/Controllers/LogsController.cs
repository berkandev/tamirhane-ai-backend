using DecoderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoderApi.Controllers
{
    [ApiController]
    [Route("api/logs")]
    [Authorize(Roles = "Admin")] // Sadece adminler log yönetebilir
    public class LogsController : ControllerBase
    {
        private readonly IApiLogService _apiLogService;

        public LogsController(IApiLogService apiLogService)
        {
            _apiLogService = apiLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastLogs()
        {
            try
            {
                var logs = await _apiLogService.GetLastLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Loglar getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLogsByUser(int userId)
        {
            try
            {
                var logs = await _apiLogService.GetLogsByUserIdAsync(userId);
                return Ok(logs);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Kullanıcı logları getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("errors")]
        public async Task<IActionResult> GetErrorLogs()
        {
            try
            {
                var logs = await _apiLogService.GetErrorLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hata logları getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpDelete("cleanup")]
        public async Task<IActionResult> CleanupLogs([FromQuery] int daysToKeep = 30)
        {
            try
            {
                var result = await _apiLogService.CleanupOldLogsAsync(daysToKeep);
                if (result)
                    return Ok(new { message = $"Başarıyla {daysToKeep} günden eski loglar temizlendi." });
                else
                    return Ok(new { message = "Temizlenecek eski log bulunamadı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Loglar temizlenirken bir hata oluştu: {ex.Message}" });
            }
        }
    }
} 