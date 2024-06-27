using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PlateService : IPlateService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientService httpClientService;


        public PlateService(HttpClient httpClient, HttpClientService httpClientService)
        {
            _httpClient = httpClient;
            this.httpClientService = httpClientService;
        }

        public async Task<PlateResponse> AddPlateAsync(PlateDTO model)
        {
            try
            {
                var data = await _httpClient.PostAsJsonAsync(Constants.AddPlateRoute, model);
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

        public Task<PlateResponse> EditPlateAsync(Plate model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PlateDTO>> GetAllPlates()
        {
            try
            {

                var response = await _httpClient.GetAsync(Constants.GetAllPlatesRoute);
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<PlateDTO>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<Plate> GetPlateByName(string plateName)
        {
            throw new NotImplementedException();
        }

        public async Task<PlateResponse> RemovePlateAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/plate/delete/plate/{id}");
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

        public async Task<Dictionary<string, string>> GetCategories()
        {
            var response = await _httpClient.GetAsync("api/plate/get/categories");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return data!;
            }
            throw new Exception();
        }

        public async Task<PlateResponse> AddCategory(Category model)
        {
            try
            {
                var data = await _httpClient.PostAsJsonAsync("api/plate/add/category", model);
                var response = await data.Content.ReadFromJsonAsync<PlateResponse>();
                if (response.flag)
                {
                    return new PlateResponse { flag = true, message = "Category added successfully" };

                }
                else
                {

                    return new PlateResponse { flag = false, message = "Failed to add category" };

                }


            }
            catch (Exception ex) { return new PlateResponse(flag: false, message: ex.Message); }
        }


        public async Task<IEnumerable<PlateDTO>> SearchPlatesAsync(string searchTerm)
        {
			try
			{
				var response = await _httpClient.GetAsync($"api/plate/search/{searchTerm}");
				string error = CheckResponseStatus(response);
				if (!string.IsNullOrEmpty(error))
					throw new Exception(error);

				var result = await response.Content.ReadFromJsonAsync<IEnumerable<PlateDTO>>();
				return result!;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<IEnumerable<PlateDTO>> GetPlatesByCategory(string category)
		{
			try
			{
				var response = await _httpClient.GetAsync($"api/plate/plates-category/{category}");
				string error = CheckResponseStatus(response);
				if (!string.IsNullOrEmpty(error))
					throw new Exception(error);

				var result = await response.Content.ReadFromJsonAsync<IEnumerable<PlateDTO>>();
				return result!;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
