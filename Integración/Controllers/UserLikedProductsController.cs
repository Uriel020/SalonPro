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
    public class UserLikedProductsController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public UserLikedProductsController(SalonDbContext context)
        {
            _context = context;
        }

        // POST: api/UserLikedProducts/like
        [HttpPost("like")]
        public async Task<IActionResult> LikeProduct([FromBody] UserLikedProduct request)
        {
            try
            {
                var existingLike = await _context.UserLikedProducts
                    .FirstOrDefaultAsync(ulp => ulp.UserId == request.UserId && ulp.ProductId == request.ProductId);

                if (existingLike != null)
                {
                    return BadRequest(new { message = "Product already liked" });
                }

                var newLike = new UserLikedProduct
                {
                    UserId = request.UserId,
                    ProductId = request.ProductId
                };

                _context.UserLikedProducts.Add(newLike);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLikedProducts), new { userId = request.UserId }, newLike);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // DELETE: api/UserLikedProducts/unlike
        [HttpDelete("unlike")]
        public async Task<IActionResult> UnlikeProduct([FromBody] UserLikedProduct request)
        {
            try
            {
                var existingLike = await _context.UserLikedProducts
                    .FirstOrDefaultAsync(ulp => ulp.UserId == request.UserId && ulp.ProductId == request.ProductId);

                if (existingLike == null)
                {
                    return NotFound(new { message = "Like not found" });
                }

                _context.UserLikedProducts.Remove(existingLike);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // GET: api/UserLikedProducts/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetLikedProducts(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.LikedProducts)
                    .ThenInclude(lp => lp.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var likedProducts = user.LikedProducts.Select(lp => new
                {
                    lp.Product.Id,
                    lp.Product.Name,
                    lp.Product.Description,
                    lp.Product.Price,
                    lp.Product.Stock,
                    lp.Product.CategoryId,
                    lp.Product.ImageUrl,
                    lp.Product.Color,
                    lp.Product.Brand,
                    lp.Product.Weight,
                    lp.Product.Size,
                    lp.Product.ExpiryDate
                });

                return Ok(likedProducts);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
