using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.ViewModels;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagement.Application.Services
{
    public class StockMovementApplication : IStockMovementApplication
    {
        #region Dependency Injection
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IProductRepository _productRepository;
        public StockMovementApplication(IStockMovementRepository IStockMovementRepository,
                                        IProductRepository productRepository)
        {
            _stockMovementRepository = IStockMovementRepository;
            _productRepository = productRepository;
        }
        #endregion
        public async Task<IEnumerable<vmStockMovement>> GetAllAsync()
        {
            var categories = await _stockMovementRepository.GetAllAsync();
            return categories.Select(x => new vmStockMovement
            {
                StockMovementId = x.StockMovementId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                MovementType = x.MovementType,
                Date = x.Date,
                Note = x.Note
            });
        }

        public async Task<vmStockMovement?> GetByIdAsync(int stockMovementId)
        {
            var stockMovement = await _stockMovementRepository.GetByIdAsync(stockMovementId);
            if (stockMovement == null) return null;

            return new vmStockMovement
            {
                StockMovementId = stockMovement.StockMovementId,
                ProductId = stockMovement.ProductId,
                Quantity = stockMovement.Quantity,
                MovementType = stockMovement.MovementType,
                Date = stockMovement.Date,
                Note = stockMovement.Note
            };
        }

        public async Task AddAsync(vmStockMovement stockMovement)
        {
            var product = await _productRepository.GetByIdAsync(stockMovement.ProductId);
            if (product == null)
                throw new Exception("Product not found.");

            switch (stockMovement.MovementType)
            {
                case MovementTypeEnum.Entry:
                    product.StockQuantity += stockMovement.Quantity;
                    break;

                case MovementTypeEnum.Exit:
                    if (product.StockQuantity < stockMovement.Quantity)
                        throw new Exception("Insufficient stock for exit.");

                    product.StockQuantity -= stockMovement.Quantity;
                    break;

                case MovementTypeEnum.Adjustment:
                    product.StockQuantity += stockMovement.Quantity;
                    // Accepts positive or negative values for adjustments
                    break;

                default:
                    throw new Exception("Invalid movement type.");
            }

            var entity = new StockMovement
            {
                ProductId = stockMovement.ProductId,
                Quantity = stockMovement.Quantity,
                MovementType = stockMovement.MovementType,
                Date = stockMovement.Date,
                Note = stockMovement.Note
            };

            await _stockMovementRepository.AddAsync(entity);
            await _productRepository.UpdateAsync(product);
        }
    }
}
