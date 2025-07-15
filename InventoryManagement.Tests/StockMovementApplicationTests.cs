using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using InventoryManagement.Application.Services;
using InventoryManagement.Application.ViewModels;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Models;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Tests.Application
{
    public class StockMovementApplicationTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private readonly StockMovementApplication _service;

        public StockMovementApplicationTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>();

            _service = new StockMovementApplication(
                _stockMovementRepositoryMock.Object,
                _productRepositoryMock.Object
            );
        }

        [Fact]
        public async Task AddAsync_ShouldIncreaseStock_WhenEntry()
        {
            // Arrange
            var product = new Product { ProductId = 1, StockQuantity = 5 };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            var movement = new vmStockMovement
            {
                ProductId = 1,
                Quantity = 10,
                MovementType = MovementTypeEnum.Entry,
                Date = DateTime.Now,
                Note = "Entry Test"
            };

            // Act
            await _service.AddAsync(movement);

            // Assert
            product.StockQuantity.Should().Be(15);
            _productRepositoryMock.Verify(x => x.UpdateAsync(product), Times.Once);
            _stockMovementRepositoryMock.Verify(x => x.AddAsync(It.IsAny<StockMovement>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldDecreaseStock_WhenExitWithSufficientStock()
        {
            // Arrange
            var product = new Product { ProductId = 1, StockQuantity = 10 };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            var movement = new vmStockMovement
            {
                ProductId = 1,
                Quantity = 5,
                MovementType = MovementTypeEnum.Exit,
                Date = DateTime.Now,
                Note = "Exit Test"
            };

            // Act
            await _service.AddAsync(movement);

            // Assert
            product.StockQuantity.Should().Be(5);
            _productRepositoryMock.Verify(x => x.UpdateAsync(product), Times.Once);
            _stockMovementRepositoryMock.Verify(x => x.AddAsync(It.IsAny<StockMovement>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenExitWithInsufficientStock()
        {
            // Arrange
            var product = new Product { ProductId = 1, StockQuantity = 3 };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            var movement = new vmStockMovement
            {
                ProductId = 1,
                Quantity = 5,
                MovementType = MovementTypeEnum.Exit,
                Date = DateTime.Now,
                Note = "Exit Insufficient"
            };

            // Act
            Func<Task> act = async () => await _service.AddAsync(movement);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Insufficient stock for exit.");

            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddAsync(It.IsAny<StockMovement>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ShouldAdjustStock_WhenAdjustmentPositive()
        {
            // Arrange
            var product = new Product { ProductId = 1, StockQuantity = 10 };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            var movement = new vmStockMovement
            {
                ProductId = 1,
                Quantity = 5,
                MovementType = MovementTypeEnum.Adjustment,
                Date = DateTime.Now,
                Note = "Adjustment Positive"
            };

            // Act
            await _service.AddAsync(movement);

            // Assert
            product.StockQuantity.Should().Be(15);
            _productRepositoryMock.Verify(x => x.UpdateAsync(product), Times.Once);
            _stockMovementRepositoryMock.Verify(x => x.AddAsync(It.IsAny<StockMovement>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAdjustStock_WhenAdjustmentNegative()
        {
            // Arrange
            var product = new Product { ProductId = 1, StockQuantity = 10 };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            var movement = new vmStockMovement
            {
                ProductId = 1,
                Quantity = -3,
                MovementType = MovementTypeEnum.Adjustment,
                Date = DateTime.Now,
                Note = "Adjustment Negative"
            };

            // Act
            await _service.AddAsync(movement);

            // Assert
            product.StockQuantity.Should().Be(7);
            _productRepositoryMock.Verify(x => x.UpdateAsync(product), Times.Once);
            _stockMovementRepositoryMock.Verify(x => x.AddAsync(It.IsAny<StockMovement>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenProductNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            var movement = new vmStockMovement
            {
                ProductId = 1,
                Quantity = 5,
                MovementType = MovementTypeEnum.Entry,
                Date = DateTime.Now,
                Note = "Product Not Found"
            };

            // Act
            Func<Task> act = async () => await _service.AddAsync(movement);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Product not found.");

            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddAsync(It.IsAny<StockMovement>()), Times.Never);
        }
    }
}
