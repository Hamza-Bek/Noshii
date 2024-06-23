using Application.DTOs.Request.Order;
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
        public async Task<OrderResponse> ChangeOrderStatusAsync(string orderId,string newStatusId)
        {
            
            var order = await context.Orders.FindAsync(orderId);
            if (order == null)
                return new OrderResponse(flag: false, message: "The order not found");

            var status = await context.OrderStatuses.FindAsync(newStatusId);
            if (status == null)
                return new OrderResponse(flag: false, message: "The status not found");          

            order.OrderStatus = status.Status;


            context.Orders.Update(order);

            await SaveChangesAsync();
            return new OrderResponse(true, "Status changed");
        }

        public async Task<IEnumerable<Order>> GetAllOrders()=> await context.Orders.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<Order>> GetOrder(string userId)
        {
            var getUser = await context.Users.FindAsync(userId);
            if (getUser == null)
            {
                Console.WriteLine("User not found");
                return Enumerable.Empty<Order>();
            }

            var getUserOrders = await context.Orders
       .Where(o => o.UserId == getUser.Id)
       .Include(o => o.OrderMaker) // Eager load OrderMaker
       .Include(o => o.CartItems) // Eager load CartItems
           .ThenInclude(ci => ci.Plate) // Eager load Plate for each CartItem
       .ToListAsync();

            if (getUserOrders == null || !getUserOrders.Any())
            {
                Console.WriteLine("User's cart not found");
                return Enumerable.Empty<Order>();
            }

            return getUserOrders
          .GroupBy(o => o.OrderId)
          .Select(g => g.First())
          .ToList();
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
                OrderNumber = GenerateRandomNumberString(5),
                UserId = userId,
                OrderDate = order.OrderDate,
                OrderTotal = orderTotal,
                OrderStatus = order.OrderStatus
            };

            getUserCart.IsOrdered = true;
            
            context.Orders.Add(o);
            // Remove cart items from the database
            context.CartItems.RemoveRange(getUserCart.CartItems);

            // Clear cart items from the cart object
            getUserCart.CartItems.Clear();
            await SaveChangesAsync();            
            return new OrderResponse(flag: true, message: "Order placed");
        }

        public async Task<IEnumerable<OrderStatus>> GetStatusesAsync() => await context.OrderStatuses.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<OrderStatus>> GetAllStatusesAsync() => await context.OrderStatuses.AsNoTracking().ToListAsync();
        private async Task SaveChangesAsync() => await context.SaveChangesAsync();

        public async Task<OrderResponse> ClearCartTotal(string userId)
        {
            // Retrieve the user from the database
            var getUser = await context.Users.FindAsync(userId);
            if (getUser == null)
                return new OrderResponse(flag: false, message: "No user was found");

            // Retrieve the user's cart
            var getUserCart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (getUserCart == null)
                return new OrderResponse(flag: false, message: "No user's cart was found");

            // Clear the CartTotal
            getUserCart.CartTotal = 0;

            // Save the changes to the database
            await context.SaveChangesAsync();

            return new OrderResponse(flag: true, message: "Cart Total cleared successfully");
        }

        public async Task<Cart> GetUserCart(string userId)
        {            
            return await context.Carts
                            .Include(p => p.CartOwner)
                            .Include(u => u.CartItems)// Include related items if necessary
                           .FirstOrDefaultAsync(c => c.UserId == userId);           
        }

        public async Task<OrderResponse> ChangeIsOrderBool(string userId)
        {
            var getUser = await context.Users.FindAsync(userId);
            if (getUser == null) return new OrderResponse(flag:false , message:"The user was not found") ;

            var getUserCart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (getUserCart == null)
                return new OrderResponse(flag: false, message: "No user's cart was found");

            getUserCart.IsOrdered = true;

            await SaveChangesAsync();
            return new OrderResponse(flag: true, message: "The column IsOrdered changed successfully");
        }

        public async Task<bool> UpdateUserCartAsync(Cart userCart)
        {
            context.Carts.Update(userCart);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<OrderResponse> ClearCartItems(string userId)
        {
            var getUser = await context.Users.FindAsync(userId);
            if (getUser == null)
                return new OrderResponse(flag: false, message: "The user was not found");

            var getUserCart = await context.Carts.Include(p => p.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (getUserCart == null)
                return new OrderResponse(flag: false, message: "The user's cart is not found!");

            // Get all cart items for the user
            var getUserCartItems = context.CartItems.Where(ci => ci.CartId == getUserCart.Id).ToList();
            if (!getUserCartItems.Any())
                return new OrderResponse(flag: false, message: "Cart is already clear");

            // Remove each cart item from the context
            context.CartItems.RemoveRange(getUserCartItems);

            // Save changes to the database
            await context.SaveChangesAsync();


            return new OrderResponse(flag:true  , message:"The cart cleared!");
        }

		private string GenerateRandomNumberString(int length)
		{
			Random random = new Random();
			char[] digits = new char[length];

			for (int i = 0; i < length; i++)
			{
				digits[i] = (char)('0' + random.Next(0, 10));
			}

			return new string(digits);
		}

	}
  
}
