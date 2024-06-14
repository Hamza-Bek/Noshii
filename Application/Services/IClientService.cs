using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IClientService
    {
        Task<PlateResponse> AddPlateToCart(string plateId, string userId);
        Task<PlateResponse> RemovePlateFromCart(string plateId, string userId);
        Task<IEnumerable<CartItem>> GetCartItems(string userId);
    }
}
