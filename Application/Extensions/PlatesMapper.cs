using Application.DTOs.Request.Order;
using Domain.Models;

namespace Application.Extensions;

public static class PlatesMapper
{
    public static PlateDTO ToPlateDto(this Plate plate)
    {
        return new()
        {
            Id = plate.Id,
            PlateName = plate.PlateName,
            PlatePrice = plate.PlatePrice,
            PlateBio = plate.PlateBio,
            CoverImageUrl = plate.Images.First().Url
        };
    }
}