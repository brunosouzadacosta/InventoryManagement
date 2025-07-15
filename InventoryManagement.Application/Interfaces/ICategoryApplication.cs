using InventoryManagement.Application.ViewModels;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.Application.Interfaces
{
    public interface ICategoryApplication
    {
        Task<IEnumerable<vmCategory>> GetAllAsync();
        Task<vmCategory?> GetByIdAsync(int categoryId);
        Task AddAsync(vmCategory category);
        Task<bool> UpdateAsync(int categoryId, vmCategory category);
        Task<bool> DeleteAsync(int categoryId);
    }
}
