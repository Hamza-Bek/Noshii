using Domain.Models.Authentication;
using Microsoft.EntityFrameworkCore;


namespace Domain.Models.OrderEntities
{
    public class Order
    {
        
        public string OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public string? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string? OrderStatus { get; set; }

        // RELATIONSHIPS                
        public string? LocationId { get; set; }
        public Location? Location { get; set; }

   
        public ApplicationUser OrderMaker { get; set; }
		public ICollection<OrderDetails> GetOrderDetails { get; set; } = new List<OrderDetails>();

		// public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
	}
}
