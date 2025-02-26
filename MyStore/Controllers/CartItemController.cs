using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartItemController(ICartItemService cartItemService) : ControllerBase
    {
        [HttpPost("AddCartItem")]
        public Task<CartItemDto> AddCartItem([FromBody] CartItemDto cartItemDto)
        {
            return cartItemService.AddCartItem(cartItemDto);
        }
        [HttpGet("GetOneCartItem/{ID:int}")]
        public Task<CartItemDto> GetCartItem(int ID)
        {
            return  cartItemService.GetCartItemById(ID);
        }
        [HttpPut("Update/{ID:int}/{quantity:int}")]
        public Task<CartItemDto> Update(int ID, int quantity)
        {
            return cartItemService.UpdateCartItemQuantity(ID, quantity);
        }
        [HttpDelete("Delete/{ID:int}")]
        public Task RemoveCartItem(int ID)
        {
            return cartItemService.RemoveCartItem(ID);
        }
    }
}
