using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models;
using Domain.Models.OrderEntities;

namespace Application.Interfaces
{
    public interface IPlateRepository
    {
        Task<IEnumerable<Plate>> GetAllPlates();
        Task<Plate> GetPlateById(string id);
        Task<Plate> GetPlateByName(string plateName);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<PlateDTO>> SearchPlatesAsync(string searchTerm);
        Task<IEnumerable<PlateDTO>> GetPlatesByCategory(string category);
        //..//

        Task<Plate> PlateExist(string id);
        Task<PlateResponse> AddCategory(Category model);
        Task<PlateResponse> AddPlateAsync(PlateDTO model);
        Task<PlateResponse> EditPlateAsync(Plate model);
        Task<PlateResponse> RemovePlateAsync(string id);  
    }
}
