using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Sale : BaseEntity
    {
        public required string SaleNumber { get; set; } 
        public DateTime SaleDate { get; set; } 
        public Guid CustomerId { get; set; } 
        public Guid BranchId { get; set; }
        public bool Canceled { get; set; }
        public decimal TotalAmount { get; set; }
        public required ICollection<SaleItem> Items { get; set; }
    }
}
