﻿using Newtonsoft.Json;
using Products.Api.Domain.Interfaces;
using Products.Api.Domain.Models;

namespace Products.Api.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private const string Path = "products.json";
    private List<Product> _products;

    public ProductRepository()
    {
        if (!File.Exists(Path))
        {
            File.Create(Path).Close();
            _products = new List<Product>();
            SaveProducts();
        }
        else
        {
            LoadProducts();
        }
    }

    private void LoadProducts()
    {
        using StreamReader reader = new StreamReader(Path);
        string json = reader.ReadToEnd();
        _products = JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
    }

    private void SaveProducts()
    {
        using StreamWriter writer = new StreamWriter(Path, false);
        var json = JsonConvert.SerializeObject(_products);
        writer.WriteAsync(json).Wait();
    }
    
    private async Task SaveProductsAsync()
    {
        await using StreamWriter writer = new StreamWriter(Path, false);
        var json = JsonConvert.SerializeObject(_products);
        await writer.WriteAsync(json);
    }

    public List<Product> GetProducts()
    {
        return [.._products];
    }

    public Product? GetProduct(Guid id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _products.Add(product);
        await SaveProductsAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        var index = _products.FindIndex(p => p.Id == product.Id);
        if (index != -1)
        {
            _products[index] = product;
            await SaveProductsAsync();
        }
        return product;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var index = _products.FindIndex(p => p.Id == id);
        if (index != -1)
        {
            _products.RemoveAt(index);
            await SaveProductsAsync();
            return true;
        }
        return false;
    }

    public bool IsProductExists(Guid id)
    {
        return _products.Any(p => p.Id == id);
    }
}
