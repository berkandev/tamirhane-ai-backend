using System.ComponentModel.DataAnnotations;

namespace DecoderApi.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;
        
        [StringLength(50)]
        public string? FullName { get; set; }
        
        [StringLength(15)]
        [Phone]
        public string? PhoneNumber { get; set; }
    }
    
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = null!;
        
        [Required]
        public string Password { get; set; } = null!;
    }
    
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
    
    public class UpdateUserDto
    {
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(50)]
        public string? FullName { get; set; }
        
        [StringLength(15)]
        [Phone]
        public string? PhoneNumber { get; set; }
        
        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }
    }
    
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public UserDto User { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
} 