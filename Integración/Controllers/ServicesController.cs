using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Context;

namespace Integracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public ServicesController(SalonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _context.Services.Include(s => s.ServiceType).ToListAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _context.Services.Include(s => s.ServiceType).FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Service service)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Service service)
        {
            if (id != service.Id)
                return BadRequest();

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Services.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
