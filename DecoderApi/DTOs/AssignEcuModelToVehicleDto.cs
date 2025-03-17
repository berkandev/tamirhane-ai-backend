using System.ComponentModel.DataAnnotations;

namespace DecoderApi.DTOs
{
    public class AssignEcuModelToVehicleDto
    {
        [Required]
        public int VehicleId { get; set; }
        
        [Required]
        public int EcuModelId { get; set; }
        
        [StringLength(255)]
        public string Notes { get; set; }
    }
} 