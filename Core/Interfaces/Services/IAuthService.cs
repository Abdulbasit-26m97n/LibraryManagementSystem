using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(UserDto userDto);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto user);
        Task<User?> RegisterAsync(UserDto userDto);
    }
}
