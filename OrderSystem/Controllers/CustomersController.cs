using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Dtos;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;
    public CustomersController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAll()
    {
        var customers = await _context.Customers
            .Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                City = c.City,
                Province = c.Province,
                Phone = c.Phone
            })
            .ToListAsync();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetById(int id)
    {
        var c = await _context.Customers.FindAsync(id);
        if (c == null) return NotFound();

        var response = new CustomerResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Address = c.Address,
            City = c.City,
            Province = c.Province,
            Phone = c.Phone
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> Create(CustomerCreateDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Address = dto.Address,
            City = dto.City,
            Province = dto.Province,
            Phone = dto.Phone
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var response = new CustomerResponseDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            City = customer.City,
            Province = customer.Province,
            Phone = customer.Phone
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CustomerCreateDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();

        customer.Name = dto.Name;
        customer.Address = dto.Address;
        customer.City = dto.City;
        customer.Province = dto.Province;
        customer.Phone = dto.Phone;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
