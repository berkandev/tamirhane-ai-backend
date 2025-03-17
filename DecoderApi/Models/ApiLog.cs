using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecoderApi.Models
{
    public class ApiLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(100)]
        public string? Endpoint { get; set; }
        
        [StringLength(10)]
        public string? HttpMethod { get; set; }
        
        public int? UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        
        [StringLength(50)]
        public string? IpAddress { get; set; }
        
        public int? StatusCode { get; set; }
        
        [StringLength(4000)]
        public string? RequestBody { get; set; }
        
        [StringLength(4000)]
        public string? ResponseBody { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public long? ExecutionTimeMs { get; set; }
    }
} 