using AutoMapper;
using Domain.Entities;
using Application.Queries.Products;
using Application.Commands.Products;
using Application.Commands.Sales;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, GetProductsQueryResponse>();
            CreateMap<Product, GetProductsQuery>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<Product, CreateProductCommandResponse>();
            CreateMap<Sale, GetSalesQueryResponse>();
            CreateMap<SaleItem, SaleItemResponse>();
            CreateMap<Sale, CreateSaleCommandResponse>();
            CreateMap<SaleItem, CreateSaleItemCommandResponse>();
        }
    }
}
