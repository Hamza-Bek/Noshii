

namespace Domain.Models
{
    public class CartItem
    {
        public string? Id { get; set; }
        public string? CartId { get; set; }
        public string? PlateId { get; set; }
        public string? PlateName { get; set; }
        public decimal? PlatePrice { get; set; }
        public decimal? Total { get; set; }
        public int? Quantity { get; set; }

        public Plate plate { get; set; }
       // public Cart cart { get; set; }
        //public virtual Cart Cart { get; set; }
    }
}
