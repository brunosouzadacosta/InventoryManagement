using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using InventoryManagement.Application.Interfaces;

namespace InventoryManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;

        public AuthController(IAuthApplication authApplication)
        {
            _authApplication = authApplication;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var token = await _authApplication.RegisterAsync(request.Username, request.Email, request.Password, request.Role);
            return Ok(new { token });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var token = await _authApplication.LoginAsync(request.Username, request.Password);
            return Ok(new { token });
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest request)
        {
            var token = await _authApplication.RefreshTokenAsync(request.Username, request.RefreshToken);
            return Ok(new { token });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _authApplication.LogoutAsync(request.Username);
            return NoContent();
        }
    }

    public record RegisterRequest(string Username, string Email, string Password, string Role);
    public record LoginRequest(string Username, string Password);
    public record RefreshRequest(string Username, string RefreshToken);
    public record LogoutRequest(string Username);

}
