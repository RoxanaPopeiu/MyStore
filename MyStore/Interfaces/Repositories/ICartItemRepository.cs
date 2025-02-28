using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Interfaces.Repositories
{
    public interface ICartItemRepository : IBaseRepository<CartItem>
    {
        Task<CartItem?> GetByCartAndProductAsync(int cartId, int productId);
        Task<CartItem> GetByCartItemAndUserIdAsync(int userId, int cartItemId);
        Task<CartItem?> GetCartItemWitheDetails(CartItemDto cartItemDto);
    }
}
