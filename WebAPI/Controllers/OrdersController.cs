﻿using Application.DTOs.Request.Order;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Domain.Models.Order;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderRepository _orderRepository , AppDbContext context) : Controller
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


     
    }
}