using System.ComponentModel.DataAnnotations;

namespace DecoderApi.DTOs
{
    public class CreateDiffModificationDto
    {
        [Required]
        public int TuneCatalogId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
        
        [Required]
        public long OffsetAddress { get; set; }
        
        [Required]
        public string OriginalDataJson { get; set; }
        
        [Required]
        public string ModifiedDataJson { get; set; }
    }
} 