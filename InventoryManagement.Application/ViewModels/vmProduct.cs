using InventoryManagement.Domain.Models;

namespace InventoryManagement.Application.ViewModels
{
    public class vmProduct
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}
