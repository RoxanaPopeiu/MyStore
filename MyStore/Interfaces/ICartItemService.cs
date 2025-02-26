using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Interfaces
{
    public interface ICartItemService
    {
        Task<CartItemDto> GetCartItemById(int cartItemId);
        Task<CartItemDto> AddCartItem(CartItemDto cartItemDto);
        Task<CartItemDto> UpdateCartItemQuantity(int cartItemId, int quantity);
        Task RemoveCartItem(int cartItemId);
        
    }
}
