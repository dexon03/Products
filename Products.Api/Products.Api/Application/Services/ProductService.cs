using AutoMapper;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Interfaces;
using Products.Api.Domain.Models;

namespace Products.Api.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<List<Product>> GetProductsAsync()
    {
        var products = await _productRepository.GetProductsAsync();
        return products;
    }

    public async Task<Product> GetProductAsync(Guid id)
    {
        var product = await _productRepository.GetProductAsync(id);
        if (product is null)
        {
            throw new ArgumentException("Product not exists");
        }
        return product;
    }

    public async Task<Product> CreateProductAsync(ProductCreate product)
    {
        var productModel = _mapper.Map<Product>(product);
        var createdProduct = await _productRepository.CreateProduct(productModel);
        return createdProduct;
    }

    public async Task<Product> UpdateProductAsync(ProductUpdate product)
    {
        var productModel = _mapper.Map<Product>(product);
        await ThrowIfProductNotExists(productModel.Id);

        var updatedProduct = await _productRepository.UpdateProduct(productModel);
        return updatedProduct;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        await ThrowIfProductNotExists(id);
        var isDeleted = await _productRepository.DeleteProduct(id);
        return isDeleted;
    }
    
    private async Task ThrowIfProductNotExists(Guid id)
    {
        var isProductExists = await _productRepository.IsProductExists(id);
        if (!isProductExists)
        {
            throw new ArgumentException("Product not exists");
        }
    }
}