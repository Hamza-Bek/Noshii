using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Order
{
    public class PlateDTO
    {
        public string Id { get; set; }
        public string? PlateName { get; set; }
        public string? PlateBio { get; set; }
        public decimal? PlatePrice { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}
