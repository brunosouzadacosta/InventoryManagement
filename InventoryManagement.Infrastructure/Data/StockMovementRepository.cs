using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Data
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly InventoryDbContext _context;

        public StockMovementRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockMovement>> GetAllAsync()
        {
            return await _context.StockMovements.ToListAsync();
        }

        public async Task<StockMovement?> GetByIdAsync(int id)
        {
            return await _context.StockMovements.FindAsync(id);
        }

        public async Task<StockMovement> AddAsync(StockMovement stockMovement)
        {
            _context.StockMovements.Add(stockMovement);
            await _context.SaveChangesAsync();
            return stockMovement;
        }
    }
}
