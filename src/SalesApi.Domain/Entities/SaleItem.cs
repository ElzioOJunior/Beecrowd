using System;

namespace Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public Guid SaleId { get; set; } 
        public Guid ProductId { get; set; } 
        public int Quantity { get; set; } 
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public bool Canceled { get; set; }
        public Sale? Sale { get; set; }
        public Product? Product { get; set; }
    }
}
