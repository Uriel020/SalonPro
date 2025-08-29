using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data.Context;
using Domain.Entities;

namespace Integracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public OrderStatusController(SalonDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderStatus
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orderStatuses = await _context.OrderStatuses
                    .Where(os => os.DeletedAt == null)
                    .Select(os => new
                    {
                        os.Id,
                        os.Name,
                        os.CreatedAt,
                        os.CreatedBy,
                        os.UpdatedAt,
                        os.UpdatedBy
                    })
                    .ToListAsync();

                return Ok(orderStatuses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // GET: api/OrderStatus/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var orderStatus = await _context.OrderStatuses
                    .Where(os => os.Id == id && os.DeletedAt == null)
                    .Select(os => new
                    {
                        os.Id,
                        os.Name,
                        os.CreatedAt,
                        os.CreatedBy,
                        os.UpdatedAt,
                        os.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                if (orderStatus == null)
                    return NotFound(new { message = "OrderStatus not found" });

                return Ok(orderStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // POST: api/OrderStatus
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderStatus orderStatus)
        {
            try
            {
                if (orderStatus == null || string.IsNullOrEmpty(orderStatus.Name))
                    return BadRequest(new { message = "Invalid data" });

                orderStatus.CreatedAt = DateTime.UtcNow;
                orderStatus.CreatedBy = "System"; // Puedes ajustar esto según el usuario autenticado.

                _context.OrderStatuses.Add(orderStatus);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = orderStatus.Id }, orderStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // PUT: api/OrderStatus/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderStatus updatedOrderStatus)
        {
            try
            {
                var orderStatus = await _context.OrderStatuses.FindAsync(id);

                if (orderStatus == null)
                    return NotFound(new { message = "OrderStatus not found" });

                orderStatus.Name = updatedOrderStatus.Name ?? orderStatus.Name;
                orderStatus.UpdatedAt = DateTime.UtcNow;
                orderStatus.UpdatedBy = "System"; // Puedes ajustar esto según el usuario autenticado.

                await _context.SaveChangesAsync();

                return Ok(orderStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // DELETE: api/OrderStatus/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteOrderStatusRequest request)
        {
            try
            {
                var orderStatus = await _context.OrderStatuses.FindAsync(id);

                if (orderStatus == null)
                    return NotFound(new { message = "OrderStatus not found" });

                orderStatus.DeletedAt = DateTime.UtcNow;
                orderStatus.DeletedBy = request.DeletedBy ?? "System";

                await _context.SaveChangesAsync();

                return Ok(new { message = "OrderStatus deleted (soft delete)" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }

    // Modelo para la solicitud de eliminación
    public class DeleteOrderStatusRequest
    {
        public string DeletedBy { get; set; }
    }
}
