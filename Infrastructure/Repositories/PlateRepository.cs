using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class PlateRepository : IPlateRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public PlateRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PlateResponse> AddPlateAsync(PlateDTO model)
        {
            var check = _context.Plates.Any(x => x.PlateName == model.PlateName);
            if(check) return new PlateResponse(false, "Plate already exist!");
            var plate = new Plate()
            {
                PlateId = Guid.NewGuid().ToString(),
                PlateName = model.PlateName,
                PlateBio = model.PlateBio,
                PlatePrice = model.PlatePrice
            };
            
            _context.Plates.Add(plate);
            await SaveChangesAsync();
            return new PlateResponse(true, "Plate Added!");
        }

        public async Task<PlateResponse> EditPlateAsync(Plate model)
        {
            _context.Plates.Update(model);
            await SaveChangesAsync();
            return new PlateResponse(true, "Plate Edited!");
        }

        public async Task<IEnumerable<Plate>> GetAllPlates() => await _context.Plates.AsNoTracking().ToListAsync();


        public async Task<Plate> GetPlateById(string id) => await _context.Plates.FindAsync(id);
       

        public async Task<Plate> GetPlateByName(string plateName) => await _context.Plates.Where(p => p.PlateName == plateName).FirstOrDefaultAsync();


        public async Task<Plate> PlateExist(string id) => await _context.Plates.FindAsync(id);
    

        public async Task<PlateResponse> RemovePlateAsync(string id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null) return new PlateResponse(false, "Plate not found!");

            _context.Plates.Remove(plate);
            await SaveChangesAsync();
            return new PlateResponse(true, "Plate deleted!");
        }

        private async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
