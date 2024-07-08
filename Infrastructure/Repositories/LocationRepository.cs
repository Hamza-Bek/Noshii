using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models.OrderEntities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LocationReposiotry (AppDbContext _context) : ILocationReposiotry
    {
        public async Task<LocationResponse> AddLocation(LocationDTO model)
        {
            if (model == null)
                return new LocationResponse(Flag: false, Message: "Null values are not accepted");

            var newAddress = new Location()
            {
                LocationId = Guid.NewGuid().ToString(),
                PhoneNumber = model.PhoneNumber,
                Country = model.Country,
                Street = model.Street,
                Building = model.Building,
                Floor = model.Floor,
                ApplicationUserId = model.ApplicationUserId,
                
            };

            _context.Locations.Add(newAddress);
            await SaveChangesAsync();

            return new LocationResponse(Flag: true, Message: "Address added successfully!");
        }

        public Task<LocationResponse> DeleteLocation(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LocationDTO>> GetLocation(string userId)
        {
            var getUser = await _context.Users.FindAsync(userId);
            if (getUser == null)
            {
				Console.WriteLine("User's cart not found");
				return Enumerable.Empty<LocationDTO>();
			}

            var getUserLocation = await _context.Locations
             .FirstOrDefaultAsync(o => o.ApplicationUserId == getUser.Id);

            if (getUserLocation == null)
            {
				Console.WriteLine("User's cart not found");
				return Enumerable.Empty<LocationDTO>();
			}

            var locationDTO = new LocationDTO
            {
                LocationId = getUserLocation.LocationId,
                PhoneNumber = getUserLocation.PhoneNumber,
                Country = getUserLocation.Country,
                Street = getUserLocation.Street,
                Building = getUserLocation.Building,
                Floor = getUserLocation.Floor,
                ApplicationUserId = getUserLocation.ApplicationUserId    
            };

			return new List<LocationDTO> { locationDTO };
		}

        public Task<IEnumerable<LocationDTO>> GetAllLocations()
        {
            throw new NotImplementedException();
        }

        public async Task<LocationResponse> UpdateLocation(LocationDTO model)
        {
            if (model == null)
                return new LocationResponse(Flag: false, Message: "Null values are not accepted");

            var location = await _context.Locations.FindAsync(model.LocationId);
            if (location == null)
                return new LocationResponse(Flag: false, Message: "Location not found");

            location.PhoneNumber = model.PhoneNumber;
            location.Country = model.Country;
            location.Street = model.Street;
            location.Building = model.Building;
            location.Floor = model.Floor;
            location.ApplicationUserId = model.ApplicationUserId;

            _context.Locations.Update(location);
            await SaveChangesAsync();

            return new LocationResponse(Flag: true, Message: "Location updated successfully!");
        }

        private async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
