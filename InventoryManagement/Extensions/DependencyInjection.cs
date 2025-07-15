using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.Services;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Data;
using InventoryManagement.Services;

namespace InventoryManagement.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application
            services.AddScoped<ICategoryApplication, CategoryApplication>();
            services.AddScoped<IProductApplication, ProductApplication>();
            services.AddScoped<IStockMovementApplication, StockMovementApplication>();
            services.AddScoped<IAuthApplication, AuthApplication>();

            // Infra
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IStockMovementRepository, StockMovementRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
