using Application.DTOs.Response;
using Domain.Models;

namespace Application.Interfaces;

public interface IFilesRepository
{
    Task<GeneralResponse> SaveImageAsync(string plateId, Image image);
    Task<GeneralResponse> DeleteImageAsync(string imageId);
    Task<Image> GetImageByIdAsync(string imageId);
    Task<IEnumerable<Image>> GetAllImagesForPlateAsync(string plateId, CancellationToken cancellationToken);
}