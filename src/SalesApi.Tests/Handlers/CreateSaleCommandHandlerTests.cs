using AutoMapper;
using Domain.Entities;
using NSubstitute;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.Sales;
using Application.Exceptions;
using Domain.Interfaces;

public class CreateSaleCommandHandlerTests
{
    private readonly IRepository<Sale> _saleRepository = Substitute.For<IRepository<Sale>>();
    private readonly IRepository<SaleItem> _saleItemRepository = Substitute.For<IRepository<SaleItem>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerTests()
    {
        _handler = new CreateSaleCommandHandler(_saleRepository, _saleItemRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldThrowCustomException_WhenProductQuantityExceedsLimit()
    {

        var createSaleCommand = SaleDataGenerator.GenerateFakeCreateSaleCommand();
        createSaleCommand.Items[0].Quantity = 25;

        var exception = await Assert.ThrowsAsync<CustomException>(() => _handler.Handle(createSaleCommand, CancellationToken.None));
        Assert.Equal("InvalidData", exception.Type);
        Assert.Equal("Quantidade máxima excedida.", exception.Message);
        Assert.Contains($"O total do produto {createSaleCommand.Items[0].ProductId} excede o limite de 20 unidades por venda.", exception.Detail);

        await _saleRepository.DidNotReceive().AddAsync(Arg.Any<Sale>()); 
    }
}
