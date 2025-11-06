using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Dtos;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailsController : ControllerBase
{
    private readonly AppDbContext _context;
    public OrderDetailsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDetailResponseFlatDto>>> GetAll()
    {
        var result = await (from od in _context.OrderDetails
                            join p in _context.Products on od.ProductId equals p.Id
                            select new OrderDetailResponseFlatDto
                            {
                                Id = od.Id,
                                OrderId = od.OrderId,
                                ProductId = od.ProductId,
                                ProductName = p.Name,
                                Price = p.Price,
                                Quantity = od.Quantity
                            }).ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDetailResponseFlatDto>> GetById(int id)
    {
        var result = await (from od in _context.OrderDetails
                            join p in _context.Products on od.ProductId equals p.Id
                            where od.Id == id
                            select new OrderDetailResponseFlatDto
                            {
                                Id = od.Id,
                                OrderId = od.OrderId,
                                ProductId = od.ProductId,
                                ProductName = p.Name,
                                Price = p.Price,
                                Quantity = od.Quantity
                            }).FirstOrDefaultAsync();

        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDetailResponseFlatDto>> Create(OrderDetailCreateDto dto)
    {
        var orderExists = await _context.Orders.FindAsync(dto.OrderId);
        if (orderExists == null)
            return BadRequest($"Order {dto.OrderId} not found");

        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
            return BadRequest($"Product {dto.ProductId} not found");

        var detail = new OrderDetail
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        _context.OrderDetails.Add(detail);
        await _context.SaveChangesAsync();

        var response = new OrderDetailResponseFlatDto
        {
            Id = detail.Id,
            OrderId = detail.OrderId,
            ProductId = detail.ProductId,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = detail.Quantity
        };

        return CreatedAtAction(nameof(GetById), new { id = detail.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderDetailCreateDto dto)
    {
        var detail = await _context.OrderDetails.FindAsync(id);
        if (detail == null) return NotFound();

        detail.ProductId = dto.ProductId;
        detail.Quantity = dto.Quantity;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var detail = await _context.OrderDetails.FindAsync(id);
        if (detail == null) return NotFound();

        _context.OrderDetails.Remove(detail);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
