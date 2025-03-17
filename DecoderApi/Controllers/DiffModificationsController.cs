using DecoderApi.DTOs;
using DecoderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DecoderApi.Controllers
{
    [ApiController]
    [Route("api/diff_modifications")]
    [Authorize]
    public class DiffModificationsController : ControllerBase
    {
        private readonly IDiffModificationService _diffModificationService;

        public DiffModificationsController(IDiffModificationService diffModificationService)
        {
            _diffModificationService = diffModificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiffModification([FromBody] CreateDiffModificationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var diffModification = await _diffModificationService.CreateDiffModificationAsync(model);
                return CreatedAtAction(nameof(GetDiffModificationById), new { id = diffModification.Id }, new
                {
                    id = diffModification.Id,
                    tuneCatalogId = diffModification.TuneCatalogId,
                    name = diffModification.Name,
                    description = diffModification.Description,
                    offsetAddress = diffModification.OffsetAddress,
                    createdAt = diffModification.CreatedAt,
                    updatedAt = diffModification.UpdatedAt
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (JsonException)
            {
                return BadRequest(new { message = "Geçersiz JSON formatı. Lütfen geçerli bir JSON formatı sağlayın." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"HEX modifikasyonu oluşturulurken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("tune/{tuneCatalogId}")]
        public async Task<IActionResult> GetDiffModificationsByTuneCatalog(int tuneCatalogId)
        {
            try
            {
                var diffModifications = await _diffModificationService.GetDiffModificationsByTuneCatalogAsync(tuneCatalogId);
                
                // JSON verilerini ön inceleme için hazırla
                var result = diffModifications.Select(dm => new
                {
                    id = dm.Id,
                    tuneCatalogId = dm.TuneCatalogId,
                    name = dm.Name,
                    description = dm.Description,
                    offsetAddress = dm.OffsetAddress,
                    originalDataPreview = GetJsonPreview(dm.OriginalDataJson),
                    modifiedDataPreview = GetJsonPreview(dm.ModifiedDataJson),
                    createdAt = dm.CreatedAt,
                    updatedAt = dm.UpdatedAt
                });
                
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"HEX modifikasyonları getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiffModificationById(int id)
        {
            try
            {
                var diffModification = await _diffModificationService.GetDiffModificationByIdAsync(id);
                
                return Ok(new
                {
                    id = diffModification.Id,
                    tuneCatalogId = diffModification.TuneCatalogId,
                    name = diffModification.Name,
                    description = diffModification.Description,
                    offsetAddress = diffModification.OffsetAddress,
                    originalDataJson = diffModification.OriginalDataJson,
                    modifiedDataJson = diffModification.ModifiedDataJson,
                    createdAt = diffModification.CreatedAt,
                    updatedAt = diffModification.UpdatedAt
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"HEX modifikasyonu getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiffModification(int id)
        {
            try
            {
                var result = await _diffModificationService.DeleteDiffModificationAsync(id);
                if (result)
                    return NoContent();
                else
                    return NotFound(new { message = $"Belirtilen ID'ye ({id}) sahip HEX modifikasyonu bulunamadı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"HEX modifikasyonu silinirken bir hata oluştu: {ex.Message}" });
            }
        }
        
        // JSON verisinden ön izleme bilgisi oluşturmak için yardımcı metod
        private string GetJsonPreview(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
                return "Veri yok";
                
            try 
            {
                var doc = JsonDocument.Parse(jsonData);
                int length = jsonData.Length;
                return $"JSON veri - {length} karakter";
            }
            catch
            {
                return "Geçersiz JSON formatı";
            }
        }
    }
} 