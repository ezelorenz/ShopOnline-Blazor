using Microsoft.EntityFrameworkCore;
using VentasOnline_Api.Entities;

namespace VentasOnline_Api.Data
{
    public class VentasOnlineDbContext : DbContext
    {
        public VentasOnlineDbContext(DbContextOptions<VentasOnlineDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
