using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecoderApi.Models
{
    public class EcuModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Manufacturer { get; set; } = null!;
        
        [Required]
        [StringLength(50)]
        public string ModelName { get; set; } = null!;
        
        [StringLength(50)]
        public string? HardwareVersion { get; set; }
        
        [StringLength(50)]
        public string? SoftwareVersion { get; set; }
        
        [StringLength(255)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // İlişkiler
        public virtual ICollection<VehicleEcuModel> VehicleEcuModels { get; set; } = new List<VehicleEcuModel>();
        public virtual ICollection<TuneCatalog> TuneCatalogs { get; set; } = new List<TuneCatalog>();
    }
} 