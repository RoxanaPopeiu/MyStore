using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class CartItemService(ApplicationDbContext context) : ICartItemService
    {
        public async Task<CartItemDto> AddCartItem(CartItemDto cartItemDto)
        {
            var existingItem = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItemDto.ProductId
                                        && ci.CartId == cartItemDto.CartId
                                        && ci.ProductSizeId == cartItemDto.ProductSizeId);

            if (existingItem != null)//update quantity
            {
                return await UpdateCartItemQuantity(existingItem.Id, cartItemDto.Quantity);

            }
            // If it doesn't exist, create a new cart item
            var cartItem = cartItemDto.ToCartItem();
            context.CartItems.Add(cartItem);
            await context.SaveChangesAsync();

            return cartItem.ToCartItemDto();
        }

        public async Task<CartItemDto> UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var existingItem = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (existingItem == null)
            {
                throw new KeyNotFoundException("Cart item not found.");
            }

            existingItem.Quantity = quantity;
            await context.SaveChangesAsync();

            return existingItem.ToCartItemDto();
        }


        public async Task<CartItemDto> GetCartItemById(int cartItemId)
        {
            var extCartItem = await context.CartItems
                 .Include(x => x.Product)
                 .Include(x => x.ProductSize)
                 .FirstOrDefaultAsync(x => x.Id == cartItemId);

            if (extCartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found.");
            }

            return extCartItem.ToCartItemDto();
        }

        public async Task RemoveCartItem(int cartItemId)
        {
            var cartItem = await context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                context.CartItems.Remove(cartItem);
                await context.SaveChangesAsync();
            }
        }

    }
}
