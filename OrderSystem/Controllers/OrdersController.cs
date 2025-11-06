using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Dtos;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAll()
    {
        var result = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer != null ? o.Customer.Name : string.Empty,
                TotalAmount = o.OrderDetails.Sum(d => (d.Product != null ? d.Product.Price : 0) * d.Quantity),
                Details = o.OrderDetails.Select(d => new OrderDetailResponseDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product != null ? d.Product.Name : string.Empty,
                    Price = d.Product != null ? d.Product.Price : 0,
                    Quantity = d.Quantity
                }).ToList()
            })
            .ToListAsync();

        return Ok(result);
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetById(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        var details = order.OrderDetails.Select(d => new OrderDetailResponseDto
        {
            ProductId = d.ProductId,
            ProductName = d.Product != null ? d.Product.Name : string.Empty,
            Price = d.Product != null ? d.Product.Price : 0,
            Quantity = d.Quantity
        }).ToList();

        var response = new OrderResponseDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.Name ?? string.Empty,
            Details = details,
            TotalAmount = details.Sum(x => x.TotalPrice) 
        };

        return Ok(response);
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> Create(OrderCreateDto dto)
    {
        // 🔹 Validate Customer
        var customer = await _context.Customers.FindAsync(dto.CustomerId);
        if (customer == null)
            return BadRequest($"Customer with ID {dto.CustomerId} not found.");

        // Validate Products
        foreach (var item in dto.Details)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Id == item.ProductId);
            if (!productExists)
                return BadRequest($"Product with ID {item.ProductId} not found.");
        }

        // Create Order
        var order = new Order
        {
            CustomerId = dto.CustomerId,
            OrderDate = dto.OrderDate
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        //  Add Details
        foreach (var item in dto.Details)
        {
            var detail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
            _context.OrderDetails.Add(detail);
        }

        await _context.SaveChangesAsync();

        // Load Details (safe for nulls)
        var details = await _context.OrderDetails
            .Where(d => d.OrderId == order.Id)
            .Include(d => d.Product)
            .Select(d => new OrderDetailResponseDto
            {
                ProductId = d.ProductId,
                ProductName = d.Product != null ? d.Product.Name : string.Empty,
                Price = d.Product != null ? d.Product.Price : 0,
                Quantity = d.Quantity
            })
            .ToListAsync();

        var response = new OrderResponseDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            CustomerId = order.CustomerId,
            CustomerName = customer.Name,
            Details = details,
            TotalAmount = details.Sum(x => x.TotalPrice)
        };

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, response);
    }

    // DELETE: api/orders/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound();

        var details = _context.OrderDetails.Where(d => d.OrderId == id);
        _context.OrderDetails.RemoveRange(details);
        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
