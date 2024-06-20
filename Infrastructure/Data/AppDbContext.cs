using Domain.Models;
using Domain.Models.Authentication;
using Domain.Models.Order;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Plate> Plates { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
            .HasOne(a => a.Cart)
            .WithOne(c => c.CartOwner)
            .HasForeignKey<Cart>(c => c.UserId)
          .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
