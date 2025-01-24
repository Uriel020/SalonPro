using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Integracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public NotificationsController(SalonDbContext context)
        {
            _context = context;
        }

        // GET: api/Notifications/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.UserId == userId)
                    .OrderByDescending(n => n.CreatedDate)
                    .Select(n => new
                    {
                        n.Id,
                        n.Title,
                        n.Message,
                        n.Type,
                        n.IsRead,
                        n.CreatedDate
                    })
                    .ToListAsync();

                if (!notifications.Any())
                {
                    return NotFound(new { message = "No notifications found for this user" });
                }

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // POST: api/Notifications
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notification request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newNotification = new Notification
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Message = request.Message,
                    Type = request.Type,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Notifications.Add(newNotification);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetByUser), new { userId = request.UserId }, newNotification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
