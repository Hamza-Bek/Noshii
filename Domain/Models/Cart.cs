using Domain.Models.Authentication;
using Domain.Models.OrderEntities;


namespace Domain.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public string? UserId { get; set; }
        public decimal CartTotal { get; set; }
        public bool? IsOrdered { get; set; }
		

        // RELATIONSHIPS

		public ApplicationUser CartOwner{ get; set; }	
		public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();

        
    }
}
