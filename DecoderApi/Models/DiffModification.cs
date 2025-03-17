using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecoderApi.Models
{
    public class DiffModification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int TuneCatalogId { get; set; }
        
        [ForeignKey("TuneCatalogId")]
        public virtual TuneCatalog TuneCatalog { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        
        [StringLength(255)]
        public string? Description { get; set; }
        
        [Required]
        public long OffsetAddress { get; set; }
        
        [Column(TypeName = "jsonb")]
        public string? OriginalDataJson { get; set; }
        
        [Column(TypeName = "jsonb")]
        public string? ModifiedDataJson { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}