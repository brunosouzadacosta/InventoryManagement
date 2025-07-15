using InventoryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Models
{
    public class StockMovement
    {
        public int StockMovementId { get; set; }
        public int ProductId { get; set; }
        public MovementTypeEnum MovementType { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public Product Product { get; set; } 

    }
}
