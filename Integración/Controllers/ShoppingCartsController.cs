using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Context;

namespace Integracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public ShoppingCartsController(SalonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shoppingCarts = await _context.ShoppingCarts.Include(s => s.User).ToListAsync();
            return Ok(shoppingCarts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shoppingCart = await _context.ShoppingCarts.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
            if (shoppingCart == null)
                return NotFound();

            return Ok(shoppingCart);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShoppingCart shoppingCart)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = shoppingCart.Id }, shoppingCart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
                return BadRequest();

            _context.Entry(shoppingCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ShoppingCarts.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
                return NotFound();

            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
