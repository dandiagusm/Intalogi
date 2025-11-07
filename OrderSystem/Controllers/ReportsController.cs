using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    // Sales by order before a given date
    [HttpGet("sales-before")]
    public async Task<IActionResult> GetSalesBeforeDate([FromQuery] DateTime date)
    {
        var result = await _context.Orders
            .Where(o => o.OrderDate < date)
            .Include(o => o.Customer)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .Select(o => new
            {
                OrderId = o.Id,
                CustomerName = o.Customer != null ? o.Customer.Name : string.Empty,
                TotalSales = o.OrderDetails.Sum(d => (d.Product != null ? d.Product.Price : 0) * d.Quantity)
            })
            .ToListAsync();

        return Ok(result);
    }

    // Sales by product type
    [HttpGet("sales-by-product-type")]
    public async Task<IActionResult> GetSalesByProductType()
    {
        // Load all data into memory first
        var orderDetails = await _context.OrderDetails
            .Include(od => od.Product)
                .ThenInclude(p => p.ProductType)
            .ToListAsync();

        // Group safely in memory
        var result = orderDetails
            .Where(od => od.Product != null)
            .GroupBy(od =>
            {
                var productType = od.Product?.ProductType;
                return productType != null ? productType.Name : "Unknown Type";
            })
            .Select(g => new
            {
                ProductType = g.Key,
                TotalSales = g.Sum(x => (x.Product != null ? x.Product.Price : 0) * x.Quantity)
            })
            .ToList();

        return Ok(result);
    }


    // Sales by product
    [HttpGet("sales-by-product")]
    public async Task<IActionResult> GetSalesByProduct()
    {
        var result = await _context.OrderDetails
            .Include(od => od.Product)
            .Where(od => od.Product != null)
            .GroupBy(od => od.Product!.Name)
            .Select(g => new
            {
                ProductName = g.Key,
                TotalSales = g.Sum(x => (x.Product != null ? x.Product.Price : 0) * x.Quantity)
            })
            .ToListAsync();

        return Ok(result);
    }

    // Products priced above average
    [HttpGet("products-above-average")]
    public async Task<IActionResult> GetProductsAboveAverage()
    {
        var avgPrice = await _context.Products.AverageAsync(p => p.Price);

        var result = await _context.Products
            .Where(p => p.Price > avgPrice)
            .Select(p => new
            {
                ProductName = p.Name,
                Price = p.Price
            })
            .ToListAsync();

        return Ok(new
        {
            AveragePrice = avgPrice,
            Products = result
        });
    }

    // Orders with total sales above 5 million
    [HttpGet("sales-above-5m")]
    public async Task<IActionResult> GetOrdersAboveFiveMillion()
    {
        var result = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .Select(o => new
            {
                OrderId = o.Id,
                CustomerName = o.Customer != null ? o.Customer.Name : string.Empty,
                TotalSales = o.OrderDetails.Sum(d => (d.Product != null ? d.Product.Price : 0) * d.Quantity)
            })
            .Where(x => x.TotalSales > 5000000)
            .ToListAsync();

        return Ok(result);
    }

    // Products grouped by type
    [HttpGet("products-by-type")]
    public async Task<IActionResult> GetProductsByType()
    {
        var result = await _context.ProductTypes
            .Include(pt => pt.Products)
            .Select(pt => new
            {
                ProductType = pt.Name,
                Products = pt.Products != null
                    ? pt.Products.Select(p => p.Name).ToList()
                    : new List<string>()
            })
            .ToListAsync();

        return Ok(result);
    }
}
