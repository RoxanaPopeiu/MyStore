using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyStore.Services
{
    public class CartService(ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        ISizeRepository sizeRepository) : ICartService
    {

        public async Task<CartDto> GetCartByUserId(int userId)
        {
            var cartDto = await cartRepository.GetCartWithDetailsAsync(userId);
            if (cartDto == null)
            {
                throw new KeyNotFoundException("Cart not found.");
            }
            
            return await UpdateTotalPrice(cartDto);
        }

        public async Task<CartDto> AddToCart(int userId, int productId, int productSizeId, int quantity)
        {
            // Retrieve the user's cart or create a new one if it doesn't exist
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    TotalPrice = 0,
                    CartItems = new List<CartItem>()
                };
                await cartRepository.AddAsync(cart);
            }

            // Check if the product exists
            var product = await productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            // Validate that the specified ProductSize exists and belongs to the correct Product
            var productSize = await sizeRepository.GetBySizeIdAsync(productSizeId);
            if (productSize == null)
            {
                throw new InvalidOperationException($"Invalid ProductSizeId: {productSizeId} does not exist for ProductId: {productId}.");
            }

            // Check if the item already exists in the cart
            var cartItem = await cartItemRepository.GetByCartAndProductAsync(cart.Id, productId);

            // If the item doesn't exist, create a new one
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    CartId = cart.Id,
                    Quantity = quantity,
                    PriceAtTimeOfAddition = product.Price,
                    ProductSizeId = productSize.Id
                };
                await cartItemRepository.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                await cartItemRepository.UpdateAsync(cartItem);
            }

            return await UpdateTotalPrice(cart.ToCartDto());
        }

        public async Task<CartDto> UpdateCart(int userId, int cartItemId, int quantity)
        {
            var cartItem = await cartItemRepository.GetByIdAsync(cartItemId);
            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart not found.");
            }

            cartItem.Quantity = quantity;
            await cartItemRepository.UpdateAsync(cartItem);
            var cartDto = await cartRepository.GetCartWithDetailsAsync(userId);

            return await UpdateTotalPrice(cartDto);
        }

        public async Task RemoveFromCart(int userId, int cartItemId)
        {
            var cartItem = await cartItemRepository.GetByCartItemAndUserIdAsync(userId, cartItemId);
            if (cartItem == null)
                throw new Exception("Cart item not found or does not belong to this user.");

            var cart = await cartRepository.GetByIdAsync(cartItem.CartId);
            await cartItemRepository.DeleteAsync(cartItem);

            cart.TotalPrice = cart.CartItems.Sum(ci => ci.PriceAtTimeOfAddition * ci.Quantity);
            await cartRepository.UpdateAsync(cart);

            if (!cart.CartItems.Any())
            {
                await cartRepository.DeleteAsync(cart);
            }
        }

        private async Task<CartDto> UpdateTotalPrice(CartDto cartDto)
        {
            var cart = cartDto.ToCart();
            cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.PriceAtTimeOfAddition);
            await cartRepository.UpdateAsync(cart);
            return cart.ToCartDto();
        }

    }

}
