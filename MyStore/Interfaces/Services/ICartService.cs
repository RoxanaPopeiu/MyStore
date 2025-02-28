using MyStore.DTO;

namespace MyStore.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCartByUserId(int userId);
        Task<CartDto> AddToCart(int userId, int productId, int prdouctSizeId, int quantity);
        Task<CartDto> UpdateCart(int userId, int cartItemId, int quantity);
        Task RemoveFromCart(int userId, int cartItemId);
    }
}
