using Application.DTOs.Request.Client;
using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Extensions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClientService (HttpClient _httpClient) : IClientService
    {
        public async Task<PlateResponse> AddPlateToCart(string plateId, string userId)
        {
            try
            {
                var requestPayLoad = new AddPlateRequest
                {
                    PlateId = plateId,
                    UserId = userId,
                };
                var data = await _httpClient.PostAsJsonAsync($"api/client/add/tocart/{userId}/{plateId}", new { });
                var response = await data.Content.ReadFromJsonAsync<PlateResponse>();
                if (response.flag)
                {
                    return new PlateResponse { flag = true, message = "Plate added successfully" };

                }
                else
                {

                    return new PlateResponse { flag = false, message = "Failed to add plate" };

                }


            }
            catch (Exception ex) { return new PlateResponse(flag: false, message: ex.Message); }
        }
        public async Task<IEnumerable<CartItem>> GetCartItems(string userId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/client/get/cart/plates/{userId}");
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<CartItem>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PlateResponse> RemovePlateFromCart(string plateId, string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/client/remove/fromcart/{userId}/{plateId}");
            if (response.IsSuccessStatusCode)
            {
                
                return new PlateResponse { flag = true, message = "Item deleted successfully." };
            }
            else
            {
                return new PlateResponse { flag = false, message = "Failed to remove the plate!" };
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
