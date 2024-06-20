using Domain.Models.Order;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }

        public string? CartId { get; set; }
                
        public Cart Cart { get; set; }
        
        public ICollection<Domain.Models.Order.Order> Orders { get; set; } = new List<Domain.Models.Order.Order>();
    }
}
