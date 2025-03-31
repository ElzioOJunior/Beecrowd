using AutoMapper;
using Domain.Entities;
using NSubstitute;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.Products;
using Domain.Interfaces;
using NSubstitute.ExceptionExtensions;

public class CreateProductCommandHandlerTests
{
    private readonly IRepository<Product> _repository = Substitute.For<IRepository<Product>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _handler = new CreateProductCommandHandler(_repository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldCreateProductSuccessfully()
    {

        var createCommand = new CreateProductCommand
        {
            Title = "Test Product",
            Price = 100,
            Description = "description",
            Category = "Category",
            Image = "https://google.com/test.jpg"
        };

        var productEntity = new Product
        {
            Id = Guid.NewGuid(),
            Title = createCommand.Title,
            Price = createCommand.Price,
            Description = createCommand.Description,
            Category = createCommand.Category,
            Image = createCommand.Image
        };

        var expectedResponse = new CreateProductCommandResponse
        {
            Id = productEntity.Id,
            Title = productEntity.Title,
            Price = productEntity.Price,
            Description = productEntity.Description,
            Category = productEntity.Category,
            Image = productEntity.Image
        };

        _mapper.Map<Product>(createCommand).Returns(productEntity); 
        _mapper.Map<CreateProductCommandResponse>(productEntity).Returns(expectedResponse);

        var result = await _handler.Handle(createCommand, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.Title, result.Title);
        Assert.Equal(expectedResponse.Price, result.Price);
        Assert.Equal(expectedResponse.Description, result.Description);
        Assert.Equal(expectedResponse.Category, result.Category);
        Assert.Equal(expectedResponse.Image, result.Image);

        await _repository.Received(1).AddAsync(productEntity); 
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {

        var createCommand = new CreateProductCommand
        {
            Title = "Product",
            Price = 100,
            Description = "description",
            Category = "Category",
            Image = "https://google.com/test.jpg"
        };

        var productEntity = new Product
        {
            Id = Guid.NewGuid(),
            Title = createCommand.Title,
            Price = createCommand.Price,
            Description = createCommand.Description,
            Category = createCommand.Category,
            Image = createCommand.Image
        };

        _mapper.Map<Product>(createCommand).Returns(productEntity); 
        _repository.AddAsync(productEntity).Throws(new Exception("Erro ao salvar no repositório")); 

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(createCommand, CancellationToken.None));
        Assert.Equal("Erro ao salvar no repositório", exception.Message);

        await _repository.Received(1).AddAsync(productEntity); 
    }
}
