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
    public class OrdersController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public OrdersController(SalonDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .Include(o => o.State)
                    .Include(o => o.PaymentMethod)
                    .Where(o => o.Id == id && o.DeletedAt == null)
                    .Select(o => new
                    {
                        o.Id,
                        o.OrderDate,
                        o.Total,
                        State = o.State.Name,
                        PaymentMethod = o.PaymentMethod.Name,
                        OrderDetails = o.OrderDetails.Select(od => new
                        {
                            od.ProductId,
                            od.Count,
                            od.UnitPrice,
                            Product = new
                            {
                                od.Product.Name,
                                od.Product.Description,
                                od.Product.Price,
                                od.Product.ImageUrl
                            }
                        }),
                        o.CreatedAt,
                        o.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // POST: api/Orders/CreateOrderFromCart
        [HttpPost("CreateOrderFromCart")]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderFromCartRequest request)
        {
            try
            {
                // Obtener carrito y sus detalles
                var cart = await _context.ShoppingCarts
                    .Include(c => c.CartDetails)
                    .FirstOrDefaultAsync(c => c.Id == request.CartId);

                if (cart == null)
                    return NotFound(new { message = "Cart not found" });

                // Crear la orden
                var newOrder = new Order
                {
                    UserId = request.UserId,
                    Total = cart.CartDetails.Sum(cd => cd.Quantity * cd.UnitPrice),
                    OrderDate = DateTime.UtcNow,
                    StateId = 1, // Estado inicial de la orden (Ej. "Pendiente")
                    PaymentMethodId = cart.CartStatusId,
                    CartId = cart.Id
                };

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                // Copiar los detalles del carrito a OrderDetails
                var orderDetails = cart.CartDetails.Select(cd => new OrderDetail
                {
                    OrderId = newOrder.Id,
                    ProductId = cd.ProductId,
                    Count = cd.Quantity,
                    UnitPrice = cd.UnitPrice
                }).ToList();

                _context.OrderDetails.AddRange(orderDetails);

                // Cambiar el estado del carrito a "Procesado"
                var processedState = await _context.CartStatuses
                    .FirstOrDefaultAsync(cs => cs.Name == "Procesado");

                cart.CartStatusId = processedState.Id;

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .Include(o => o.State)
                    .Include(o => o.PaymentMethod)
                    .Select(o => new
                    {
                        o.Id,
                        o.UserId,
                        o.OrderDate,
                        o.Total,
                        State = o.State.Name,
                        PaymentMethod = o.PaymentMethod.Name,
                        OrderDetails = o.OrderDetails.Select(od => new
                        {
                            od.ProductId,
                            od.Count,
                            od.UnitPrice,
                            Product = new
                            {
                                od.Product.Name,
                                od.Product.Description,
                                od.Product.Price,
                                od.Product.ImageUrl
                            }
                        }),
                        o.CreatedAt,
                        o.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // GET: api/Orders/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId && o.DeletedAt == null)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .Include(o => o.State)
                    .Include(o => o.PaymentMethod)
                    .Select(o => new
                    {
                        o.Id,
                        o.OrderDate,
                        o.Total,
                        State = o.State.Name,
                        PaymentMethod = o.PaymentMethod.Name,
                        OrderDetails = o.OrderDetails.Select(od => new
                        {
                            od.ProductId,
                            od.Count,
                            od.UnitPrice,
                            Product = new
                            {
                                od.Product.Name,
                                od.Product.Description,
                                od.Product.Price,
                                od.Product.ImageUrl
                            }
                        }),
                        o.CreatedAt,
                        o.UpdatedAt
                    })
                    .ToListAsync();

                if (!orders.Any())
                    return NotFound(new { message = "No orders found for this user" });

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteOrderRequest request)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);

                if (order == null)
                    return NotFound(new { message = "Order not found" });

                order.DeletedAt = DateTime.UtcNow;
                order.DeletedBy = request.DeletedBy ?? "Unknown";

                await _context.SaveChangesAsync();

                return Ok(new { message = "Order deleted (soft delete)" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }

    // Request Models
    public class CreateOrderFromCartRequest
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
    }

    public class DeleteOrderRequest
    {
        public string DeletedBy { get; set; }
    }
}
