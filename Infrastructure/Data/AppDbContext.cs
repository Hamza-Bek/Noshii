using Domain.Models;
using Domain.Models.Authentication;
using Domain.Models.OrderEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


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

			builder.Entity<Order>()
				.HasMany(o => o.GetOrderDetails)
				.WithOne(od => od.Order)
				.HasForeignKey(od => od.OrderId)
				  .OnDelete(DeleteBehavior.Cascade);

			builder.Entity<OrderDetails>()
		 .HasKey(od => od.OrderDetailId);

			builder.Entity<OrderDetails>()
				.Property(od => od.PlateName)
				.IsRequired();

			builder.Entity<OrderDetails>()
				.Property(od => od.Quantity)
				.IsRequired();


		}
	}
}
