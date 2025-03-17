using DecoderApi.DTOs;
using DecoderApi.Models;

namespace DecoderApi.Services
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto model);
        Task<AuthResponseDto> LoginAsync(LoginDto model);
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> GetProfileAsync(int userId);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<UserDto>> GetAllAsync();
    }
} 