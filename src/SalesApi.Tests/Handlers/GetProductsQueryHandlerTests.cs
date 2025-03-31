using AutoMapper;
using Domain.Entities;
using NSubstitute;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Handlers;
using Application.Queries.Products;
using Domain.Interfaces;
using NSubstitute.ExceptionExtensions;

public class GetProductsQueryHandlerTests
{
    private readonly IRepository<Product> _repository = Substitute.For<IRepository<Product>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly GetProductsQueryHandler _handler;

    public GetProductsQueryHandlerTests()
    {
        _handler = new GetProductsQueryHandler(_repository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductsSuccessfully()
    {

        var fakeProducts = ProductDataGenerator.GenerateFakeEntityProducts(2);

        var expectedResponse = new List<GetProductsQueryResponse>
        {
            new GetProductsQueryResponse
            {
                Id = fakeProducts[0].Id,
                Title = fakeProducts[0].Title,
                Price = fakeProducts[0].Price,
                Description = fakeProducts[0].Description,
                Category = fakeProducts[0].Category,
                Image = fakeProducts[0].Image
            },
            new GetProductsQueryResponse
            {
                Id = fakeProducts[1].Id,
                Title = fakeProducts[1].Title,
                Price = fakeProducts[1].Price,
                Description = fakeProducts[1].Description,
                Category = fakeProducts[1].Category,
                Image = fakeProducts[1].Image
            }
        };

        _repository.GetAllAsync().Returns(fakeProducts);
        _mapper.Map<IEnumerable<GetProductsQueryResponse>>(fakeProducts).Returns(expectedResponse);

        var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Count, result.ToList().Count);
        Assert.Equal(expectedResponse[0].Title, result.First().Title);
        Assert.Equal(expectedResponse[1].Title, result.Last().Title);

        await _repository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {

        _repository.GetAllAsync().Throws(new Exception("Erro ao acessar o repositório"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetProductsQuery(), CancellationToken.None));
        Assert.Equal("Erro ao acessar o repositório", exception.Message);

        await _repository.Received(1).GetAllAsync(); 
    }
}
