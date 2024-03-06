using Products.Api.Domain.Models;

namespace Products.Api.Domain.Interfaces;

public interface IProductRepository
{
    Task<List<Product>?> GetProductsAsync();
    Task<Product?> GetProductAsync(Guid id);
    Task<Product> CreateProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task<bool> DeleteProduct(Guid id);
    Task<bool> IsProductExists(Guid id);
}