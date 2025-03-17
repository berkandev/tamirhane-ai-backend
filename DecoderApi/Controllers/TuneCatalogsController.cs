using DecoderApi.DTOs;
using DecoderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoderApi.Controllers
{
    [ApiController]
    [Route("api/tune_catalog")]
    [Authorize]
    public class TuneCatalogsController : ControllerBase
    {
        private readonly ITuneCatalogService _tuneCatalogService;

        public TuneCatalogsController(ITuneCatalogService tuneCatalogService)
        {
            _tuneCatalogService = tuneCatalogService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTuneCatalog([FromForm] CreateTuneCatalogDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var tuneCatalog = await _tuneCatalogService.CreateTuneCatalogAsync(model);
                return CreatedAtAction(nameof(GetTuneCatalogById), new { id = tuneCatalog.Id }, new
                {
                    id = tuneCatalog.Id,
                    name = tuneCatalog.Name,
                    version = tuneCatalog.Version,
                    ecuModelId = tuneCatalog.EcuModelId,
                    description = tuneCatalog.Description,
                    fileName = tuneCatalog.FileName,
                    createdAt = tuneCatalog.CreatedAt,
                    updatedAt = tuneCatalog.UpdatedAt
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Yazılım güncellemesi oluşturulurken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("ecu/{ecuModelId}")]
        public async Task<IActionResult> GetTuneCatalogsByEcuModel(int ecuModelId)
        {
            try
            {
                var tuneCatalogs = await _tuneCatalogService.GetTuneCatalogsByEcuModelAsync(ecuModelId);
                
                // FileData alanını döndürme
                var result = tuneCatalogs.Select(tc => new
                {
                    id = tc.Id,
                    name = tc.Name,
                    version = tc.Version,
                    ecuModelId = tc.EcuModelId,
                    description = tc.Description,
                    fileName = tc.FileName,
                    createdAt = tc.CreatedAt,
                    updatedAt = tc.UpdatedAt
                });
                
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Yazılım güncellemeleri getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTuneCatalogById(int id)
        {
            try
            {
                var tuneCatalog = await _tuneCatalogService.GetTuneCatalogByIdAsync(id);
                
                return Ok(new
                {
                    id = tuneCatalog.Id,
                    name = tuneCatalog.Name,
                    version = tuneCatalog.Version,
                    ecuModelId = tuneCatalog.EcuModelId,
                    description = tuneCatalog.Description,
                    fileName = tuneCatalog.FileName,
                    createdAt = tuneCatalog.CreatedAt,
                    updatedAt = tuneCatalog.UpdatedAt
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Yazılım güncellemesi getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadTuneCatalog(int id)
        {
            try
            {
                var tuneCatalog = await _tuneCatalogService.GetTuneCatalogByIdAsync(id);
                
                return File(tuneCatalog.FileData, "application/octet-stream", tuneCatalog.FileName);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Yazılım güncellemesi indirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTuneCatalog(int id)
        {
            try
            {
                var result = await _tuneCatalogService.DeleteTuneCatalogAsync(id);
                if (result)
                    return NoContent();
                else
                    return NotFound(new { message = $"Belirtilen ID'ye ({id}) sahip yazılım güncellemesi bulunamadı." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Yazılım güncellemesi silinirken bir hata oluştu: {ex.Message}" });
            }
        }
    }
} 