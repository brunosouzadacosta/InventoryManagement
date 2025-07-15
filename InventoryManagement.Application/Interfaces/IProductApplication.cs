using InventoryManagement.Application.ViewModels;

namespace InventoryManagement.Application.Interfaces
{
    public interface IProductApplication
    {
        Task<IEnumerable<vmProduct>> GetAllAsync();
        Task<vmProduct?> GetByIdAsync(int productId);
        Task AddAsync(vmProduct product);
        Task<bool> UpdateAsync(int productId, vmProduct product);
        Task<bool> DeleteAsync(int productId);
    }
}