using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService(IJwtService jwtService, IUserRepository userRepository) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserDto userDto)
        {
            if (await userRepository.UserExistsAsync(userDto.Username))
            {
                Debug.WriteLine("--------------------------");
                return null;
            }
            Debug.WriteLine("=================================");
            var user = new User();
            user.Id = Guid.NewGuid();
            user.Username = userDto.Username;
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, userDto.Password);
            user.Role = "Customer";
            await userRepository.AddUserAsync(user);

            return user;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto userDto)
        {
            var user = await userRepository.GetUserAsync(userDto.Username);
            if (user == null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userDto.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }
            var token = new TokenResponseDto(jwtService.GenerateToken(user), jwtService.GenerateRefreshToken(user));
            await userRepository.SaveRefreshTokenAsync();
            return token;
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto userDto)
        {
            var user = await userRepository.GetUserByIdAsync(userDto.UserId);
            if(user == null || user.RefreshToken != userDto.RefreshToken || user.RefreshTokenExpiry<DateTime.UtcNow)
            {
                return null;
            }
            var token = new TokenResponseDto(jwtService.GenerateToken(user), jwtService.GenerateRefreshToken(user));
            await userRepository.SaveRefreshTokenAsync();
            return token;
        }
    }
}
