using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Repositories
{
    public class CartRepository(ApplicationDbContext context) : BaseRepository<Cart>(context), ICartRepository
    {

        public async Task<CartDto> GetCartWithDetailsAsync(int userId)
        {
            var cart = await _dbSet
              .Include(c => c.CartItems)
                  .ThenInclude(ci => ci.Product)
                  .ThenInclude(p => p.ProductSizes)
                  .AsNoTracking()
              .FirstOrDefaultAsync(c => c.UserId == userId);
            var cartDto = cart.ToCartDto();
            return cartDto;

        }
        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
