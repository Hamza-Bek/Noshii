﻿using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Domain.Models.Order;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Repositories
{
    public class OrderRepository(AppDbContext context , IMapper mapper) : IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetAllOrders()=> await context.Orders.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<Order>> GetOrder(string userId)
        {
            var getUser = await context.Users.FindAsync(userId);
            if (getUser == null)
            {
                Console.WriteLine("User not found");
                return Enumerable.Empty<Order>();
            }


            var getUserOrder = await context.Orders
             .Where(ci => ci.UserId == getUser.Id)
             .ToListAsync();

            if (getUserOrder == null || !getUserOrder.Any())
            {
                Console.WriteLine("User's cart not found");
                return Enumerable.Empty<Order>();
            }

            return (IEnumerable<Order>)getUserOrder;
        }

        public async Task<OrderResponse> PlaceOrderAsync(OrderDTO order , string userId ,string cartId)
        {
            var map = mapper.Map<Order>(order);

            var getUser = await context.Users.FindAsync(userId);
            if(getUser == null)
                return new OrderResponse(flag: false, message: "user not found");

            var getUserCart = await context.Carts.Include(p => p.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (getUserCart == null)
                return new OrderResponse(flag: false, message: "The user's cart is not found!");

            var getStatus = await context.OrderStatuses.FirstOrDefaultAsync(s => s.Status == order.OrderStatus);
            if (getStatus == null)
                return new OrderResponse(flag: false, message: $"Invalid OrderStatus: {order.OrderStatus}");

            // Check if the OrderStatus retrieved from the database matches the Order's OrderStatus
            if (getStatus.Status != order.OrderStatus)
                return new OrderResponse(flag: false, message: $"Mismatched OrderStatus: {order.OrderStatus}");

            decimal orderTotal = (decimal)getUserCart.CartTotal;

            var o = new Order()
            {
                OrderId = order.OrderId,
                UserId = userId,
                OrderDate = order.OrderDate,
                OrderTotal = orderTotal,
                OrderStatus = order.OrderStatus
            };      

            context.Orders.Add(o);
            // Remove cart items from the database
            context.CartItems.RemoveRange(getUserCart.CartItems);

            // Clear cart items from the cart object
            getUserCart.CartItems.Clear();
            await SaveChangesAsync();            
            return new OrderResponse(flag: true, message: "Order placed");
        }
      


        private async Task SaveChangesAsync() => await context.SaveChangesAsync();
    }
  
}
