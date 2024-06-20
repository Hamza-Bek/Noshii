
using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models;
using Domain.Models.Order;

namespace Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Cart> GetUserCart(string userId);
        Task<OrderResponse> ClearCartTotal(string userId);
        Task<OrderResponse> ClearCartItems(string userId);
        Task<OrderResponse> ChangeIsOrderBool(string userId);
        Task<bool> UpdateUserCartAsync(Cart userCart);

        Task<IEnumerable<Order>> GetOrder(string userId);               
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<OrderStatus>> GetAllStatusesAsync();
        Task<OrderResponse> ChangeOrderStatusAsync(string orderId, string newStatusId);
        Task<OrderResponse> PlaceOrderAsync(OrderDTO order , string userId, string cartId);
    }
}
