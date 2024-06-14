using Application.DTOs.Request.Order;
using AutoMapper;
using Domain.Models;

namespace Application.Extensions
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Cart, CartDTO>();
            CreateMap<CartDTO, Cart>();

            CreateMap<CartItem, CartItemDTO>();
            CreateMap<CartItemDTO, CartItem>();

            CreateMap<Plate, PlateDTO>();
            CreateMap<PlateDTO, Plate>();

        }
        
    }
}
