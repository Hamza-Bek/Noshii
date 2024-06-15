using Microsoft.AspNetCore.Http;


namespace Domain.Models
{
    public class Plate
    {
        public string? PlateId { get; set; }
        public string? PlateName { get; set; }
        public string? PlateBio { get; set; }
        public decimal? PlatePrice { get; set; }
       // public IFormFile Image { get; set; }
    }
}
