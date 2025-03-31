
using MediatR;

namespace Application.Queries.Products
{

    public class GetProductsQuery : IRequest<IEnumerable<GetProductsQueryResponse>>
    {
    }

    public class GetProductsQueryResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
    }
}
