using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecoderApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; } = null!;
        
        [StringLength(50)]
        public string? FullName { get; set; }
        
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        
        public bool IsAdmin { get; set; } = false;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        // İlişkiler
        public virtual ICollection<License> Licenses { get; set; } = new List<License>();
    }
} 