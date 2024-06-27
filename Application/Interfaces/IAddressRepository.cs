using Application.DTOs.Response;
using Domain.Models.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAddressRepository
    {

        //GET HTTP 
        Task<AddressResponse> GetAddress(string userId);
        Task<IEnumerable<Address>> GetAllAddresses();

        //POST HTTP
        Task<AddressResponse> AddAddress(Address model);
        Task<AddressResponse> UpdateAddress(Address model);

        //DELETE HTTP
        Task<AddressResponse> DeleteAddress(string userId);



        
    }
}
