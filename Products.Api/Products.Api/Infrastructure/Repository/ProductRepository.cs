using Newtonsoft.Json;
using Products.Api.Domain.Interfaces;
using Products.Api.Domain.Models;

namespace Products.Api.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly string path = "products.json";
    public ProductRepository()
    {
        if (!File.Exists(path))
        {
            File.Create(path);
        }
    }
    public async Task<List<Product>?> GetProductsAsync()
    {
        return await GetProducts();
    }

    public async Task<Product?> GetProductAsync(Guid id)
    {
        var products = await GetProducts();
        
        var product = products
            .FirstOrDefault(p => p.Id == id);
        return product;
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await using StreamWriter writer = new StreamWriter(path, true);
        var json = JsonConvert.SerializeObject(product);
        await writer.WriteAsync(json);
        return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        var products = await GetProducts();
        
        var index = products.FindIndex(p => p.Id == product.Id);
        products[index] = product;
        
        await using StreamWriter writer = new StreamWriter(path, false);
        var json = JsonConvert.SerializeObject(products);
        await writer.WriteAsync(json);
        return product;
        
        
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        var products = await GetProducts();
        
        var index = products.FindIndex(p => p.Id == id);
        products.RemoveAt(index);
        
        await using StreamWriter writer = new StreamWriter(path, false);
        var json = JsonConvert.SerializeObject(products);
        await writer.WriteAsync(json);
        return true;
    }

    public async Task<bool> IsProductExists(Guid id)
    {
        var products = await GetProducts();
        
        var isExists = products.Any(p => p.Id == id);
        return isExists;
    }

    private async Task<List<Product>> GetProducts()
    {
        using StreamReader reader = new StreamReader(path);
        string json = await reader.ReadToEndAsync();
        return JsonConvert.DeserializeObject<List<Product>>(json) ?? throw new InvalidOperationException();
    }
}