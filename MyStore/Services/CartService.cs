using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyStore.Services
{
    public class CartService(ApplicationDbContext context, ICartItemService cartItemService) : ICartService
    {

        public async Task<CartDto> GetCartByUserId(int userId)
        {
            var cart = await context.Carts
              .Include(c => c.CartItems)
                  .ThenInclude(ci => ci.Product)
                  .ThenInclude(p => p.ProductSizes)   
              .SingleOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found.");
            }
            await UpdateTotalPrice(cart);
            var cartDto=cart.ToCartDto();
            return cartDto;

        }
        public async Task<CartDto> AddToCart(int userId, int productId, int? productSizeId, int quantity)
        {
            //Retrive the user's cart or create a new one if it doesn't exist
            var cart = context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    TotalPrice = 0,
                    CartItems = new List<CartItem>()
                };
                context.Carts.Add(cart);
                await context.SaveChangesAsync();//Save to get the new Cart.Id
            }
            //Check if the product exists
            var product = context.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }
            //Validate that the specified ProductSize exists 
            var productSize = context.ProductSizes
             .FirstOrDefault(ps => ps.ProductId == productId && ps.SizeId == productSizeId);

            if (productSize == null)
            {
                throw new InvalidOperationException("Invalid ProductSizeId: The specified ProductSize does not exist.");
            }
            //Check if the item already exists in the cart
            var cartItem = context.CartItems
                      .FirstOrDefault(ci => ci.ProductId == productId && ci.CartId == cart.Id);
            //If the item doesn't exist, create a new one
            CartItemDto cartItemDto;

            if (cartItem == null)  
            {
                cartItemDto = new CartItemDto
                {
                    ProductId = productId,
                    CartId = cart.Id,
                    Quantity = quantity,
                    PriceAtTimeOfAddition = product.Price,
                    ProductSizeId = productSize.Id
                };
            }
            else // If it exists, update the quantity
            {
                cartItemDto = cartItem.ToCartItemDto(); 
                cartItemDto.Quantity += quantity; 
            }

            //Save the item to the database
            await cartItemService.AddCartItem(cartItemDto);
            //Update the total cart price and return the updated cart
            return await UpdateTotalPrice(cart);
        }
        public async Task<CartDto> UpdateCart(int userId, int cartItemId, int quantity)
        {
            var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found.");
            }
            //  Folosim CartItemService pentru a actualiza cantitatea
            var updatedItem = await cartItemService.UpdateCartItemQuantity(cartItemId, quantity);
            return await UpdateTotalPrice(cart);

        }
        public async Task RemoveFromCart(int userId, int cartItemId)
        {
            var cartItem = await context.CartItems
                .Include(ci => ci.Cart) 
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);

            if (cartItem == null)
                throw new Exception("Cart item not found or does not belong to this user.");

            var cart = cartItem.Cart;

            context.CartItems.Remove(cartItem);
            await context.SaveChangesAsync(); 

            cart.TotalPrice = context.CartItems
                .Where(ci => ci.CartId == cart.Id)
                .Sum(ci => ci.PriceAtTimeOfAddition * ci.Quantity);


            if (!cart.CartItems.Any())
            {
                context.Carts.Remove(cart);
            }

            await context.SaveChangesAsync(); 
        }
        private async Task<CartDto> UpdateTotalPrice(Cart cart)
        {
            cart.TotalPrice = await context.CartItems
                .Where(ci => ci.CartId == cart.Id)
                .SumAsync(ci => ci.PriceAtTimeOfAddition * ci.Quantity);

            await context.SaveChangesAsync();
            return cart.ToCartDto();
        }


    }
}
