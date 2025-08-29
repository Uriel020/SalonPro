using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Integracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public CategoriesController(SalonDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? userId)
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.ServiceTypes)
                    .ThenInclude(st => st.Services)
                    .ToListAsync();

                if (!categories.Any())
                    return NotFound(new { message = "No categories found" });

                var likedProducts = userId.HasValue
                    ? await _context.UserLikedProducts
                        .Where(lp => lp.UserId == userId.Value)
                        .Select(lp => lp.ProductId)
                        .ToListAsync()
                    : new List<int>();

                var result = categories.Select(category => new
                {
                    category.Id,
                    category.Name,
                    category.Description,
                    ServiceTypes = category.ServiceTypes.Select(st => new
                    {
                        st.Id,
                        st.Name,
                        st.Description,
                        Services = st.Services.Select(s => new
                        {
                            s.Id,
                            s.Description,
                            s.Price,
                            s.IsActive,
                            IsLiked = likedProducts.Contains(s.Id)
                        })
                    })
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // GET: api/Categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.ServiceTypes)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return NotFound(new { message = "Category not found" });

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // PUT: api/Categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            try
            {
                if (id != category.Id)
                    return BadRequest(new { message = "ID mismatch" });

                var existingCategory = await _context.Categories.FindAsync(id);
                if (existingCategory == null)
                    return NotFound(new { message = "Category not found" });

                existingCategory.Name = category.Name ?? existingCategory.Name;
                existingCategory.Description = category.Description ?? existingCategory.Description;

                _context.Entry(existingCategory).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(c => c.Id == id))
                    return NotFound(new { message = "Category not found" });

                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // DELETE: api/Categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return NotFound(new { message = "Category not found" });

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Category deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
