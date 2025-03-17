using DecoderApi.DTOs;
using DecoderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoderApi.Controllers
{
    [ApiController]
    [Route("api/vehicle_ecumodel")]
    [Authorize]
    public class VehicleEcuModelsController : ControllerBase
    {
        private readonly IVehicleEcuModelService _vehicleEcuModelService;

        public VehicleEcuModelsController(IVehicleEcuModelService vehicleEcuModelService)
        {
            _vehicleEcuModelService = vehicleEcuModelService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignEcuModelToVehicle([FromBody] AssignEcuModelToVehicleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var relation = await _vehicleEcuModelService.AssignEcuModelToVehicleAsync(model);
                return CreatedAtAction(nameof(GetVehicleEcuModels), new { vehicleId = relation.VehicleId }, relation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"ECU modeli araca atanırken bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("{vehicleId}")]
        public async Task<IActionResult> GetVehicleEcuModels(int vehicleId)
        {
            try
            {
                var relations = await _vehicleEcuModelService.GetVehicleEcuModelsByVehicleIdAsync(vehicleId);
                return Ok(relations);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Araç-ECU modelleri getirilirken bir hata oluştu: {ex.Message}" });
            }
        }
    }
} 