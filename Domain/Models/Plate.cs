
namespace Domain.Models
{
    public class Plate
    {
        public string? PlateId { get; set; }
        public string? PlateName { get; set; }
        public string? PlateBio { get; set; }
        public decimal? PlatePrice { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
