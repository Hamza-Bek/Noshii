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
    public interface IPlateService
    {
        Task<IEnumerable<PlateDTO>> GetAllPlates();        
        Task<Plate> GetPlateByName(string plateName);
        Task<Dictionary<string, string>> GetCategories();

        //..//

        Task<PlateResponse> AddPlateAsync(PlateDTO model);
        Task<PlateResponse> EditPlateAsync(Plate model);
        Task<PlateResponse> RemovePlateAsync(string id);
    }
}
