using Application.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public interface IFilesService
{
    Task<ImageResponse> UploadImageAsync(IFormFile image, string plateId);
    Task<GeneralResponse> DeleteImageAsync(string imageId);
    // TODO: Add the rest of the methods here
}