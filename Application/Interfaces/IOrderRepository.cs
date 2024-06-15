
using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models.Order;

namespace Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrder(string userId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<OrderStatus>> GetAllStatusesAsync();
        Task<OrderResponse> ChangeOrderStatusAsync(string orderId, string newStatusId);
        Task<OrderResponse> PlaceOrderAsync(OrderDTO order , string userId, string cartId);
    }
}
