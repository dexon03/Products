using Products.Api.Domain.Contracts;
using Products.Api.Domain.Models;

namespace Products.Api.Domain.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProducts();
    Task<Product> GetProduct(Guid id);
    Task<Product> CreateProduct(ProductCreate product);
    Task<Product> UpdateProduct(ProductUpdate product);
    Task<bool> DeleteProduct(Guid id);
}