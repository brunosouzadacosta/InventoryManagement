using InventoryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Interfaces
{
    public interface IStockMovementRepository
    {
        Task<IEnumerable<StockMovement>> GetAllAsync();
        Task<StockMovement?> GetByIdAsync(int stockMovementId);
        Task<StockMovement> AddAsync(StockMovement stockMovement);
    }
}
