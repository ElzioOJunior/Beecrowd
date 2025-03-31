using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Bogus;
using MediatR;
using Application.Commands.Sales;
using SalesApi.Controllers;

public class SalesControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly Faker _faker = new Faker();


    #region "success"
    [Fact]
    public async Task GetSales_ShouldReturnOkWithData()
    {
        var fakeSalesResponse = new List<GetSalesQueryResponse>
        {
            new GetSalesQueryResponse
            {
                Id = Guid.NewGuid(),
                SaleNumber = _faker.Random.AlphaNumeric(8),
                SaleDate = _faker.Date.Past(),
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                TotalAmount = _faker.Finance.Amount(),
                Cancelled = _faker.Random.Bool(),
                Items = new List<SaleItemResponse>
                {
                    new SaleItemResponse
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        Quantity = _faker.Random.Int(1, 10),
                        UnitPrice = _faker.Finance.Amount(),
                        Discount = _faker.Random.Decimal(0, 0.2m),
                        Total = _faker.Finance.Amount(),
                        SaleId = Guid.NewGuid(),
                        IsCancelled = _faker.Random.Bool()
                    }
                }
            }
        };

        _mediator.Send(Arg.Any<GetSalesQuery>()).Returns(fakeSalesResponse);

        var controller = new SalesController(_mediator);

        var result = await controller.GetSales() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var response = result.Value as ApiResponse;

        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal("Operação concluída com sucesso", response.Message);
        Assert.NotEmpty(response.Data as IEnumerable<object>);
    }

    [Fact]
    public async Task CreateSale_ShouldReturnOkWithCreatedData()
    {
        var fakeSaleResponse = new CreateSaleCommandResponse
        {
            Id = Guid.NewGuid(),
            SaleNumber = _faker.Random.AlphaNumeric(8),
            Date = _faker.Date.Past(),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            TotalAmount = _faker.Finance.Amount(),
            Cancelled = false,
            Items = new List<CreateSaleItemCommandResponse>
            {
                new CreateSaleItemCommandResponse
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    Quantity = _faker.Random.Int(1, 10),
                    UnitPrice = _faker.Finance.Amount(),
                    Discount = _faker.Random.Decimal(0, 0.2m),
                    Total = _faker.Finance.Amount(),
                    SaleId = Guid.NewGuid(),
                    IsCancelled = false
                }
            }
        };

        _mediator.Send(Arg.Any<CreateSaleCommand>()).Returns(fakeSaleResponse);

        var controller = new SalesController(_mediator);

        var result = await controller.CreateSale(new CreateSaleCommand()) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var response = result.Value as ApiResponse;

        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal("Operação concluída com sucesso", response.Message);
    }

    [Fact]
    public async Task DeleteSale_ShouldReturnOkWithDeletedData()
    {

        var fakeDeleteResponse = new bool(); 

        _mediator.Send(Arg.Any<DeleteSaleCommand>()).Returns(fakeDeleteResponse);

        var controller = new SalesController(_mediator);
  
        var result = await controller.DeleteSale(Guid.NewGuid()) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var response = result.Value as ApiResponse;

        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal("Operação concluída com sucesso", response.Message);

    }

    #endregion

    #region "failure"

    [Fact]
    public async Task GetSales_ShouldReturnNotFound_WhenNoSalesExist()
    {
        _mediator.Send(Arg.Any<GetSalesQuery>()).Returns(new List<GetSalesQueryResponse>());

        var controller = new SalesController(_mediator);

        var result = await controller.GetSales() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var response = result.Value as ApiResponse;

        Assert.NotNull(response);
        Assert.Equal("success", (string)response.Status);
        Assert.Equal("Operação concluída com sucesso", (string)response.Message);

        var data = response.Data as IEnumerable<object>;
        Assert.NotNull(data);
        Assert.Empty(data);
    }

    #endregion
}
