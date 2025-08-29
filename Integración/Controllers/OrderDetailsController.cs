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
    public class OrderDetailsController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public OrderDetailsController(SalonDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orderDetails = await _context.OrderDetails
                    .Where(od => od.DeletedAt == null)
                    .Select(od => new
                    {
                        od.Id,
                        od.OrderId,
                        od.ProductId,
                        od.Count,
                        od.UnitPrice,
                        od.CreatedAt,
                        od.CreatedBy,
                        od.UpdatedAt,
                        od.UpdatedBy
                    })
                    .ToListAsync();

                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // GET: api/OrderDetails/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var orderDetail = await _context.OrderDetails
                    .Where(od => od.Id == id && od.DeletedAt == null)
                    .Select(od => new
                    {
                        od.Id,
                        od.OrderId,
                        od.ProductId,
                        od.Count,
                        od.UnitPrice,
                        od.CreatedAt,
                        od.CreatedBy,
                        od.UpdatedAt,
                        od.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                if (orderDetail == null)
                    return NotFound(new { message = "OrderDetails not found" });

                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // POST: api/OrderDetails
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDetail orderDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                orderDetail.CreatedAt = DateTime.UtcNow;
                orderDetail.CreatedBy = "System"; // Cambiar a usuario real más adelante

                _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = orderDetail.Id }, orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // PUT: api/OrderDetails/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDetail orderDetail)
        {
            try
            {
                var existingOrderDetail = await _context.OrderDetails.FindAsync(id);

                if (existingOrderDetail == null)
                    return NotFound(new { message = "OrderDetails not found" });

                existingOrderDetail.OrderId = orderDetail.OrderId != 0 ? orderDetail.OrderId : existingOrderDetail.OrderId;
                existingOrderDetail.ProductId = orderDetail.ProductId != 0 ? orderDetail.ProductId : existingOrderDetail.ProductId;
                existingOrderDetail.Count = orderDetail.Count != 0 ? orderDetail.Count : existingOrderDetail.Count;
                existingOrderDetail.UnitPrice = orderDetail.UnitPrice != 0 ? orderDetail.UnitPrice : existingOrderDetail.UnitPrice;
                existingOrderDetail.UpdatedAt = DateTime.UtcNow;
                existingOrderDetail.UpdatedBy = "System"; // Cambiar a usuario real más adelante

                await _context.SaveChangesAsync();

                return Ok(existingOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // DELETE: api/OrderDetails/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] string deletedBy)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FindAsync(id);

                if (orderDetail == null)
                    return NotFound(new { message = "OrderDetails not found" });

                orderDetail.DeletedAt = DateTime.UtcNow;
                orderDetail.DeletedBy = deletedBy ?? "Unknown";

                await _context.SaveChangesAsync();

                return Ok(new { message = "OrderDetails deleted (soft delete)" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
