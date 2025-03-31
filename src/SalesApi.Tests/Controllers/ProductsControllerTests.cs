using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Bogus;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Application.Commands.Products;
using Application.Queries.Products;
using MediatR;
using SalesApi.Controllers;

public class ProductsControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly Faker _faker = new Faker();

    [Fact]
    public async Task CreateProduct_ShouldReturnOkWithValidData()
    {
        var createProductCommand = new CreateProductCommand
        {
            Title = _faker.Commerce.ProductName(),
            Price = _faker.Finance.Amount(10, 100),
            Description = _faker.Commerce.ProductDescription(),
            Category = _faker.Commerce.Categories(1)[0],
            Image = _faker.Image.PicsumUrl()
        };

        var fakeProductResponse = new CreateProductCommandResponse
        {
            Id = Guid.NewGuid(),
            Title = createProductCommand.Title,
            Price = createProductCommand.Price,
            Description = createProductCommand.Description,
            Category = createProductCommand.Category,
            Image = createProductCommand.Image
        };

        _mediator.Send(Arg.Any<CreateProductCommand>()).Returns(fakeProductResponse);

        var controller = new ProductsController(_mediator);

        var result = await controller.CreateProduct(createProductCommand) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var response = result.Value as ApiResponse;
        Assert.NotNull(response);
        Assert.Equal("success", (string)response.Status);
        Assert.Equal("Operação concluída com sucesso", (string)response.Message);

        var data = response.Data as CreateProductCommandResponse;
        Assert.NotNull(data);
        Assert.Equal(fakeProductResponse.Id, data.Id);
        Assert.Equal(fakeProductResponse.Title, data.Title);
        Assert.Equal(fakeProductResponse.Price, data.Price);
        Assert.Equal(fakeProductResponse.Description, data.Description);
        Assert.Equal(fakeProductResponse.Category, data.Category);
        Assert.Equal(fakeProductResponse.Image, data.Image);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnBadRequest_WhenInputIsInvalid()
    {
        CreateProductCommand invalidCommand = null;
        var controller = new ProductsController(_mediator);

        var result = await controller.CreateProduct(invalidCommand) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);

        var response = result.Value as string;
        Assert.NotNull(response);
        Assert.Equal("Invalid product data.", response);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOkWithData()
    {
        var fakeProducts = ProductDataGenerator.GenerateFakeProducts(2);

        _mediator.Send(Arg.Any<GetProductsQuery>()).Returns(fakeProducts);

        var controller = new ProductsController(_mediator);

        var result = await controller.GetProducts() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var response = result.Value as ApiResponse;
        Assert.NotNull(response);
        Assert.Equal("success", (string)response.Status);
        Assert.Equal("Operação concluída com sucesso", (string)response.Message);

        var data = response.Data as IEnumerable<GetProductsQueryResponse>;
        Assert.NotNull(data);
        Assert.NotEmpty(data);

        Assert.Equal(fakeProducts.Count, data.Count());
        Assert.Equal(fakeProducts.First().Title, data.First().Title);
        Assert.Equal(fakeProducts.First().Price, data.First().Price);
    }


}
