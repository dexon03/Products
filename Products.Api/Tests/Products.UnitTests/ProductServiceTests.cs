using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Products.Api.Application.Services;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Interfaces;
using Products.Api.Domain.Models;

namespace Products.UnitTests;

public class ProductServiceTests
{
    private readonly IProductRepository  _productRepository;
    private readonly IMapper _mapper; 
    private readonly ProductService _productService;
    public ProductServiceTests()
    {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _productService = new ProductService(_productRepository, _mapper);
    }
    
    [Fact]
    public void GetProducts_ShouldReturnListOfProducts()
    {
        // Arrange
        List<Product> products = new()
        {
            new() { Id = Guid.NewGuid(), Name = "Product1", Price = 100 },
            new() { Id = Guid.NewGuid(), Name = "Product2", Price = 100 }
        };
        _productRepository.GetProducts()!
            .Returns(products);
        
        // Act
        List<Product> result = _productService.GetProducts();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(products);     
    }
    
    [Fact]
    public async Task GetProductAsync_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product {Id = Guid.NewGuid(), Name = "Product1", Price = 100};
        _productRepository.GetProduct(Arg.Any<Guid>())!
            .Returns(c => product);
        
        // Act
        var result =  _productService.GetProduct(product.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }
    
    [Fact]
    public void GetProductAsync_WhenProductNotExists_ShouldReturnNull()
    {
        // Arrange
        _productRepository.GetProduct(Arg.Any<Guid>())!
            .Returns(c => null!);
        
        // Act
        Func<Product> act = () =>  _productService.GetProduct(Guid.NewGuid());
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Product not exists");
    }
    
    [Fact]
    public async Task CreateProductAsync_ShouldReturnCreatedProduct()
    {
        // Arrange
        var productCreate = new ProductCreate { Name = "Product1", Price = 100 };
        var product = new Product {Id = Guid.NewGuid(), Name = "Product1", Price = 100};
        _mapper.Map<Product>(productCreate)!
            .Returns(product);
        _productRepository.CreateProductAsync(product)!
            .Returns(Task.FromResult(product));
        
        // Act
        var result = await _productService.CreateProductAsync(productCreate);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }
    
    [Fact]
    public async Task UpdateProductAsync_ShouldReturnUpdatedProduct()
    {
        // Arrange
        var productUpdate = new ProductUpdate { Id = Guid.NewGuid(), Name = "Product1", Price = 100 };
        var product = new Product {Id = productUpdate.Id, Name = "Product1", Price = 100};
        _mapper.Map<Product>(productUpdate)!
            .Returns(product);
        _productRepository.IsProductExists(Arg.Any<Guid>())!
            .Returns(true);
        _productRepository.UpdateProductAsync(product)!
            .Returns(Task.FromResult(product));
        
        // Act
        var result = await _productService.UpdateProductAsync(productUpdate);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }
    
    [Fact]
    public async Task UpdateProductAsync_WhenProductNotExists_ShouldThrowException()
    {
        // Arrange
        var productUpdate = new ProductUpdate { Id = Guid.NewGuid(), Name = "Product1", Price = 100 };
        _mapper.Map<Product>(productUpdate)!
            .Returns(new Product {Id = productUpdate.Id, Name = "Product1", Price = 100});
        _productRepository.IsProductExists(Arg.Any<Guid>())!
            .Returns(false);
        
        // Act
        Func<Task> act = async () => await _productService.UpdateProductAsync(productUpdate);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Product not exists");
    }
    
    [Fact]
    public async Task DeleteProductAsync_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        _productRepository.IsProductExists(id)!
            .Returns(true);
        _productRepository.DeleteProductAsync(id)!
            .Returns(Task.FromResult(true));
        
        // Act
        var result = await _productService.DeleteProductAsync(id);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteProductAsync_WhenProductNotExists_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        _productRepository.IsProductExists(Arg.Any<Guid>())!
            .Returns(false);
        
        // Act
        Func<Task> act = async () => await _productService.DeleteProductAsync(id);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Product not exists");
    }
}