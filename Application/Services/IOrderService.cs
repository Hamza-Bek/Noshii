using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrder(string userId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<OrderStatus>> GetAllStatuses();
        Task<Dictionary<string, string>> GetAllStatusesDic();
        Task<OrderResponse> ChangeOrderStatus(string orderId, string newStatusId);
        Task<OrderResponse> PlaceOrder( string userId, string cartId);

    }
}
