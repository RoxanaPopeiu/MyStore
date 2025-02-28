using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpPost("AddToCart/{userID:int}/{productId:int}/{productSizeId:int}/{quantity:int}")]
        public Task<CartDto> AddToCart(int userId, int productId, int? productSizeId, int quantity)
        {
            return cartService.AddToCart(userId,productId, productSizeId ?? 0, quantity);
        }
        [HttpGet("GetCartByUserId/{ID:int}")]
        public Task<CartDto> GetCartByUserId(int ID)
        {
            return cartService.GetCartByUserId(ID);
        }
        [HttpPut("UpdateCart/{userID:int}/{cartItemId:int}/{quantity:int}")]
        public Task<CartDto> UpdateCart(int userId, int cartItemId, int quantity)
        {
            return cartService.UpdateCart(userId,cartItemId,quantity);
        }
        [HttpDelete("Delete/{userID:int}/{cartItemId:int}")]
        public Task RemoveFromCart(int userId, int cartItemId)
        {
            return cartService.RemoveFromCart(userId,cartItemId);
        }
    }
}
