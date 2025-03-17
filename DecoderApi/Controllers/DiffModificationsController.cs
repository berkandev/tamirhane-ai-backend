using DecoderApi.DTOs;
using DecoderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DecoderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiffModificationsController : ControllerBase
    {
        private readonly IDiffModificationService _diffModificationService;

        public DiffModificationsController(IDiffModificationService diffModificationService)
        {
            _diffModificationService = diffModificationService;
        }

        // POST: api/DiffModifications
        [HttpPost]
        public async Task<IActionResult> CreateDiffModification([FromBody] CreateDiffModificationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // JSON formatının geçerli olduğunu kontrol et
                try
                {
                    JsonDocument.Parse(model.OriginalDataJson);
                    JsonDocument.Parse(model.ModifiedDataJson);
                }
                catch (JsonException ex)
                {
                    return BadRequest($"Geçersiz JSON formatı: {ex.Message}");
                }

                var diffModification = await _diffModificationService.CreateDiffModificationAsync(model);
                return CreatedAtAction(nameof(GetDiffModificationById), new { id = diffModification.Id }, diffModification);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"İç sunucu hatası: {ex.Message}");
            }
        }

        // GET: api/DiffModifications/TuneCatalog/5
        [HttpGet("TuneCatalog/{tuneCatalogId}")]
        public async Task<IActionResult> GetDiffModificationsByTuneCatalog(int tuneCatalogId)
        {
            try
            {
                var diffModifications = await _diffModificationService.GetDiffModificationsByTuneCatalogAsync(tuneCatalogId);
                return Ok(diffModifications);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"İç sunucu hatası: {ex.Message}");
            }
        }

        // GET: api/DiffModifications/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiffModificationById(int id)
        {
            try
            {
                var diffModification = await _diffModificationService.GetDiffModificationByIdAsync(id);
                return Ok(diffModification);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"İç sunucu hatası: {ex.Message}");
            }
        }

        // POST: api/DiffModifications/PreviewJson
        [HttpPost("PreviewJson")]
        public IActionResult PreviewJson([FromBody] JsonPreviewDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalJson = JsonDocument.Parse(model.JsonContent);
                return Ok(new { isValid = true, parsed = originalJson });
            }
            catch (JsonException ex)
            {
                return BadRequest(new { isValid = false, error = ex.Message });
            }
        }

        // DELETE: api/DiffModifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiffModification(int id)
        {
            try
            {
                var result = await _diffModificationService.DeleteDiffModificationAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound($"Belirtilen ID'ye ({id}) sahip HEX modifikasyonu bulunamadı.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"İç sunucu hatası: {ex.Message}");
            }
        }
    }

    public class JsonPreviewDto
    {
        public string JsonContent { get; set; }
    }
}