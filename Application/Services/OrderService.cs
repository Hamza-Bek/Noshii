using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Extensions;
using Domain.Models.Order;
using System.Net.Http.Json;


namespace Application.Services
{
    public class OrderService (HttpClient _httpClient): IOrderService
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

        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"sorry unkown error occured.{Environment.NewLine} Error Description : {Environment.NewLine} Status Code : {response.StatusCode}{Environment.NewLine} Reason Phrase : {response.ReasonPhrase}";
            else
                return null!;
        }

     
    }
}
