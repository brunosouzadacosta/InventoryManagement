using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.ViewModels;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryManagement.Services
{
    public class AuthApplication : IAuthApplication
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthApplication(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<vmAuthResult> RegisterAsync(string username, string email, string password, string role)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null) throw new Exception("Username already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var refreshToken = Guid.NewGuid().ToString();
            var refreshExpiry = DateTime.UtcNow.AddDays(7);

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = role,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshExpiry
            };

            await _userRepository.AddAsync(user);

            var accessToken = GenerateToken(user);

            return new vmAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


        public async Task<vmAuthResult> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) throw new Exception("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            var refreshToken = Guid.NewGuid().ToString();
            var refreshExpiry = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshExpiry;
            await _userRepository.UpdateAsync(user);

            var accessToken = GenerateToken(user);

            return new vmAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


        public async Task<vmAuthResult> RefreshTokenAsync(string username, string refreshToken)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new Exception("Invalid refresh token.");

            // Gera novo refresh token se quiser reforçar a segurança
            var newRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            var accessToken = GenerateToken(user);

            return new vmAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }


        public async Task LogoutAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) throw new Exception("User not found.");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userRepository.UpdateAsync(user);
        }

        private string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}