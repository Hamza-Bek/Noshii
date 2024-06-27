using Application.DTOs.Request.Order;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PlateController : Controller
    {
        private readonly IPlateRepository _plateRepository;
        private readonly IAccountRepository _aacountRepository;
        public readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public PlateController(IPlateRepository plateRepository, IMapper mapper, AppDbContext context)
        {
            _plateRepository = plateRepository;
            _mapper = mapper;
            _context = context; 
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



		[HttpPost("add/category")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> AddCategory([FromBody] Category model)
		{
			var result = await _plateRepository.AddCategory(model);
			return Ok(result);
		}


        [HttpGet("get/categories")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategoryAsync()
        {
            var categories = _context.Categories.ToList(); // GET ALL CATEGORIES FROM THE DB

            var categoriesDic = categories.ToDictionary(e => e.CategoryId , e => e.CategoryTag);

            return Ok(categoriesDic);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> SearchPlates(string searchTerm)
        {
            var plates = await _plateRepository.SearchPlatesAsync(searchTerm);
            return Ok(plates);
        }

		[HttpGet("get/plates-category/{category}")]
		public async Task<IActionResult> GetPlatesByCategory(string category ="*")
		{
			var plates = await _plateRepository.GetPlatesByCategory(category);
			return Ok(plates);
		}
	}
}


