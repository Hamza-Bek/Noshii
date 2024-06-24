using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Domain.Models.OrderEntities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderRepository _orderRepository, AppDbContext context) : Controller
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
        [HttpPut("change/status/{orderId}/{newStatusId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangeOrderStatus(string orderId, string newStatusId)
        {

            var data = await _orderRepository.ChangeOrderStatusAsync(orderId, newStatusId);
            return Ok(data);
        }

        [HttpGet]
        [Route("get/statuses/dic")]
        public async Task<IActionResult> GetAllStatusesDic()
        {

            var brands = context.OrderStatuses.ToList(); // Retrieve all employees from the database

            // Create a dictionary where the key is the ID and the value is the name
            var brandsDictionary = brands.ToDictionary(e => e.Id, e => e.Status);

            return Ok(brandsDictionary);


        }

        [HttpGet("get/statuses")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderStatus>))]
        public async Task<IActionResult> GetAllStatuses()
        {

            var data = await _orderRepository.GetAllStatusesAsync();
            return Ok(data);
        }


        [HttpDelete("clear-cart-total/{userId}")]
        public async Task<ActionResult<OrderResponse>> ClearCartTotal(string userId)
        {
            try
            {
                var response = await _orderRepository.ClearCartTotal(userId);
                if (!response.flag)
                {
                    return NotFound(response); // Return 404 Not Found if user or cart not found
                }
                return Ok(response); // Return 200 OK with response object if successful
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 Internal Server Error on unexpected errors
            }
        }

        [HttpGet("get-cart-details/{userId}")]
        public async Task<ActionResult<Cart>> GetCartDetails(string userId)
        {
            try
            {
                var cart = await _orderRepository.GetUserCart(userId);
                if (cart == null)
                {
                    return NotFound(new { message = "No cart found for the user." });
                }
                return Ok(cart); // Return 200 OK with the cart details
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error on unexpected errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("change/is-order/{userid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangeIsOrder(string userId)
        {

            var data = await _orderRepository.ChangeIsOrderBool(userId);
            return Ok(data);
        }
        [HttpPut("update/user/cart/{userId}")]
        public async Task<IActionResult> UpdateIsOrdered(string userId)
        {
            try
            {
                var getUserCart = await _orderRepository.GetUserCart(userId);

                if (getUserCart == null)
                {
                    return NotFound("User cart not found.");
                }

                // Setting IsOrdered to false
                getUserCart.IsOrdered = false;

                // Save the updated cart back to the database
                var updateResult = await _orderRepository.UpdateUserCartAsync(getUserCart);

                if (updateResult)
                {
                    return Ok(new { Message = "User cart updated successfully!", UserCart = getUserCart });
                }
                else
                {
                    return StatusCode(500, "Failed to save the updated UserCart.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occurred: {ex.Message}");
            }
        }

        [HttpDelete("clear/cart{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemovePlate(string userId)
        {
            var result = await _orderRepository.ClearCartItems(userId);
            return Ok(result);
        }

    }
}