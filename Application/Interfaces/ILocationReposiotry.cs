using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILocationReposiotry
    {

        //GET HTTP 
        Task<IEnumerable<LocationDTO>> GetLocation(string userId);
        Task<IEnumerable<LocationDTO>> GetAllLocations();

        //POST HTTP
        Task<LocationResponse> AddLocation(LocationDTO model);
        Task<LocationResponse> UpdateLocation(LocationDTO model);

        //DELETE HTTP
        Task<LocationResponse> DeleteLocation(string userId);



        
    }
}
