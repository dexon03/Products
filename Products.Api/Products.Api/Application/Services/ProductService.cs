using AutoMapper;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Interfaces;
using Products.Api.Domain.Models;

namespace Products.Api.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    public async Task<List<Product>> GetProducts()
    {
        var products = await productRepository.GetProductsAsync();
        return products;
    }

    public async Task<Product> GetProduct(Guid id)
    {
        var product = await productRepository.GetProductAsync(id);
        return product;
    }

    public async Task<Product> CreateProduct(ProductCreate product)
    {
        var productModel = mapper.Map<Product>(product);
        var createdProduct = await productRepository.CreateProduct(productModel);
        return createdProduct;
    }

    public async Task<Product> UpdateProduct(ProductUpdate product)
    {
        var productModel = mapper.Map<Product>(product);
        await ThrowIfProductNotExists(productModel.Id);

        var updatedProduct = await productRepository.UpdateProduct(productModel);
        return updatedProduct;
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        await ThrowIfProductNotExists(id);
        var isDeleted = await productRepository.DeleteProduct(id);
        return isDeleted;
    }
    
    private async Task ThrowIfProductNotExists(Guid id)
    {
        var isProductExists = await productRepository.IsProductExists(id);
        if (!isProductExists)
        {
            throw new ArgumentException("Product not exists");
        }
    }
}