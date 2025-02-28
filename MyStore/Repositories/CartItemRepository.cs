using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Models;

namespace MyStore.Repositories
{
    public class CartItemRepository(ApplicationDbContext context) : BaseRepository<CartItem>(context), ICartItemRepository
    {
        public async Task<CartItem?> GetByCartAndProductAsync(int cartId, int productId)
        {
            return await _dbSet.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
        public async Task<CartItem> GetByCartItemAndUserIdAsync(int userId, int cartItemId)
        {
            return await _dbSet
                            .Include(ci => ci.Cart) 
                            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);
        }
        public async Task<CartItem?> GetCartItemWitheDetails(CartItemDto cartItemDto)
        {
            return  await _dbSet.FirstOrDefaultAsync(ci => ci.ProductId == cartItemDto.ProductId
                            && ci.CartId == cartItemDto.CartId
                            && ci.ProductSizeId == cartItemDto.ProductSizeId);
        }
    }
}
