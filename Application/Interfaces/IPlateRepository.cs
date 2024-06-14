using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IPlateRepository
    {
        Task<IEnumerable<Plate>> GetAllPlates();
        Task<Plate> GetPlateById(string id);
        Task<Plate> GetPlateByName(string plateName);

        //..//

        Task<Plate> PlateExist(string id);
        Task<PlateResponse> AddPlateAsync(PlateDTO model);
        Task<PlateResponse> EditPlateAsync(Plate model);
        Task<PlateResponse> RemovePlateAsync(string id);  
    }
}
