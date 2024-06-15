using Application.DTOs.Request.Order;
using Application.Interfaces;
using Domain.Models;
using Domain.Models.Order;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderRepository _orderRepository) : Controller
    {

        [HttpPost("order/place/{userId}/{cartId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDTO model, string userId, string cartId)
        {
            var result = await _orderRepository.PlaceOrderAsync(model, userId, cartId);
            return Ok(result);
        }

        [HttpGet("order/get/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CartItem>))]
        public async Task<IActionResult> GetOrder(string userId)
        {
            var data = await _orderRepository.GetOrder(userId);
            return Ok(data);
        }


        [HttpGet("get/allorders")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public async Task<IActionResult> GetAllOrders()
        {
            var data = await _orderRepository.GetAllOrders();
            return Ok(data);
        }
    }
}
