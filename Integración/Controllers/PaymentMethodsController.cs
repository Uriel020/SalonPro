using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data.Context;
using Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public PaymentMethodsController(SalonDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentMethods
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var paymentMethods = _context.PaymentMethods
                    .Where(pm => pm.DeletedAt == null)
                    .Select(pm => new
                    {
                        pm.Id,
                        pm.Name,
                        pm.CreatedAt,
                        pm.CreatedBy,
                        pm.UpdatedAt,
                        pm.UpdatedBy
                    })
                    .ToList();

                return Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching payment methods: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/PaymentMethods/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paymentMethod = _context.PaymentMethods
                    .Where(pm => pm.DeletedAt == null && pm.Id == id)
                    .Select(pm => new
                    {
                        pm.Id,
                        pm.Name,
                        pm.CreatedAt,
                        pm.CreatedBy,
                        pm.UpdatedAt,
                        pm.UpdatedBy
                    })
                    .FirstOrDefault();

                if (paymentMethod == null)
                {
                    return NotFound(new { Message = "Payment method not found" });
                }

                return Ok(paymentMethod);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching payment method by ID: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/PaymentMethods
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethod paymentMethod)
        {
            try
            {
                paymentMethod.CreatedAt = DateTime.UtcNow;
                paymentMethod.CreatedBy = "System";
                _context.PaymentMethods.Add(paymentMethod);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = paymentMethod.Id }, paymentMethod);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating payment method: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/PaymentMethods/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethod updatedPaymentMethod)
        {
            try
            {
                var paymentMethod = _context.PaymentMethods.FirstOrDefault(pm => pm.Id == id && pm.DeletedAt == null);
                if (paymentMethod == null)
                {
                    return NotFound(new { Message = "Payment method not found" });
                }

                paymentMethod.Name = updatedPaymentMethod.Name ?? paymentMethod.Name;
                paymentMethod.UpdatedAt = DateTime.UtcNow;
                paymentMethod.UpdatedBy = "System";

                _context.PaymentMethods.Update(paymentMethod);
                await _context.SaveChangesAsync();

                return Ok(paymentMethod);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating payment method: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/PaymentMethods/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] string deletedBy)
        {
            try
            {
                var paymentMethod = _context.PaymentMethods.FirstOrDefault(pm => pm.Id == id && pm.DeletedAt == null);
                if (paymentMethod == null)
                {
                    return NotFound(new { Message = "Payment method not found" });
                }

                paymentMethod.DeletedAt = DateTime.UtcNow;
                paymentMethod.DeletedBy = deletedBy ?? "Unknown";

                _context.PaymentMethods.Update(paymentMethod);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Payment method deleted (soft delete)" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting payment method: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
