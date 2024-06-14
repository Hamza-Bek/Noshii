using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IClientRepository
    {
        Task <PlateResponse>AddPlateToCartAsync(string plateId, string userId);
        Task <PlateResponse>RemovePlateFromCartAsync(string plateId, string userId);
        Task <IEnumerable<CartItem>> GetAllCartPlatesAsync(string userId);

    }
}
