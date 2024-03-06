﻿using Microsoft.AspNetCore.Mvc;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Interfaces;

namespace Products.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await productService.GetProducts();
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var result = await productService.GetProduct(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductCreate product)
    {
        var result = await productService.CreateProduct(product);
        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductUpdate product)
    {
        var result = await productService.UpdateProduct(product);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await productService.DeleteProduct(id);
        
        return Ok(result ? "Product deleted" : "Product not deleted");
    }
}