using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mapping
{
    public static class CartMapping
    {
        public static CartDto ToCartDto(this Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.TotalPrice,
                CartItems = cart.CartItems?.Select(c => c.ToCartItemDto()).ToList() ?? new List<CartItemDto>()

            };
        }
        public static List<CartDto> ToCartDtoList(this IEnumerable<Cart> carts)
        {
            return carts.Select(cart=>cart.ToCartDto()).ToList();
        }
        public static Cart ToCart(this CartDto cartDto)
        {
            return new Cart
            {
                Id = cartDto.Id,
                UserId = cartDto.UserId,
                TotalPrice = cartDto.TotalPrice,
                CartItems = cartDto.CartItems?.Select(item => item.ToCartItem()).ToList() ?? new List<CartItem>()
            };
        }

    }
}
