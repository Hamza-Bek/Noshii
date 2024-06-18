using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;

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
        var image = await _context.Images.FindAsync(imageId);

        // TODO: Same as before buddy!

        _context.Images.Remove(image!);

        // IMPORTANT: Delete the image from the file system
        try
        {
            File.Delete(image?.AbsolutePath!);
        }
        catch (ArgumentNullException) // TODO: In case the file deletion failed, re add the image to the database
        {
            _context.Images.Add(image!);
            await _context.SaveChangesAsync();

            return new GeneralResponse()
            {
                Flag = false,
                Message = "Failed to delete image"
            };
        }
        catch (IOException)
        {
            _context.Images.Add(image!);
            await _context.SaveChangesAsync();

            return new GeneralResponse()
            {
                Flag = false,
                Message = "Failed to delete image"
            };
        }

        return new GeneralResponse()
        {
            Flag = true,
            Message = "Image deleted successfully"
        };
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