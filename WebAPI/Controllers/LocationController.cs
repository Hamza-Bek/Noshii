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


        [HttpGet("location/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Location>))]
        public async Task<IActionResult> GetOrder(string userId)
        {
            var data = await _locationReposiotry.GetLocation(userId);
            return Ok(data);
        }

    }  
}
