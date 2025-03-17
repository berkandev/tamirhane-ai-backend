using System.ComponentModel.DataAnnotations;

namespace DecoderApi.DTOs
{
    public class CreateLicenseDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LicenseKey { get; set; } = null!;
        
        [Required]
        public DateTime ExpiryDate { get; set; }
        
        [StringLength(255)]
        public string? Notes { get; set; }
        
        [StringLength(50)]
        public string LicenseType { get; set; } = "Standard";
    }
    
    public class LicenseDto
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; } = null!;
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public string LicenseType { get; set; } = null!;
        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public int DaysRemaining => IsExpired ? 0 : (int)(ExpiryDate - DateTime.UtcNow).TotalDays;
    }
    
    public class DeactivateLicenseDto
    {
        [StringLength(255)]
        public string? DeactivationReason { get; set; }
    }
} 