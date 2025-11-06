using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.Models;

namespace OrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/producttypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetAll()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        // GET: api/producttypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetById(int id)
        {
            var type = await _context.ProductTypes.FindAsync(id);
            if (type == null)
                return NotFound();

            return type;
        }

        // POST: api/producttypes
        [HttpPost]
        public async Task<ActionResult<ProductType>> Create(ProductType type)
        {
            _context.ProductTypes.Add(type);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
        }

        // PUT: api/producttypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductType type)
        {
            if (id != type.Id)
                return BadRequest();

            _context.Entry(type).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/producttypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.ProductTypes.FindAsync(id);
            if (type == null)
                return NotFound();

            _context.ProductTypes.Remove(type);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
