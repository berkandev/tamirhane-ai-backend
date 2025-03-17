using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecoderApi.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Make { get; set; } = null!;
        
        [Required]
        [StringLength(50)]
        public string Model { get; set; } = null!;
        
        [Required]
        [StringLength(4)]
        public string Year { get; set; } = null!;
        
        [StringLength(20)]
        public string? Generation { get; set; }
        
        [StringLength(20)]
        public string? Engine { get; set; }
        
        [StringLength(255)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // İlişkiler
        public virtual ICollection<VehicleEcuModel> VehicleEcuModels { get; set; } = new List<VehicleEcuModel>();
    }
} 