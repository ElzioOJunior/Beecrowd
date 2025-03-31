using MediatR;

public class GetSalesQuery : IRequest<List<GetSalesQueryResponse>> { }

public class GetSalesQueryResponse
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }
    public List<SaleItemResponse> Items { get; set; }
}

public class SaleItemResponse
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
