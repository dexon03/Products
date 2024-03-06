using Products.Api.Domain.Contracts;
using Products.Api.Domain.Models;

namespace Products.Api.Domain.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync();
    Task<Product> GetProductAsync(Guid id);
    Task<Product> CreateProductAsync(ProductCreate product);
    Task<Product> UpdateProductAsync(ProductUpdate product);
    Task<bool> DeleteProductAsync(Guid id);
}