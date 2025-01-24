using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data.Context;
using Domain.Entities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartStatusController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public CartStatusController(SalonDbContext context)
        {
            _context = context;
        }

        // GET: api/CartStatus
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var cartStatuses = await _context.CartStatuses
                    .Where(cs => cs.DeletedAt == null)
                    .Select(cs => new
                    {
                        cs.Id,
                        cs.Name,
                        cs.CreatedAt,
                        cs.CreatedBy,
                        cs.UpdatedAt,
                        cs.UpdatedBy
                    })
                    .ToListAsync();

                return Ok(cartStatuses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // GET: api/CartStatus/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cartStatus = await _context.CartStatuses
                    .Where(cs => cs.Id == id && cs.DeletedAt == null)
                    .Select(cs => new
                    {
                        cs.Id,
                        cs.Name,
                        cs.CreatedAt,
                        cs.CreatedBy,
                        cs.UpdatedAt,
                        cs.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                if (cartStatus == null)
                {
                    return NotFound(new { message = "CartStatus not found" });
                }

                return Ok(cartStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // POST: api/CartStatus
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartStatus cartStatus)
        {
            try
            {
                if (cartStatus == null)
                {
                    return BadRequest(new { message = "Invalid data" });
                }

                cartStatus.CreatedAt = DateTime.UtcNow;
                cartStatus.CreatedBy = "System"; // Replace with user authentication logic if applicable
                cartStatus.UpdatedAt = DateTime.UtcNow;

                _context.CartStatuses.Add(cartStatus);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = cartStatus.Id }, cartStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // PUT: api/CartStatus/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CartStatus updatedCartStatus)
        {
            try
            {
                var cartStatus = await _context.CartStatuses.FindAsync(id);

                if (cartStatus == null)
                {
                    return NotFound(new { message = "CartStatus not found" });
                }

                cartStatus.Name = updatedCartStatus.Name ?? cartStatus.Name;
                cartStatus.UpdatedAt = DateTime.UtcNow;
                cartStatus.UpdatedBy = "System"; // Replace with user authentication logic if applicable

                await _context.SaveChangesAsync();

                return Ok(cartStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // DELETE: api/CartStatus/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] string deletedBy)
        {
            try
            {
                var cartStatus = await _context.CartStatuses.FindAsync(id);

                if (cartStatus == null)
                {
                    return NotFound(new { message = "CartStatus not found" });
                }

                cartStatus.DeletedAt = DateTime.UtcNow;
                cartStatus.DeletedBy = deletedBy ?? "Unknown";

                await _context.SaveChangesAsync();

                return Ok(new { message = "CartStatus deleted (soft delete)" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
