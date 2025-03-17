using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DecoderApi.DTOs;
using DecoderApi.Services;
using System.Security.Claims;

namespace DecoderApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            try
            {
                var result = await _userService.RegisterAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _userService.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("create-test-user")]
        public async Task<IActionResult> CreateTestUser([FromQuery] string? username = null, [FromQuery] string? email = null)
        {
            try
            {
                // Varsayılan değerleri oluştur
                username ??= $"test.user.{DateTime.Now.Ticks}";
                email ??= $"{username}@test.com";
                
                var testUser = new RegisterUserDto
                {
                    Username = username,
                    Email = email,
                    Password = "Test123!",
                    FullName = "Test Kullanıcısı",
                    PhoneNumber = "05551234567"
                };
                
                var result = await _userService.RegisterAsync(testUser);
                return Ok(new 
                { 
                    message = "Test kullanıcısı başarıyla oluşturuldu", 
                    user = result.User,
                    token = result.Token,
                    expiration = result.Expiration,
                    credentials = new { username = testUser.Username, password = "Test123!" }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _userService.GetProfileAsync(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var isAdmin = User.FindFirst(ClaimTypes.Role)?.Value == "Admin";
                
                // Sadece kendi hesabını veya admin ise herhangi bir hesabı güncelleyebilir
                if (userId != id && !isAdmin)
                    return Forbid();
                    
                var user = await _userService.UpdateAsync(id, model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                
                if (result)
                    return Ok(new { message = "Kullanıcı başarıyla silindi." });
                else
                    return NotFound(new { message = "Kullanıcı bulunamadı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 