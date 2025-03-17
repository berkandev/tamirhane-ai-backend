using System.ComponentModel.DataAnnotations;

namespace DecoderApi.DTOs
{
    public class UpdateVehicleDto
    {
        [StringLength(50)]
        public string Make { get; set; }
        
        [StringLength(50)]
        public string Model { get; set; }
        
        [StringLength(4)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Yıl 4 haneli sayı olmalıdır.")]
        public string Year { get; set; }
        
        [StringLength(20)]
        public string Generation { get; set; }
        
        [StringLength(20)]
        public string Engine { get; set; }
        
        [StringLength(255)]
        public string Notes { get; set; }
    }
} 