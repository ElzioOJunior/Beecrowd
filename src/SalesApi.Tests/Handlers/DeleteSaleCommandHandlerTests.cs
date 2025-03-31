using NSubstitute;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Commands.Sales;
using Application.Exceptions;
using Domain.Interfaces;
using System.Linq.Expressions;

public class DeleteSaleCommandHandlerTests
{
    private readonly IRepository<Sale> _repository = Substitute.For<IRepository<Sale>>();
    private readonly DeleteSaleCommandHandler _handler;

    public DeleteSaleCommandHandlerTests()
    {
        _handler = new DeleteSaleCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ShouldCancelSaleSuccessfully()
    {

        var saleId = Guid.NewGuid();
        var fakeSale = new Sale
        {
            SaleNumber = "123",
            Id = saleId,
            Canceled = false,
            Items = new List<SaleItem>
        {
            new SaleItem { Id = Guid.NewGuid(), Canceled = false },
            new SaleItem { Id = Guid.NewGuid(), Canceled = false }
        }
        };

        _repository.GetByIdWithIncludesAsync(saleId, Arg.Any<Expression<Func<Sale, object>>>()).Returns(fakeSale);

        var result = await _handler.Handle(new DeleteSaleCommand { Id = saleId }, CancellationToken.None);

        Assert.True(result); 
        Assert.True(fakeSale.Canceled); 
        Assert.All(fakeSale.Items, item => Assert.True(item.Canceled)); 

        await _repository.Received(1).GetByIdWithIncludesAsync(saleId, Arg.Any<Expression<Func<Sale, object>>>()); 
        await _repository.Received(1).UpdateAsync(fakeSale); 
    }


    [Fact]
    public async Task Handle_ShouldThrowCustomException_WhenSaleNotFound()
    {

        var saleId = Guid.NewGuid();
        _repository.GetByIdWithIncludesAsync(saleId, Arg.Any<Expression<Func<Sale, object>>>()).Returns((Sale)null);


        var exception = await Assert.ThrowsAsync<CustomException>(() => _handler.Handle(new DeleteSaleCommand { Id = saleId }, CancellationToken.None));
        Assert.Equal("InvalidData", exception.Type);
        Assert.Equal("Sale not found", exception.Message);
        Assert.Equal($"The sale with ID {saleId} was not found in the system.", exception.Detail);

        await _repository.Received(1).GetByIdWithIncludesAsync(saleId, Arg.Any<Expression<Func<Sale, object>>>()); 
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
    }
}
