using AutoMapper;
using Domain.Entities;
using NSubstitute;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using NSubstitute.ExceptionExtensions;

public class GetSalesQueryHandlerTests
{
    private readonly IRepository<Sale> _saleRepository = Substitute.For<IRepository<Sale>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly GetSalesQueryHandler _handler;

    public GetSalesQueryHandlerTests()
    {
        _handler = new GetSalesQueryHandler(_saleRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSalesSuccessfully()
    {

        var fakeSales = SaleDataGenerator.GenerateFakeSales();

        var expectedResponse = new List<GetSalesQueryResponse>
        {
            new GetSalesQueryResponse
            {
                Id = fakeSales[0].Id,
                SaleNumber = fakeSales[0].SaleNumber,
                SaleDate = fakeSales[0].SaleDate,
                CustomerId = fakeSales[0].CustomerId,
                BranchId = fakeSales[0].BranchId,
                TotalAmount = fakeSales[0].TotalAmount,
                Cancelled = fakeSales[0].Canceled,
                Items = fakeSales[0].Items.Select(i => new SaleItemResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = 0,
                    Total = i.Total,
                    SaleId = fakeSales[0].Id,
                    IsCancelled = false
                }).ToList()
            },
            new GetSalesQueryResponse
            {
                Id = fakeSales[1].Id,
                SaleNumber = fakeSales[1].SaleNumber,
                SaleDate = fakeSales[1].SaleDate,
                CustomerId = fakeSales[1].CustomerId,
                BranchId = fakeSales[1].BranchId,
                TotalAmount = fakeSales[1].TotalAmount,
                Cancelled = fakeSales[1].Canceled,
                Items = fakeSales[1].Items.Select(i => new SaleItemResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = 0,
                    Total = i.Total,
                    SaleId = fakeSales[1].Id,
                    IsCancelled = true
                }).ToList()
            }
        };

        _saleRepository.GetAllWithIncludes(Arg.Any<Expression<Func<Sale, object>>>()).Returns(fakeSales);
        _mapper.Map<List<GetSalesQueryResponse>>(fakeSales).Returns(expectedResponse);

        var result = await _handler.Handle(new GetSalesQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Count, result.Count);
        Assert.Equal(expectedResponse[0].SaleNumber, result[0].SaleNumber);
        Assert.Equal(expectedResponse[1].SaleNumber, result[1].SaleNumber);

        _saleRepository.Received(1).GetAllWithIncludes(Arg.Any<Expression<Func<Sale, object>>>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        _saleRepository.GetAllWithIncludes(Arg.Any<Expression<Func<Sale, object>>>())
            .Throws(new Exception("Erro ao acessar o repositório"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetSalesQuery(), CancellationToken.None));
        Assert.Equal("Erro ao acessar o repositório", exception.Message);

        _saleRepository.Received(1).GetAllWithIncludes(Arg.Any<Expression<Func<Sale, object>>>());
    }

}
