using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, List<GetSalesQueryResponse>>
{
    private readonly IRepository<Sale> _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesQueryHandler(IRepository<Sale> saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<List<GetSalesQueryResponse>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.GetAllWithIncludes(sale => sale.Items);

        return _mapper.Map<List<GetSalesQueryResponse>>(sales);
    }
}
