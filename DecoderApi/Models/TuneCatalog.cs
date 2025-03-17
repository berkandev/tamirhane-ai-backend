using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecoderApi.Models
{
    public class TuneCatalog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        
        public int EcuModelId { get; set; }
        
        [ForeignKey("EcuModelId")]
        public virtual EcuModel EcuModel { get; set; } = null!;
        
        [StringLength(50)]
        public string Version { get; set; } = "1.0";
        
        [StringLength(255)]
        public string? Description { get; set; }
        
        [Column(TypeName = "bytea")]
        public byte[]? FileData { get; set; }
        
        [StringLength(100)]
        public string? FileName { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // İlişkiler
        public virtual ICollection<DiffModification> DiffModifications { get; set; } = new List<DiffModification>();
    }
} 