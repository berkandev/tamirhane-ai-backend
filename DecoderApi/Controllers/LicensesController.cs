using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DecoderApi.DTOs;
using DecoderApi.Services;

namespace DecoderApi.Controllers
{
    [ApiController]
    [Route("api/licenses")]
    [Authorize]
    public class LicensesController : ControllerBase
    {
        private readonly ILicenseService _licenseService;
        
        public LicensesController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }
        
        [HttpPost("assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignLicense([FromBody] CreateLicenseDto model)
        {
            try
            {
                var license = await _licenseService.AssignLicenseAsync(model);
                return Ok(license);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("generate-key")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateLicenseKey()
        {
            try
            {
                var licenseKey = await _licenseService.GenerateLicenseKeyAsync();
                return Ok(new { licenseKey });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserLicenses(int userId)
        {
            try
            {
                var licenses = await _licenseService.GetUserLicensesAsync(userId);
                return Ok(licenses);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetLicenseById(int id)
        {
            try
            {
                var license = await _licenseService.GetLicenseByIdAsync(id);
                return Ok(license);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllLicenses()
        {
            try
            {
                var licenses = await _licenseService.GetAllLicensesAsync();
                return Ok(licenses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateLicense(int id, [FromBody] DeactivateLicenseDto model)
        {
            try
            {
                var license = await _licenseService.DeactivateLicenseAsync(id, model);
                return Ok(license);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            try
            {
                var result = await _licenseService.DeleteLicenseAsync(id);
                
                if (result)
                    return Ok(new { message = "Lisans başarıyla silindi." });
                else
                    return NotFound(new { message = "Lisans bulunamadı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 