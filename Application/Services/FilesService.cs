using System.Net.Http.Json;
using Application.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class FilesService : IFilesService
{
    private readonly HttpClient _httpClient;

    public FilesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<ImageResponse> UploadImageAsync(IFormFile image, string plateId)
    {
        var formData = new MultipartFormDataContent();
        var streamContent = new StreamContent(image.OpenReadStream());
        formData.Add(streamContent, "file", image.FileName);

        string requestUrl = $"api/files/upload-plate-image/{plateId}";
        HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, formData);

        if (!response.IsSuccessStatusCode)
        {
            // TODO: In case something faield, do something!
        }

        var result = await response.Content.ReadFromJsonAsync<ImageResponse>();
        return result!;
    }

    public async Task<GeneralResponse> DeleteImageAsync(string imageId)
    {	
		var response = await _httpClient.DeleteAsync($"api/files/delete-image/{imageId}");

		if (!response.IsSuccessStatusCode)
		{
			var error = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return new GeneralResponse(Flag: false, Message: "Failed to remove the image");
		}

		var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
		return result!;
	}
}