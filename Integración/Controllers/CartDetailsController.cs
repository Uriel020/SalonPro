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
    public class CartDetailsController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public CartDetailsController(SalonDbContext context)
        {
            _context = context;
        }

        // Upsert (Crear o actualizar un detalle del carrito)
        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertCartDetails([FromBody] CartDetail cartDetail)
        {
            try
            {
                // Verificar si el carrito existe
                var cart = await _context.ShoppingCarts.FindAsync(cartDetail.CartId);
                if (cart == null)
                    return NotFound(new { message = "Shopping cart not found" });

                // Verificar si el producto existe
                var product = await _context.Products.FindAsync(cartDetail.ProductId);
                if (product == null)
                    return NotFound(new { message = "Product not found" });

                // Intentar encontrar el detalle del carrito
                var existingCartDetail = await _context.CartDetails
                    .FirstOrDefaultAsync(cd => cd.CartId == cartDetail.CartId && cd.ProductId == cartDetail.ProductId);

                if (existingCartDetail != null)
                {
                    // Si el detalle ya existe, actualizar cantidad y precio unitario
                    existingCartDetail.Quantity = cartDetail.Quantity;
                    existingCartDetail.UnitPrice = cartDetail.UnitPrice != 0 ? cartDetail.UnitPrice : existingCartDetail.UnitPrice;
                    existingCartDetail.UpdatedAt = DateTime.UtcNow;
                    existingCartDetail.UpdatedBy = "System"; // Cambiar por el usuario autenticado

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Cart detail updated successfully", existingCartDetail });
                }
                else
                {
                    // Si no existe el detalle, crearlo
                    cartDetail.CreatedAt = DateTime.UtcNow;
                    cartDetail.CreatedBy = "System"; // Cambiar por el usuario autenticado
                    cartDetail.UpdatedAt = DateTime.UtcNow;

                    _context.CartDetails.Add(cartDetail);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetCartDetails), new { cartId = cartDetail.CartId }, cartDetail);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }


        // Obtener detalles del carrito por CartID
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartDetails(int cartId)
        {
            try
            {
                var cartDetails = await _context.CartDetails
                    .Where(cd => cd.CartId == cartId)
                    .Include(cd => cd.Product)
                    .Select(cd => new
                    {
                        cd.Id,
                        cd.CartId,
                        cd.ProductId,
                        cd.Quantity,
                        cd.UnitPrice,
                        Product = new
                        {
                            cd.Product.Id,
                            cd.Product.Name,
                            cd.Product.Description,
                            cd.Product.Price,
                            cd.Product.ImageUrl
                        }
                    })
                    .ToListAsync();

                if (!cartDetails.Any())
                    return NotFound(new { message = "No cart details found for this cart" });

                return Ok(cartDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
