using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DecoderApi.Data;
using DecoderApi.DTOs;
using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DecoderApi.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        
        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto model)
        {
            // Kullanıcı adı veya e-posta zaten kullanılıyor mu kontrol et
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                throw new Exception("Bu kullanıcı adı zaten kullanılıyor.");
                
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                throw new Exception("Bu e-posta adresi zaten kullanılıyor.");
                
            // Şifreyi hashle
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            
            // Yeni kullanıcı oluştur
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsAdmin = false
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            // JWT token oluştur
            var token = GenerateJwtToken(user);
            
            return new AuthResponseDto
            {
                Token = token.token,
                User = MapToUserDto(user),
                Expiration = token.expiration
            };
        }
        
        public async Task<AuthResponseDto> LoginAsync(LoginDto model)
        {
            // Kullanıcıyı bul
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);
                
            if (user == null)
                throw new Exception("Kullanıcı adı veya şifre hatalı.");
                
            // Şifreyi doğrula
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new Exception("Kullanıcı adı veya şifre hatalı.");
                
            // Kullanıcı aktif mi kontrol et
            if (!user.IsActive)
                throw new Exception("Bu hesap devre dışı bırakılmış.");
                
            // Son giriş zamanını güncelle
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            // JWT token oluştur
            var token = GenerateJwtToken(user);
            
            return new AuthResponseDto
            {
                Token = token.token,
                User = MapToUserDto(user),
                Expiration = token.expiration
            };
        }
        
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
                
            return MapToUserDto(user);
        }
        
        public async Task<UserDto> GetProfileAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
                
            return MapToUserDto(user);
        }
        
        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto model)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
                
            // E-posta güncelleniyor ve başka bir kullanıcı tarafından kullanılıyor mu kontrol et
            if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                    throw new Exception("Bu e-posta adresi zaten kullanılıyor.");
                    
                user.Email = model.Email;
            }
            
            // Diğer alanları güncelle
            if (!string.IsNullOrEmpty(model.FullName))
                user.FullName = model.FullName;
                
            if (!string.IsNullOrEmpty(model.PhoneNumber))
                user.PhoneNumber = model.PhoneNumber;
                
            // Şifre güncelleniyor mu?
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                
            await _context.SaveChangesAsync();
            
            return MapToUserDto(user);
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
                return false;
                
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(MapToUserDto);
        }
        
        // Yardımcı metotlar
        private (string token, DateTime expiration) GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);
            var expirationHours = int.Parse(jwtSettings["ExpirationInHours"] ?? "24");
            var expiration = DateTime.UtcNow.AddHours(expirationHours);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
                }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return (tokenHandler.WriteToken(token), expiration);
        }
        
        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                IsAdmin = user.IsAdmin,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
} 