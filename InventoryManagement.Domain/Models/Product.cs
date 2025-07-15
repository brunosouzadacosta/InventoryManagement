using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    }
}
