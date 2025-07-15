using InventoryManagement.Application.ViewModels;

namespace InventoryManagement.Application.Interfaces
{
    public interface IAuthApplication
    {
        Task<vmAuthResult> RegisterAsync(string username, string email, string password, string role);
        Task<vmAuthResult> LoginAsync(string username, string password);
        Task<vmAuthResult> RefreshTokenAsync(string username, string refreshToken);
        Task LogoutAsync(string username);
    }

}
