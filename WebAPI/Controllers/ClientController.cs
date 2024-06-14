using Application.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;
       
        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpPost("add/tocart/{userId}/{plateId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Plate>))]
        public async Task<IActionResult> AddPlateToCart(string plateId, string userId)
        {
            var data = await _clientRepository.AddPlateToCartAsync(plateId, userId);
            return Ok(data);
        }

        [HttpDelete("remove/fromcart/{userId}/{plateId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Plate>))]
        public async Task<IActionResult> RemovePlateFromCart(string plateId, string userId)
        {
            var data = await _clientRepository.RemovePlateFromCartAsync(plateId, userId);
            return Ok(data);
        }

        [HttpGet("get/cart/plates/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CartItem>))]
        public async Task<IActionResult> GetCartPlates(string userId)
        {
            var data = await _clientRepository.GetAllCartPlatesAsync(userId);
            return Ok(data);
        }
    }
}
