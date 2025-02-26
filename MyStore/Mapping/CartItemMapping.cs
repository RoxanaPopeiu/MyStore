using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mapping
{
    public static class CartItemMapping
    {
        public static CartItemDto ToCartItemDto(this CartItem cartItem)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                CartId = cartItem.CartId,
                Quantity = cartItem.Quantity,
                PriceAtTimeOfAddition = cartItem.PriceAtTimeOfAddition,
                ProductSizeId = cartItem.ProductSizeId, 

            };
        }
        public static List<CartItemDto> ToCartItemDtoList(this IEnumerable<CartItem> cartItems) 
        { 
            return cartItems.Select(cartItem=>cartItem.ToCartItemDto()).ToList();
        }
        public static CartItem ToCartItem(this CartItemDto cartItemDto)
        {
            return new CartItem
            {
                CartId = cartItemDto.CartId,
                ProductId=cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity,
                PriceAtTimeOfAddition = cartItemDto.PriceAtTimeOfAddition,
                ProductSizeId = cartItemDto.ProductSizeId
            };
        }
    }
}
