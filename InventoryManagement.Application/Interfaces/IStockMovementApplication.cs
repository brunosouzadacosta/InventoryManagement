using InventoryManagement.Application.ViewModels;

namespace InventoryManagement.Application.Interfaces
{
    public interface IStockMovementApplication
    {
        Task<IEnumerable<vmStockMovement>> GetAllAsync();
        Task<vmStockMovement?> GetByIdAsync(int id);
        Task AddAsync(vmStockMovement stockMovement);
    }
}
