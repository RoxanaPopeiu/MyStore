using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class CartItemService(ICartItemRepository cartItemRepository) : ICartItemService
    {
        public async Task<CartItemDto> AddCartItem(CartItemDto cartItemDto)
        {
            var existingItem = await cartItemRepository.GetCartItemWitheDetails(cartItemDto);
            if (existingItem != null)
            {
                return await UpdateCartItemQuantity(existingItem.Id, cartItemDto.Quantity);

            }
            var cartItem = cartItemDto.ToCartItem();
            await cartItemRepository.AddAsync(cartItem);

            return cartItem.ToCartItemDto();
        }

        public async Task<CartItemDto> UpdateCartItemQuantity(int cartItemId, int quantity)
        {
           var existingItem = await cartItemRepository.GetByIdAsync(cartItemId);
           if (existingItem == null)
            {
                throw new KeyNotFoundException("Cart item not found.");
            }

            existingItem.Quantity = quantity;
            await cartItemRepository.UpdateAsync(existingItem);
            return existingItem.ToCartItemDto();
        }


        public async Task<CartItemDto> GetCartItemById(int cartItemId)
        {
            var extCartItem = await cartItemRepository.GetByIdAsync(cartItemId);

            if (extCartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found.");
            }

            return extCartItem.ToCartItemDto();
        }

        public async Task RemoveCartItem(int cartItemId)
        {
            var cartItem = await cartItemRepository.GetByIdAsync(cartItemId);
            if (cartItem != null)
            {
                await cartItemRepository.DeleteAsync(cartItem);
            }
        }

    }
}
