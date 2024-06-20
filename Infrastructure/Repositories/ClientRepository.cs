using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Repositories
{
    public class ClientRepository (AppDbContext context): IClientRepository
    {
       
        
        public async Task<PlateResponse> AddPlateToCartAsync(string plateId, string userId)
        {
            int Quantity = 1;
            var plate = await context.Plates.FindAsync(plateId);
            if (plate == null)
                return new PlateResponse(flag:false, message:"The plate is not found!");

            var getUser = await context.Users.FindAsync(userId);
            
            if(getUser == null)
                return new PlateResponse(flag: false, message: "The user is not found!");

            var getUserCart = await context.Carts.Include(p => p.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (getUserCart == null)
                return new PlateResponse(flag: false, message: "The user's cart is not found!");



            var existingCartItem = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.PlateId == plate.Id && ci.CartId == getUser.CartId);

            if (existingCartItem != null)
            {
                // If the plate is already in the cart, increase the quantity
                existingCartItem.Quantity += Quantity;
                existingCartItem.Total = existingCartItem.Quantity * existingCartItem.PlatePrice;

                getUserCart.CartTotal += (decimal)(Quantity * existingCartItem.PlatePrice); // Update only with the additional quantity
                // Save the updated cart item
                await SaveChangesAsync();
                return new PlateResponse(flag: true, message: $"{plate.PlateName} quantity increased to {existingCartItem.Quantity}");
                
            }

            else
            {
                var c = new CartItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    CartId = getUser.CartId,
                    PlateId = plate.Id,
                    PlateName = plate.PlateName,
                    PlatePrice = plate.PlatePrice,
                    Quantity = Quantity,
                    Total = Quantity * plate.PlatePrice
                };

                getUserCart.CartTotal += (decimal)c.Total;
               


                await context.CartItems.AddAsync(c);
           
            }
            //getUserCart.CartTotal += (decimal)(Quantity * plate.PlatePrice);
            
            await SaveChangesAsync();
            return new PlateResponse(flag: true, message: "The plate is here :)");
        }     

        public async Task<PlateResponse> RemovePlateFromCartAsync(string plateId, string userId)
        {
            var plate = await context.Plates.FindAsync(plateId);
            if (plate == null)
                return new PlateResponse(flag: false, message: "The plate is not found!");

            var getUser = await context.Users.FindAsync(userId);
            if (getUser == null) return new PlateResponse(flag: false, message:"No user was selected");

            var getUserCart = await context.Carts.Include(p => p.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (getUserCart == null)
                return new PlateResponse(flag: false, message: "The user's cart is not found!");

            var cartItemToRemove = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.PlateId == plate.Id && ci.CartId == getUser.CartId);
           
            if (cartItemToRemove == null)
                return new PlateResponse(flag: false, message: "The plate is not in the cart!");

            if (cartItemToRemove.Quantity > 1)
            {
                cartItemToRemove.Quantity --;
                cartItemToRemove.Total = cartItemToRemove.Total - cartItemToRemove.PlatePrice;
                getUserCart.CartTotal = (decimal)cartItemToRemove.Total;
                await context.SaveChangesAsync();
            }
            else
            {
                context.CartItems.Remove(cartItemToRemove);
            }
            

            await SaveChangesAsync();
            
            return new PlateResponse(flag: true, message: "The plate has been removed from the cart.");
        }

        public async Task<IEnumerable<CartItem>> GetAllCartPlatesAsync(string userId)
        {
            var getUser = await context.Users.FindAsync(userId);
            if (getUser == null)
            {
                Console.WriteLine("User not found");
                return Enumerable.Empty<CartItem>();
            }


            var userCartItems = await context.CartItems
             .Where(ci => ci.CartId == getUser.CartId)
             .ToListAsync();

            if (userCartItems == null || !userCartItems.Any())
            {
                Console.WriteLine("User's cart not found");
                return Enumerable.Empty<CartItem>();
            }

            return userCartItems;

        }

        private async Task SaveChangesAsync() => await context.SaveChangesAsync();
    }
}
