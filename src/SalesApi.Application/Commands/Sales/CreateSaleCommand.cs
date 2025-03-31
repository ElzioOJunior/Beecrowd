using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Commands.Sales
{
    public class CreateSaleCommand : IRequest<CreateSaleCommandResponse>
    {
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<CreateSaleItemCommand> Items { get; set; }
    }

    public class CreateSaleItemCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CreateSaleCommandResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Cancelled { get; set; }
        public List<CreateSaleItemCommandResponse> Items { get; set; }
    }

    public class CreateSaleItemCommandResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public Guid SaleId { get; set; }
        public bool IsCancelled { get; set; }
    }
}
