using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.ViewModels;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.Application.Services
{
    public class ProductApplication : IProductApplication
    {
        #region Dependency Injection
        private readonly IProductRepository _productRepository;
        public ProductApplication(IProductRepository IProductRepository)
        {
            _productRepository = IProductRepository;
        }
        #endregion
        public async Task<IEnumerable<vmProduct>> GetAllAsync()
        {
            var categories = await _productRepository.GetAllAsync();
            return categories.Select(x => new vmProduct
            {
                ProductId = x.ProductId,
                Name = x.Name,
                SKU = x.SKU,
                UnitPrice = x.UnitPrice,
                StockQuantity = x.StockQuantity,
                CategoryId = x.CategoryId
            });
        }

        public async Task<vmProduct?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            return new vmProduct
            {
                ProductId = product.ProductId,
                Name = product.Name,
                SKU = product.SKU,
                UnitPrice = product.UnitPrice,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };
        }

        public async Task AddAsync(vmProduct product)
        {
            var entity = new Product
            {
                Name = product.Name,
                SKU = product.SKU,
                UnitPrice = product.UnitPrice,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };

            await _productRepository.AddAsync(entity);
        }


        public async Task<bool> UpdateAsync(int productId, vmProduct product)
        {
            var getProduct = await _productRepository.GetByIdAsync(productId);
            if (getProduct == null) return false;

            getProduct.Name = product.Name;
            getProduct.SKU = product.SKU;
            getProduct.UnitPrice = product.UnitPrice;
            getProduct.StockQuantity = product.StockQuantity;
            getProduct.CategoryId = product.CategoryId;

            await _productRepository.UpdateAsync(getProduct);
            return true;
        }


        public async Task<bool> DeleteAsync(int productId)
        {
            await _productRepository.DeleteAsync(productId);
            return true;
        }
    }
}
