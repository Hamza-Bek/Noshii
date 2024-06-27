using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Domain.Models.OrderEntities;
using Infrastructure.Data;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class PlateRepository : IPlateRepository
    {
        private readonly AppDbContext _context;        
        private readonly IFilesRepository _filesRepository;
        public PlateRepository(AppDbContext context,  IFilesRepository filesRepository)
        {
            _context = context;
            _filesRepository = filesRepository;
        }
        public async Task<PlateResponse> AddPlateAsync(PlateDTO model)
        {
            if(model == null)
                return new PlateResponse(flag:false , message:"Can not insert null values");

            var check = _context.Plates.Any(x => x.PlateName == model.PlateName);
            if(check) return new PlateResponse(false, "Plate already exist!");

            var getCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryTag == model.CategoryTag);
            if(getCategory.CategoryTag == null)
				return new PlateResponse(false, "Category not found!");

			var plate = new Plate()
            {
                Id = model.Id,
                PlateName = model.PlateName,
                PlateBio = model.PlateBio,
                PlatePrice = model.PlatePrice,
                CategoryTag = model.CategoryTag
            };
            
            _context.Plates.Add(plate);
            await SaveChangesAsync();
            return new PlateResponse(true, "Plate Added!");
        }

		public async Task<PlateResponse> AddCategory(Category model)
		{
			if (model == null)
				return new PlateResponse(flag: false, message: "Can not insert null values");

            var newCategory = new Category()
            {
                CategoryId = Guid.NewGuid().ToString(),
				CategoryTag = model.CategoryTag,
            };

            _context.Categories.Add(newCategory);
            await SaveChangesAsync();
			return new PlateResponse(flag: true, message: "Category added successfully");
		}

		public async Task<PlateResponse> EditPlateAsync(Plate model)
        {
            _context.Plates.Update(model);
            await SaveChangesAsync();
            return new PlateResponse(true, "Plate Edited!");
        }

        public async Task<IEnumerable<Plate>> GetAllPlates() => await _context
             .Plates
             .Include(p => p.Images)
             .AsNoTracking()
             .ToListAsync();

        public async Task<Plate> GetPlateById(string id) => await _context.Plates.FindAsync(id);
       

        public async Task<Plate> GetPlateByName(string plateName) => await _context.Plates.Where(p => p.PlateName == plateName).FirstOrDefaultAsync();


        public async Task<Plate> PlateExist(string id) => await _context.Plates.FindAsync(id);
    

        public async Task<PlateResponse> RemovePlateAsync(string id)
        {
			var plate = await _context.Plates
							  .Include(p => p.Images)
							  .FirstOrDefaultAsync(p => p.Id == id);
			if (plate == null) return new PlateResponse(false, "Plate not found!");
			

			if (plate.Images is not null && plate.Images.Any())
            {
				for (int i = 0; i < plate.Images.Count; i++)
				{
					await _filesRepository.DeleteImageAsync(plate.Images.ElementAt(i).imageId);
				}
			}

            _context.Plates.Remove(plate);
            await SaveChangesAsync();
            return new PlateResponse(true, "Plate deleted!");
        }

        private async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Category>> GetCategoriesAsync() => await _context.Categories.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<PlateDTO>> SearchPlatesAsync(string searchTerm)
        {			
			var query = _context.Plates.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				query = query.Where(p => p.PlateName.StartsWith(searchTerm));
			}

			var plates = await query
				.Include(p => p.Images)
				.AsNoTracking()
				.ToListAsync();

			// Project Plate entities to PlateDTO
			var plateDTOs = plates.Select(p => new PlateDTO
			{
				Id = p.Id,
				PlateName = p.PlateName,
				PlatePrice = p.PlatePrice,
				PlateBio = p.PlateBio,
				CoverImageUrl = p.Images.First().Url,
				CategoryTag = p.CategoryTag
			}).ToList();

			return plateDTOs;
		}

		public async Task<IEnumerable<PlateDTO>> GetPlatesByCategory(string category)
		{
			var query = _context.Plates.AsQueryable();

			if (!string.IsNullOrWhiteSpace(category))
			{
				query = query.Where(p => p.CategoryTag == category);
			}

			var plates = await query
				.Include(p => p.Images)
				.AsNoTracking()
				.ToListAsync();

			// Project Plate entities to PlateDTO
			var plateDTOs = plates.Select(p => new PlateDTO
			{
				Id = p.Id,
				PlateName = p.PlateName,
				PlatePrice = p.PlatePrice,
				PlateBio = p.PlateBio,
				CoverImageUrl = p.Images.FirstOrDefault()?.Url,
				CategoryTag = p.CategoryTag
			}).ToList();

			return plateDTOs;

		}
	}
}
