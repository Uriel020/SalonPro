using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Context;

namespace Integracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTypesController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public ServiceTypesController(SalonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceTypes = await _context.ServiceTypes.Include(st => st.Category).ToListAsync();
            return Ok(serviceTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var serviceType = await _context.ServiceTypes.Include(st => st.Category).FirstOrDefaultAsync(st => st.Id == id);
            if (serviceType == null)
                return NotFound();

            return Ok(serviceType);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceType serviceType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.ServiceTypes.Add(serviceType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = serviceType.Id }, serviceType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceType serviceType)
        {
            if (id != serviceType.Id)
                return BadRequest();

            _context.Entry(serviceType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ServiceTypes.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceType = await _context.ServiceTypes.FindAsync(id);
            if (serviceType == null)
                return NotFound();

            _context.ServiceTypes.Remove(serviceType);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
