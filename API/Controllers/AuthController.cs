using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User?>> Register(UserDto request)
        {
            var response = await authService.RegisterAsync(request);
            if(response == null)
            {
                return BadRequest("User already exists.");
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
           var response = await authService.LoginAsync(request);
            if(response == null)
            {
                return BadRequest("Incorrect username or password");
            }
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var response = await authService.RefreshTokenAsync(request);
            if (response == null)
            {
                return BadRequest("Invalid or expired token");
            }
            return Ok(response);
        }
    }
}
