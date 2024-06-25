
namespace Domain.Models
{
    public class Plate
    {
        public string Id { get; set; }
        public string? PlateName { get; set; }
        public string? PlateBio { get; set; }
        public decimal? PlatePrice { get; set; }
        public string? CategoryTag { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
