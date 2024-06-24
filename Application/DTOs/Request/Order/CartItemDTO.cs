using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Order
{
    public class CartItemDTO
    {
        public string Id { get; set; }        
        public string? CartId { get; set; }
        public string? PlateId { get; set; }
        public string? PlateName { get; set; }
        public decimal? PlatePrice { get; set; }
        public decimal? Total { get; set; }
        public int? Quantity { get; set; }
    }
}
