using Application.DTOs.Request.Order;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PlateController : Controller
    {
        private readonly IPlateRepository _plateRepository;
        private readonly IAccountRepository _aacountRepository;

        private readonly IMapper _mapper;
        public PlateController(IPlateRepository plateRepository, IMapper mapper)
        {
            _plateRepository = plateRepository;
            _mapper = mapper;
        }

        [HttpGet("get/plates")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Plate>))]
        public async Task<IActionResult> GetPlates()
        {
            var data = await _plateRepository.GetAllPlates();

            // Manual mapping
            var mappedPlates = data.Select(p => p.ToPlateDto());
            return Ok(mappedPlates);
        }

        [HttpGet("get-id/plate")]
        [ProducesResponseType(200, Type = typeof(Plate))] //MAKE THE END POINT A MORE CLEANER
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPlateById(string plateId)
        {
            var data = await _plateRepository.GetPlateById(plateId);
            return Ok(data);
                
        }


        [HttpPost("add/plate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddPlate([FromBody]PlateDTO model)
        {
            var result = await _plateRepository.AddPlateAsync(model);
            return Ok(result);
        }

        [HttpPut("edit/plate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EditPlate([FromBody] Plate model)
        {

            var result = await _plateRepository.EditPlateAsync(model);
            return Ok(result);
        }
        [HttpDelete("delete/plate/{plateId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemovePlate(string plateId)
        {
            var result = await _plateRepository.RemovePlateAsync(plateId);
            return Ok(result);
        }
    }
}


