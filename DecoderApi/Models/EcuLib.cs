using System.ComponentModel.DataAnnotations;

namespace DecoderApi.Models
{
    public class EcuLib
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string SoftwareNumber { get; set; }
        
        [Required]
        [StringLength(100)]
        public string UpgradeNumber { get; set; }
        
        public string Description { get; set; }
        
        public ICollection<TuneMaps> TuneMaps { get; set; }
        
        public ICollection<EcuVehicleEcuLib> EcuVehicleEcuLibs { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}