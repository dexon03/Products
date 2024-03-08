using Microsoft.AspNetCore.Mvc;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Interfaces;

namespace Products.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        var result = productService.GetProducts();
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetProduct(Guid id)
    {
        var result =  productService.GetProduct(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductCreate product)
    {
        var result = await productService.CreateProductAsync(product);
        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductUpdate product)
    {
        var result = await productService.UpdateProductAsync(product);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await productService.DeleteProductAsync(id);
        
        return Ok();
    }
}