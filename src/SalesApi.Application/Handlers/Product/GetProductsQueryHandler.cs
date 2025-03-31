using Domain.Interfaces;
using AutoMapper;
using MediatR;
using Application.Queries.Products;
using Domain.Entities;

namespace Application.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<GetProductsQueryResponse>>
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetProductsQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetProductsQueryResponse>>(products);
        }
    }
}
