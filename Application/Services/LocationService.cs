using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class LocationService(HttpClient httpClient) : ILocationService
	{
	

		public async Task<IEnumerable<LocationDTO>> GetLocation(string userId)
		{
			try
			{

				var response = await httpClient.GetAsync($"api/location/location/{userId}");
				string error = CheckResponseStatus(response);
				if (!string.IsNullOrEmpty(error))
					throw new Exception(error);

				var result = await response.Content.ReadFromJsonAsync<IEnumerable<LocationDTO>>();
				return result!;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

        public async Task<LocationResponse> AddLocation(LocationDTO model)
        {
			try
			{
				var data = await httpClient.PostAsJsonAsync("api/location/add-location", model);
				var response = await data.Content.ReadFromJsonAsync<LocationResponse>();
				if(response.Flag)
				{
					return new LocationResponse(Flag: true, Message: "Location added successfully.");
				}
				else
				{
                    return new LocationResponse(Flag: false, Message: "Something went wrong while trying to add user's location");
                }
			}
			catch
			{
                return new LocationResponse(Flag: false, Message: "Something went wrong!");
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
