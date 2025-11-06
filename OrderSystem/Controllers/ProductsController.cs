using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Dtos;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProductsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
    {
        var result = await (from p in _context.Products
                            join t in _context.ProductTypes on p.ProductTypeId equals t.Id
                            select new ProductResponseDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Price = p.Price,
                                ProductTypeId = t.Id,
                                ProductTypeName = t.Name
                            }).ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var result = await (from p in _context.Products
                            join t in _context.ProductTypes on p.ProductTypeId equals t.Id
                            where p.Id == id
                            select new ProductResponseDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Price = p.Price,
                                ProductTypeId = t.Id,
                                ProductTypeName = t.Name
                            }).FirstOrDefaultAsync();

        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(ProductCreateDto dto)
    {
        var typeExists = await _context.ProductTypes.FindAsync(dto.ProductTypeId);
        if (typeExists == null)
            return BadRequest($"Product type {dto.ProductTypeId} not found");

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            ProductTypeId = dto.ProductTypeId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var response = new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ProductTypeId = dto.ProductTypeId,
            ProductTypeName = typeExists.Name
        };

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductCreateDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.ProductTypeId = dto.ProductTypeId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
