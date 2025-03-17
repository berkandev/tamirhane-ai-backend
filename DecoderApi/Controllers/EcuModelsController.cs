using DecoderApi.DTOs;
using DecoderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoderApi.Controllers
{
    [ApiController]
    [Route("api/ecu_models")]
    [Authorize]
    public class EcuModelsController : ControllerBase
    {
        private readonly IEcuModelService _ecuModelService;

        public EcuModelsController(IEcuModelService ecuModelService)
        {
            _ecuModelService = ecuModelService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEcuModel([FromBody] CreateEcuModelDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var ecuModel = await _ecuModelService.CreateEcuModelAsync(model);
                return CreatedAtAction(nameof(GetEcuModelById), new { id = ecuModel.Id }, ecuModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"ECU modeli oluşturulurken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEcuModels()
        {
            try
            {
                var ecuModels = await _ecuModelService.GetAllEcuModelsAsync();
                return Ok(ecuModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"ECU modelleri getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEcuModelById(int id)
        {
            try
            {
                var ecuModel = await _ecuModelService.GetEcuModelByIdAsync(id);
                return Ok(ecuModel);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"ECU modeli getirilirken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEcuModel(int id)
        {
            try
            {
                var result = await _ecuModelService.DeleteEcuModelAsync(id);
                if (result)
                    return NoContent();
                else
                    return NotFound(new { message = $"Belirtilen ID'ye ({id}) sahip ECU modeli bulunamadı." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"ECU modeli silinirken bir hata oluştu: {ex.Message}" });
            }
        }
    }
} 