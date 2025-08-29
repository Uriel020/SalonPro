using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Context;

namespace Integracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public UserRolesController(SalonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userRoles = await _context.UserRoles
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .ToListAsync();
            return Ok(userRoles);
        }

        [HttpGet("{userId}/{roleId}")]
        public async Task<IActionResult> GetById(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null)
                return NotFound();

            return Ok(userRole);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRole userRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { userId = userRole.UserId, roleId = userRole.RoleId }, userRole);
        }

        [HttpDelete("{userId}/{roleId}")]
        public async Task<IActionResult> Delete(int userId, int roleId)
        {
            var userRole = await _context.UserRoles.FindAsync(userId, roleId);
            if (userRole == null)
                return NotFound();

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
