using System.ComponentModel.DataAnnotations;

namespace DecoderApi.DTOs
{
    public class CreateEcuModelDto
    {
        [Required]
        [StringLength(50)]
        public string Manufacturer { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ModelName { get; set; }
        
        [StringLength(50)]
        public string HardwareVersion { get; set; }
        
        [StringLength(50)]
        public string SoftwareVersion { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
    }
} 