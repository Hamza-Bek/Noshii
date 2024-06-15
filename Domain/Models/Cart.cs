using Domain.Models.Authentication;


namespace Domain.Models
{
    public class Cart
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public decimal CartTotal { get; set; }
        public ApplicationUser CartOwner{ get; set; }

        public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();

        public decimal? FinalTotalPrice => CartItems is not null ? CartItems.Sum(i => i.PlatePrice * i.Quantity) : 0;
    }
}
