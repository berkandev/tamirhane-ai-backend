using System.ComponentModel.DataAnnotations;

namespace DecoderApi.Models
{
    public class TuneMaps
    {
        public int Id { get; set; }
        
        public int TuneCatalogId { get; set; }
        public TuneCatalog TuneCatalog { get; set; }
        
        public int EcuLibId { get; set; }
        public EcuLib EcuLib { get; set; }
        
        [Required]
        public string MapName { get; set; }
        
        public string Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}