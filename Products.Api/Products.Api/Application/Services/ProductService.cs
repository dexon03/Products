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
    
    public List<Product> GetProducts()
    {
        var products =  _productRepository.GetProducts();
        return products;
    }

    public Product GetProduct(Guid id)
    {
        var product = _productRepository.GetProduct(id);
        if (product is null)
        {
            throw new ArgumentException("Product not exists");
        }
        return product;
    }

    public async Task<Product> CreateProductAsync(ProductCreate product)
    {
        var productModel = _mapper.Map<Product>(product);
        var createdProduct = await _productRepository.CreateProductAsync(productModel);
        return createdProduct;
    }

    public async Task<Product> UpdateProductAsync(ProductUpdate product)
    {
        var productModel = _mapper.Map<Product>(product);
        ThrowIfProductNotExists(productModel.Id);

        var updatedProduct = await _productRepository.UpdateProductAsync(productModel);
        return updatedProduct;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        ThrowIfProductNotExists(id);
        var isDeleted = await _productRepository.DeleteProductAsync(id);
        return isDeleted;
    }
    
    private void ThrowIfProductNotExists(Guid id)
    {
        var isProductExists = _productRepository.IsProductExists(id);
        if (!isProductExists)
        {
            throw new ArgumentException("Product not exists");
        }
    }
}