using E_Com.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Com.Data
{
    public class EComDbContext : DbContext
    {
        public EComDbContext(DbContextOptions<EComDbContext> options) : base(options) { }

        public DbSet<TblUsers> TblUsers { get; set; }
        public DbSet<TblCategory> TblCategory { get; set; }
        public DbSet<TblProduct> TblProduct { get; set; }
        public DbSet<TblSeller> TblSeller { get; set; }
        public DbSet<TblCart> TblCart { get; set; }
        public DbSet<TblCartItem> TblCartItem { get; set; }

        // Add the DbSet for the tblReviews (Review) table
        public DbSet<Review> Reviews { get; set; }

        public object CartDetailsByCartIdResponseDTO { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationships
            modelBuilder.Entity<TblCart>()
                .HasOne(c => c.User)  // One Cart has one User
                .WithOne(u => u.Cart) // One User has one Cart
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            modelBuilder.Entity<TblCartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TblCartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TblProduct>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TblProduct>()
                .HasOne(p => p.Seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-One relationship between TblUsers and TblSeller
            modelBuilder.Entity<TblSeller>()
                .HasOne(s => s.User)  // Seller has one User
                .WithOne(u => u.Seller)  // User has one Seller
                .HasForeignKey<TblSeller>(s => s.UserId)  // Foreign key in TblSeller is UserId
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            // Configure the relationship for the Reviews table
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)  // A review is written by one user
                .WithMany(u => u.Reviews)  // A user can write many reviews
                .HasForeignKey(r => r.UserId)  // Foreign key to the TblUser table
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)  // A review is about one product
                .WithMany(p => p.Reviews)  // A product can have many reviews
                .HasForeignKey(r => r.ProductId)  // Foreign key to the TblProduct table
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete for product

            modelBuilder.Entity<TblUsers>()
    .HasOne(u => u.Cart)
    .WithOne(c => c.User)
    .HasForeignKey<TblUsers>(u => u.CartId)
    .OnDelete(DeleteBehavior.Cascade); // or Restrict, depending on your requirements


            base.OnModelCreating(modelBuilder);
        }
    }
}
