using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }

        public string? CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}
