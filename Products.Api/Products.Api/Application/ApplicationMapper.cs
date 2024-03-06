using AutoMapper;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Models;

namespace Products.Api.Application;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<ProductCreate, Product>()
            .AfterMap((src, dest) => dest.Id = Guid.NewGuid());
        CreateMap<ProductUpdate, Product>();
    }
}