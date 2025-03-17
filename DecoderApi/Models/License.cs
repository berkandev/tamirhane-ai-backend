using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecoderApi.Models
{
    public class License
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LicenseKey { get; set; } = null!;
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        
        public DateTime ExpiryDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(255)]
        public string? Notes { get; set; }
        
        [StringLength(50)]
        public string LicenseType { get; set; } = "Standard";
    }
} 