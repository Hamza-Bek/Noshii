using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AddressReposiotry : IAddressRepository
    {
        public async Task<AddressResponse> AddAddress(Address model)
        {
            if (model == null)
                return new AddressResponse(Flag: false, Message: "Null values are not accepted");

            var newAddress = new Address()
            {
                AddressId = Guid.NewGuid().ToString(),
                PhoneNumber = model.PhoneNumber,
                Country = model.Country,
                Street = model.Street,
                Building = model.Building,
                Floor = model.Floor,
            };


            return new AddressResponse(Flag: true, Message: "Address added successfully!");
        }

        public Task<AddressResponse> DeleteAddress(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<AddressResponse> GetAddress(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Address>> GetAllAddresses()
        {
            throw new NotImplementedException();
        }

        public Task<AddressResponse> UpdateAddress(Address model)
        {
            throw new NotImplementedException();
        }
    }
}
