using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Interfaces;
using Blazored.LocalStorage;
using Domain.Models;
using Domain.Models.OrderEntities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class LocationController(ILocationReposiotry _locationReposiotry, AppDbContext _context) : Controller
    {
        [HttpPost("add-location")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddLocation([FromBody] LocationDTO model)
        {
            var result = await _locationReposiotry.AddLocation(model);
            return Ok(result);
        }

        [HttpPut("update-location")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.LocationId) || string.IsNullOrEmpty(model.ApplicationUserId))
            {
                return BadRequest("Invalid location data.");
            }

            var existingLocation = await _context.Locations.FindAsync(model.LocationId);
            if (existingLocation == null)
            {
                return NotFound("Location not found.");
            }

            existingLocation.PhoneNumber = model.PhoneNumber;
            existingLocation.Country = model.Country;
            existingLocation.Street = model.Street;
            existingLocation.Building = model.Building;
            existingLocation.Floor = model.Floor;

            _context.Locations.Update(existingLocation);
            await _context.SaveChangesAsync();

            return Ok(new LocationResponse(true, "Location updated successfully."));
        }

        [HttpGet("location/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Location>))]
        public async Task<IActionResult> GetOrder(string userId)
        {
            var data = await _locationReposiotry.GetLocation(userId);
            return Ok(data);
        }

    }  
}
