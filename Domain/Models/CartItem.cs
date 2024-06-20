

namespace Domain.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public string? CartId { get; set; }
        public string? PlateId { get; set; }
        public string? PlateName { get; set; }
        public decimal? PlatePrice { get; set; }
        public decimal? Total { get; set; }
        public int? Quantity { get; set; }

        public Plate Plate { get; set; }
        public Cart? Cart { get; set; }
        
        
        
    }
}
