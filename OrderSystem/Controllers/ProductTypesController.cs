using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Dtos;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductTypesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProductTypesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductTypeResponseDto>>> GetAll()
    {
        var result = await _context.ProductTypes
            .Select(pt => new ProductTypeResponseDto
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductTypeResponseDto>> GetById(int id)
    {
        var pt = await _context.ProductTypes.FindAsync(id);
        if (pt == null) return NotFound();

        return Ok(new ProductTypeResponseDto
        {
            Id = pt.Id,
            Name = pt.Name,
            Description = pt.Description
        });
    }

    [HttpPost]
    public async Task<ActionResult<ProductTypeResponseDto>> Create(ProductTypeCreateDto dto)
    {
        var pt = new ProductType
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.ProductTypes.Add(pt);
        await _context.SaveChangesAsync();

        var response = new ProductTypeResponseDto
        {
            Id = pt.Id,
            Name = pt.Name,
            Description = pt.Description
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductTypeCreateDto dto)
    {
        var pt = await _context.ProductTypes.FindAsync(id);
        if (pt == null) return NotFound();

        pt.Name = dto.Name;
        pt.Description = dto.Description;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pt = await _context.ProductTypes.FindAsync(id);
        if (pt == null) return NotFound();

        _context.ProductTypes.Remove(pt);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
