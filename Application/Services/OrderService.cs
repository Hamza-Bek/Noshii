using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Extensions;
using Domain.Models.OrderEntities;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Domain.Models;
using Application.Interfaces;


namespace Application.Services
{
    public class OrderService (HttpClient _httpClient ): IOrderService
    {
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            try
            {

                var response = await _httpClient.GetAsync(Constants.GetAllOrdersRoute);
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<OrderResponse> PlaceOrder( string userId, string cartId)
        {
            try
            {
                var model = new OrderDTO()
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending",
                    OrderTotal = 0,
                };

                var data = await _httpClient.PostAsJsonAsync($"api/orders/order/place/{userId}/{cartId}", model);
                var response = await data.Content.ReadFromJsonAsync<OrderResponse>();
                if (response.flag)
                {
                    return new OrderResponse { flag = true, message = "Plate added successfully" };

                }
                else
                {

                    return new OrderResponse { flag = false, message = "Failed to add plate" };

                }


            }
            catch (Exception ex) { return new OrderResponse(flag: false, message: ex.Message); }

        }
        
        public async Task<IEnumerable<Order>> GetOrder(string userId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/orders/order/get/{userId}");
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<OrderResponse> ChangeOrderStatus(string orderId, string newStatusId)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { newStatusId }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/orders/change/status/{orderId}/{newStatusId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new OrderResponse(false, $"Error changing order status: {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
            return result!;
        }

        public async Task<Dictionary<string, string>> GetAllStatusesDic()
        {
            var response = await _httpClient.GetAsync("api/orders/get/statuses/dic");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return data!;
            }
            throw new Exception();
        }

        public async Task<IEnumerable<OrderStatus>> GetAllStatuses()
        {
            try
            {

                var response = await _httpClient.GetAsync("api/orders/get/statuses");
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<OrderStatus>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<OrderResponse> ClearCartTotal(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/orders/clear-cart-total/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return new OrderResponse { flag = true, message = "Item deleted successfully." };
            }
            else
            {
                return new OrderResponse { flag = false, message = "Failed to remove the plate!" };
            }
        }

        public async Task<Cart> GetUserCart(string userId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/orders/get-cart-details/{userId}");
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<Cart>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"sorry unkown error occured.{Environment.NewLine} Error Description : {Environment.NewLine} Status Code : {response.StatusCode}{Environment.NewLine} Reason Phrase : {response.ReasonPhrase}";
            else
                return null!;
        }

        public async Task<OrderResponse> ChangeIsOrderBool(string userId)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { userId }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new OrderResponse(false, $"Error changing order status: {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
            return result!;
        }
        
        public async Task<bool> UpdateUserCartAsync(string userId)
        {
            var response = await _httpClient.PutAsync($"api/orders/update/user/cart/{userId}", null);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<OrderResponse> ClearCartItems(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/orders/clear/cart{userId}");
            if (response.IsSuccessStatusCode)
            {
                return new OrderResponse { flag = true, message = "Item deleted successfully." };
            }
            else
            {
                return new OrderResponse { flag = false, message = "Failed to remove the plate!" };
            }
        }
    }
}
