using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DecoderApi.DTOs
{
    public class CreateTuneCatalogDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        public int EcuModelId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Version { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
        
        [Required]
        public IFormFile File { get; set; }
    }
} 