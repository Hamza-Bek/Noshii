using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FilesRepository : IFilesRepository
{
    private readonly AppDbContext _context;

    public FilesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralResponse> SaveImageAsync(string plateId, Image image)
    {
        var plate = await _context.Plates.FindAsync(plateId);

        // TODO: Check for null plates! Throw new Not Found later

        plate?.Images.Add(image);
        await _context.SaveChangesAsync();

        return new GeneralResponse()
        {
            Flag = true,
            Message = "Image saved successfully"
        };
    }

    public async Task<GeneralResponse> DeleteImageAsync(string imageId)
    {
		var imageToDelete = await _context.Images.FindAsync(imageId);

		if (imageToDelete is not null)
		{
			File.Delete(imageToDelete.AbsolutePath!); // we know for a fact, the Path cannot be null

			var result = _context.Images.Remove(imageToDelete);

			if (result.State == EntityState.Deleted)
			{
				await _context.SaveChangesAsync();
			}
		}
		else
		{
			return new GeneralResponse(Flag: false,Message: "The image doesn't exist in the system");
		}

		return new GeneralResponse(Flag: true, Message: "The image deleted!");
	}


    // TODO: These are yours to implement!
    public async Task<Image> GetImageByIdAsync(string imageId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Image>> GetAllImagesForPlateAsync(string plateId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}