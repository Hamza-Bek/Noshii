using Domain.Models;
using Domain.Models.Authentication;
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



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>() // One to one relationship
              .HasOne(p => p.Cart)
              .WithOne(p => p.CartOwner)
              .OnDelete(DeleteBehavior.NoAction);
            // .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Cart>() // Many to many relationship
             .HasMany(p => p.CartItems)
             .WithMany();
        }
    }
}
