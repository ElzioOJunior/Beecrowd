using Application.Commands.Sales;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleCommandResponse>
{
    private readonly IRepository<Sale> _saleRepository;
    private readonly IRepository<SaleItem> _itemRepository;
    private readonly IMapper _mapper;

    public CreateSaleCommandHandler(IRepository<Sale> saleRepository, IRepository<SaleItem> itemRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleCommandResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = request.SaleNumber,
            SaleDate = request.SaleDate,
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            Canceled = false,
            Items = new List<SaleItem>()
        };

        decimal totalAmount = 0;
        var productQuantities = new Dictionary<Guid, int>();

        foreach (var item in request.Items)
        {
            if (productQuantities.ContainsKey(item.ProductId))
            {
                productQuantities[item.ProductId] += item.Quantity;
            }
            else
            {
                productQuantities[item.ProductId] = item.Quantity;
            }

            if (productQuantities[item.ProductId] > 20)
            {
                throw new CustomException(
                    type: "InvalidData",
                    message: "Quantidade máxima excedida.",
                    detail: $"O total do produto {item.ProductId} excede o limite de 20 unidades por venda."
                );
            }

             decimal discount = 0;
            if (item.Quantity >= 4 && item.Quantity < 10)
            {
                discount = 0.10m;
            }
            else if (item.Quantity >= 10 && item.Quantity <= 20)
            {
                discount = 0.20m; 
            }
            else if (item.Quantity < 4)
            {
                discount = 0.00m; 
            }

            decimal total = item.Quantity * item.UnitPrice * (1 - discount);

            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = discount,
                Total = total,
                SaleId = sale.Id,
                Canceled = false
            };

            sale.Items.Add(saleItem);
            totalAmount += total;
        }

        sale.TotalAmount = totalAmount;

        await _saleRepository.AddAsync(sale);

        return _mapper.Map<CreateSaleCommandResponse>(sale);
    }

}

